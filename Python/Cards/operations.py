from os.path import exists
from operator import itemgetter
from itertools import groupby

#my files
import localization
import sql
import exceptions
import files

#-------------------- Create, drop, delete table operations -------------------

def RunSimpleTableOperation(operation, cursor):
    print(localization.headers[operation])
    for tableName in sql.tables:
        if sql.Execute(sql.scripts[operation][tableName], cursor):
            print(localization.messages[operation][tableName])

#-------------------- Create, drop view operations ----------------------------

def RunSimpleViewOperation(operation, cursor):
    print(localization.headers[operation])
    for viewName in sql.views:
        if sql.Execute(sql.scripts[operation][viewName], cursor):
            print(localization.messages[operation][viewName])

#----------------------- Show Tables, Views, Cards ----------------------------

def ShowAllTables(cursor):
    print(localization.headers['ShowTables'])
    for tableName in sql.tables:
        ShowTable(tableName, cursor)

def ShowAllViews(cursor):
    print(localization.headers['ShowViews'])
    for viewName in sql.views:
        ShowView(viewName, cursor)

def GetTableRowsDescription(operation, tableName, cursor):
    script = sql.scripts[operation][tableName]
    rows = None
    header = ""
    if sql.Execute(script, cursor):
        rows = cursor.fetchall()
        columnNames = [f'{i[0]}' for i in cursor.description]
        columnCount = len(columnNames)
        for i in range(columnCount):
            header += columnNames[i]
            if i < columnCount - 1:
                header += ", "
    return { 'Script' : script, 'Rows' : rows, 'Header' : header }

def ShowTable(tableName, cursor):
    ShowTableQuery(GetTableRowsDescription('SelectAllFromTable', tableName, cursor), "",
    localization.messages['ContentTable'][tableName],
    localization.messages['EmptyTable'][tableName])

def ShowView(viewName, cursor):
    ShowTableQuery(GetTableRowsDescription('SelectAllFromView', viewName, cursor), "",
    localization.messages['ContentView'][viewName],
    localization.messages['EmptyView'][viewName])

def ShowAllCards(cursor):
    ShowTableQuery(GetTableRowsDescription('SelectAllFromView', 'AllCards', cursor),
    localization.headers['ShowCards'],
    localization.messages['ContentView']['AllCards'],
    localization.messages['EmptyView']['AllCards'])

def ShowTableQuery(tableRowsDescription, header, notEmptyTableHeader, emptyTableHeader):
    if header:
        print(header)
    rows = tableRowsDescription['Rows']
    if rows:
        print(notEmptyTableHeader)
        print(tableRowsDescription['Header'])
        [print(row) for row in rows]
    else:
        print(emptyTableHeader)
        print(tableRowsDescription['Header'])

#-------------------------------- Add Cards -----------------------------------

def AddCards(inputRows, hasUpdate, cursor):
    if not inputRows:
        print(localization.messages['MissingView']['AllCards'])
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
    for key, items in groupby(rows, itemgetter(primaryIndex)):
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
    rows = GetTableRowsDescription('SelectAllFromTable', tableName, cursor)['Rows']
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
    rows = GetTableRowsDescription('SelectAllFromTable', tableName, cursor)['Rows']
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

#---------------------------- Export Cards ------------------------------------

def ExportCards(file_name, cursor):
    if not file_name:
        print(localization.files['EmptyFileName'])
        return
    if not sql.Execute(sql.scripts['SelectAllFromView']['AllCards'], cursor):
        return
    rows = cursor.fetchall()
    if not rows:
        print(localization.messages['EmptyView']['AllCards'])
        return
    if not exists(file_name):
        print(localization.files['CreateFile'].format(file_name))
    else:
        print(localization.files['ReWriteFile'].format(file_name))
    if files.WriteFile(file_name, GetLinesFromRows(rows, 6, ", ")):
        print(localization.files['FileContent'].format(file_name))
        for line in files.ReadFile(file_name):
            print(line)

def GetLinesFromRows(rows, columnCount, delimeter):
    return [GetRowString(row, columnCount, delimeter) for row in rows]

def GetFormatColumn(column):
    if column == 0:
        return f'{column}'
    elif column == '0':
        return f'{column}'
    elif column:
        return f'{column}'
    else:
        return ''

def GetRowString(row, columnCount, delimeter):
    result = ""
    for i in range(columnCount):
        result += GetFormatColumn(row[i])
        if i < columnCount - 1:
            result += delimeter
    return result

#----------------------------- Commit Changes ---------------------------------

def CommitChanges(cursor):
    print(localization.headers['ApplyChanges'])
    cursor.commit()
    print(localization.messages['ApplyChanges'])

#----------------------------- Run Testing ------------------------------------

def RunTesting(cursor):
    print(localization.headers['Testing'])
    account = input(localization.dialogs['InputExistingAccount'])
    if not account:
        print('Не введен пользователь.')
        if input('Хотите выбрать вопросы без пользователя (1 - да)?') != '1':
            return
    theme = input(localization.dialogs['InputExistingTheme'])
    if not account:
        print('Не введена тема.')
        if input('Хотите выбрать вопросы без темы (1 - да)?') != '1':
            return
    print(localization.messages['TempImposible'])