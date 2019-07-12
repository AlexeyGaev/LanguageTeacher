from os.path import exists
from operator import itemgetter
from itertools import groupby

#my files
import localization
import sql
import exceptions
import files

def RunSimpleTableOperation(operation, cursor):
    print(localization.headers[operation])
    for tableName in sql.tables:
        if sql.Execute(sql.scripts[operation][tableName], cursor):
            print(localization.messages[operation][tableName])

#------------------------------- Show Tables/Cards ----------------------------

def ShowAllTables(cursor):
    print(localization.headers['ShowTables'])
    for tableName in sql.tables:
        ShowTable(tableName, cursor)

def GetTableRowsDescription(operation, tableName, cursor):
    script = sql.scripts[operation][tableName]
    rows = cursor.fetchall() if sql.Execute(script, cursor) else None
    columnNames = [f"{i[0]} ({i[1].__name__})" for i in cursor.description]
    header = ""
    columnCount = len(columnNames)
    for i in range(columnCount):
        header += f"{columnNames[i]}"
        if i < columnCount - 1:
            header += ", "
    return { 'Script' : script, 'Rows' : rows, 'Header' : header }

def ShowTable(tableName, cursor):
    ShowTableQuery(GetTableRowsDescription('SelectAllFrom', tableName, cursor), "",
    localization.messages['ContentTable'][tableName],
    localization.messages['EmptyTable'][tableName])

def QueryAllCards(cursor):
    ShowTableQuery(GetTableRowsDescription('Query', 'AllCards', cursor),
    localization.headers['ShowCards'],
    localization.messages['ContentTable']['QueryAllCards'],
    localization.messages['EmptyTable']['QueryAllCards'])

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
        print(localization.messages['MissingTable']['QueryAllCards'])
        return

    inputCardInfos = CreateInputInfos(inputRows, 0, 3)
    inputThemeInfos = CreateInputInfos(inputRows, 1, 2)
    inputAccountInfos = CreateInputInfos(inputRows, 2, 1)
    inputThemeCardInfos = CreateInputRelationInfos(inputRows, 1, 0, 2, 3)
    inputAccountCardInfos = CreateInputRelationInfos(inputRows, 2, 0, 1, 3)

    existingCardInfos = CreateExistingInfos('Cards', [1, 2, 3], cursor)
    existingThemeInfos = CreateExistingInfos('Themes', [1, 2], cursor)
    existingAccountInfos = CreateExistingInfos('Accounts', [1], cursor)
    existingThemeCardInfos = CreateExistingRelationInfos('ThemeCards', existingThemeInfos, existingCardInfos, cursor)
    existingAccountCardInfos = CreateExistingRelationInfos('AccountCards', existingAccountInfos, existingCardInfos, cursor)

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

    print(localization.headers['AddCards'])

    print("Подготовка скриптов: ")

    ShowInfos(inputRows, 'Введены строки:')
    ShowInfos(inputCardInfos, 'Введены карточки:')
    ShowInfos(inputThemeInfos, "Введены темы:")
    ShowInfos(inputAccountInfos, "Введены пользователи:")
    ShowInfos(inputThemeCardInfos, "Введены связи (theme, card): ")
    ShowInfos(inputAccountCardInfos, "Введены связи (account, card): ")

    ShowInfos(existingCardInfos, "Имеются Cards infos:")
    ShowInfos(existingThemeInfos, "Имеются Themes infos:")
    ShowInfos(existingAccountInfos, "Имеются Accounts infos:")
    ShowInfos(existingThemeCardInfos, "Имеются связи (theme, card): ")
    ShowInfos(existingAccountCardInfos, "Имеются связи (account, card): ")

    ShowInfos(addedCardsInfos, 'AddedCardsInfos:')
    ShowInfos(addedThemeInfos, 'AddedThemeInfos:')
    ShowInfos(addedAccountInfos, 'AddedAccountInfos:')
    ShowInfos(addedThemeCardInfos, 'AddedThemeCardInfos:')
    ShowInfos(addedAccountCardInfos, 'AddedAccountCardInfos:')

    ShowInfos(addedScripts, "Получены скрипты:")

    ExecuteAddedScripts(addedScripts, cursor)

def CreateInputInfos(rows, index, columnCount):
    result = []
    for row in rows:
        info = row[index]
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
        if item[i] != '':
            return False
    return True

def CreateExistingInfos(tableName, secondaryColumnIndexes, cursor):
    result = []
    rows = GetTableRowsDescription('SelectAllFrom', tableName, cursor)['Rows']
    for row in rows:
        secondaryColumns = ()
        for index in secondaryColumnIndexes:
            secondaryColumns += (row[index], )
        result.append((row[0], secondaryColumns))
    return result

def CreateExistingRelationInfos(tableName, keyInfos, valueInfos, cursor):
    result = []
    rows = GetTableRowsDescription('SelectAllFrom', tableName, cursor)['Rows']
    for row in rows:
        keyValue = keyInfos[row[0]][1]
        valueValue =  valueInfos[row[1]][1]
        result.append((keyValue, valueValue))
    return result

def CreateAddedInfos(inputInfos, existingInfos, hasUpdate, columnCount):
    def GetMaxId(rows):
        result = 0
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

def ShowInfos(rows, header):
    print(header)
    [print(row) for row in rows]

def ExecuteAddedScripts(addedScripts, cursor):
    resultExecuteScripts = [(script, sql.Execute(script, cursor)) for script in addedScripts]
    print("Результаты выполнения скриптов: ")
    for script, result in resultExecuteScripts:
        print(script + " " + "Выполнен." if result else "Не выполнен.")

#---------------------------- Export Cards ------------------------------------

def ExportCards(file_name, cursor):
    rows = cursor.fetchall() if sql.Execute(sql.scripts['Query']['AllCards'], cursor) else None
    if files.WriteFile(file_name, GetLinesFromRows(rows, 6, ", ")):
        print(localization.messages['FileContent'].format(file_name))
        for line in GetLinesFromFile(file_name):
            print(line)

def GetLinesFromRows(rows, columnCount, delimeter):
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

    return [GetRowString(row, columnCount, delimeter) for row in rows]

#----------------------------- Commit Changes ---------------------------------

def CommitChanges(cursor):
    print(localization.headers['ApplyChanges'])
    cursor.commit()
    print(localization.messages['ApplyChanges'])

#----------------------------- Run Testing ------------------------------------

def RunTesting(cursor):
    print(localization.headers['Testing'])
    print(localization.messages['TempImposible'])
