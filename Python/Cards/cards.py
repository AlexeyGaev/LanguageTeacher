import pyodbc
import os
import msvcrt

from sys import argv
from os.path import exists
from msvcrt import getch

script, db_connection_string = argv

localization_stars = "======================================"

localization_testing_header = "Тестирование:"
localization_tempImposible = "Временно не поддерживается"

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
localization_headerTableColumnDescriptions = "Описание колонок таблицы:"

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

localization_import_input_fileName = "Введите имя файла:"
localization_import_nothingLinesCards = "Нет строк с карточками."
localization_import_invalidFileName = "Файл {} не существует."

localization_export_header = "Экспорт карточек в текстовый файл."
localization_export_allCards = "База карточек :"
localization_export_input_fileName = "Введите имя файла:"
localization_export_emptyFileName = "Введена пустая строка."
localization_export_createNewFile = "Создаем файл {}."
localization_export_contentFile = "Содержимое файла {} :"
localization_export_menu_header = "Выберите действие:"
localization_export_menu_rewrite = "Перезаписать (все прежние данные очищаются)"
localization_export_menu_add = "Добавить в конец"
localization_export_menu_update = "Обновить (одинаковые карточки не дублируются)"
localization_export_rewriteFile = "Перезаписываем файл {}."
localization_export_addToFile = "Добавляем в конец файла {}."
localization_export_updateFile ="Обновляем файл {}."

localization_input_pressAnyKey = "Нажмите любую клавишу..."
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

currentLog = []

def InitCurrentLog():
    currentLog.clear()

def AppendCurrentLog(line):
    currentLog.append(line)

def GetFirstCurrentRowValue(script, cursor):
    return GetCurrentRow(script, cursor)[0]

def GetLinesFromRows(rows, columnCount, delimeter):
    return [GetRowString(row, columnCount, delimeter) for row in rows]

def GetRowString(row, columnCount, delimeter):
    result = ""
    for i in range(columnCount):
        column = row[i]
        if column == 0:
            result += f"{column}"
        elif column == "0":
            result += f"{column}"
        elif column:
            result += f"{column}"
        if i < columnCount - 1:
            result += delimeter
    return result

def GetRowsFromTable(script, cursor):
    cursor.execute(script)
    return cursor.fetchall()

def GetCurrentRow(script, cursor):
    cursor.execute(script)
    return cursor.fetchone()

def GetColumnNames(cursor):
    return [i[0] for i in cursor.description]

def GetTableHeader(columnNames):
    result = ""
    columnCount = len(columnNames)
    for i in range(columnCount):
        result += f"{columnNames[i]}"
        if i < columnCount - 1:
            result += ", "
    return result

def HasAllTables(tableNames, cursor):
    for tableName in tableNames:
        sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
        if not GetCurrentRow(sql_getTableName.format(tableName), cursor):
            return False
    return True

def GetTableNames():
    return ('Themes', 'Cards', 'Accounts', 'ThemeCards', 'AccountCards', 'Answers')

def GetCardColumnNames():
    return "Primary_Side, Secondary_Side, Card_Level, Theme_Desc, Theme_Level, Account_Name"

def GetSqlAllCards():
    return """Select Primary_side, Secondary_side, Card_Level, Theme_desc, Theme_Level, Account_Name from Cards
    left outer join ThemeCards on ThemeCards.Card_Id = Cards.Card_Id
    left outer join Themes on ThemeCards.Theme_Id = Themes.Theme_Id
    left outer join AccountCards on AccountCards.Card_Id = Cards.Card_Id
    left outer join Accounts on ThemeCards.Card_Id = AccountCards.Card_Id
                    and AccountCards.Account_Id = Accounts.Account_Id"""

#---------------------------- Create Tables -----------------------------------

def GetCreateTableScriptTuples():
    result = ()
    result += (('Themes', 'Theme_Id integer not null default 1 primary key, Theme_Desc text, Theme_Level text'), )
    result += (('Cards', 'Card_Id integer not null default 1 primary key, Primary_Side text, Secondary_Side text, Card_Level text'), )
    result += (('Accounts', 'Account_Id integer not null default 1 primary key, Account_Name text'), )
    result += (('ThemeCards', 'Theme_Id integer not null, Card_Id integer not null'), )
    result += (('AccountCards', 'Account_Id integer not null, Card_Id integer not null'), )
    result += (('Answers', 'Answer_Id integer not null default 1 primary key, BeginDateTime DateTime, EndDateTime DateTime, Card_Id integer, AnswerResult integer'), )
    return result

def CreateTables(tableScriptTuples, cursor):
    AppendCurrentLog(localization_createAllTables)
    for tableScriptTuple in tableScriptTuples:
        CreateTable(tableScriptTuple, cursor)

def CreateTable(tableScriptTuple, cursor):
    tableName = tableScriptTuple[0]
    createColumns = tableScriptTuple[1]
    sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
    if GetCurrentRow(sql_getTableName.format(tableName), cursor):
        AppendCurrentLog(localization_existTable.format(tableName))
    else:
        AppendCurrentLog(localization_creatingTable.format(tableName))
        sql_createTable = "Create table {}({})"
        cursor.execute(sql_createTable.format(tableName, createColumns))
    sql_getTableColumnDescriptions = "select column_name, data_type from information_schema.columns where table_name like '{}' order by ordinal_position"
    AppendCurrentLog(localization_headerTableColumnDescriptions)
    for line in GetLinesFromRows(GetRowsFromTable(sql_getTableColumnDescriptions.format(tableName), cursor), 2, ", "):
        AppendCurrentLog(line)
    ShowTableCore(tableName, cursor)
    AppendCurrentLog(localization_stars)

#-------------------------------- DropTables ----------------------------------

def DropTables(tableNames, cursor):
    AppendCurrentLog(localization_dropAllTables)
    for tableName in tableNames:
        DropTable(tableName, cursor)
    AppendCurrentLog(localization_stars)

def DropTable(tableName, cursor):
    sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
    if not GetCurrentRow(sql_getTableName.format(tableName), cursor):
        AppendCurrentLog(localization_nothingTable.format(tableName))
        return
    AppendCurrentLog(localization_dropTable.format(tableName))
    cursor.execute("drop table {}".format(tableName))

#------------------------------- Clear Tables ---------------------------------

def ClearTables(tableNames, cursor):
    AppendCurrentLog(localization_deleteAllTables)
    for tableName in tableNames:
        ClearTable(tableName, cursor)
    AppendCurrentLog(localization_stars)

def ClearTable(tableName, cursor):
    sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
    if not GetCurrentRow(sql_getTableName.format(tableName), cursor):
        AppendCurrentLog(localization_nothingTable.format(tableName))
        return
    AppendCurrentLog(localization_deleteTable.format(tableName))
    cursor.execute("delete from {}".format(tableName))

#------------------------------- Show Tables ----------------------------------

def ShowTables(tableNames, cursor):
    AppendCurrentLog(localization_showTables)
    for tableName in tableNames:
        ShowTable(tableName, cursor)
        AppendCurrentLog(localization_stars)

def ShowTable(tableName, cursor):
    sql_getTableName = "select table_name from information_schema.tables where table_name = '{}'"
    if not GetCurrentRow(sql_getTableName.format(tableName), cursor):
        AppendCurrentLog(localization_nothingTable.format(tableName))
        return
    ShowTableCore(tableName, cursor)

def ShowTableCore(tableName, cursor):
    sql_select = "select * from {}"
    rows = GetRowsFromTable(sql_select.format(tableName), cursor)
    if not rows:
        AppendCurrentLog(localization_emptyTable.format(tableName))
        return
    AppendCurrentLog(localization_showTable.format(tableName))
    columnNames = GetColumnNames(cursor)
    AppendCurrentLog(GetTableHeader(columnNames))
    lines = GetLinesFromRows(rows, len(columnNames), ", ")
    for line in lines:
        AppendCurrentLog(line)

#------------------------------ Show Cards ------------------------------------

def ShowCards(tableNames, cursor):
    if not HasAllTables(tableNames, cursor):
        AppendCurrentLog(localization_invalidAllTables)
        AppendCurrentLog(localization_cantShowCards)
        AppendCurrentLog(localization_stars)
        return
    rows = GetRowsFromTable(GetSqlAllCards(), cursor)
    if not rows:
        AppendCurrentLog(localization_nothingCardTable)
        AppendCurrentLog(localization_stars)
        return
    AppendCurrentLog(localization_cardTable)
    columnNames = GetColumnNames(cursor)
    AppendCurrentLog(GetTableHeader(columnNames))
    lines = GetLinesFromRows(rows, len(columnNames), ", ")
    for line in lines:
        AppendCurrentLog(line)
    AppendCurrentLog(localization_stars)

#------------------------------ Add Cards -------------------------------------

def AddCards(tableNames, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
    if not HasAllTables(tableNames, cursor):
        AppendCurrentLog(localization_invalidAllTables)
        AppendCurrentLog(localization_cantAddCards)
        AppendCurrentLog(localization_stars)
        return
    AppendCurrentLog(localization_addCards)
    AppendCurrentLog(localization_inputAccountAndTheme)
    for inputArg in InputAddCards():
        AddCard(inputArg, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor)

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

def AddCard(inputArg, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
    primary_side = inputArg[0].strip()
    secondary_side = inputArg[1].strip()
    card_level = inputArg[2].strip()
    theme_desc = inputArg[3].strip()
    theme_level = inputArg[4].strip()
    account_name = inputArg[5].strip()
    AppendCurrentLog(localization_addCurrentCard)
    if not primary_side:
        AppendCurrentLog(localization_emptyOneField.format("primary_side"))
        AppendCurrentLog(localization_cantAddCard)
        AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor)
        AddAccount(account_name, hasUpdateAccount, cursor)
        AppendCurrentLog(localization_stars)
        return
    sql_where = "cards.primary_side like \'{}\'".format(primary_side)
    card_id_row = GetIdRow('card_id', 'cards', sql_where, cursor)
    if card_id_row:
        card_id = card_id_row[0]
        AppendCurrentLog(localization_existCard.format(card_id, primary_side))
        if hasUpdateCard:
            update_set = "primary_side = \'{}\', secondary_side = \'{}\', card_level = \'{}\'".format(primary_side, secondary_side, card_level)
            update_where = "card_id = {}".format(card_id)
            UpdateRow('cards', update_set, update_where, cursor)
            AppendCurrentLog(localization_updateSuccessCard)
            theme_id = AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor)
            account_id = AddAccount(account_name, hasUpdateAccount, cursor)
            AddThemeCard(theme_id, card_id, cursor)
            AddAccountCard(account_id, card_id, cursor)
        else:
            AppendCurrentLog(localization_ignoreAddCard)
            AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor)
            AddAccount(account_name, hasUpdateAccount, cursor)
    else:
        card_id = CreateId('cards', cursor)
        values = "{}, \'{}\', \'{}\', \'{}\'".format(card_id, primary_side, secondary_side, card_level)
        AddRow('cards', values, cursor)
        AppendCurrentLog(localization_addedCard.format(card_id, primary_side, secondary_side, card_level))
        theme_id = AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor)
        account_id = AddAccount(account_name, hasUpdateAccount, cursor)
        AddThemeCard(theme_id, card_id, cursor)
        AddAccountCard(account_id, card_id, cursor)
    AppendCurrentLog(localization_stars)

def AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor):
    AppendCurrentLog(localization_addCurrentTheme)
    if theme_desc == "" and theme_level == "":
        AppendCurrentLog(localization_emptyTwoFields.format("theme_desc", "theme_level"))
        AppendCurrentLog(localization_cantAddTheme)
        return -1
    theme_id_row = GetIdRow('theme_id', 'themes', GetThemeIdWhere(theme_desc, theme_level), cursor)
    if not theme_id_row:
        theme_id = CreateId('theme_id', cursor)
        AddRow('themes', GetThemeAddValues(theme_id, theme_desc, theme_level), cursor)
        AppendCurrentLog(localization_addedTheme.format(theme_id, theme_desc, theme_level))
        return theme_id
    theme_id = theme_id_row[0]
    AppendCurrentLog(localization_existThemeCard.format(theme_id, theme_desc, theme_level))
    if hasUpdateTheme:
        UpdateRow('themes', GetThemeUpdateSet(theme_desc, theme_level), "theme_id = {}".format(theme_id), cursor)
        AppendCurrentLog(localization_updateSuccessTheme)
    else:
        AppendCurrentLog(localization_ignoreAddTheme)
    return theme_id

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

def AddAccount(account_name, hasUpdateAccount, cursor):
    AppendCurrentLog(localization_addCurrentAccount)
    if not account_name:
        AppendCurrentLog(localization_emptyOneField.format("account_name"))
        AppendCurrentLog(localization_cantAddAccount)
        return -1
    sql_where = "accounts.account_name = \'{}\'".format(account_name)
    account_id_row = GetIdRow('account_id', 'accounts', sql_where, cursor)
    if not account_id_row:
        account_id = CreateId('accounts', cursor)
        values = "{}, \'{}\', \'{}\', \'{}\'".format(account_id, account_name)
        AddRow('accounts', values, cursor)
        AppendCurrentLog(localization_addedAccount.format(account_id, account_name))
        return account_id
    account_id = account_id_row[0]
    AppendCurrentLog(localization_existAccount.format(account_id, account_name))
    if hasUpdateAccount:
        UpdateRow('accounts', "account_name = \'{}\'".format(account_name), "account_id = {}".format(account_id), cursor)
        AppendCurrentLog(localization_updateSuccessAccount)
    else:
        AppendCurrentLog(localization_ignoreAddAccount)
    return account_id

def AddThemeCard(theme_id, card_id, cursor):
    if theme_id == -1:
        return
    AddRow('ThemeCards', "{}, {}".format(theme_id, card_id), cursor)
    AppendCurrentLog(localization_addedThemeCard.format(theme_id, card_id))

def AddAccountCard(account_id, card_id, cursor):
    if account_id == -1:
        return
    AddRow('AccountCards', "{}, {}".format(account_id, card_id), cursor)
    AppendCurrentLog(localization_addedAccountCard.format(account_id, card_id))

def GetIdRow(sql_select, sql_from, sql_where, cursor):
    return GetCurrentRow("Select {} from {} where {}".format(sql_select, sql_from, sql_where), cursor)

def CreateId(tableName, cursor):
    return GetCurrentRow("select COUNT(*) from {}".format(tableName), cursor)[0]

def UpdateRow(tableName, set, where, cursor):
    cursor.execute("update {} set {} where {}".format(tableName, set, where))

def AddRow(tableName, values, cursor):
    cursor.execute("insert into {} values({})".format(tableName, values))

#----------------------------- Import Cards -----------------------------------

def InputCards(fileName, tableNames, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
    if not HasAllTables(tableNames, cursor):
        AppendCurrentLog(localization_invalidAllTables)
        AppendCurrentLog(localization_cantImportCards)
        AppendCurrentLog(localization_stars)
        return
    AppendCurrentLog(localization_header_importCards)
    fileName = fileName.strip()
    if not fileName:
        AppendCurrentLog(localization_export_emptyFileName)
        AppendCurrentLog(localization_cantImportCards)
        AppendCurrentLog(localization_stars)
        return
    if not exists(fileName):
        AppendCurrentLog(localization_import_invalidFileName.format(file_name))
        AppendCurrentLog(localization_cantImportCards)
        AppendCurrentLog(localization_stars)
        return
    inputArgs = InputImportCards(fileName)
    if not inputArgs:
        AppendCurrentLog(localization_import_nothingLinesCards)
        AppendCurrentLog(localization_cantImportCards)
        AppendCurrentLog(localization_stars)
        return
    for inputArg in inputArgs:
        AddCard(inputArg, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor)

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

def GetLinesFromFile(file_name):
    txt_file = open(file_name, 'r')
    lines = [line.strip() for line in txt_file]
    txt_file.close()
    return lines

#----------------------------- Export Cards -----------------------------------

def ExportCards(fileName, tableNames, exportType, cursor):
    if not HasAllTables(tableNames, cursor):
        AppendCurrentLog(localization_invalidAllTables)
        AppendCurrentLog(localization_cantExportCards)
        AppendCurrentLog(localization_stars)
        return
    AppendCurrentLog(localization_export_header)
    rows = GetRowsFromTable(GetSqlAllCards(), cursor)
    if not rows:
        AppendCurrentLog(localization_nothingCardTable)
        AppendCurrentLog(localization_cantExportCards)
        AppendCurrentLog(localization_stars)
        return
    AppendCurrentLog(localization_export_allCards)
    AppendCurrentLog(GetCardColumnNames())
    linesFromRows = GetLinesFromRows(rows, 6, ", ")
    for line in linesFromRows:
        AppendCurrentLog(line)
    fileName = fileName.strip()
    if not fileName:
        AppendCurrentLog(localization_export_emptyFileName)
        AppendCurrentLog(localization_cantExportCards)
        AppendCurrentLog(localization_stars)
        return
    if not exists(fileName):
        AppendCurrentLog(localization_export_createNewFile.format(fileName))
        ExportToNewTxtFile(fileName, linesFromRows)
        ShowTextFile(fileName);
        AppendCurrentLog(localization_stars)
        return
    AppendCurrentLog(localization_export_contentFile.format(fileName))
    linesFromFile = GetLinesFromFile(fileName)
    if exportType == 1:
        AppendCurrentLog(localization_export_rewriteFile.format(fileName))
        ExportToNewTxtFile(fileName, linesFromRows)
    if exportType == 2:
        AppendCurrentLog(localization_export_addToFile.format(fileName))
        ExportToEndTxtFile(fileName, linesFromRows)
    if exportType == 3:
        AppendCurrentLog(localization_export_updateFile.format(fileName))
        ExportToNewTxtFile(fileName, JoinLines(linesFromFile, linesFromRows, 6, ','))
    ShowTextFile(fileName);
    AppendCurrentLog(localization_stars)

def ExportToNewTxtFile(file_name, lines):
    txt_file = open(file_name, 'w')
    for line in lines:
        txt_file.write(line + "\n")
    txt_file.close()

def ExportToEndTxtFile(file_name, lines):
    txt_file = open(file_name, 'a')
    for line in lines:
        txt_file.write(line + "\n")
    txt_file.close()

def JoinLines(targetLines, addedLines, columnCount, delimeter):
    result = ()
    for targetLine in targetLines:
        result += (targetLine, )
    for addedLine in addedLines:
        if not Contains(targetLines, addedLine, columnCount, delimeter):
            result += (addedLine, )
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

def ShowTextFile(file_name):
    AppendCurrentLog(localization_export_contentFile.format(file_name))
    for line in GetLinesFromFile(file_name):
        AppendCurrentLog(line)

#----------------------------- Commit Changes ---------------------------------

def CommitChanges(cursor):
    AppendCurrentLog(localization_dataBaseBeginChanges)
    cursor.commit()
    AppendCurrentLog(localization_dataBaseEndChanges)
    AppendCurrentLog(localization_stars)

#----------------------------- Run Testing ------------------------------------

def RunTesting(cursor):
    AppendCurrentLog(localization_testing_header)
    AppendCurrentLog(localization_tempImposible)
    AppendCurrentLog(localization_stars)

#----------------------------- Menu and Utils ---------------------------------

def ClearScreen():
    os.system('cls' if os.name=='nt' else 'clear')

def PrintLines(lines):
    [print(line) for line in lines]

def CreateMainMenu():
    result = []
    result.append(localization_menu_header)
    result.append(localization_stars)
    result.append("1 - " + localization_menu_createNewTables)
    result.append("2 - " + localization_menu_dropTables)
    result.append("3 - " + localization_menu_deleteTables)
    result.append("4 - " + localization_menu_showTables)
    result.append("5 - " + localization_menu_showCards)
    result.append("6 - " + localization_menu_addCards)
    result.append("7 - " + localization_menu_importCards)
    result.append("8 - " + localization_menu_exportCards)
    result.append("9 - " + localization_menu_changeDataBase)
    result.append("a - " + localization_menu_testing)
    result.append("0 - " + localization_menu_exit)
    return result

def GetMainMenuActionType(key):
    if key == b'0':
        return 0
    if key == b'1':
        return 1
    if key == b'2':
        return 2
    if key == b'3':
        return 3
    if key == b'4':
        return 4
    if key == b'5':
        return 5
    if key == b'6':
        return 6
    if key == b'7':
        return 7
    if key == b'8':
        return 8
    if key == b'9':
        return 9
    if key == b'a':
        return 10
    return -1

def CreateExportMenu():
    result = []
    result.append(localization_export_menu_header)
    result.append("1 - " + localization_export_menu_rewrite)
    result.append("2 - " + localization_export_menu_add)
    result.append("3 - " + localization_export_menu_update)
    return result

def GetExportType(key):
    if key == b'1':
        return 1
    if key == b'2':
        return 2
    return 3

def MainMenu(cursor):
    mainMenu = CreateMainMenu()
    tableNames = GetTableNames()
    createTablesScriptTuples = GetCreateTableScriptTuples()
    exportMenu = CreateExportMenu()
    while True:
        ClearScreen()
        PrintLines(mainMenu)
        actionType = GetMainMenuActionType(getch())
        if actionType == 0:
            break
        if actionType == 1:
            ClearScreen()
            InitCurrentLog()
            CreateTables(createTablesScriptTuples, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 2:
            ClearScreen()
            InitCurrentLog()
            DropTables(tableNames, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 3:
            ClearScreen()
            InitCurrentLog()
            ClearTables(tableNames, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 4:
            ClearScreen()
            InitCurrentLog()
            ShowTables(tableNames, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 5:
            ClearScreen()
            InitCurrentLog()
            ShowCards(tableNames, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 6:
            ClearScreen()
            InitCurrentLog()
            AddCards(tableNames, False, False, False, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 7:
            ClearScreen()
            InitCurrentLog()
            fileName = input(localization_import_input_fileName)
            ImportCards(fileName, tableNames, False, False, False, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 8:
            ClearScreen()
            InitCurrentLog()
            fileName = input(localization_export_input_fileName)
            PrintLines(exportMenu)
            exportType = GetExportType(getch())
            ExportCards(fileName, tableNames, exportType, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 9:
            ClearScreen()
            InitCurrentLog()
            CommitChanges(cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 10:
            ClearScreen()
            InitCurrentLog()
            RunTesting(cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()

#-------------------------------- Main -----------------------------------------

try:
    db_connection = pyodbc.connect(db_connection_string)
except:
    ClearScreen()
    InitCurrentLog()
    AppendCurrentLog(localization_except_main)
    AppendCurrentLog(localization_input_pressAnyKey)
    PrintLines(currentLog)
    getch()
else:
    db_cursor = db_connection.cursor()
    MainMenu(db_cursor)
    db_cursor.close()
    db_connection.close()
