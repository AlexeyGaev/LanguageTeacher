import os
import os.path
import msvcrt

import localization.rus as localization
import sql.descriptions as sql
import sql.operations as operations
import utils.files as files
import utils.format as format

def StartDialog(script, hasUpdate):
    print(localization.messages['Start'].format(script[0]))
    print(localization.messages['DataBaseSync'])
    EndDialog()
    connect = operations.Connect()
    if connect == None:
        ShowInvalidDataBaseDialog(localization.messages['MainException'])
        return;
    serverMode, connection, database = connect
    cursor = connection.cursor()
    print(localization.messages['DataBaseSyncSaccess'])
    print(localization.messages['ShowOptions'])
    print('DataBase: {}'.format(database))
    print('HasUpdate: {}'.format(hasUpdate))
    EndDialog()
    print(localization.messages['DataBaseVerify'])
    ok, script, log = operations.GetValidTables(sql.tables, sql.table_columns, cursor)
    if not ok:
        if not log:
            ShowScriptException(log)
        ShowInvalidDataBaseDialog(localization.messages['MainException'])
        cursor.close()
        connection.close()
        return
    if not log:
        ShowInvalidDataBaseDialog(localization.messages['DataBaseMissingTableNames'])
        cursor.close()
        connection.close()
        return;
    validTables = log['ValidTables']
    invalidTables = log['InvalidTables']
    exceptionTables = log['ExceptionTables']
    missingTableNames = log['MissingTableNames']
    extraTableNames = log['ExtraTableNames']
    if not validTables and not invalidTables and not exceptionTables and len(missingTableNames) == len(sql.tables) and not extraTableNames:
        print(localization.messages['DataBaseMissingAllTables'])
        EndDialog()
        ShowMainDialog(serverMode, hasUpdate, cursor, connection)
        return;
    if validTables and not invalidTables and not exceptionTables and len(missingTableNames) == 0 and not extraTableNames:
        ShowValidTables(validTables)
        EndDialog()
        ShowMainDialog(serverMode, hasUpdate, cursor, connection)
        return;
    if validTables:
        ShowValidTables(validTables)
    if invalidTables:
        ShowInvalidTables(invalidTables)
    if exceptionTables:
        ShowExceptionTables(exceptionTables)
    for tableName in missingTableNames:
        print(localization.messages['MissingTable'][tableName])
    for tableName in extraTableNames:
        print(localization.messages['ExtraTableName'].format(tableName))
    ShowInvalidDataBaseDialog(localization.messages['DataBaseInvalidTables'])
    cursor.close()
    connection.close()

def ShowInvalidDataBaseDialog(header):
    print(header)
    print(localization.messages['DataBaseException'])
    EndDialog()

def ShowValidTables(validTables):
    for tableName in validTables.keys():
        columnsRowsLog = validTables[tableName]
        print(localization.messages['DataBaseValidTable'].format(tableName))
        print(localization.messages['ContentTable'][tableName])
        okColumns, scriptColumns, dataColumns = columnsRowsLog['Columns']
        columns = dataColumns['ValidColumns']
        print(localization.messages['ColumnCount'][tableName].format(len(columns)))
        ShowValidColumns(columns)
        ShowTableRowsLog(tableName, columnsRowsLog['Rows'])

def ShowInvalidTables(invalidTables):
    for tableName in invalidTables.keys():
        columnsRowsLog = invalidTables[tableName]
        print(localization.messages['DataBaseInvalidTable'].format(tableName))
        print(localization.messages['ContentTable'][tableName])
        okColumns, scriptColumns, dataColumns = columnsRowsLog['Columns']
        dataValid = dataColumns['ValidColumns']
        dataMissing = dataColumns['MissingColumns']
        dataExtra = dataColumns['ExtraColumns']
        print(localization.messages['ColumnCount'][tableName].format(len(dataValid) + len(dataExtra)))
        ShowValidColumns(dataValid)
        ShowMissingColumns(dataMissing)
        ShowExtraColumns(dataExtra)
        ShowTableRowsLog(tableName, columnsRowsLog['Rows'])

def ShowExceptionTables(exceptionTables):
    for tableName in exceptionTables.keys():
        columnsRowsLog = exceptionTables[tableName]
        print(localization.messages['DataBaseExceptionTable'].format(tableName))
        okColumns, scriptColumns, dataColumns = columnsRowsLog['Columns']
        if not okColumns:
            ShowScriptException(dataColumns, scriptColumns)
        else:
            dataValid = dataColumns['ValidColumns']
            dataMissing = dataColumns['MissingColumns']
            dataExtra = dataColumns['ExtraColumns']
            print(localization.messages['ColumnCount'][tableName].format(len(dataValid) + len(dataExtra)))
            ShowValidColumns(dataValid)
            ShowMissingColumns(dataMissing)
            ShowExtraColumns(dataExtra)
        okRows, scriptRows, dataRows = columnsRowsLog['Rows']
        if not okRows:
            ShowScriptException(dataRows, scriptRows)
        else:
            ShowTableRowsLog(tableName, dataRows)

def ShowValidColumns(validColumns):
    for column in validColumns:
        print(localization.messages['IsValidColumn'].format(column))

def ShowMissingColumns(missingColumns):
    for column in missingColumns:
        print(localization.messages['MissingColumn'].format(column))

def ShowExtraColumns(extraColumns):
    for column in extraColumns:
        print(localization.messages['ExtraColumn'].format(column))

def ShowMainDialog(serverMode, hasUpdate, cursor, connection):
    MainMenu(hasUpdate, cursor)
    CommitChanges(cursor if serverMode else connection)
    cursor.close()
    connection.close()

def MainMenu(hasUpdate, cursor):
    menu = CreateMenu('Main')
    while True:
        key = ShowMenu(menu)
        if key == b'\x1b':
            break
        actionType = GetActionType('Main', key)
        if actionType == 1:
            TablesMenu(cursor)
        if actionType == 2:
            CardsMenu(hasUpdate, cursor)
        if actionType == 3:
            TestingDialog(cursor)

def TablesMenu(cursor):
    menu = CreateMenu('Tables')
    while True:
        key = ShowMenu(menu)
        if key == b'\x1b':
            break
        actionType = GetActionType('Tables', key)
        if actionType == 1:
            CreateTablesDialog(cursor)
        if actionType == 2:
            DropTablesDialog(cursor)
        if actionType == 3:
            ClearTablesDialog(cursor)
        if actionType == 4:
            ShowTablesDialog(cursor)

def CardsMenu(hasUpdate, cursor):
    menu = CreateMenu('Cards')
    while True:
        key = ShowMenu(menu)
        if key == b'\x1b':
            break
        actionType = GetActionType('Cards', key)
        if actionType == 1:
            ShowAllCardsDialog(cursor)
        if actionType == 2:
            AddCardsDialog(hasUpdate, cursor)
        if actionType == 3:
            ImportCardsDialog(hasUpdate, cursor)
        if actionType == 4:
            ExportCardsDialog(cursor)

def CreateTablesDialog(cursor):
    ClearScreen()
    print(localization.headers['CreateTable'])
    for tableName in sql.tables:
        log = operations.CreateTable(tableName, cursor)
        ShowSimpleTableOperation('CreateTable', tableName, log)
    EndDialog()

def DropTablesDialog(cursor):
    ClearScreen()
    print(localization.headers['DropTable'])
    for tableName in sql.tables:
        log = operations.DropTable(tableName, cursor)
        ShowSimpleTablesOperation('DropTable', tableName, log)
    EndDialog()

def ClearTablesDialog(cursor):
    ClearScreen()
    print(localization.headers['DeleteFrom'])
    for tableName in [tableName for tableName in sql.tables if sql.views.count(tableName) == 0]:
        log = operations.DeleteTable(tableName, cursor)
        ShowSimpleTablesOperation('DeleteTable', tableName, log)
    EndDialog()

def ShowTablesDialog(cursor):
    ClearScreen()
    print(localization.headers['ShowTables'])
    for tableName in sql.tables:
        columnsLog = operations.GetAllTableColumns(tableName, cursor)
        rowsLog = operations.GetAllTableRows(tableName, cursor)
        ShowTableLog(tableName, columnsLog, rowsLog)
    EndDialog()

def ShowSimpleTableOperation(operation, tableName, log):
    ok, script, exception = log
    if ok:
        print(localization.messages[operation][tableName])
    elif exception:
        ShowScriptException(exception)
    elif not script:
        print(localization.messages['EmptyScript'].format(script))
    else:
        print(localization.messages['ScriptHeader'].format(script))
        print(localization.messages['ScriptEmptyResult'])

def ShowTableLog(tableName, columnsLog, rowsLog):
    print(localization.messages['ContentTable'][tableName])
    ShowTableColumnsLog(tableName, columnsLog)
    ShowTableRowsLog(tableName, rowsLog)

def ShowTableColumnsLog(tableName, columnsLog):
    ok, script, data = columnsLog
    if not data:
        print(localization.messages['MissingColumns'][tableName])
    elif not ok:
        ShowScriptException(data, script)
    else:
        print(localization.messages['ColumnCount'][tableName].format(len(data)))
        for dataItem in data:
            print(dataItem)

def ShowTableRowsLog(tableName, rowsLog):
    ok, script, data = rowsLog
    if not data:
        print(localization.messages['MissingRows'][tableName])
    elif not ok:
        ShowScriptException(data, script)
    else:
        print(localization.messages['RowCount'][tableName].format(len(data)))
        for dataItem in data:
            print(dataItem)

def ShowAllCardsDialog(cursor):
    ClearScreen()
    print(localization.headers['ShowCards'])
    columnsLog = operations.GetAllTableColumns('AllCards', cursor)
    rowsLog = operations.GetAllTableRows('AllCards', cursor)
    ShowTableLog('AllCards', columnsLog, rowsLog)
    EndDialog()

def AddCardsDialog(hasUpdate, cursor):
    ClearScreen()
    ShowAddCards(operations.AddCards(InputAddCards(), hasUpdate, cursor))
    EndDialog()

def ImportCardsDialog(hasUpdate, cursor):
    ClearScreen()
    ShowAddCards(operations.AddCards(InputImportCards(','), hasUpdate, cursor))
    EndDialog()

def ShowAddCards(log):
    ok, data = log
    print(localization.headers['AddCards'])
    if not ok:
        print('При расчете строк произошли исключения.')
        if data:
            for row in data:
                ok, script, exception = row
                ShowScriptException(exception, script)
        return
    if not data:
        print(localization.messages['MissingAddedRows'])
        return

    ShowInfos(data['InputRows'], 'HasInputRows', 'MissingInputRows', None)
    ShowInfos(data['InputCardInfos'], 'HasInput', 'MissingInput', 'Cards')
    ShowInfos(data['InputThemeInfos'], 'HasInput', 'MissingInput', 'Themes')
    ShowInfos(data['InputAccountInfos'], 'HasInput', 'MissingInput', 'Accounts')
    ShowInfos(data['InputThemeCardInfos'], 'HasInput', 'MissingInput', 'ThemeCards')
    ShowInfos(data['InputAccountCardInfos'], 'HasInput', 'MissingInput', 'AccountCards')
    ShowInfos(data['ExistingCardInfos'], 'HasExisting', 'MissingExisting', 'Cards')
    ShowInfos(data['ExistingThemeInfos'], 'HasExisting', 'MissingExisting', 'Themes')
    ShowInfos(data['ExistingAccountInfos'], 'HasExisting', 'MissingExisting', 'Accounts')
    ShowInfos(data['ExistingThemeCardInfos'], 'HasExisting', 'MissingExisting', 'ThemeCards')
    ShowInfos(data['ExistingAccountCardInfos'], 'HasExisting', 'MissingExisting', 'AccountCards')
    ShowInfos(data['AddedCardsInfos'], 'HasAdded', 'MissingAdded', 'Cards')
    ShowInfos(data['AddedThemeInfos'], 'HasAdded', 'MissingAdded', 'Themes')
    ShowInfos(data['AddedAccountInfos'], 'HasAdded', 'MissingAdded', 'Accounts')
    ShowInfos(data['AddedThemeCardInfos'], 'HasAdded', 'MissingAdded', 'ThemeCards')
    ShowInfos(data['AddedAccountCardInfos'], 'HasAdded', 'MissingAdded', 'AccountCards')
    ShowAddedScripts(data['AddedScripts'])

def ShowInfos(rows, description_has, descrition_missing, tableName):
    locStr = localization.add_cards[description_has] if rows else localization.add_cards[descrition_missing]
    print(locStr[tableName] if tableName else locStr)
    if rows:
        [print(row) for row in rows]

def ShowAddedScripts(log):
    if not log:
        print(localization.add_cards['MissingSqlScripts'])
        return
    for ok, script, data in log:
        scriptResult = localization.add_cards['SuccessRunSqlScript'] if ok else localization.add_cards['FailRunSqlScript']
        print(script, scriptResult)

def ExportCardsDialog(cursor):
    ClearScreen()
    print(localization.headers['ExportCards'])
    file_name = input(localization.dialogs['InputFileName'])
    file_name = file_name.strip()
    if not file_name:
        print(localization.files['EmptyFileName'])
        return
    ok, script, data = operations.GetAllTableRows('AllCards', cursor)
    if not ok:
        ShowScriptException(data, script)
        return
    if not os.path.exists(file_name):
        print(localization.files['CreateFile'].format(file_name))
    else:
        print(localization.files['ReWriteFile'].format(file_name))
    if files.WriteFile(file_name, format.GetLinesFromRows(data, 6, ", ")):
        print(localization.files['FileContent'].format(file_name))
        for line in files.ReadFile(file_name):
            print(line)
    EndDialog()

def ShowScriptException(e, script):
    print(localization.messages['HasException'].format(e.args[0]))
    print(localization.messages['ScriptException'].format(script))
    if e.args[0] == '42S01' or e.args[0] == '42S02':
        print(localization.messages['InvalidTable'], e.args[1]);

#--------------------- TODO : TestingDialog -----------------------------------

def TestingDialog(cursor):
    # TODO dialog
    print(localization.headers['Testing'])
    account = input(localization.dialogs['InputExistingAccount'])
    if not account:
        print(localization.messages['NothingInputAccount'])
        if input(localization.messages['InputHasAnswersWithoutAccount']) != '1':
            return
    theme = input(localization.dialogs['InputExistingTheme'])
    if not theme:
        print(localization.messages['NothingInputTheme'])
        if input(localization.messages['InputHasAnswersWithoutTheme']) != '1':
            return
    print(localization.messages['TempImposible'])

#--------------------------- End TODO -----------------------------------------

def CommitChanges(cursor):
    print(localization.messages['EndProgram'])
    print(localization.messages['InputApplyChanges'])
    if msvcrt.getch() != b'0':
        print(localization.headers['ApplyChanges'])
        cursor.commit()
        print(localization.messages['ApplyChanges'])

#-------------------------  Add cards dialog ----------------------------------

def InputAddCards():
    result = []
    primary_side = input("Primary Side : ").strip()
    secondary_side = input("Secondary Side : ").strip()
    card_level = StringToInt(input("Card Level : "))
    theme_name = input("Theme : ").strip()
    theme_level = StringToInt(input("Theme Level : "))
    account_name = input("Account : ").strip()
    result.append((primary_side, secondary_side, card_level, theme_name, theme_level, account_name))
    while True:
        if input(localization.dialogs['InputContinueCreateCard']) != '1':
            break
        primary_side = input("Primary Side : ").strip()
        secondary_side = input("Secondary Side : ").strip()
        card_level = StringToInt(input("Card Level : "))
        theme_name = input("Theme : ").strip()
        theme_level = StringToInt(input("Theme Level : "))
        account_name = input("Account : ").strip()
        result.append((primary_side, secondary_side, card_level, theme_name, theme_level, account_name))
    return result

#-------------------- Import cards from file dialog ---------------------------

def InputImportCards(delimeter):
    print(localization.headers['ImportCards'])
    file_name = input(localization.dialogs['InputFileName'])
    file_name = file_name.strip()
    if not file_name:
        print(localization.files['EmptyFileName'])
        return None
    if not os.path.exists(file_name):
        print(localization.files['InvalidFile'].format(file_name))
        return None
    linesFromFile = files.ReadFile(file_name)
    result = []
    for line in linesFromFile:
        columns = line.split(delimeter)
        primary_side = columns[0].strip()
        secondary_side = columns[1].strip()
        card_level = StringToInt(columns[2].strip())
        theme_name = columns[3].strip()
        theme_level = StringToInt(columns[4].strip())
        account_name = columns[5].strip()
        result.append((primary_side, secondary_side, card_level, theme_name, theme_level, account_name))
    return result

def StringToInt(value):
    if not value:
        return None
    elif value == '':
        return None
    else:
        try:
            return int(value)
        except:
            return None

#-------------------------------- Utils ---------------------------------------

def ClearScreen():
    os.system('cls' if os.name=='nt' else 'clear')

def EndDialog():
    print(localization.messages['PressAnyKey'])
    msvcrt.getch()

def ShowMenu(menu):
    ClearScreen()
    PrintLines(menu)
    return msvcrt.getch()

def CreateMenu(description):
    result = []
    localizationMenu = localization.menues[description]
    for header in localizationMenu['Headers']:
        result.append(header)
    for item in localizationMenu['Items']:
        h, n, k = item
        result.append("{} - {}".format(h, n))
    return result

def GetActionType(description, key):
    actions = {}
    for item in localization.menues[description]['Items']:
        h, n, k = item
        if k == key:
            return h
    return -1

def PrintLines(lines):
    [print(line) for line in lines]
