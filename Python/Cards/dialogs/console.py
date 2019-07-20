# !!!TODO : rename to console dialog, add web dialog -----------------------------

import os
import os.path
import msvcrt

import localization.rus as localization
import sql.descriptions as sql
import sql.operations as operations
import utils.files as files
import utils.format as format

# -------------------------- TODO : refactor, localize ------------------------

def StartDialog(script, hasUpdate):
    print(localization.messages['Start'].format(script))
    print(localization.messages['DataBaseSync'])
    print(localization.messages['PressAnyKey'])
    msvcrt.getch()
    connect = operations.Connect()
    if connect == None:
        print(localization.messages['MainException'])
        print(localization.messages['PressAnyKey'])
        msvcrt.getch()
        return;
    print("Установлена связь с базой данных.")
    serverMode, connection, database = connect
    ShowOptionsLog(CreateOptionsLog(database, hasUpdate))
    cursor = connection.cursor()
    print("Проверка содержимого базы данных.")
    print(localization.messages['PressAnyKey'])
    msvcrt.getch()
    check = operations.CheckValidTableNames(sql.tables, cursor)
    if not check:
        print('Не могу сделать запрос на проверку таблиц.')
        print('Работа с приложением невозможна.')
        cursor.close()
        connection.close()
        return;
    validTables = {}
    for tableName in check.keys():
        if check[tableName]:
            log = operations.CheckValidTableColumnsRows(tableName, sql.table_columns, cursor)
            if log:
                ShowCheckValidTableColumnsRows(tableName, log)
                isValidTable = True
                for value in log['ValidColumns'].values():
                    if not value:
                        isValidTable = False
                        break;
                validTables[tableName] = isValidTable
                if isValidTable:
                    print("Таблица {} валидная ".format(tableName))
                else:
                    print("Таблица {} не валидная ".format(tableName))
        else:
            print(localization.messages['MissingTable'][tableName])
            validTables[tableName] = True
    print(localization.messages['PressAnyKey'])
    msvcrt.getch()
    for tableName in validTables.keys():
        if not validTables[tableName]:
            print('Не все таблицы валидные.')
            print('Работа с приложением невозможна.')
            cursor.close()
            connection.close()
            return;
    MainMenu(hasUpdate, cursor)
    CommitChanges(cursor if serverMode else connection)
    cursor.close()
    connection.close()

# ------------------------- End TODO ------------------------------------------

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

def ShowCheckValidTableColumnsRows(tableName, log):
    print(localization.messages['ContentTable'][tableName])
    ShowCheckValidTableColumns(tableName, log['Columns'], log['ValidColumns'])
    ShowSelectAllRowsFromTable(tableName, log['Rows'])

#------- TODO : localization, TestingDialog --------------------- -------------

def TestingDialog(cursor):
    # TODO dialog
    print(localization.headers['Testing'])
    account = input(localization.dialogs['InputExistingAccount'])
    if not account:
        print('Не введен пользователь.')
        if input('Хотите выбрать вопросы без пользователя (1 - да)?') != '1':
            return
    theme = input(localization.dialogs['InputExistingTheme'])
    if not theme:
        print('Не введена тема.')
        if input('Хотите выбрать вопросы без темы (1 - да)?') != '1':
            return
    print(localization.messages['TempImposible'])

def CommitChanges(cursor):
    print("Вы закончили работать с базой данных.")
    print("Отправить изменения в базу данных (0 - нет)?")
    if msvcrt.getch() != b'0':
        print(localization.headers['ApplyChanges'])
        cursor.commit()
        print(localization.messages['ApplyChanges'])

#--------------------------- End TODO -----------------------------------------
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

def CreateOptionsLog(database, hasUpdate):
    result = []
    result.append('DataBase: {}'.format(database))
    result.append('HasUpdate: {}'.format(hasUpdate))
    return result

def ShowOptionsLog(log):
    print(localization.messages['ShowOptions'])
    [print(row) for row in log]

def ShowSimpleTablesOperation(operation, log):
    [ShowSimpleTableOperation(operation, key) for key in log.keys() if log[key]]

def ShowSimpleTableOperation(operation, tableName):
    print(localization.messages[operation][tableName])

def ShowSelectAllColumnsAndAllRowsFromTables(log):
    for tableName in log.keys():
        ShowSelectAllColumnsAndAllRowsFromTable(tableName, log[tableName])

def ShowSelectAllColumnsAndAllRowsFromTable(tableName, columnsRows):
    print(localization.messages['ContentTable'][tableName])
    ShowSelectAllColumnsFromTable(tableName, columnsRows['Columns'])
    ShowSelectAllRowsFromTable(tableName, columnsRows['Rows'])

def ShowCheckValidTableColumns(tableName, columnsLog, validColumnsLog):
    if not columnsLog:
        print(localization.messages['MissingColumns'][tableName])
    else:
        print(localization.messages['ColumnCount'][tableName].format(len(columnsLog)))
        for column in columnsLog:
            if validColumnsLog[column]:
                print(localization.messages['IsValidColumn'].format(column))
            else:
                print(localization.messages['InvalidColumn'].format(column))

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
