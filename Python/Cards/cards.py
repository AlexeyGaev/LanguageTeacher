import pyodbc
import os
import msvcrt

from sys import argv, exc_info
from os.path import exists
from msvcrt import getch

script, db_connection_string = argv

localization_stars = "======================================"
localization_tempImposible = "Временно не поддерживается"

localization_createAllTables = "Создаем все новые таблицы."
localization_dropAllTables = "Удаляем все существующие таблицы."
localization_deleteAllTables = "Очищаем все существующие таблицы."
localization_showTables = "Содержимое таблиц :"
localization_cardTable = "Таблица карточек :"
localization_header_importCards = "Импорт карточек из текстового файла."
localization_dataBaseBeginChanges = "Отправляем изменения в базу данных."
localization_header_testing = "Тестирование:"

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
localization_except_invalidTable = "Проблема с доступом к таблице."

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

def ExceptError(cursor, e):
    if e.args[0] == '42S01':
        tableName = GetTableNameString(GetTableNames(), e.args[1])
        if tableName:
            AppendCurrentLog(localization_existTable.format(tableName))
            ShowTable(tableName, cursor)
        else:
            AppendCurrentLog(localization_except_invalidTable)
    elif e.args[0] == '42S02':
        tableName = GetTableNameString(GetTableNames(), e.args[1])
        if tableName:
            AppendCurrentLog(localization_nothingTable.format(tableName))
        else:
            AppendCurrentLog(localization_except_invalidTable)
    else:
        AppendCurrentLog(e.args[0])

def GetTableNameString(tableNames, errorDecription):
    for tableName in tableNames:
        position = errorDecription.find(tableName)
        if position > 0 and errorDecription[position - 1] == "\'" and errorDecription[position + len(tableName)] == "\'":
            return tableName
    return ""

def InitCurrentLog():
    currentLog.clear()

def AppendCurrentLog(line):
    currentLog.append(line)

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

def GetColumnNames(cursor):
    return [f"{i[0]} ({i[1].__name__})" for i in cursor.description]

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
        cursor.execute("select table_name from information_schema.tables where table_name = '{}'".format(tableName))
        if not cursor.fetchone():
            return False
    return True

def GetTableNames():
    return ['Themes', 'Cards', 'Accounts', 'ThemeCards', 'AccountCards', 'Answers']

def GetCardColumnNames():
    return "Primary_Side, Secondary_Side, Card_Level, Theme_Desc, Theme_Level, Account_Name"

def GetSqlAllCards():
    return """Select Primary_side, Secondary_side, Card_Level, Theme_desc, Theme_Level, Account_Name from Cards
    left outer join ThemeCards on ThemeCards.Card_Id = Cards.Card_Id
    left outer join Themes on ThemeCards.Theme_Id = Themes.Theme_Id
    left outer join AccountCards on AccountCards.Card_Id = Cards.Card_Id
    left outer join Accounts on ThemeCards.Card_Id = AccountCards.Card_Id
                    and AccountCards.Account_Id = Accounts.Account_Id"""

def GetCreateTableColumns():
    return ['Theme_Id integer not null default 1 primary key, Theme_Desc text, Theme_Level text',
     'Card_Id integer not null default 1 primary key, Primary_Side text, Secondary_Side text, Card_Level text',
     'Account_Id integer not null default 1 primary key, Account_Name text',
     'Theme_Id integer not null, Card_Id integer not null',
     'Account_Id integer not null, Card_Id integer not null',
     'Answer_Id integer not null default 1 primary key, BeginDateTime DateTime, EndDateTime DateTime, Card_Id integer, AnswerResult integer']

def GetCreateTableScripts(tableNames, tableColumnsNames):
    result = []
    for i in range(len(tableNames)):
        result.append((tableNames[i], tableColumnsNames[i]))
    return result

#---------------------------- Create Tables -----------------------------------

def CreateTables(tableScripts, cursor):
    AppendCurrentLog(localization_createAllTables)
    for tableScript in tableScripts:
        tableName = tableScript[0]
        try:
            cursor.execute("Create table {}({})".format(tableName, tableScript[1]))
        except Exception as e:
            ExceptError(cursor, e)
        else:
            AppendCurrentLog(localization_creatingTable.format(tableName))
            ShowTable(tableName, cursor)
        AppendCurrentLog(localization_stars)

#-------------------------------- DropTables ----------------------------------

def DropTables(tableNames, cursor):
    AppendCurrentLog(localization_dropAllTables)
    for tableName in tableNames:
        try:
            cursor.execute("drop table {}".format(tableName))
        except Exception as e:
            ExceptError(cursor, e)
        else:
            AppendCurrentLog(localization_dropTable.format(tableName))
    AppendCurrentLog(localization_stars)

#------------------------------- Clear Tables ---------------------------------

def ClearTables(tableNames, cursor):
    AppendCurrentLog(localization_deleteAllTables)
    for tableName in tableNames:
        try:
            cursor.execute("delete from {}".format(tableName))
        except Exception as e:
            ExceptError(cursor, e)
        else:
            AppendCurrentLog(localization_deleteTable.format(tableName))
    AppendCurrentLog(localization_stars)

#------------------------------- Show Tables ----------------------------------

def ShowTables(tableNames, cursor):
    AppendCurrentLog(localization_showTables)
    for tableName in tableNames:
        ShowTable(tableName, cursor)
        AppendCurrentLog(localization_stars)

def ShowTable(tableName, cursor):
    try:
        cursor.execute("select * from {}".format(tableName))
    except Exception as e:
        ExceptError(cursor, e)
    else:
        rows = cursor.fetchall()
        if not rows:
            AppendCurrentLog(GetTableHeader(GetColumnNames(cursor)))
            AppendCurrentLog(localization_emptyTable.format(tableName))
            return
        AppendCurrentLog(localization_showTable.format(tableName))
        ShowQueryTable(rows, cursor)

def ShowQueryTable(rows, cursor):
    columnNames = GetColumnNames(cursor)
    AppendCurrentLog(GetTableHeader(columnNames))
    for line in GetLinesFromRows(rows, len(columnNames), ", "):
        AppendCurrentLog(line)

#------------------------------ Show Cards ------------------------------------

def ShowCards(tableNames, cursor):
    AppendCurrentLog(localization_cardTable)
    try:
        cursor.execute(GetSqlAllCards())
    except Exception as e:
        ExceptError(cursor, e)
    else:
        rows = cursor.fetchall()
        if not rows:
            AppendCurrentLog(localization_nothingCardTable)
            AppendCurrentLog(localization_stars)
            return
        ShowQueryTable(rows, cursor)
    AppendCurrentLog(localization_stars)

#------------------------------ Add Cards -------------------------------------

def AddCards(tableNames, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
    AppendCurrentLog(localization_addCards)
    AppendCurrentLog(localization_inputAccountAndTheme)
    for inputArg in InputAddCards():
        AddCard(inputArg, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor)
        AppendCurrentLog(localization_stars)

def InputAddCards():
    result = []
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
            result.append((primary_side, secondary_side, card_level, theme_desc, theme_level, account_name))
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
        return
    try:
        cursor.execute("Select Card_Id from Cards where Cards.Primary_Side like \'{}\'".format(primary_side))
    except Exception as e:
        ExceptError(cursor, e)
        AppendCurrentLog(localization_cantAddCard)
        AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor)
        AddAccount(account_name, hasUpdateAccount, cursor)
    else:
        card_id_row = cursor.fetchone()
        if card_id_row:
            card_id = card_id_row[0]
            AppendCurrentLog(localization_existCard.format(card_id, primary_side))
            if hasUpdateCard:
                try:
                    cursor.execute("update Cards set Primary_Side = \'{}\', Secondary_Side = \'{}\', Card_Level = \'{}\' where card_id = {}".format(primary_side, secondary_side, card_level, card_id))
                except Exception as e:
                    ExceptError(cursor, e)
                    AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor)
                    AddAccount(account_name, hasUpdateAccount, cursor)
                else:
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
            try:
                cursor.execute("select COUNT(*) from Cards")
            except Exception as e:
                ExceptError(cursor, e)
                AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor)
                AddAccount(account_name, hasUpdateAccount, cursor)
            else:
                card_id = cursor.fetchone()[0]
                try:
                    cursor.execute("Insert into Cards values({}, \'{}\', \'{}\', \'{}\')".format(card_id, primary_side, secondary_side, card_level))
                except Exception as e:
                    ExceptError(cursor, e)
                    AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor)
                    AddAccount(account_name, hasUpdateAccount, cursor)
                else:
                    AppendCurrentLog(localization_addedCard.format(card_id, primary_side, secondary_side, card_level))
                    theme_id = AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor)
                    account_id = AddAccount(account_name, hasUpdateAccount, cursor)
                    AddThemeCard(theme_id, card_id, cursor)
                    AddAccountCard(account_id, card_id, cursor)

def AddTheme(theme_desc, theme_level, hasUpdateTheme, cursor):
    AppendCurrentLog(localization_addCurrentTheme)
    if theme_desc == "" and theme_level == "":
        AppendCurrentLog(localization_emptyTwoFields.format("theme_desc", "theme_level"))
        AppendCurrentLog(localization_cantAddTheme)
        return -1
    try:
        cursor.execute("Select Theme_Id from Themes where {}".format(GetThemeIdWhere(theme_desc, theme_level)))
    except Exception as e:
        ExceptError(cursor, e)
        return -1
    else:
        theme_id_row = cursor.fetchone()
        if not theme_id_row:
            try:
                cursor.execute("Select COUNT(*) from Themes")
            except Exception as e:
                ExceptError(cursor, e)
                return -1
            else:
                theme_id = cursor.fetchone()[0]
                try:
                    cursor.execute("Insert into Themes values({})".format(GetThemeAddValues(theme_id, theme_desc, theme_level)))
                except Exception as e:
                    ExceptError(cursor, e)
                    return -1
                else:
                    AppendCurrentLog(localization_addedTheme.format(theme_id, theme_desc, theme_level))
                    return theme_id
        theme_id = theme_id_row[0]
        AppendCurrentLog(localization_existThemeCard.format(theme_id, theme_desc, theme_level))
        if hasUpdateTheme:
            try:
                cursor.execute("Update Themes set {} where Theme_Id = {}".format(GetThemeUpdateSet(theme_desc, theme_level), theme_id))
            except Exception as e:
                ExceptError(cursor, e)
                return -1
            else:
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
    try:
        cursor.execute("Select Account_Id from Accounts where Accounts.Account_Name = \'{}\'".format(account_name))
    except Exception as e:
        ExceptError(cursor, e)
        return -1
    else:
        account_id_row = cursor.fetchone()
        if not account_id_row:
            try:
                cursor.execute("select COUNT(*) from Accounts")
            except Exception as e:
                ExceptError(cursor, e)
                return -1
            else:
                account_id = cursor.fetchone()[0]
                try:
                    cursor.execute("Insert into Accounts values({}, \'{}\')".format(account_id, account_name))
                except Exception as e:
                    ExceptError(cursor, e)
                    return -1
                else:
                    AppendCurrentLog(localization_addedAccount.format(account_id, account_name))
                    return account_id
        account_id = account_id_row[0]
        AppendCurrentLog(localization_existAccount.format(account_id, account_name))
        if hasUpdateAccount:
            try:
                cursor.execute("Update Accounts set Account_Name = \'{}\' where account_id = {}".format(account_name, account_id))
            except Exception as e:
                ExceptError(cursor, e)
                return -1
            else:
                AppendCurrentLog(localization_updateSuccessAccount)
        else:
            AppendCurrentLog(localization_ignoreAddAccount)
        return account_id

def AddThemeCard(theme_id, card_id, cursor):
    if theme_id == -1 or card_id == -1:
        return
    try:
        cursor.execute("insert into ThemeCards values({}, {})".format(theme_id, card_id))
    except Exception as e:
        ExceptError(cursor, e)
    else:
        AppendCurrentLog(localization_addedThemeCard.format(theme_id, card_id))

def AddAccountCard(account_id, card_id, cursor):
    if account_id == -1 or card_id == -1:
        return
    try:
        cursor.execute("Insert into AccountCards values({}, {})".format(account_id, card_id))
    except Exception as e:
        ExceptError(cursor, e)
    else:
        AppendCurrentLog(localization_addedAccountCard.format(account_id, card_id))

#----------------------------- Import Cards -----------------------------------

def ImportCards(fileName, tableNames, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor):
    AppendCurrentLog(localization_header_importCards)
    fileName = fileName.strip()
    if not fileName:
        AppendCurrentLog(localization_export_emptyFileName)
        AppendCurrentLog(localization_cantImportCards)
        AppendCurrentLog(localization_stars)
        return
    if not exists(fileName):
        AppendCurrentLog(localization_import_invalidFileName.format(fileName))
        AppendCurrentLog(localization_cantImportCards)
        AppendCurrentLog(localization_stars)
        return
    linesFromFile = GetLinesFromFile(fileName)
    if not linesFromFile:
        AppendCurrentLog(localization_import_nothingLinesCards)
        AppendCurrentLog(localization_cantImportCards)
        AppendCurrentLog(localization_stars)
        return
    inputArgs = InputImportCards(linesFromFile)
    if not inputArgs:
        AppendCurrentLog(localization_import_nothingLinesCards)
        AppendCurrentLog(localization_cantImportCards)
        AppendCurrentLog(localization_stars)
        return
    for inputArg in inputArgs:
        AddCard(inputArg, hasUpdateCard, hasUpdateTheme, hasUpdateAccount, cursor)
        AppendCurrentLog(localization_stars)

def InputImportCards(linesFromFile):
    result = []
    for line in linesFromFile:
        lineItems = line.split(',')
        primary_side = lineItems[0].strip()
        secondary_side = lineItems[1].strip()
        card_level = lineItems[2].strip()
        theme_desc = lineItems[3].strip()
        theme_level = lineItems[4].strip()
        account_name = lineItems[5].strip()
        result.append((primary_side, secondary_side, card_level, theme_desc, theme_level, account_name))
    return result

def GetLinesFromFile(file_name):
    txt_file = open(file_name, 'r')
    lines = [line.strip() for line in txt_file]
    txt_file.close()
    return lines

#----------------------------- Export Cards -----------------------------------

def ExportCards(fileName, tableNames, exportType, cursor):
    AppendCurrentLog(localization_export_header)
    try:
        cursor.execute(GetSqlAllCards())
    except Exception as e:
        ExceptError(cursor, e)
        AppendCurrentLog(localization_cantExportCards)
        AppendCurrentLog(localization_stars)
    else:
        rows = cursor.fetchall()
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
    AppendCurrentLog(localization_header_testing)
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
    createTablesScripts = GetCreateTableScripts(tableNames, GetCreateTableColumns())
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
            CreateTables(createTablesScripts, cursor)
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
