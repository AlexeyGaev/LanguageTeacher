import sqlite3
import pyodbc
import operator
import itertools

import sql.scripts as sql
import localization.rus as localization

def Connect():
    serverMode = True
    try:
        if serverMode:
            database = 'DRIVER=SQL Server;SERVER=HOME\SQLEXPRESS;DATABASE=Cards'
            return True, pyodbc.connect(database), database
        else:
            database = 'Cards.db'
            return False, sqlite3.connect(database), database
    except:
        return None

def CreateTables(tableNames, cursor):
    result = {}
    for tableName in tableNames:
        result[tableName] = CreateTable(tableName, cursor)
    return result

def CreateTable(tableName, cursor):
    return sql.Execute(sql.scripts['CreateTable'][tableName], cursor)

def DropTables(tableNames, cursor):
    result = {}
    for tableName in tableNames:
        result[tableName] = DropTable(tableName, cursor)
    return result

def DropTable(tableName, cursor):
    return sql.Execute(sql.scripts['DropTable'][tableName], cursor)

def DeleteTables(tableNames, cursor):
    result = {}
    for tableName in tableNames:
        result[tableName] = DeleteTable(tableName, cursor)
    return result

def DeleteTable(tableName, cursor):
    return sql.Execute(sql.scripts['DeleteFrom'][tableName], cursor)

def SelectAllRowsFromTables(tableNames, cursor):
    result = {}
    for tableName in tableNames:
        result[tableName] = SelectAllFromTable(tableName, cursor)
    return result

def SelectAllRowsFromTable(tableName, cursor):
    if not sql.Execute(sql.scripts['SelectAllRowsFromTable'][tableName], cursor):
        return None
    return [GetRowItems(row) for row in cursor.fetchall()]

def GetRowItems(row):
    result = ()
    for item in row:
        result += (item, )
    return result

def SelectAllColumnsFromTable(tableName, cursor):
    if not sql.Execute(sql.scripts['SelectAllColumnsFromTable'][tableName], cursor):
        return None
    return [GetRowItems(row) for row in cursor.fetchall()]

def SelectAllColumnsAndAllRowsFromTables(tableNames, cursor):
    result = {}
    for tableName in tableNames:
        result[tableName] = SelectAllColumnsAndAllRowsFromTable(tableName, cursor)
    return result

def SelectAllColumnsAndAllRowsFromTable(tableName, cursor):
    return {
        'Columns' : SelectAllColumnsFromTable(tableName, cursor),
        'Rows' : SelectAllRowsFromTable(tableName, cursor)
        }

def CheckValidTableColumnsRows(tableName, validTableColumns, cursor):
    columns = SelectAllColumnsFromTable(tableName, cursor)
    validColumns = {}
    for column in columns:
        validColumns[column] = validTableColumns[tableName].count(column) > 0
    return {
        'Columns' : columns,
        'Rows' : SelectAllRowsFromTable(tableName, cursor),
        'ValidColumns': validColumns
        }

def SelectAllTableNames(cursor):
    return sql.Execute(sql.scripts['SelectAllTableNames'], cursor)

def CheckValidTableNames(tableNames, cursor):
    if not SelectAllTableNames(cursor):
        return None
    rows = [row[0] for row in cursor.fetchall()]
    result = {}
    for tableName in tableNames:
        result[tableName] = rows.count(tableName) > 0
    return result

#------------------------  TODO -----------------------------------------------

#-------------------------------- Add Cards -----------------------------------

def AddCards(rows, hasUpdate, cursor):
    inputRows = []
    for row in rows:
        cardInfo = (row[0], row[1], row[2])
        themeInfo = (row[3], row[4])
        accountInfo = ()
        accountInfo += (row[5], )
        inputRows.append((cardInfo, themeInfo, accountInfo))

    if not inputRows:
        print(localization.messages['MissingTable']['AllCards'])
        return

    print(localization.headers['AddCards'])
    print(localization.add_cards['PrepareSqlScripts'])
    ShowInfos(inputRows, 'HasInputRows', 'MissingInputRows', None)

    inputCardInfos = CreateInputInfos(inputRows, 0, 3)
    inputThemeInfos = CreateInputInfos(inputRows, 1, 2)
    inputAccountInfos = CreateInputInfos(inputRows, 2, 1)
    inputThemeCardInfos = CreateInputRelationInfos(inputRows, 1, 0, 2, 3)
    inputAccountCardInfos = CreateInputRelationInfos(inputRows, 2, 0, 1, 3)

    ShowInfos(inputCardInfos, 'HasInput', 'MissingInput', 'Cards')
    ShowInfos(inputThemeInfos, 'HasInput', 'MissingInput', 'Themes')
    ShowInfos(inputAccountInfos, 'HasInput', 'MissingInput', 'Accounts')
    ShowInfos(inputThemeCardInfos, 'HasInput', 'MissingInput', 'ThemeCards')
    ShowInfos(inputAccountCardInfos, 'HasInput', 'MissingInput', 'AccountCards')

    existingCardInfos = CreateExistingInfos('Cards', [1, 2, 3], cursor)
    existingThemeInfos = CreateExistingInfos('Themes', [1, 2], cursor)
    existingAccountInfos = CreateExistingInfos('Accounts', [1], cursor)
    existingThemeCardInfos = CreateExistingRelationInfos('ThemeCards', existingThemeInfos, existingCardInfos, cursor)
    existingAccountCardInfos = CreateExistingRelationInfos('AccountCards', existingAccountInfos, existingCardInfos, cursor)

    ShowInfos(existingCardInfos, 'HasExisting', 'MissingExisting', 'Cards')
    ShowInfos(existingThemeInfos, 'HasExisting', 'MissingExisting', 'Themes')
    ShowInfos(existingAccountInfos, 'HasExisting', 'MissingExisting', 'Accounts')
    ShowInfos(existingThemeCardInfos, 'HasExisting', 'MissingExisting', 'ThemeCards')
    ShowInfos(existingAccountCardInfos, 'HasExisting', 'MissingExisting', 'AccountCards')

    addedCardsInfos = CreateAddedInfos(inputCardInfos, existingCardInfos, hasUpdate, 3)
    addedThemeInfos = CreateAddedInfos(inputThemeInfos, existingThemeInfos, hasUpdate, 2)
    addedAccountInfos = CreateAddedInfos(inputAccountInfos, existingAccountInfos, hasUpdate, 1)
    addedThemeCardInfos = CreateAddedRelationInfos(inputThemeCardInfos, existingThemeCardInfos, addedThemeInfos, addedCardsInfos)
    addedAccountCardInfos = CreateAddedRelationInfos(inputAccountCardInfos, existingAccountCardInfos, addedAccountInfos, addedCardsInfos)

    ShowInfos(addedCardsInfos, 'HasAdded', 'MissingAdded', 'Cards')
    ShowInfos(addedThemeInfos, 'HasAdded', 'MissingAdded', 'Themes')
    ShowInfos(addedAccountInfos, 'HasAdded', 'MissingAdded', 'Accounts')
    ShowInfos(addedThemeCardInfos, 'HasAdded', 'MissingAdded', 'ThemeCards')
    ShowInfos(addedAccountCardInfos, 'HasAdded', 'MissingAdded', 'AccountCards')

    addedScripts = []
    addedScripts.extend(CreateAddedCardScripts(addedCardsInfos))
    addedScripts.extend(CreateAddedThemeScripts(addedThemeInfos))
    addedScripts.extend(CreateAddedAccountScripts(addedAccountInfos))
    addedScripts.extend(CreateAddedRelationScripts(addedThemeCardInfos, 'ThemeCards'))
    addedScripts.extend(CreateAddedRelationScripts(addedAccountCardInfos, 'AccountCards'))

    ExecuteAddedScripts(addedScripts, cursor)

def CreateInputInfos(rows, firstColumnIndex, columnCount):
    result = []
    for row in rows:
        info = row[firstColumnIndex]
        if not IsEmpty(info, columnCount) and result.count(info) == 0:
            result.append(info)
    return result

def CreateInputRelationInfos(rows, primaryIndex, groupItemIndex, primaryColumnCount, groupItemColumnCount):
    def GetGroupItems(items, itemIndex):
        result = []
        for item in items:
            result.append(item[itemIndex])
        return result

    result = []
    for key, items in itertools.groupby(rows, operator.itemgetter(primaryIndex)):
        for item in GetGroupItems(items, groupItemIndex):
            if not IsEmpty(key, primaryColumnCount) and not IsEmpty(item, groupItemColumnCount):
                info = (key, item)
                if result.count(info) == 0:
                    result.append(info)
    return result

def IsEmpty(item, columnCount):
    for i in range(columnCount):
        column = item[i]
        if column and column != '':
            return False
    return True

def CreateExistingInfos(tableName, secondaryColumnIndexes, cursor):
    result = []
    rows = SelectAllRowsFromTable(tableName, cursor)
    if not rows:
        return result
    for row in rows:
        secondaryColumns = ()
        for index in secondaryColumnIndexes:
            secondaryColumns += (row[index], )
        result.append((row[0], secondaryColumns))
    return result

def CreateExistingRelationInfos(tableName, keyInfos, valueInfos, cursor):
    result = []
    rows = SelectAllRowsFromTable(tableName, cursor)
    if not rows:
        return result
    for row in rows:
        keyValue = FindInfoById(keyInfos, row[0])
        valueValue = FindInfoById(valueInfos, row[1])
        if keyValue and valueValue:
            result.append((keyValue, valueValue))
    return result

def FindInfoById(infos, id):
    for info in infos:
        if info[0] == id:
            return info[1]
    return None

def CreateAddedInfos(inputInfos, existingInfos, hasUpdate, columnCount):
    def GetMaxId(rows):
        result = 0
        if not rows:
            return result
        for row in rows:
            id = row[0]
            if (id > result):
                result = id
        return result

    def CreateTuple(id, info, infoColumnCount, update):
        result = ()
        result += (id, )
        columns = ()
        for i in range(infoColumnCount):
            columns += (info[i], )
        result += (columns, )
        result += (update, )
        return result

    def GetFirstIndex(column, existingInfos):
        for existingInfo in existingInfos:
            if column == existingInfo[0]:
                return existingInfos.index(existingInfo)
        return -1

    startId = GetMaxId(existingInfos) + 1
    existingPartialInfos = [info[1] for info in existingInfos]
    result = []
    for inputInfo in inputInfos:
        existingIndex = GetFirstIndex(inputInfo[0], existingPartialInfos)
        if existingIndex == -1:
            result.append(CreateTuple(startId, inputInfo, columnCount, 'InsertInto'))
            startId += 1
        elif hasUpdate:
            existingInfo = existingInfos[existingIndex]
            hasDifferences = False
            for i in range(columnCount):
                if existingInfo[1][i] != inputInfo[i]:
                    hasDifferences = True
                    break
            if hasDifferences:
                result.append(CreateTuple(existingInfo[0], inputInfo, columnCount, ('Update', existingInfo)))
    return result

def CreateAddedRelationInfos(inputRelationInfos, existingRelationInfos, addedKeyInfos, addedValueInfos):
    def GetRelationId(infos, targetItem):
        for info in infos:
            id, compareItem, update = info
            if update == 'InsertInto' and compareItem == targetItem:
                return id
        return None

    result = []
    for inputInfo in inputRelationInfos:
        if existingRelationInfos.count(inputInfo) == 0:
            theme_id = GetRelationId(addedKeyInfos, inputInfo[0])
            if theme_id:
                card_id = GetRelationId(addedValueInfos, inputInfo[1])
                if card_id:
                    result.append((theme_id, card_id))
    return result

def CreateAddedCardScripts(addedInfos):
    result = []
    for info in addedInfos:
        id, inputInfo, update = info
        if update == 'InsertInto':
            script = CreateCardInsertIntoScript(id, inputInfo)
        else:
            script = CreateCardUpdateScript(id, inputInfo, update[1][1])
        result.append(script)
    return result

def CreateAddedThemeScripts(addedInfos):
    result = []
    for info in addedInfos:
        id, inputInfo, update = info
        if update == 'InsertInto':
            script = CreateThemeInsertIntoScript(id, inputInfo)
        else:
            script = CreateThemeUpdateScript(id, inputInfo, update[1][1])
        result.append(script)
    return result

def CreateAddedAccountScripts(addedInfos):
    result = []
    for info in addedInfos:
        id, inputInfo, update = info
        result.append(CreateAccountInsertIntoScript(id, inputInfo))
    return result

def CreateCardInsertIntoScript(id, inputInfo):
    if inputInfo[0] == "" and inputInfo[1] == "" and inputInfo[2] == "":
        return sql.scripts['InsertInto']['Cards'][0].format(id)
    elif inputInfo[0] == "" and inputInfo[1] == "" and inputInfo[2] != "":
        return sql.scripts['InsertInto']['Cards'][1].format(id, inputInfo[2])
    elif inputInfo[0] == "" and inputInfo[1] != "" and inputInfo[2] == "":
        return sql.scripts['InsertInto']['Cards'][2].format(id, inputInfo[1])
    elif inputInfo[0] == "" and inputInfo[1] != "" and inputInfo[2] != "":
        return sql.scripts['InsertInto']['Cards'][3].format(id, inputInfo[1], inputInfo[2])
    elif inputInfo[0] != "" and inputInfo[1] == "" and inputInfo[2] == "":
        return sql.scripts['InsertInto']['Cards'][4].format(id, inputInfo[0])
    elif inputInfo[0] != "" and inputInfo[1] == "" and inputInfo[2] != "":
        return sql.scripts['InsertInto']['Cards'][5].format(id, inputInfo[0], inputInfo[2])
    elif inputInfo[0] != "" and inputInfo[1] != "" and inputInfo[2] == "":
        return sql.scripts['InsertInto']['Cards'][6].format(id, inputInfo[0], inputInfo[1])
    else:
        return sql.scripts['InsertInto']['Cards'][7].format(id, inputInfo[0], inputInfo[1], inputInfo[2])

def CreateThemeInsertIntoScript(id, inputInfo):
    if inputInfo[0] == "" and inputInfo[1] == "":
        return sql.scripts['InsertInto']['Themes'][0].format(id)
    elif inputInfo[0] == "" and inputInfo[1] != "":
        return sql.scripts['InsertInto']['Themes'][1].format(id, inputInfo[1])
    elif inputInfo[0] != "" and inputInfo[1] == "":
        return sql.scripts['InsertInto']['Themes'][2].format(id, inputInfo[0])
    else:
        return sql.scripts['InsertInto']['Themes'][3].format(id, inputInfo[0], inputInfo[1])

def CreateAccountInsertIntoScript(id, inputInfo):
    if inputInfo[0] == "":
        return sql.scripts['InsertInto']['Accounts'][0].format(id)
    else:
        return sql.scripts['InsertInto']['Accounts'][1].format(id, inputInfo[0])

def CreateCardUpdateScript(id, inputInfo, existingInfo):
    if inputInfo[1] == existingInfo[1] and inputInfo[2] != existingInfo[2]:
        return sql.scripts['Update']['Cards'][0].format(inputInfo[2], id)
    elif inputInfo[1] != existingInfo[1] and inputInfo[2] == existingInfo[2]:
        return sql.scripts['Update']['Cards'][1].format(inputInfo[1], id)
    else:
        return sql.scripts['Update']['Cards'][2].format(inputInfo[1], inputInfo[2], id)

def CreateThemeUpdateScript(id, inputInfo, existingInfo):
    return sql.scripts['Update']['Cards'][0].format(inputInfo[1], id)

def CreateAddedRelationScripts(infos, tableName):
    return [sql.scripts['InsertInto'][tableName][0].format(info[0], info[1]) for info in infos]

def ShowInfos(rows, description_has, descrition_missing, tableName):
    locStr = localization.add_cards[description_has] if rows else localization.add_cards[descrition_missing]
    print(locStr[tableName] if tableName else locStr)
    if rows:
        [print(row) for row in rows]

def ExecuteAddedScripts(addedScripts, cursor):
    if not addedScripts:
        print(localization.add_cards['MissingSqlScripts'])
        return
    resultExecuteScripts = [(script, sql.Execute(script, cursor)) for script in addedScripts]
    print(localization.add_cards['HeaderRunSqlScripts'])
    for script, result in resultExecuteScripts:
        scriptResult = localization.add_cards['SuccessRunSqlScript'] if result else localization.add_cards['FailRunSqlScript']
        print(script, scriptResult)
