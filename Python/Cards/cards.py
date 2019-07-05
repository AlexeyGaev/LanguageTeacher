import pyodbc
import os

from sys import argv
from os.path import exists

script, db_connection_string = argv

db_themesTableName = "Themes"
db_cardsTableName = "Cards"
db_accountsTableName = "Accounts"
db_answersTableName = "Answers"
db_themeCardsTableName = "ThemeCards"
db_accountCardsTableName = "AccountCards"

sql_createTable = "Create table {}({})"
sql_dropTable = "drop table {}"
sql_deleteTable = "delete from {}"
sql_select = "select * from {}"
sql_getThemeId = "Select Theme_Id from Themes where Themes.Theme_desc like '{}'"
sql_getAccountId = "Select Account_Id from Accounts where Accounts.Account_Name like '{}'"

sql_createThemesColumns = "Theme_Id integer not null default 1 primary key, Theme_Desc text, Theme_Level integer"
sql_createCardsColumns = "Card_Id integer not null default 1 primary key, Primary_Side text, Secondary_Side text, Card_Level integer"
sql_createAccountsColumns = "Account_Id integer not null default 1 primary key, Account_Name text"
sql_createAnswersColumns = "Answer_Id integer not null default 1 primary key, BeginDateTime DateTime, EndDateTime DateTime, Card_Id integer, AnswerResult integer"
sql_createThemeCardsColumns = "Theme_Id integer not null, Card_Id integer not null"
sql_createAccountCardsColumns = "Account_Id integer not null, Card_Id integer not null"

sql_getTableNames = "select table_name from information_schema.tables where table_name != 'sysdiagrams'"
sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
sql_getTableColumnDescriptions = "select column_name, data_type, character_maximum_length, table_name, ordinal_position, is_nullable from information_schema.columns where table_name like '{}' order by ordinal_position"
sql_getTableColumnNames = "select column_name from information_schema.columns where table_name like '{}'"
sql_insertRow = "insert into {} values({})"
sql_getColumnCount = "select COUNT(*) from INFORMATION_SCHEMA.COLUMNS where table_name = '{}'"
sql_getRowCount = "select COUNT(*) from {}"
sql_getAllCards = "Select Primary_side, Secondary_side, Card_Level, Theme_desc, Theme_Level, Account_Name from Cards left join ThemeCards on ThemeCards.Card_Id = Cards.Card_Id left join Themes on ThemeCards.Theme_Id = Themes.Theme_Id left join AccountCards on AccountCards.Card_Id = Cards.Card_Id left join Accounts on ThemeCards.Card_Id = AccountCards.Card_Id"

def ClearScreen():
    os.system('cls' if os.name=='nt' else 'clear')

def PressAnyKey():
    input("Нажмите Enter...")

def CreateTables(cursor):
    ClearScreen()
    print("Создаем все новые таблицы")
    CreateTable(db_themesTableName, sql_createThemesColumns, cursor)
    CreateTable(db_cardsTableName, sql_createCardsColumns, cursor)
    CreateTable(db_accountsTableName, sql_createAccountsColumns, cursor)
    CreateTable(db_answersTableName, sql_createAnswersColumns, cursor)
    CreateTable(db_themeCardsTableName, sql_createThemeCardsColumns, cursor)
    CreateTable(db_accountCardsTableName, sql_createAccountCardsColumns, cursor)
    PressAnyKey()

def CreateTable(tableName, columns, cursor):
    tableRow = GetCurrentRow(sql_getTableName.format(tableName), cursor)
    if tableRow:
        print("Таблица", tableName, "существует :")
    else:
        print("Создаем таблицу", tableName, ":")
        cursor.execute(sql_createTable.format(tableName, columns))
    rows = GetRowsFromTable(sql_getTableColumnDescriptions.format(tableName), cursor)
    PrintLines(GetLinesFromRows(rows, 6, ", "))
    print("======================================")

def DropTables(cursor):
    ClearScreen()
    print("Удаляем все существующие таблицы")
    DropTable(db_themesTableName, cursor)
    DropTable(db_cardsTableName, cursor)
    DropTable(db_accountsTableName, cursor)
    DropTable(db_answersTableName, cursor)
    DropTable(db_themeCardsTableName, cursor)
    DropTable(db_accountCardsTableName, cursor)
    print("======================================")
    PressAnyKey()

def DropTable(tableName, cursor):
    tableRow = GetCurrentRow(sql_getTableName.format(tableName), cursor)
    if tableRow:
        print("Удаляем таблицу :", tableName)
        cursor.execute(sql_dropTable.format(tableName))
    else:
        print("Таблицы", tableName, "не существует")

def ClearTables(cursor):
    ClearScreen()
    print("Очищаем все существующие таблицы")
    ClearTable(db_themesTableName, cursor)
    ClearTable(db_cardsTableName, cursor)
    ClearTable(db_accountsTableName, cursor)
    ClearTable(db_answersTableName, cursor)
    ClearTable(db_themeCardsTableName, cursor)
    ClearTable(db_accountCardsTableName, cursor)
    print("======================================")
    PressAnyKey()

def ClearTable(tableName, cursor):
    tableRow = GetCurrentRow(sql_getTableName.format(tableName), cursor)
    if tableRow:
        print("Очищаем таблицу :", tableName)
        cursor.execute(sql_deleteTable.format(tableName))
    else:
        print("Таблицы", tableName, "не существует")

def AddCards(cursor):
    ClearScreen()
    print("Добавляем карточки в таблицы")
    print("Введите пользователя и тему карточек:")
    shouldCreateAccount = input("Создать пользователя (1 - да)?")
    accountId = -1
    if shouldCreateAccount == "1":
        account = input("Account : ")
        accountId = AddToAccountsTable(account, cursor)
    shouldCreateTheme = input("Создать тему (1 - да)?")
    themeId = -1
    if shouldCreateTheme == "1":
        theme_desc = input("Theme : ")
        theme_level = input("Theme Level: ")
        themeId = AddToThemesTable(theme_desc, theme_level, cursor)
    print("======================================")
    while True:
        switch = input("Создать карточку (1 - да)?")
        if switch == "1":
            primary_side = input("Primary Side : ");
            secondary_side = input("Secondary Side : ");
            card_level = input("Card Level : ");
            cardId = AddToCardsTable(primary_side, secondary_side, card_level, cursor)
            if themeId != -1:
                AddToThemeCardsTable(themeId, cardId, cursor)
            if accountId != -1:
                AddToAccountCardsTable(accountId, cardId, cursor)
            print("======================================")
        else:
            break
    PressAnyKey()

def AddToAccountsTable(account, cursor):
    row = GetCurrentRow(sql_getAccountId.format(account), cursor)
    if row:
        return row[0]
    else:
        accountId = GetFirstCurrentRowValue(sql_getRowCount.format(db_accountsTableName), cursor)
        AddRowToTable(db_accountsTableName, f"{accountId}, \'{account}\'", cursor)
        return accountId

def AddToThemesTable(theme_desc, theme_level, cursor):
    row = GetCurrentRow(sql_getThemeId.format(theme_desc), cursor)
    if row:
        return row[0]
    else:
        themeId = GetFirstCurrentRowValue(sql_getRowCount.format(db_themesTableName), cursor)
        AddRowToTable(db_themesTableName, f"{themeId}, \'{theme_desc}\', {theme_level}", cursor)
        return themeId

def AddToCardsTable(primary_side, secondary_side, card_level, cursor):
    index = GetFirstCurrentRowValue(sql_getRowCount.format(db_cardsTableName), cursor)
    AddRowToTable(db_cardsTableName, f"{index}, \'{primary_side}\', \'{secondary_side}\', {card_level}", cursor)
    return index

def AddToThemeCardsTable(themeId, cardId, cursor):
    AddRowToTable(db_themeCardsTableName, f"{themeId}, {cardId}", cursor)

def AddToAccountCardsTable(accountId, cardId, cursor):
    AddRowToTable(db_accountCardsTableName, f"{accountId}, {cardId}", cursor)

def AddRowToTable(tableName, valuesString, cursor):
    cursor.execute(sql_insertRow.format(tableName, valuesString))

def ImportCards(cursor):
    ClearScreen()
    print("Импорт карточек из текстового файла.")
    file_name = input("Введите имя файла:")
    if exists(file_name):
        lines = GetLinesFromFile(file_name)
        if lines:
            print("Содержимое файла {file_name}:")
            PrintLines(lines)
            for line in lines:
                lineItems = line.split(',')
                primary_side = lineItems[0].strip()
                secondary_side = lineItems[1].strip()
                card_level = lineItems[2].strip()
                theme_desc = lineItems[3].strip()
                theme_level = lineItems[4].strip()
                account_name = lineItems[5].strip()
                AddToTables(primary_side, secondary_side, card_level, theme_desc, theme_level, account_name)
        else:
            print("Файл {file_name} пустой.")
    else:
        print("Файл {file_name} не существует.")
    print("======================================")
    PressAnyKey()

def GetLinesFromFile(file_name):
    txt_file = open(file_name, 'r')
    lines = [line.strip() for line in txt_file]
    txt_file.close()
    return lines

def AddToTables(primary_side, secondary_side, card_level, theme_desc, theme_level, account_name):
    print("TODO add: ", primary_side, secondary_side, card_level, theme_desc, theme_level, account_name)

def ShowTables(cursor):
    ClearScreen()
    print("Содержимое базы данных:")
    ShowTable(db_themesTableName, cursor)
    ShowTable(db_cardsTableName, cursor)
    ShowTable(db_accountsTableName, cursor)
    ShowTable(db_answersTableName, cursor)
    ShowTable(db_themeCardsTableName, cursor)
    ShowTable(db_accountCardsTableName, cursor)
    PressAnyKey()

def ShowTable(tableName, cursor):
    rows = GetRowsFromTable(sql_select.format(tableName), cursor)
    if rows:
        print("Содержимое таблицы", tableName, ":")
        columnCount = GetFirstCurrentRowValue(sql_getColumnCount.format(tableName), cursor)
        header = GetTableHeader(tableName, columnCount, cursor)
        print(header)
        PrintLines(GetLinesFromRows(rows, columnCount, ", "))
    else:
        print("Таблица", tableName, "пустая.")
    print("======================================")

def GetTableHeader(tableName, columnCount, cursor):
    columnNamesRows = GetRowsFromTable(sql_getTableColumnDescriptions.format(tableName), cursor)
    header = ""
    i = 0
    for row in columnNamesRows:
        header += f"{row[0]}"
        if i < columnCount - 1:
            header += ", "
        i += 1
    return header

def GetFirstCurrentRowValue(script, cursor):
    return GetCurrentRow(script, cursor)[0]

def GetRowsFromTable(script, cursor):
    cursor.execute(script)
    return cursor.fetchall()

def GetCurrentRow(script, cursor):
    cursor.execute(script)
    return cursor.fetchone()

def GetRowString(row, columnCount, delimeter):
    result = ""
    for i in range(columnCount):
        result += f"{row[i]}"
        if i < columnCount - 1:
            result += delimeter
    return result

def ShowCards(cursor):
    ClearScreen()
    print("Карточки :")
    rows = GetRowsFromTable(sql_getAllCards, cursor)
    if rows:
        print("База карточек :")
        print("Формат: Primary_Side, Secondary_Side, Card_Level, Theme_Desc, Theme_Level, Acccount_Name")
        PrintLines(GetLinesFromRows(rows, 6, ", "))
    else:
        print("Карточки отсутствуют.")
    print("======================================")
    PressAnyKey()

def CommitChanges(cursor):
    ClearScreen()
    print("Отправляем изменения в базу данных")
    cursor.commit()
    print("Изменения успешно отправлены")
    print("======================================")
    PressAnyKey()

def ExportCards(cursor):
    ClearScreen()
    print("Экспорт карточек в текстовый файл:")
    rows = GetRowsFromTable(sql_getAllCards, cursor)
    if not rows:
        print("Карточки отсутствуют.")
    else:
        print("База карточек :")
        print("Формат: Primary_Side, Secondary_Side, Card_Level, Theme_Desc, Theme_Level, Acccount_Name")
        linesFromRows = GetLinesFromRows(rows, 6, ", ")
        PrintLines(linesFromRows)
        file_name = input("Введите имя файла:")
        if not exists(file_name):
            print("Создаем файл {file_name}.")
            ExportToNewTxtFile(file_name, linesFromRows)
        else:
            print("Файл {file_name} существует:")
            linesFromFile = GetLinesFromFile(file_name)
            PrintLines(linesFromFile)
            print("Выберите действие:")
            print("1 - перезаписать (все прежние данные очищаются)")
            print("2 - добавить в конец")
            print("3 - обновить (одинаковые карточки не дублируются)")
            reWriteAction = input("\> ")
            if reWriteAction == "1":
                print("Перезаписываем файла {file_name}.")
                ExportToNewTxtFile(file_name, linesFromRows)
            if reWriteAction == "2":
                print("Добавляем в конец файла {file_name}.")
                ExportToEndTxtFile(file_name, linesFromRows)
            if reWriteAction == "3":
                print("Обновляем файл {file_name}.")
                ExportToNewTxtFile(file_name, JoinLines(linesFromFile, linesFromRows, 6, ','))
    print("======================================")
    PressAnyKey()

def ExportToNewTxtFile(file_name, lines):
    txt_file = open(file_name, 'w')
    [txt_file.write(line + "\n") for line in lines]
    txt_file.close()

def ExportToEndTxtFile(file_name, lines):
    txt_file = open(file_name, 'a')
    [txt_file.write(line + "\n") for line in lines]
    txt_file.close()

def JoinLines(targetLines, addedLines, columnCount, delimeter):
    result = ()
    for targetLine in targetLines:
        result += (targetLine,)
    for addedLine in addedLines:
        if not Contains(targetLines, addedLine, columnCount, delimeter):
            result += (addedLine,)
    return result

def Contains(targetLines, line, columnCount, delimeter):
    lineItems = line.split(',')
    lineItems = [lineItem.strip() for lineItem in lineItems]
    for targetLine in targetLines:
        targetLineItems = targetLine.split(delimeter)
        targetLineItems = [targetLineItem.strip() for targetLineItem in targetLineItems]
        hasDifferences = False
        for i in range(columnCount):
            if targetLineItems[i] != lineItems[i]:
                hasDifferences = True
                break;
        if not hasDifferences:
            return True
    return False

def PrintLines(lines):
    [print(line) for line in lines]

def GetLinesFromRows(rows, columnCount, delimeter):
    return [GetRowString(row, columnCount, delimeter) for row in rows]

def RunTesting(cursor):
    ClearScreen()
    print("Тестирование:")
    print("Временно не поддерживается")
    print("======================================")
    PressAnyKey()

def ShowMainMenu():
    ClearScreen()
    print("Начинаем работу по заполнению карточек")
    print("======================================")
    print("1 - Создать все новые таблицы")
    print("2 - Удалить все существующие таблицы")
    print("3 - Очистить все существующие таблицы")
    print("4 - Добавить карточки вручную")
    print("5 - Импортировать карточки из текстового файла")
    print("6 - Показать содержимое всех таблиц")
    print("7 - Показать карточки")
    print("8 - Экспортировать карточки в текстовый файл")
    print("9 - Отправить изменения в базу данных")
    print("a - Пройти тестирование")
    print("0 - Выход")
    return input("\> ")

def MainMenu(cursor):
    while True:
        showMainMenuSwitch = ShowMainMenu();
        if showMainMenuSwitch == "1":
            CreateTables(cursor)
        if showMainMenuSwitch == "2":
            DropTables(db_cursor)
        if showMainMenuSwitch == "3":
            ClearTables(db_cursor)
        if showMainMenuSwitch == "4":
            AddCards(cursor)
        if showMainMenuSwitch == "5":
            ImportCards(cursor)
        if showMainMenuSwitch == '6':
            ShowTables(cursor)
        if showMainMenuSwitch == '7':
            ShowCards(cursor)
        if showMainMenuSwitch == '8':
            ExportCards(cursor)
        if showMainMenuSwitch == '9':
            CommitChanges(cursor)
        if showMainMenuSwitch == 'a':
            RunTesting(cursor)
        if (showMainMenuSwitch == "0"):
            break

#try:
db_connection = pyodbc.connect(db_connection_string)
db_cursor = db_connection.cursor()
MainMenu(db_cursor)
db_cursor.close()
db_connection.close()
#except:
#    ClearScreen()
#    print("Увы. Не могу связаться к базой данных.")
#    print("Работа с приложением невозможна.")
#    print("======================================")
#    PressAnyKey()
