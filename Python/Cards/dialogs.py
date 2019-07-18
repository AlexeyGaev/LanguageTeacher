import os.path
import msvcrt

import localization
import files
import menues
import operations
import sql

#---------------------- Starting dialog TODO ----------------------------------

def StartDialog(connect, hasUpdate):
    print("Установлена связь с базой данных.")
    serverMode, connection, database = connect
    ShowOptionsLog(CreateOptionsLog(database, hasUpdate))
    cursor = connection.cursor()
    print("Проверка содержимого базы данных.")
    print(localization.messages['PressAnyKey'])
    msvcrt.getch()
    tableNames = SelectAllTableNames(cursor)
    if not tableNames:
        print('Не могу сделать запрос на проверку таблиц.')
        print('Работа с приложением невозможна.')
    else:
        log = operations.SelectAllColumnsAndRowsFromTables(tableNames, cursor)
        if log:
            ShowSelectAllColumnsAndAllRowsFromTablesResult(log)
            print(localization.messages['PressAnyKey'])
            msvcrt.getch()
        menues.MainMenu(hasUpdate, cursor)
        CommitChanges(serverMode, cursor, connection)
    cursor.close()
    connection.close()

def ShowOptionsLog(log):
    print(localization.messages['ShowOptions'])
    [print(row) for row in log]

def CreateOptionsLog(database, hasUpdate):
    result = []
    result.append('DataBase: {}'.format(database))
    result.append('HasUpdate: {}'.format(hasUpdate))
    return result

def ShowSimpleOperationResult(operation, log):
    [print(localization.messages[operation][key]) for key in log.keys() if log[key]]

def ShowSelectAllColumnsAndAllRowsFromTablesResult(log):
    for tableName in log.keys():
        print(tableName)
        columnsRows = tablesInfo[tableName]
        print('Column count: {}'.format(len(columnsRows['Columns'])))
        for column in columnsRows['Columns']:
            print(column)
        print('Row count: {}'.format(len(columnsRows['Rows'])))
        for row in columnsRows['Rows']:
            print(row)

#------------------------- Add cards dialog -----------------------------------

def InputAddCards():
    result = []
    InputAddCard(result)
    while True:
        if input(localization.dialogs['InputContinueCreateCard']) != '1':
            break
        InputAddCard(result)
    return result

def InputAccount():
    result = ()
    result += (input("Account : ").strip(), )
    return result

def InputTheme():
    result = ()
    result += (input("Theme : ").strip(), )
    result += (StringToInt(input("Theme Level : ")), )
    return result

def InputCard():
    result = ()
    result += (input("Primary Side : ").strip(), )
    result += (input("Secondary Side : ").strip(), )
    result += (StringToInt(input("Card Level : ")), )
    return result

def InputAddCard(cardsList):
    cardInfo = InputCard()
    themeInfo = InputTheme()
    accountInfo = InputAccount()
    if cardInfo != ("", "", "") or themeInfo != ("", "") or accountInfo != "":
        cardsList.append((cardInfo,themeInfo,accountInfo))

#-------------------- Import cards from file dialog ---------------------------

def InputImportCards():
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
        lineItems = line.split(',')
        cardInfo = (lineItems[0].strip(), lineItems[1].strip(), StringToInt(lineItems[2].strip()))
        themeInfo = (lineItems[3].strip(), StringToInt(lineItems[4].strip()))
        accountInfo = ()
        accountInfo += (lineItems[5].strip(), )
        result.append((cardInfo, themeInfo, accountInfo))
    return result

def StringToInt(value):
    if not value:
        return None
    elif value == '':
        return 0
    else:
        return int(value)

#--------------------- Export to file dialog ----------------------------------

def InputExportCards():
    print(localization.headers['ExportCards'])
    file_name = input(localization.dialogs['InputFileName'])
    return file_name.strip()

#-------------------- Commit dialog -------------------------------------------

def CommitChanges(serverMode, cursor, connection):
    print("Вы закончили работать с базой данных.")
    print("Отправить изменения в базу данных (0 - нет)?")
    if msvcrt.getch() != b'0':
        operations.CommitChanges(cursor if serverMode else connection)
