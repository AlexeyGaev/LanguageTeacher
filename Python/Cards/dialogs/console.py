# !!!TODO : rename to console dialog, add web dialog -----------------------------

import os
import os.path
import msvcrt

import localization.rus as localization
import sql.descriptions as sql
import sql.operations as operations
import utils.files as files
import utils.format as format

def StartDialog(script, hasUpdate):
    ShowScriptDialog(script)
    connect = operations.Connect()
    if connect == None:
        ShowInvalidDataBaseDialog(localization.messages['MainException'])
        return;
    serverMode, connection, database = connect
    cursor = connection.cursor()
    print(localization.messages['DataBaseSyncSaccess'])
    ShowOptionsDialog(database, hasUpdate)
    print(localization.messages['DataBaseVerify'])
    log = operations.GetValidTables(sql.tables, sql.table_columns, cursor)
    if not log:
        ShowInvalidDataBaseDialog(localization.messages['DataBaseMissingTableNames'])
        cursor.close()
        connection.close()
        return;
    validTables = log['ValidTables']
    invalidTables = log['InvalidTables']
    missingTableNames = log['MissingTableNames']
    extraTableNames = log['ExtraTableNames']
    if not validTables and not invalidTables and len(missingTableNames) == len(sql.tables) and len(extraTableNames) == 0:
        print(localization.messages['DataBaseMissingAllTables'])
        EndDialog()
        ShowMainDialog(serverMode, hasUpdate, cursor, connection)
        return;
    if validTables and not invalidTables and len(missingTableNames) == 0 and len(extraTableNames) == 0:
        ShowValidTablesDialog(validTables)
        ShowMainDialog(serverMode, hasUpdate, cursor, connection)
        return;
    if validTables:
        ShowValidTablesDialog()
    if invalidTables:
        ShowInvalidTablesDialog(invalidTables)
    if len(missingTableNames) > 0:
        ShowMissingTableNames(missingTableNames)
    if len(extraTableNames) > 0:
        ShowExtraTableNames(extraTableNames)
    ShowInvalidDataBaseDialog(localization.messages['DataBaseInvalidTables'])
    cursor.close()
    connection.close()

def ShowScriptDialog(script):
    print(localization.messages['Start'].format(script))
    print(localization.messages['DataBaseSync'])
    EndDialog()

def ShowInvalidDataBaseDialog(header):
    print(header)
    print(localization.messages['DataBaseException'])
    EndDialog()

def ShowOptionsDialog(database, hasUpdate):
    print(localization.messages['ShowOptions'])
    print('DataBase: {}'.format(database))
    print('HasUpdate: {}'.format(hasUpdate))
    EndDialog()

def ShowValidTablesDialog(validTables):
    for tableName in validTables.keys():
        columnsRows = validTables[tableName]
        print(localization.messages['DataBaseValidTable'].format(tableName))
        ShowSelectAllColumnsAndAllRowsFromTable(tableName, columnsRows['Columns']['ValidColumns'], columnsRows['Rows'])
    EndDialog()

def ShowInvalidTablesDialog(invalidTables):
    for tableName in invalidTables.keys():
        columnsRows = invalidTables[tableName]
        print(localization.messages['DataBaseInvalidTable'].format(tableName))
        print(localization.messages['ContentTable'][tableName])
        validColumns = columnsRows['Columns']['ValidColumns']
        missingColumns = columnsRows['Columns']['MissingColumns']
        extraColumns =  columnsRows['Columns']['ExtraColumns']
        print(localization.messages['ColumnCount'][tableName].format(len(validColumns) + len(extraColumns)))
        if validColumns:
            ShowValidColumns(validColumns)
        if missingColumns:
            ShowMissingColumns(missingColumns)
        if extraColumns:
            ShowExtraColumns(extraColumns)
        ShowSelectAllRowsFromTable(columnsRows['Rows'])
    EndDialog()

def ShowMissingTableNames(missingTableNames):
    for tableName in missingTableNames:
        print(localization.messages['MissingTable'][tableName])

def ShowExtraTableNames(extraTableNames):
    for tableName in extraTableNames:
        print(localization.messages['ExtraTableName'].format(tableName))

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
    log = operations.CreateTables(sql.tables, cursor)
    if log:
        ShowSimpleTablesOperation('CreateTable', log)
    EndDialog()

def DropTablesDialog(cursor):
    ClearScreen()
    print(localization.headers['DropTable'])
    log = operations.DropTables(sql.tables, cursor)
    if log:
        ShowSimpleTablesOperation('DropTable', log)
    EndDialog()

def ClearTablesDialog(cursor):
    ClearScreen()
    print(localization.headers['DeleteFrom'])
    tableNames = [tableName for tableName in sql.tables if sql.views.count(tableName) == 0]
    log = operations.DeleteTables(tableNames, cursor)
    if log:
        ShowSimpleTablesOperation('DeleteFrom', log)
    EndDialog()

def ShowTablesDialog(cursor):
    ClearScreen()
    print(localization.headers['ShowTables'])
    log = operations.SelectAllColumnsAndAllRowsFromTables(sql.tables, cursor)
    if log:
        ShowSelectAllColumnsAndAllRowsFromTables(log)
    EndDialog()

def ShowAllCardsDialog(cursor):
    ClearScreen()
    print(localization.headers['ShowCards'])
    log = operations.SelectAllColumnsAndAllRowsFromTable('AllCards', cursor)
    if log:
        ShowSelectAllColumnsAndAllRowsFromTable('AllCards', log)
    EndDialog()

def AddCardsDialog(hasUpdate, cursor):
    ClearScreen()
    operations.AddCards(InputAddCards(), hasUpdate, cursor)
    EndDialog()

def ImportCardsDialog(hasUpdate, cursor):
    ClearScreen()
    operations.AddCards(InputImportCards(','), hasUpdate, cursor)
    EndDialog()

def ExportCardsDialog(cursor):
    ClearScreen()
    print(localization.headers['ExportCards'])
    file_name = input(localization.dialogs['InputFileName'])
    file_name = file_name.strip()
    if not file_name:
        print(localization.files['EmptyFileName'])
        return
    rows = operations.SelectAllRowsFromTable('AllCards', cursor)
    if not rows:
        return
    if not os.path.exists(file_name):
        print(localization.files['CreateFile'].format(file_name))
    else:
        print(localization.files['ReWriteFile'].format(file_name))
    if files.WriteFile(file_name, format.GetLinesFromRows(rows, 6, ", ")):
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

def ShowSimpleTablesOperation(operation, log):
    [ShowSimpleTableOperation(operation, key) for key in log.keys() if log[key]]

def ShowSimpleTableOperation(operation, tableName):
    print(localization.messages[operation][tableName])

def ShowSelectAllColumnsAndAllRowsFromTables(log):
    for tableName in log.keys():
        columnsRows = log[tableName]
        ShowSelectAllColumnsAndAllRowsFromTable(tableName, columnsRows['Columns'], columnsRows['Rows'])

def ShowSelectAllColumnsAndAllRowsFromTable(tableName, columns, rows):
    print(localization.messages['ContentTable'][tableName])
    ShowSelectAllColumnsFromTable(tableName, columns)
    ShowSelectAllRowsFromTable(tableName, rows)

def ShowSelectAllColumnsFromTable(tableName, columnsLog):
    if not columnsLog:
        print(localization.messages['MissingColumns'][tableName])
    else:
        print(localization.messages['ColumnCount'][tableName].format(len(columnsLog)))
        for column in columnsLog:
            print(column)

def ShowSelectAllRowsFromTable(tableName, rowsLog):
    if not rowsLog:
        print(localization.messages['MissingRows'][tableName])
    else:
        print(localization.messages['RowCount'][tableName].format(len(rowsLog)))
        for row in rowsLog:
            print(row)

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
