import pyodbc
import os
import msvcrt

from sys import argv
from os.path import exists
from msvcrt import getch

script, db_connection_string = argv

localization_stars = "======================================"

localization_createAllTables = "Создаем все новые таблицы."
localization_dropAllTables = "Удаляем все существующие таблицы."
localization_deleteAllTables = "Очищаем все существующие таблицы."
localization_showTables = "Содержимое таблиц : "
localization_cardTable = "Таблица карточек :"
localization_header_importCards = "Импорт карточек из текстового файла."
localization_dataBaseBeginChanges = "Отправляем изменения в базу данных."

localization_existTable = "Таблица {} существует."
localization_creatingTable = "Создаем таблицу {}."
localization_dropTable = "Удаляем таблицу {}."
localization_nothingTable = "Таблица {} не существует."
localization_deleteTable = "Очищаем таблицу {}."
localization_showTable = "Содержимое таблицы {} : "
localization_emptyTable = "Таблица {} пустая."
localization_nothingCardTable = "Карточки отсутствуют."
localization_dataBaseBeginChanges = "Отправляем изменения в базу данных."
localization_dataBaseEndChanges = "Изменения успешно отправлены."
localization_addCards = "Добавляем карточки в таблицы."
localization_inputAccountAndTheme = "Введите пользователя и тему карточек : "

localization_addCurrentCard = "Добавляю текущую карточку."
localization_addCurrentTheme = "Добавляю текущую тему."
localization_addCurrentAccount = "Добавляю текущего пользователя."
localization_emptyOneField = "Поле {} пустое."
localization_emptyTwoFields = "Поля {} и {} пустые."
localization_cantAddCard = "Добавить карточку не могу."
localization_cantAddTheme = "Добавить тему не могу."
localization_cantAddAccount = "Добавить пользователя не могу."
localization_updateSuccessCard = "Карточка успешно обновлена."
localization_ignoreAddCard = "Карточка пропущена."
localization_updateSuccessTheme = "Тема успешно обновлена."
localization_ignoreAddTheme = "Тема пропущена."
localization_updateSuccessAccount = "Пользователь успешно обновлен."
localization_ignoreAddAccount = "Пользователь пропущен."
localization_existCard = "Имеется карточка с id = {} primary_side = {}."
localization_existTheme = "Имеется тема с id = {} desc = \'{}\' или level = \'{}\'."
localization_existAccount = "Имеется пользователь с id = {} name = {}."
localization_addedAccount = "Пользователь (id = {}, name = {}) добавлен."
localization_addedTheme = "Тема (id = {}, Desc = {}, Level = {}) добавлена."
localization_addedCard = "Карточка (id = {}, PrimarySide = {}, SecondarySide = {}, Level = {}) добавлена."
localization_addedThemeCard = "Связь (theme_id = {}, card_id = {}) добавлена."
localization_addedAccountCard = "Связь (account_id = {}, card_id = {}) добавлена."

localization_invalidAllTables = "Не все таблицы присутствуют в базе данных."
localization_cantShowCards = "Показать карточки не могу."
localization_cantAddCards = "Добавить карточки не могу."
localization_cantImportCards = "Импортировать карточки не могу."
localization_cantExportCards = "Эскпортировать карточки не могу."

localization_import_nothingLinesCards = "Нет строк с карточками."
localization_import_invalidFileName = "Файл {} не существует."

localization_input_pressAnyKey = "Нажмите любую клавишу..."
localization_input_createAccount = "Создать пользователя (1 - да)?"
localization_input_createTheme = "Создать тему (1 - да)?"
localization_input_createCard = "Создать карточку (1 - да)?"
localization_input_inputFileName = "Введите имя файла:"

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
    return ('Primary_Side', 'Secondary_Side', 'Card_Level', 'Theme_Desc', 'Theme_Level', 'Account_Name')

def GetCreateTableScriptTuples():
    result = ()
    result += (('Themes', 'Theme_Id integer not null default 1 primary key, Theme_Desc text, Theme_Level text'), )
    result += (('Cards', 'Card_Id integer not null default 1 primary key, Primary_Side text, Secondary_Side text, Card_Level text'), )
    result += (('Accounts', 'Account_Id integer not null default 1 primary key, Account_Name text'), )
    result += (('ThemeCards', 'Theme_Id integer not null, Card_Id integer not null'), )
    result += (('AccountCards', 'Account_Id integer not null, Card_Id integer not null'), )
    result += (('Answers', 'Answer_Id integer not null default 1 primary key, BeginDateTime DateTime, EndDateTime DateTime, Card_Id integer, AnswerResult integer'), )
    return result

def GetSqlAllCards():
    return """Select Primary_side, Secondary_side, Card_Level, Theme_desc, Theme_Level, Account_Name from Cards
    left outer join ThemeCards on ThemeCards.Card_Id = Cards.Card_Id
    left outer join Themes on ThemeCards.Theme_Id = Themes.Theme_Id
    left outer join AccountCards on AccountCards.Card_Id = Cards.Card_Id
    left outer join Accounts on ThemeCards.Card_Id = AccountCards.Card_Id
                    and AccountCards.Account_Id = Accounts.Account_Id"""

#------------------------------------------------------------------------------

def CreateTables(tableScriptTuples, cursor):
    output = ()
    output += (localization_createAllTables, )
    for tableScriptTuple in tableScriptTuples:
        for line in CreateTable(tableScriptTuple, cursor):
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
    for row in GetRowsFromTable(sql_getTableColumnDescriptions.format(tableName), cursor):
        output += (row, )
    output += (localization_stars, )
    return output

def DropTables(tableNames, cursor):
    output = ()
    output += (localization_dropAllTables, )
    for tableName in tableNames:
        for line in DropTable(tableName, cursor):
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
        for line in ClearTable(tableName, cursor):
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
        for line in ShowTable(tableName, cursor):
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

def ShowCards(tableNames, cursor):
    output = ()
    if not HasAllTables(tableNames, cursor):
        lines = ShowInvalidAllTables(localization_cantShowCards)
        for line in lines:
            output += (line, )
    else:
        rows = GetRowsFromTable(GetSqlAllCards(), cursor)
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
        if input(localization_input_createCard) == "1":
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

def AddCards(inputArgs, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
    output = ()
    for inputArg in inputArgs:
        for line in AddCard(inputArg, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
            output += (line, )
    return output

def AddCard(inputArg, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
    output = ()
    primary_side = inputArg[0].strip()
    secondary_side = inputArg[1].strip()
    card_level = inputArg[2].strip()
    theme_desc = inputArg[3].strip()
    theme_level = inputArg[4].strip()
    account_name = inputArg[5].strip()
    output += (localization_addCurrentCard, )
    if not primary_side:
        output += (localization_emptyOneField.format("primary_side"), )
        output += (localization_cantAddCard, )
        for line in AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor):
            output += (line, )
        for line in AddAccount(account_name, hasUpdateAccount, cursor):
            output += (line, )
        output += (localization_stars)
        return output
    sql_where = "cards.primary_side like \'{}\'".format(primary_side)
    card_id_row = GetIdRow('card_id', 'cards', sql_where, cursor)
    if card_id_row:
        card_id = card_id_row[0]
        output += (localization_existCard.format(card_id, primary_side), )
        if hasUpdateCard:
            update_set = "primary_side = \'{}\', secondary_side = \'{}\', card_level = \'{}\'".format(primary_side, secondary_side, card_level)
            update_where = "card_id = {}".format(card_id)
            UpdateRow('cards', update_set, update_where, cursor)
            output += (localization_updateSuccessCard, )
            for line in AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor):
                output += (line, )
            for line in AddAccount(account_name, hasUpdateAccount, cursor):
                output += (line, )
            for line in AddThemeCard(card_id, theme_desc, theme_level, cursor):
                output += (line, )
            for line in AddAccountCard(card_id, account_name, cursor):
                output += (line, )
        else:
            output += (localization_ignoreAddCard, )
            for line in AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor):
                output += (line, )
            for line in AddAccount(account_name, hasUpdateAccount, cursor):
                output += (line, )
    else:
        card_id = CreateId('cards', cursor)
        values = "{}, \'{}\', \'{}\', \'{}\'".format(card_id, primary_side, secondary_side, card_level)
        AddRow('cards', values, cursor)
        output += (localization_addedCard.format(card_id, primary_side, secondary_side, card_level), )
        for line in AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor):
            output += (line, )
        for line in AddAccount(account_name, hasUpdateAccount, cursor):
            output += (line, )
        for line in AddThemeCard(card_id, theme_desc, theme_level, cursor):
            output += (line, )
        for line in AddAccountCard(card_id, account_name, cursor):
            output += (line, )
    output += (localization_stars, )
    return output

def AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor):
    output = ()
    output += (localization_addCurrentTheme, )
    if theme_desc == "" and theme_level == "":
        output += (localization_emptyTwoFields.format("theme_desc", "theme_level"), )
        output += (localization_cantAddTheme, )
        return output
    theme_id_row = GetIdRow('theme_id', 'themes', GetThemeIdWhere(theme_desc, theme_level), cursor)
    if theme_id_row:
        theme_id = theme_id_row[0]
        output += (localization_existThemeCard.format(theme_id, theme_desc, theme_level), )
        if hasUpdateTheme:
            UpdateRow('themes', GetThemeUpdateSet(theme_desc, theme_level), "theme_id = {}".format(theme_id), cursor)
            output += (localization_updateSuccessTheme, )
        else:
                output += (localization_ignoreAddTheme, )
    else:
        theme_id = CreateId('theme_id', cursor)
        AddRow('themes', GetThemeAddValues(theme_id, theme_desc, theme_level), cursor)
        output += (localization_addedTheme.format(theme_id, theme_desc, theme_level), )
    return output

def GetThemeAddValues(theme_id, theme_desc, theme_level):
    if not theme_desc:
        return "{}, \'{}\'".format(theme_id, theme_level)
    elif not theme_level:
        return "{}, \'{}\'".format(theme_id, theme_desc)
    else:
        return "{}, \'{}\', \'{}\'".format(theme_id, theme_desc, theme_level)

def GetThemeUpdateSet(theme_desc, theme_level):
    if not theme_desc:
        return "theme_level = \'{}\'".format(theme_level)
    elif not theme_level:
        return "theme_desc = \'{}\'".format(theme_desc)
    else:
        return "theme_desc = \'{}\', theme_level = \'{}\'".format(theme_desc, theme_level)

def GetThemeIdWhere(theme_desc, theme_level):
    if not theme_desc:
        return "themes.theme_desc is null and themes.theme_level = \'{}\'".format(theme_level)
    elif not theme_level:
        return "themes.theme_desc = \'{}\' and themes.theme_level is null".format(theme_desc)
    return "themes.theme_desc = \'{}\' or themes.theme_level = \'{}\'".format(theme_desc, theme_level)

def AddAccount(account_name, hasUpdateAccount, cursor):
    output = ()
    output += (localization_addCurrentAccount, )
    if not account_name:
        output += (localization_emptyOneField.format("account_name"), )
        output += (localization_cantAddAccount, )
        return output
    sql_where = "accounts.account_name = \'{}\'".format(account_name)
    account_id_row = GetIdRow('account_id', 'accounts', sql_where, cursor)
    if not account_id_row:
        account_id = CreateId('accounts', cursor)
        values = "{}, \'{}\', \'{}\', \'{}\'".format(account_id, account_name)
        AddRow('accounts', values, cursor)
        output += (localization_addedAccount.format(account_id, account_name), )
        return output
    account_id = account_id_row[0]
    output += (localization_existAccount.format(account_id, account_name), )
    if not hasUpdateAccount:
        output += (localization_ignoreAddAccount, )
        return output
    update_set = "account_name = \'{}\'".format(account_name)
    update_where = "account_id = {}".format(account_id)
    UpdateRow('accounts', update_set, update_where, cursor)
    output += (localization_updateSuccessAccount, )
    return output

def AddThemeCard(card_id, theme_desc, theme_level, cursor):
    output = ()
    if not theme_desc and not theme_level:
        return output
    theme_id_row = GetIdRow('theme_id', 'themes', GetThemeIdWhere(theme_desc, theme_level), cursor)
    if not theme_id_row:
        return output
    theme_id = theme_id_row[0]
    AddRow('ThemeCards', "{}, {}".format(theme_id, card_id), cursor)
    output += (localization_addedThemeCard.format(theme_id, card_id), )
    return output

def AddAccountCard(card_id, account_name, cursor):
    output = ()
    if not account_name:
        return output
    account_id_row = GetIdRow('account_id', 'accounts', "accounts.account_name = \'{}\'".format(account_name), cursor)
    if not account_id_row:
        return output
    account_id = account_id_row[0]
    AddRow('AccountCards', "{}, {}".format(account_id, card_id), cursor)
    output += (localization_addedAccountCard.format(account_id, card_id), )
    return output

def GetIdRow(sql_select, sql_from, sql_where, cursor):
    sql_getId = "Select {} from {} where {}"
    return GetCurrentRow(sql_getId.format(sql_select, sql_from, sql_where), cursor)

def CreateId(tableName, cursor):
    sql_getRowCount = "select COUNT(*) from {}"
    return GetCurrentRow(sql_getRowCount.format(tableName), cursor)[0]

def UpdateRow(tableName, set, where, cursor):
    sql_updateRow = "update {} set {} where {}"
    cursor.execute(sql_updateRow.format(tableName, set, where))

def AddRow(tableName, values, cursor):
    sql_insertRow = "insert into {} values({})"
    cursor.execute(sql_insertRow.format(tableName, values))

#-----------------------------------------------------------------------------

def HeaderImportCards():
    output = ()
    output += (localization_header_importCards, )
    return output

def InputFileName():
    return input(localization_input_inputFileName)

def InvalidFileName(file_name):
    output = ()
    output += (localization_import_invalidFileName.format(file_name), )
    return output

def InputImportCards(file_name):
    result = ()
    lines = GetLinesFromFile(file_name)
    if not lines:
        return result
    for line in lines:
        lineItems = line.split(',')
        primary_side = lineItems[0].strip()
        secondary_side = lineItems[1].strip()
        card_level = lineItems[2].strip()
        theme_desc = lineItems[3].strip()
        theme_level = lineItems[4].strip()
        account_name = lineItems[5].strip()
        result += ((primary_side, secondary_side, card_level, theme_desc, theme_level, account_name), )
    return result

def ImportCards(inputArgs, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
    output = ()
    if not inputArgs:
        output += (localization_import_nothingLinesCards, )
        return output
    for line in AddCards(inputArgs, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
        output += (line, )
    return output

def GetLinesFromFile(file_name):
    txt_file = open(file_name, 'r')
    lines = [line.strip() for line in txt_file]
    txt_file.close()
    return lines

#------------------------------------------------------------------------------

def ExportCards(cursor):
    print("Экспорт карточек в текстовый файл:")
    rows = GetRowsFromTable(GetSqlAllCards(), cursor)
    if not rows:
        print("Карточки отсутствуют.")
    else:
        print("База карточек :")
        print(GetCardColumnNames())
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
    print(localization_input_pressAnyKey)
    getch()

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
        key = getch()
        if key == b'1':
            ClearScreen()
            PrintLines(CreateTables(GetCreateTableScriptTuples(), cursor))
            PressAnyKey()
        if key == b'2':
            ClearScreen()
            PrintLines(DropTables(GetTableNames(), cursor))
            PressAnyKey()
        if key == b'3':
            ClearScreen()
            PrintLines(ClearTables(GetTableNames(), cursor))
            PressAnyKey()
        if key == b'4':
            ClearScreen()
            PrintLines(ShowTables(GetTableNames(), cursor))
            PressAnyKey()
        if key == b'5':
            ClearScreen()
            PrintLines(ShowCards(GetTableNames(), cursor))
            PressAnyKey()
        if key == b'6':
            ClearScreen()
            if HasAllTables(GetTableNames(), cursor):
                PrintLines(HeaderAddCards())
                PrintLines(AddCards(InputAddCards(), False, False, False, cursor))
            else:
                PrintLines(ShowInvalidAllTables(localization_cantAddCards))
            PressAnyKey()
        if key == b'7':
            ClearScreen()
            if HasAllTables(GetTableNames(), cursor):
                HeaderImportCards()
                fileName = InputFileName()
                if not exists(fileName):
                    PrintLines(InvalidFileName(fileName))
                else:
                    PrintLines(ImportCards(InputImportCards(fileName), False, False, False, cursor))
            else:
                PrintLines(ShowInvalidAllTables(localization_cantImportCards))
            PressAnyKey()
        if key == b'8':
            ClearScreen()
            if HasAllTables(GetTableNames(), cursor):
                ExportCards(cursor)
            else:
                PrintLines(ShowInvalidAllTables(localization_cantExportCards))
            PressAnyKey()
        if key == b'9':
            ClearScreen()
            PrintLines(CommitChanges(cursor))
            PressAnyKey()
        if key == b'a':
            ClearScreen()
            RunTesting(cursor)
            PressAnyKey()
        if (key == b'0'):
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
