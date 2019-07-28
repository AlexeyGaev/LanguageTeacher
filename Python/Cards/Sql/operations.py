import sqlite3
import pyodbc
import operator
import itertools

#------------------------------ SQL scripts -----------------------------------

def GetSelectAllTableNamesScript():
    return "Select Table_Name from information_schema.tables where Table_Name != 'sysdiagrams'"

def GetCreateTableScript(tableName):
    if tableName == 'Themes':
        return 'Create table Themes(Id integer not null primary key, Name char(255), Level integer)'
    elif tableName == 'Cards':
        return 'Create table Cards(Id integer not null primary key, Primary_Side text, Secondary_Side text, Level integer)'
    elif tableName == 'Accounts':
        return 'Create table Accounts(Id integer not null primary key, Name char(255))'
    elif tableName == 'ThemeCards':
        return 'Create table ThemeCards(Theme_Id integer not null, Card_Id integer not null)'
    elif tableName == 'AccountCards':
        return 'Create table AccountCards(Account_Id integer not null, Card_Id integer not null)'
    elif tableName == 'Answers':
        return 'Create table Answers(Card_Id integer not null, Side_Order bit not null, Result integer not null, Level integer)'
    elif tableName == 'AllCards':
        return 'Create view AllCards as Select Primary_Side, Secondary_Side, Cards.Level as Card_Level, RTRIM(Themes.Name) as Theme_Name, Themes.Level as Theme_Level, RTRIM(Accounts.Name) as Account_Name from Cards left join ThemeCards on ThemeCards.Card_Id = Cards.Id left join Themes on ThemeCards.Theme_Id = Themes.Id left join AccountCards on AccountCards.Card_Id = Cards.Id left join Accounts on AccountCards.Account_Id = Accounts.Id'

def GetDropTableScript(tableName):
    return 'Drop table {}'.format(tableName)

def GetDeleteTableScript(tableName):
    return 'Delete from {}'.format(tableName)

def GetSelectAllRowsFromTableScript(tableName):
    if tableName == 'Themes':
        return 'Select Id, RTRIM(Themes.Name) as Name, Level from Themes'
    elif tableName == 'Accounts':
        return 'Select Id, RTRIM(Accounts.Name) as Name from Accounts'
    return 'Select * from {}'.format(tableName)

def GetSelectAllColumnsFromTableScript(tableName):
    return "Select column_name, data_type, character_maximum_length, ordinal_position, is_nullable from information_schema.columns where table_name = '{}'".format(tableName)

def GetCardInsertIntoScript(id, primary_side, secondary_side, level):
    if primary_side == "" and secondary_side == "" and level == "":
        return "Insert into Cards(Id) values({})".format(id)
    elif primary_side == "" and secondary_side == "" and level != "":
        return "Insert into Cards(Id, Level) values({}, {})".format(id, level)
    elif primary_side == "" and secondary_side != "" and level == "":
        return "Insert into Cards(Id, Secondary_Side) values({}, '{}')".format(id, secondary_side)
    elif primary_side == "" and secondary_side != "" and level != "":
        return "Insert into Cards(Id, Secondary_Side, Level) values({}, '{}', {})".format(id, secondary_side, level)
    elif primary_side != "" and secondary_side == "" and level == "":
        return "Insert into Cards(Id, Primary_Side) values({}, '{}')".format(id, primary_side)
    elif primary_side != "" and secondary_side == "" and level != "":
        return "Insert into Cards(Id, Primary_Side, Level) values({}, '{}', {})".format(id, primary_side, level)
    elif primary_side != "" and secondary_side != "" and level == "":
        return "Insert into Cards(Id, Primary_Side, Secondary_Side) values({}, '{}', '{}')".format(id, primary_side, secondary_side)
    else:
        return "Insert into Cards values({}, '{}', '{}', {})".format(id, primary_side, secondary_side, level)

def GetThemeInsertIntoScript(id, name, level):
    if name == "" and level == "":
        return "Insert into Themes(Id) values({})".format(id)
    elif name == "" and level != "":
        return "Insert into Themes(Id, Level) values({}, {})".format(id, level)
    elif name != "" and level == "":
        return "Insert into Themes(Id, Name) values({}, '{}')".format(id, name)
    else:
        return "Insert into Themes values({}, '{}', {})".format(id, name, level)

def GetAccountInsertIntoScript(id, name):
    if name == "":
        return "Insert into Accounts(Id) values({})".format(id)
    else:
        return "Insert into Accounts values({}, '{}')".format(id, name)

def GetAddedRelationScript(tableName, id, value):
    return "Insert into {} values({}, {})".format(tableName, id, value)

def GetCardUpdateScript(id, secondary_side, level, has_secondary_side, has_level):
    if not has_secondary_side and has_level:
        return "Update Cards set Level = '{}' where Id = {}".format(level, id)
    elif has_secondary_side and not has_level:
        return "Update Cards set Secondary_Side = '{}' where Id = {}".format(secondary_side, id)
    else:
        return "Update Cards set Secondary_Side = '{}', Level = '{}' where Id = {}".format(secondary_side, level, id)

def GetThemeUpdateScript(id, level):
    return "Update Themes set Level = {} where Id = {}".format(level, id)

def GetSelectAllCardsByThemeAndAccount(theme_name, account_name):
    if not theme_name and not account_name:
        return 'Select * from AllCards where theme_name is null and account_name is null'
    elif not account_name:
        return 'Select * from AllCards where account_name is null'
    elif not theme_name:
        return 'Select * from AllCards where theme_name is null'
    else:
        return "Select * from AllCards where theme_name = '{}' and account_name = '{}'".format(theme_name, account_name)

def GetSelectCardIdBySide(side, side_order):
    if side_order == 0:
        return "Select Id from Cards where Primary_Side like '{}'".format(side)
    elif side_order == 1:
        return "Select Id from Cards where Secondary_Side like '{}'".format(side)
    return None

def GetInsertIntoAnswer(card_id, side_order, result, level):
    if not level:
        return "Insert into Answers(Card_Id, Side_Order, Result) values({}, {}, {})".format(card_id, side_order, result)
    return "Insert into Answers values({}, {}, {}, {})".format(card_id, side_order, result, level)

def ExecuteSqlScript(script, cursor):
    if not script:
        return False, script, None;
    try:
        cursor.execute(script)
    except Exception as e:
        return False, script, e;
    else:
        return True, script, None

#-------------------------- Connect ------------------------------------------

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

#=-------------------------- Validation ---------------------------------------

def GetValidTables(validTableNames, validTableColumns, cursor):
    script = GetSelectAllTableNamesScript()
    ok, script, exception = ExecuteSqlScript(script, cursor)
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

#--------------------------- Columns or rows ----------------------------------

def GetAllTableRows(tableName, cursor):
    return GetAllTableColumnsOrRows(GetSelectAllRowsFromTableScript(tableName), cursor)

def GetAllTableColumns(tableName, cursor):
    return GetAllTableColumnsOrRows(GetSelectAllColumnsFromTableScript(tableName), cursor)

def GetAllTableColumnsOrRows(script, cursor):
    def GetRowItems(row):
        result = ()
        for item in row:
            result += (item, )
        return result

    ok, script, exception = ExecuteSqlScript(script, cursor)
    if not ok:
        return ok, script, exception
    return True, script, [GetRowItems(row) for row in cursor.fetchall()]

#------------------------ Simple operations -----------------------------------

def CreateTable(tableName, cursor):
    return ExecuteSqlScript(GetCreateTableScript(tableName), cursor)

def DropTable(tableName, cursor):
    return ExecuteSqlScript(GetDropTableScript(tableName), cursor)

def DeleteTable(tableNames, cursor):
    return ExecuteSqlScript(GetDeleteTableScript(tableName), cursor)

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
        return True, None

    inputCardInfos = CreateInputInfos(inputRows, 0, 3)
    inputThemeInfos = CreateInputInfos(inputRows, 1, 2)
    inputAccountInfos = CreateInputInfos(inputRows, 2, 1)
    inputThemeCardInfos = CreateInputRelationInfos(inputRows, 1, 0, 2, 3)
    inputAccountCardInfos = CreateInputRelationInfos(inputRows, 2, 0, 1, 3)

    cardRowsLog = GetAllTableRows('Cards', cursor)
    themeRowsLog = GetAllTableRows('Themes', cursor)
    accountRowsLog = GetAllTableRows('Accounts', cursor)
    themeCardRowsLog = GetAllTableRows('ThemeCards', cursor)
    accountCardRowsLog = GetAllTableRows('AccountCards', cursor)
    cardRowsOk, cardRowsScript, cardRowsData = cardRowsLog
    themeRowsOk, themeRowsScript, themeRowsData = themeRowsLog
    accountRowsOk, accountRowsScript, accountRowsData = accountRowsLog
    themeCardRowsOk, themeCardRowsScript, themeCardRowsData = themeCardRowsLog
    accountCardRowsOk, accountCardRowsScript, accountCardRowsData = accountCardRowsLog
    if not cardRowsOk or not themeRowsOk or not accountRowsOk or not themeCardRowsOk or not accountCardRowsOk:
        result = {}
        result['Cards'] = cardRowsLog
        result['Themes'] = themeRowsLog
        result['Accounts'] = accountRowsLog
        result['ThemeCards'] = themeCardRowsLog
        result['AccountCards'] = accountCardRowsLog
        return False, result

    existingCardInfos = CreateExistingInfos(cardRowsData, [1, 2, 3])
    existingThemeInfos = CreateExistingInfos(themeRowsData, [1, 2])
    existingAccountInfos = CreateExistingInfos(accountRowsData, [1])
    existingThemeCardInfos = CreateExistingRelationInfos(themeCardRowsData, existingThemeInfos, existingCardInfos)
    existingAccountCardInfos = CreateExistingRelationInfos(accountCardRowsData, existingAccountInfos, existingCardInfos)
    addedCardsInfos = CreateAddedInfos(inputCardInfos, existingCardInfos, hasUpdate, 3, [0, 1])
    addedThemeInfos = CreateAddedInfos(inputThemeInfos, existingThemeInfos, hasUpdate, 2, [0])
    addedAccountInfos = CreateAddedInfos(inputAccountInfos, existingAccountInfos, hasUpdate, 1, [0])
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
    return True, result

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
    def FindInfoById(infos, id):
        for info in infos:
            if info[0] == id:
                return info[1]
        return None

    result = []
    for row in rows:
        keyValue = FindInfoById(keyInfos, row[0])
        valueValue = FindInfoById(valueInfos, row[1])
        if keyValue and valueValue:
            result.append((keyValue, valueValue))
    return result

def CreateAddedInfos(inputInfos, existingInfos, hasUpdate, columnCount, compareColumnIndexes):
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

    def GetFirstIndex(inputInfo, indexes, existingInfos):
        for existingInfo in existingInfos:
            hasDifferences = False
            for index in indexes:
                if inputInfo[index] != existingInfo[index]:
                    hasDifferences = True
                    break;
            if not hasDifferences:
                return existingInfos.index(existingInfo)
        return -1

    startId = GetMaxId(existingInfos) + 1
    existingPartialInfos = [info[1] for info in existingInfos]
    result = []
    for inputInfo in inputInfos:
        existingIndex = GetFirstIndex(inputInfo, compareColumnIndexes, existingPartialInfos)
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
            script = GetCardInsertIntoScript(id, inputInfo[0], inputInfo[1], inputInfo[2])
        else:
            existingInfo = update[1][1]
            script = GetCardUpdateScript(id, inputInfo[1], inputInfo[2], existingInfo[1] != inputInfo[1], existingInfo[2] != inputInfo[2])
        result.append(script)
    return result

def CreateAddedThemeScripts(addedInfos):
    result = []
    for info in addedInfos:
        id, inputInfo, update = info
        if update == 'InsertInto':
            script = GetThemeInsertIntoScript(id, inputInfo[0], inputInfo[1])
        else:
            script = GetThemeUpdateScript(id, inputInfo[1])
        result.append(script)
    return result

def CreateAddedAccountScripts(addedInfos):
    result = []
    for info in addedInfos:
        id, inputInfo, update = info
        result.append(GetAccountInsertIntoScript(id, inputInfo[0]))
    return result

def CreateAddedRelationScripts(infos, tableName):
    return [GetAddedRelationScript(tableName, info[0], info[1]) for info in infos]
