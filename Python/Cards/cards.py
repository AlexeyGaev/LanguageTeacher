import pyodbc
import os
import sys

script, db_connection_string = sys.argv

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

sql_createThemesColumns = "Theme_Id integer not null default 1 primary key, Theme_Desc text"
sql_createCardsColumns = "Card_Id integer not null default 1 primary key, Primary_Side text, Secondary_Side text"
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
sql_getAllCards = "Select Primary_side, Secondary_side, Theme_desc, Account_Name from Cards left join ThemeCards on ThemeCards.Card_Id = Cards.Card_Id left join Themes on ThemeCards.Theme_Id = Themes.Theme_Id left join AccountCards on AccountCards.Card_Id = Cards.Card_Id left join Accounts on ThemeCards.Card_Id = AccountCards.Card_Id"

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
    for row in rows:
        print(GetRowString(row, 6, ", "))
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
        theme = input("Theme : ")
        themeId = AddToThemesTable(theme, cursor)
    print("======================================")
    while True:
        switch = input("Создать карточку (1 - да)?")
        if switch == "1":
            primary_side = input("Primary Side : ");
            secondary_side = input("Secondary Side : ");
            cardId = AddToCardsTable(primary_side, secondary_side, cursor)
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

def AddToThemesTable(theme, cursor):
    row = GetCurrentRow(sql_getThemeId.format(theme), cursor)
    if row:
        return row[0]
    else:
        themeId = GetFirstCurrentRowValue(sql_getRowCount.format(db_themesTableName), cursor)
        AddRowToTable(db_themesTableName, f"{themeId}, \'{theme}\'", cursor)
        return themeId

def AddToCardsTable(primary_side, secondary_side, cursor):
    index = GetFirstCurrentRowValue(sql_getRowCount.format(db_cardsTableName), cursor)
    AddRowToTable(db_cardsTableName, f"{index}, \'{primary_side}\', \'{secondary_side}\'", cursor)
    return index

def AddToThemeCardsTable(themeId, cardId, cursor):
    AddRowToTable(db_themeCardsTableName, f"{themeId}, {cardId}", cursor)

def AddToAccountCardsTable(accountId, cardId, cursor):
    AddRowToTable(db_accountCardsTableName, f"{accountId}, {cardId}", cursor)

def AddRowToTable(tableName, valuesString, cursor):
    cursor.execute(sql_insertRow.format(tableName, valuesString))

def ImportCards(cursor):
    ClearScreen()
    print("Импорт карточек из текстового файла:")
    print("Временно не поддерживается")
    print("======================================")
    PressAnyKey()

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
        for row in rows:
            print(GetRowString(row, columnCount, ", "))
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
        print("Primary_Side, Secondary_Side, Theme, Acccount")
        for row in rows:
            print(GetRowString(row, 4, ", "))
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
    print("Временно не поддерживается")
    print("======================================")
    PressAnyKey()

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

try:
    db_connection = pyodbc.connect(db_connection_string)
    db_cursor = db_connection.cursor()
    MainMenu(db_cursor)
    db_cursor.close()
    db_connection.close()
except:
    ClearScreen()
    print("Увы. Не могу приконектиться к базе.")
    print("Работа с приложением невозможна.")
    print("======================================")
    PressAnyKey()
