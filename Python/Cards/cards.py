import pyodbc
import os

from sys import argv
from os.path import exists

script, db_connection_string = argv

localization_stars = "======================================"

localization_createAllTables = "Создаем все новые таблицы : "
localization_dropAllTables = "Удаляем все существующие таблицы : "
localization_deleteAllTables = "Очищаем все существующие таблицы : "
localization_existTable = "Таблица {} существует."
localization_creatingTable = "Создаем таблицу {}."
localization_dropTable = "Удаляем таблицу {}."
localization_nothingTable = "Таблица {} не существует."
localization_deleteTable = "Очищаем таблицу {}."
localization_showTables = "Содержимое таблиц : "
localization_showTable = "Содержимое таблицы {} : "
localization_emptyTable = "Таблица {} пустая."
localization_cardTable = "Таблица карточек :"
localization_nothingCardTable = "Карточки отсутствуют."
localization_dataBaseBeginChanges = "Отправляем изменения в базу данных."
localization_dataBaseEndChanges = "Изменения успешно отправлены."
localization_addCards = "Добавляем карточки в таблицы."
localization_inputAccountAndTheme = "Введите пользователя и тему карточек : "
localization_addedAccount = "Пользователь (id = {}, name = {}) добавлен."
localization_addedTheme = "Тема (id = {}, Desc = {}, Level = {}) добавлена."
localization_addedCard = "Карточка (id = {}, PrimarySide = {}, SecondarySide = {}, Level = {}) добавлена."
localization_addedThemeCard = "Связь (theme_id = {}, card_id = {}) добавлена."
localization_addedAccountCard = "Связь (account_id = {}, card_id = {}) добавлена."

localization_invalidAllTables = "Не все таблицы присутствуют в базе данных."
localization_cantShowCards = "Показать карточки не могу."
localization_cantImportCards = "Импортировать карточки не могу."
localization_cantExportCards = "Эскпортировать карточки не могу."

localization_input_pressAnyKey = "Нажмите Enter..."
localization_input_createAccount = "Создать пользователя (1 - да)?"
localization_input_createTheme = "Создать тему (1 - да)?"
localization_input_createCard = "Создать карточку (1 - да)?"

localization_except_main = "Не могу связаться к базой данных. \nРабота с приложением невозможна."

localization_menu_header = "Начинаем работу по заполнению карточек"
localization_menu_createNewTables = "Создать все новые таблицы"
localization_menu_dropTables = "Удалить все существующие таблицы"
localization_menu_deleteTables = "Очистить все существующие таблицы"
localization_menu_showTables = "Показать содержимое всех таблиц"
localization_menu_showCards = "Показать карточки"
localization_menu_addCards = "Добавить карточки вручную"
localization_menu_importCards = "Импортировать карточки из текстового файла"
localization_menu_exportCards = "Экспортировать карточки в текстовый файл"
localization_menu_changeDataBase = "Отправить изменения в базу данных"
localization_menu_testing = "Пройти тестирование"
localization_menu_exit = "Выход"

def GetFirstCurrentRowValue(script, cursor):
    return GetCurrentRow(script, cursor)[0]

def GetRowsFromTable(script, cursor):
    cursor.execute(script)
    return cursor.fetchall()

def GetCurrentRow(script, cursor):
    cursor.execute(script)
    return cursor.fetchone()

def GetTableNames():
    return ('Themes', 'Cards', 'Accounts', 'ThemeCards', 'AccountCards', 'Answers')

def GetCardColumnNames():
    return ('Primary_Side', 'Secondary_Side', 'Card_Level', 'Theme_Desc', 'Theme_Level', 'Acccount_Name')

def GetCreateTableScriptTuples():
    result = ()
    result += (('Themes', 'Theme_Id integer not null default 1 primary key, Theme_Desc text, Theme_Level text'), )
    result += (('Cards', 'Card_Id integer not null default 1 primary key, Primary_Side text, Secondary_Side text, Card_Level text'), )
    result += (('Accounts', 'Account_Id integer not null default 1 primary key, Account_Name text'), )
    result += (('ThemeCards', 'Theme_Id integer not null, Card_Id integer not null'), )
    result += (('AccountCards', 'Account_Id integer not null, Card_Id integer not null'), )
    result += (('Answers', 'Answer_Id integer not null default 1 primary key, BeginDateTime DateTime, EndDateTime DateTime, Card_Id integer, AnswerResult integer'), )
    return result

#------------------------------------------------------------------------------

def CreateTables(tableScriptTuples, cursor):
    output = ()
    output += (localization_createAllTables, )
    for tableScriptTuple in tableScriptTuples:
        lines = CreateTable(tableScriptTuple, cursor)
        for line in lines:
            output += (line, )
    return output

def CreateTable(tableScriptTuple, cursor):
    output = ()
    tableName = tableScriptTuple[0]
    createColumns = tableScriptTuple[1]
    sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
    if GetCurrentRow(sql_getTableName.format(tableName), cursor):
        output += (localization_existTable.format(tableName), )
    else:
        output += (localization_creatingTable.format(tableName), )
        sql_createTable = "Create table {}({})"
        cursor.execute(sql_createTable.format(tableName, createColumns))
    sql_getTableColumnDescriptions = "select column_name, data_type, character_maximum_length, ordinal_position, is_nullable from information_schema.columns where table_name like '{}' order by ordinal_position"
    rows = GetRowsFromTable(sql_getTableColumnDescriptions.format(tableName), cursor)
    for row in rows:
        output += (row, )
    output += (localization_stars, )
    return output

def DropTables(tableNames, cursor):
    output = ()
    output += (localization_dropAllTables, )
    for tableName in tableNames:
        lines = DropTable(tableName, cursor)
        for line in lines:
            output += (line, )
    output += (localization_stars, )
    return output

def DropTable(tableName, cursor):
    output = ()
    sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
    if GetCurrentRow(sql_getTableName.format(tableName), cursor):
        output += (localization_dropTable.format(tableName), )
        sql_dropTable = "drop table {}"
        cursor.execute(sql_dropTable.format(tableName))
    else:
        output += (localization_nothingTable.format(tableName), )
    return output

def ClearTables(tableNames, cursor):
    output = ()
    output += (localization_deleteAllTables, )
    for tableName in tableNames:
        lines = ClearTable(tableName, cursor)
        for line in lines:
            output += (line, )
    output += (localization_stars, )
    return output

def ClearTable(tableName, cursor):
    output = ()
    sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
    if GetCurrentRow(sql_getTableName.format(tableName), cursor):
        output += (localization_deleteTable.format(tableName), )
        sql_deleteTable = "delete from {}"
        cursor.execute(sql_deleteTable.format(tableName))
    else:
        output += (localization_nothingTable.format(tableName), )
    return output

def ShowTables(tableNames, cursor):
    output = ()
    output += (localization_showTables, )
    for tableName in tableNames:
        lines = ShowTable(tableName, cursor)
        for line in lines:
            output += (line, )
    return output

def ShowTable(tableName, cursor):
    output = ()
    sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
    if not GetCurrentRow(sql_getTableName.format(tableName), cursor):
        output += (localization_nothingTable.format(tableName), )
    else:
        sql_select = "select * from {}"
        rows = GetRowsFromTable(sql_select.format(tableName), cursor)
        if rows:
            output += (localization_showTable.format(tableName), )
            sql_getColumns = "select column_name from information_schema.columns where table_name like '{}' order by ordinal_position"
            columnsRows = GetRowsFromTable(sql_getColumns.format(tableName), cursor)
            output += (tuple([columnsRow[0] for columnsRow in columnsRows]), )
            for row in rows:
                output += (row, )
        else:
            output += (localization_emptyTable.format(tableName), )
    output += (localization_stars, )
    return output

#------------------------------------------------------------------------------

def ShowCards(cursor):
    output = ()
    if not HasAllTables():
        lines = ShowInvalidAllTables(localization_cantShowCards)
        for line in lines:
            output += (line, )
    else:
        sql_getAllCards = "Select Primary_side, Secondary_side, Card_Level, Theme_desc, Theme_Level, Account_Name from Cards left join ThemeCards on ThemeCards.Card_Id = Cards.Card_Id left join Themes on ThemeCards.Theme_Id = Themes.Theme_Id left join AccountCards on AccountCards.Card_Id = Cards.Card_Id left join Accounts on ThemeCards.Card_Id = AccountCards.Card_Id"
        rows = GetRowsFromTable(sql_getAllCards, cursor)
        if rows:
            output += (localization_cardTable, )
            output += (GetCardColumnNames(), )
            for row in rows:
                output += (row, )
        else:
            output += (localization_nothingCardTable, )
    output += (localization_stars, )
    return output

def ShowInvalidAllTables(cant):
    output = ()
    output += (localization_invalidAllTables, )
    output += (cant, )
    return output

def HasAllTables(tableNames, cursor):
    for tableName in tableNames:
        sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
        if not GetCurrentRow(sql_getTableName.format(tableName), cursor):
            return False
    return True

#------------------------------------------------------------------------------

def HeaderAddCards():
    output = ()
    output += (localization_addCards, )
    output += (localization_inputAccountAndTheme, )
    return output

def InputAddCards():
    result = ()
    while True:
        switch = input(localization_input_createCard)
        if switch == "1":
            primary_side = input("Primary Side : ")
            secondary_side = input("Secondary Side : ")
            card_level = input("Card Level : ")
            theme_desc = ""
            theme_level = ""
            account_name = ""
            if input(localization_input_createTheme) == "1":
                theme_desc = input("Theme : ")
                theme_level = input("Theme Level : ")
            if input(localization_input_createAccount) == "1":
                account_name = input("Account : ")
            result += ((primary_side, secondary_side, card_level, theme_desc, theme_level, account_name), )
        else:
            break
    return result

def OutputAddCards(inputArgs, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
    output = ()
    for inputArg in inputArgs:
        primary_side = arg[0].strip()
        secondary_side = arg[1].strip()
        card_level = arg[2].strip()
        theme_desc = arg[3].strip()
        theme_level = arg[4].strip()
        account_name = arg[5].strip()
        if primary_side == "":
            output += ("Поле {} не заполнено. Добавление невозможно.".format("primary_side"))
            break;
        sql_where = "cards.primary_side like \'{}\'".format(primary_side)
        card_id_row = GetIdRow('card_id', 'cards', sql_where, cursor)
        if card_id_row:
            card_id = card_id_row[0]
            output += ("Карточка с id = {} primary_side = {} имеется.".format(card_id, primary_side))
            if hasUpdateCard:
                update_set = "primary_side = \'{}\', secondary_side = \'{}\', card_level = \'{}\'".format(primary_side, secondary_side, card_level)
                update_where = "card_id = \'{}\'".format(card_id)
                UpdateRow('cards', update_set, update_where, cursor)
                output += ("Карточка с id = {} обновлена.".format(card_id))
                if not (theme_desc == "" and theme_level == ""):
                    sql_where = "themes.theme_desc = \'{}\' or themes.theme_level = \'{}\'".format(theme_desc, theme_level)
                    theme_id_row = GetIdRow('theme_id', 'themes', sql_where, cursor)
                    if theme_id_row:
                        theme_id = theme_id_row[0]
                        output += ("Тема с id = {} theme_desc = \'{}\' или theme_level = \'{}\' имеется.".format(theme_id, theme_desc, theme_level))
                        if hasUpdateTheme:
                            update_set = ""
                            if theme_desc == "":
                                update_set = "theme_level = \'{}\'".format(theme_level)
                            elif theme_level == "":
                                update_set = "theme_desc = \'{}\'".format(theme_desc)
                            else:
                                update_set = "theme_desc = \'{}\', theme_level = \'{}\'".format(theme_desc, theme_level)
                            update_where = "theme_id = \'{}\'".format(theme_id)
                            UpdateRow('themes', update_set, update_where, cursor)
                            output += ("Тема с id = {} обновлена.".format(card_id))
                        else:
#-----------------------------------------------TODO---------------------------
            else:
                output += ("Карточка пропущена.")
        else:
            card_id = CreateId('cards', cursor)
            values = "{}, \'{}\', \'{}\', \'{}\'".format(card_id, primary_side, secondary_side, card_level)
            AddRow('cards', values, cursor)
            output += (localization_addedCard.format(card_id, primary_side, secondary_side, card_level), )
    else:
        output += ("Не введено ни одной карточки", )
    return output

def GetIdRow(select, from, where, cursor):
    sql_getId = "Select {} from {} where {}"
    return GetCurrentRow(sql_getId.format(select, from, where, like), cursor)

def CreateId(tableName, cursor):
    sql_getRowCount = "select COUNT(*) from {}"
    return GetCurrentRow(sql_getRowCount.format(tableName), cursor)[0]

def UpdateRow(tableName, set, where, cursor):
    sql_updateRow = "update {} set {} where {}"
    cursor.execute(sql_updateRow.format(tableName, set, where))

def AddRow(tableName, values, cursor):
    sql_insertRow = "insert into {} values({})"
    cursor.execute(sql_insertRow.format(tableName, values))

#------------------------------------------------------------------------------

def AddCards(cursor):
    output = ()
    output += (localization_addCards, )
    output += (localization_inputAccountAndTheme, )
    shouldCreateAccount = input(localization_input_createAccount)
    accountId = -1
    if shouldCreateAccount == "1":
        account_name = input("Account : ")
        account_id = AddToAccountsTable(account_name, cursor)
        output += (localization_addedAccount.format(account_id, account_name), )
    shouldCreateTheme = input(localization_input_createTheme)
    themeId = -1
    if shouldCreateTheme == "1":
        theme_desc = input("Theme : ")
        theme_level = input("Theme Level : ")
        theme_id = AddToThemesTable(theme_desc, theme_level, cursor)
        output += (localization_addedTheme.format(theme_id, theme_desc, theme_level), )
    while True:
        switch = input(localization_input_createCard)
        if switch == "1":
            primary_side = input("Primary Side : ");
            secondary_side = input("Secondary Side : ");
            card_level = input("Card Level : ");
            card_id = AddToCardsTable(primary_side, secondary_side, card_level, cursor)
            output += (localization_addedCard.format(card_id, primary_side, secondary_side, card_level), )
            if theme_id != -1:
                AddToThemeCardsTable(theme_id, card_id, cursor)
                output += (localization_addedThemeCard.format(theme_id, card_id), )
            if account_id != -1:
                AddToAccountCardsTable(account_id, card_id, cursor)
                output += (localization_addedAccountCard.format(account_id, card_id), )
        else:
            break
    return output

def AddToAccountsTable(account_name, cursor):
    row = GetAccountIdRowByName(account_name, cursor)
    return row[0] if row else AddNewAccountRow(account_name, cursor)

def GetAccountIdRowByName(account_name, cursor):
    sql_getAccountId = "Select Account_Id from Accounts where Accounts.Account_Name like '{}'"
    return GetCurrentRow(sql_getAccountId.format(account_name), cursor)

def AddNewAccountRow(account_name, cursor):
    sql_getRowCount = "select COUNT(*) from {}"
    account_id = GetFirstCurrentRowValue(sql_getRowCount.format(db_accountsTableName), cursor)
    AddRowToTable(db_accountsTableName, f"{account_id}, \'{account_name}\'", cursor)
    return account_id

def AddToThemesTable(theme_desc, theme_level, cursor):
    row = GetThemeIdRowByName(theme_desc, cursor)
    return row[0] if row else AddNewThemeRow(theme_desc, theme_level, cursor)

def GetThemeIdRowByName(theme_desc, cursor):
    sql_getThemeId = "Select Theme_Id from Themes where Themes.Theme_desc like '{}'"
    return GetCurrentRow(sql_getThemeId.format(theme_desc), cursor)

def AddNewThemeRow(theme_desc, theme_level, cursor):
    sql_getRowCount = "select COUNT(*) from {}"
    theme_id = GetFirstCurrentRowValue(sql_getRowCount.format(db_themesTableName), cursor)
    AddRowToTable(db_themesTableName, f"{theme_id}, \'{theme_desc}\', {theme_level}", cursor)
    return theme_id

def AddToCardsTable(primary_side, secondary_side, card_level, cursor):
    row = GetCardIdRowByPrimarySide(primary_side, cursor)
    return row[0] if row else AddNewCardRow(primary_side, secondary_side, card_level, cursor)



def AddToThemeCardsTable(theme_id, card_id, cursor):
    AddRowToTable(db_themeCardsTableName, f"{theme_id}, {card_id}", cursor)

def AddToAccountCardsTable(account_id, card_id, cursor):
    AddRowToTable(db_accountCardsTableName, f"{account_id}, {card_id}", cursor)



#------------------------------------------------------------------------------

def ImportCards(cursor):
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

def GetLinesFromFile(file_name):
    txt_file = open(file_name, 'r')
    lines = [line.strip() for line in txt_file]
    txt_file.close()
    return lines

def AddToTables(primary_side, secondary_side, card_level, theme_desc, theme_level, account_name):
    print("TODO add: ", primary_side, secondary_side, card_level, theme_desc, theme_level, account_name)

#------------------------------------------------------------------------------

def ExportCards(cursor):
    print("Экспорт карточек в текстовый файл:")
    sql_getAllCards = "Select Primary_side, Secondary_side, Card_Level, Theme_desc, Theme_Level, Account_Name from Cards left join ThemeCards on ThemeCards.Card_Id = Cards.Card_Id left join Themes on ThemeCards.Theme_Id = Themes.Theme_Id left join AccountCards on AccountCards.Card_Id = Cards.Card_Id left join Accounts on ThemeCards.Card_Id = AccountCards.Card_Id"
    rows = GetRowsFromTable(sql_getAllCards, cursor)
    if not rows:
        print("Карточки отсутствуют.")
    else:
        print("База карточек :")
        print("Primary_Side, Secondary_Side, Card_Level, Theme_Desc, Theme_Level, Acccount_Name")
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
                print("Перезаписываем файл {file_name}.")
                ExportToNewTxtFile(file_name, linesFromRows)
            if reWriteAction == "2":
                print("Добавляем в конец файла {file_name}.")
                ExportToEndTxtFile(file_name, linesFromRows)
            if reWriteAction == "3":
                print("Обновляем файл {file_name}.")
                ExportToNewTxtFile(file_name, JoinLines(linesFromFile, linesFromRows, 6, ','))
    print("======================================")

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

def GetLinesFromRows(rows, columnCount, delimeter):
    return [GetRowString(row, columnCount, delimeter) for row in rows]

def GetRowString(row, columnCount, delimeter):
    result = ""
    for i in range(columnCount):
        result += f"{row[i]}"
        if i < columnCount - 1:
            result += delimeter
    return result

#------------------------------------------------------------------------------

def CommitChanges(cursor):
    output = ()
    output += (localization_dataBaseBeginChanges, )
    cursor.commit()
    output += (localization_dataBaseEndChanges, )
    output += (localization_stars, )
    return output

#------------------------------------------------------------------------------

def RunTesting(cursor):
    print("Тестирование:")
    print("Временно не поддерживается")
    print("======================================")

#------------------------------------------------------------------------------

def MainExcept():
    output = ()
    output += (localization_except_main, )
    return output

#------------------------------------------------------------------------------

def ClearScreen():
    os.system('cls' if os.name=='nt' else 'clear')

def PressAnyKey():
    input(localization_input_pressAnyKey)

def PrintLines(lines):
    [print(line) for line in lines]

def CreateMainMenu():
    output = ()
    output += (localization_menu_header, )
    output += (localization_stars, )
    output += ("1 - " + localization_menu_createNewTables, )
    output += ("2 - " + localization_menu_dropTables, )
    output += ("3 - " + localization_menu_deleteTables, )
    output += ("4 - " + localization_menu_showTables, )
    output += ("5 - " + localization_menu_showCards, )
    output += ("6 - " + localization_menu_addCards, )
    output += ("7 - " + localization_menu_importCards, )
    output += ("8 - " + localization_menu_exportCards, )
    output += ("9 - " + localization_menu_changeDataBase, )
    output += ("a - " + localization_menu_testing, )
    output += ("0 - " + localization_menu_exit, )
    return output

def MainMenu(cursor):
    mainMenu = CreateMainMenu();
    while True:
        ClearScreen()
        PrintLines(mainMenu)
        showMainMenuSwitch = input("\> ")
        if showMainMenuSwitch == "1":
            ClearScreen()
            PrintLines(CreateTables(GetCreateTableScriptTuples(), cursor))
            PressAnyKey()
        if showMainMenuSwitch == "2":
            ClearScreen()
            PrintLines(DropTables(GetTableNames(), cursor))
            PressAnyKey()
        if showMainMenuSwitch == "3":
            ClearScreen()
            PrintLines(ClearTables(GetTableNames(), cursor))
            PressAnyKey()
        if showMainMenuSwitch == "4":
            ClearScreen()
            PrintLines(ShowTables(GetTableNames(), cursor))
            PressAnyKey()
        if showMainMenuSwitch == '5':
            ClearScreen()
            PrintLines(ShowCards(cursor))
            PressAnyKey()
        if showMainMenuSwitch == "6":
            ClearScreen()
            PrintLines(AddCards(cursor))
            PressAnyKey()
        if showMainMenuSwitch == '7':
            ClearScreen()
            ImportCards(cursor)
            PressAnyKey()
        if showMainMenuSwitch == '8':
            ClearScreen()
            ExportCards(cursor)
            PressAnyKey()
        if showMainMenuSwitch == '9':
            ClearScreen()
            PrintLines(CommitChanges(cursor))
            PressAnyKey()
        if showMainMenuSwitch == 'a':
            ClearScreen()
            RunTesting(cursor)
            PressAnyKey()
        if (showMainMenuSwitch == "0"):
            break

#------------------------------------------------------------------------------

try:
    db_connection = pyodbc.connect(db_connection_string)
except:
    ClearScreen()
    PrintLines(MainExcept())
    PressAnyKey()
else:
    db_cursor = db_connection.cursor()
    MainMenu(db_cursor)
    db_cursor.close()
    db_connection.close()
