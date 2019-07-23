import sqlite3
import pyodbc
import operator
import itertools

#------------------------------SQL --------------------------------------------
import sql.scripts as sql
def SelectAllTableNames(cursor):
    return ExecuteSqlScript(sql.scripts['SelectAllTableNames'], cursor)

def CreateTable(tableName, cursor):
    return ExecuteSqlScript(sql.scripts['CreateTable'][tableName], cursor)

def DropTable(tableName, cursor):
    return ExecuteSqlScript(sql.scripts['DropTable'][tableName], cursor)

def DeleteTable(tableName, cursor):
    return ExecuteSqlScript(sql.scripts['DeleteFrom'][tableName], cursor)

def SelectAllRowsFromTable(tableName, cursor):
    return ExecuteSqlScript(sql.scripts['SelectAllRowsFromTable'][tableName], cursor)

def SelectAllColumnsFromTable(tableName, cursor):
    return ExecuteSqlScript(sql.scripts['SelectAllColumnsFromTable'][tableName], cursor)

def ExecuteSqlScript(script, cursor):
    if not script:
        return False, script, None;
    try:
        cursor.execute(script)
    except Exception as e:
        return False, script, e;
    else:
        return True, script, None

#------------------------------------------------------------------------------

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

def GetValidTables(validTableNames, validTableColumns, cursor):
    ok, script, exception = SelectAllTableNames(cursor)
    if not ok:
        return ok, script, exception
    tableNameRows = cursor.fetchall()
    if not tableNameRows:
        return True, script, {
            'ValidTables' : None,
            'InvalidTables': None,
            'ExceptionTables': None,
            'MissingTableNames' : validTableNames.copy(),
            'ExtraTableNames' : None }
    validTables = {}
    invalidTables = {}
    exceptionTables = {}
    missingTableNames = validTableNames.copy();
    extraTableNames = []
    for row in tableNameRows:
        tableName = row[0]
        if missingTableNames.count(tableName) > 0:
            missingTableNames.remove(tableName)
            validColumnsLog = GetValidTableColumns(tableName, validTableColumns[tableName], cursor)
            rowsLog = GetAllTableRows(tableName, cursor)
            columnsRowsLog = { 'Columns' : validColumnsLog, 'Rows' : rowsLog }
            okColumns, scriptColumns, dataColumns = validColumnsLog
            okRows, scriptRows, dataRows = rowsLog
            if not okColumns or not okRows:
                exceptionTables[tableName] = columnsRowsLog
            elif not dataColumns and okRows:
                validTables[tableName] = columnsRowsLog
            else:
                validColumns = dataColumns['ValidColumns']
                missingColumns = dataColumns['MissingColumns']
                extraColumns = dataColumns['ExtraColumns']
                if validColumns and not missingColumns and not extraColumns:
                    validTables[tableName] = columnsRowsLog
                elif not validColumns and len(missingColumns) == len(validTableColumns[tableName]) and not extraColumns:
                    validTables[tableName] = columnsRowsLog
                else:
                    invalidTables[tableName] = columnsRowsLog
        else:
            extraTableNames.append(tableName)
    return True, script, {
        'ValidTables' : validTables,
        'InvalidTables': invalidTables,
        'ExceptionTables': exceptionTables,
        'MissingTableNames' : missingTableNames,
        'ExtraTableNames' : extraTableNames }

def GetAllTableRows(tableName, cursor):
    ok, script, exception = SelectAllRowsFromTable(tableName, cursor)
    if not ok:
        return ok, script, exception
    return True, script, [GetRowItems(row) for row in cursor.fetchall()]

def GetAllTableColumns(tableName, cursor):
    ok, script, exception = SelectAllColumnsFromTable(tableName, cursor)
    if not ok:
        return ok, script, exception
    return True, script, [GetRowItems(row) for row in cursor.fetchall()]

def GetRowItems(row):
    result = ()
    for item in row:
        result += (item, )
    return result

def GetValidTableColumns(tableName, validColumns, cursor):
    ok, script, log = GetAllTableColumns(tableName, cursor)
    if not ok:
        return ok, script, log
    newValidColumns = []
    missingColumns = validColumns.copy();
    extraColumns = []
    for column in log:
        if missingColumns.count(column) > 0:
            missingColumns.remove(column)
            newValidColumns.append(column)
        else:
            extraColumns.append(column)
    return True, script, {
        'ValidColumns' : newValidColumns,
        'MissingColumns' : missingColumns,
        'ExtraColumns' :  extraColumns
        }

def CreateTables(tableNames, cursor):
    result = {}
    for tableName in tableNames:
        result[tableName] = CreateTable(tableName, cursor)
    return result

def DropTables(tableNames, cursor):
    result = {}
    for tableName in tableNames:
        result[tableName] = DropTable(tableName, cursor)
    return result

def DeleteTables(tableNames, cursor):
    result = {}
    for tableName in tableNames:
        result[tableName] = DeleteTable(tableName, cursor)
    return result

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
        return None
    inputCardInfos = CreateInputInfos(inputRows, 0, 3)
    inputThemeInfos = CreateInputInfos(inputRows, 1, 2)
    inputAccountInfos = CreateInputInfos(inputRows, 2, 1)
    inputThemeCardInfos = CreateInputRelationInfos(inputRows, 1, 0, 2, 3)
    inputAccountCardInfos = CreateInputRelationInfos(inputRows, 2, 0, 1, 3)

    cardRowsOk, cardRowsScript, cardRowsData = GetAllTableRows('Cards', cursor)
    themeRowsOk, themeRowsScript, themeRowsData = GetAllTableRows('Themes', cursor)
    accountRowsOk, accountRowsScript, accountRowsData = GetAllTableRows('Accounts', cursor)
    themeCardRowsOk, themeCardRowsScript, themeCardRowsData = GetAllTableRows('ThemeCards', cursor)
    accountCardRowsOk, accountCardRowsScript, accountCardRowsData = GetAllTableRows('AccountCards', cursor)
    if not cardRowsOk or not themeRowsOk or not accountRowsOk or not themeCardRowsOk or not accountCardRowsOk:
        print("!!!ОШИБКА!!!")
        input()
        return

#------------------ TODO -------------------------------------------------------

    existingCardInfos = CreateExistingInfos(cardRowsData, [1, 2, 3])
    existingThemeInfos = CreateExistingInfos(themeRowsData, [1, 2])
    existingAccountInfos = CreateExistingInfos(accountRowsData, [1])
    existingThemeCardInfos = CreateExistingRelationInfos(themeCardRowsData, existingThemeInfos, existingCardInfos)
    existingAccountCardInfos = CreateExistingRelationInfos(accountCardRowsData, existingAccountInfos, existingCardInfos)
    addedCardsInfos = CreateAddedInfos(inputCardInfos, existingCardInfos, hasUpdate, 3)
    addedThemeInfos = CreateAddedInfos(inputThemeInfos, existingThemeInfos, hasUpdate, 2)
    addedAccountInfos = CreateAddedInfos(inputAccountInfos, existingAccountInfos, hasUpdate, 1)
    addedThemeCardInfos = CreateAddedRelationInfos(inputThemeCardInfos, existingThemeCardInfos, addedThemeInfos, addedCardsInfos)
    addedAccountCardInfos = CreateAddedRelationInfos(inputAccountCardInfos, existingAccountCardInfos, addedAccountInfos, addedCardsInfos)
    addedScripts = []
    addedScripts.extend(CreateAddedCardScripts(addedCardsInfos))
    addedScripts.extend(CreateAddedThemeScripts(addedThemeInfos))
    addedScripts.extend(CreateAddedAccountScripts(addedAccountInfos))
    addedScripts.extend(CreateAddedRelationScripts(addedThemeCardInfos, 'ThemeCards'))
    addedScripts.extend(CreateAddedRelationScripts(addedAccountCardInfos, 'AccountCards'))

    result = {}
    result['InputRows'] = inputRows
    result['InputCardInfos'] = inputCardInfos
    result['InputThemeInfos'] = inputThemeInfos
    result['InputAccountInfos'] = inputAccountInfos
    result['InputThemeCardInfos'] = inputThemeCardInfos
    result['InputAccountCardInfos'] = inputAccountCardInfos
    result['ExistingCardInfos'] = existingCardInfos
    result['ExistingThemeInfos'] = existingThemeInfos
    result['ExistingAccountInfos'] = existingAccountInfos
    result['ExistingThemeCardInfos'] = existingThemeCardInfos
    result['ExistingAccountCardInfos'] = existingAccountCardInfos
    result['AddedCardsInfos'] = addedCardsInfos
    result['AddedThemeInfos'] = addedThemeInfos
    result['AddedAccountInfos'] = addedAccountInfos
    result['AddedThemeCardInfos'] = addedThemeCardInfos
    result['AddedAccountCardInfos'] = addedAccountCardInfos
    result['AddedScripts'] = [ExecuteSqlScript(script, cursor) for script in addedScripts] if addedScripts else None

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

def CreateExistingInfos(rows, secondaryColumnIndexes):
    result = []
    for row in rows:
        secondaryColumns = ()
        for index in secondaryColumnIndexes:
            secondaryColumns += (row[index], )
        result.append((row[0], secondaryColumns))
    return result

def CreateExistingRelationInfos(rows, keyInfos, valueInfos):
    result = []
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
