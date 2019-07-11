import pyodbc
import os
import msvcrt
import operator
import itertools

from sys import argv, exc_info
from os.path import exists
from msvcrt import getch
from operator import itemgetter, attrgetter, methodcaller
from itertools import groupby

script, db_connection_string = argv

currentLog = []

# ------------ SQL Scripts / Localization (TODO : move to txt file) -----------

localization_except_main = "Не могу связаться к базой данных."
localization_except_existTable = "Таблица {} существует."
localization_except_nothingTable = "Таблица {} не существует."
localization_except_invalidTable = "Проблема с доступом к таблице."

localization_header_showTables = "Вывод содержимого таблиц."
localization_header_cardTable = "Вывод базы карточек."

localization_header_addCards = "Добавляем карточки в таблицы."
localization_header_importCards = "Импорт карточек из текстового файла."
localization_header_exportCards = "Экспорт карточек в текстовый файл."
localization_header_applyChanges = "Отправляем изменения в базу данных."
localization_header_testing = "Тестирование."

localization_showTables_notEmptyTable = "Содержимое таблицы {}:"
localization_showTables_emptyTable = "Таблица {} пустая."
localization_showCards_emptyTable = "Таблица карточек пустая."
localization_showCards_notEmptyTable = "Таблица карточек :"

localization_addCards_input_createAccount = "Создать пользователя (1 - да)?"
localization_addCards_input_createTheme = "Создать тему (1 - да)?"
localization_addCards_input_createCard = "Создать карточку (1 - да)?"
localization_addCards_addCard = "Добавляю текущую карточку."
localization_addCards_addTheme = "Добавляю текущую тему."
localization_addCards_addAccount = "Добавляю текущего пользователя."
localization_addCards_cantAddCard = "Добавить карточку не могу."
localization_addCards_cantAddTheme = "Добавить тему не могу."
localization_addCards_cantAddAccount = "Добавить пользователя не могу."
localization_addCards_cantUpdateCard = "Обновить карточку не могу."
localization_addCards_cantUpdateTheme = "Обновить тему не могу."
localization_addCards_cantUpdateAccount = "Обновить пользователя не могу."
localization_addCards_updateSuccessCard = "Карточка успешно обновлена."
localization_addCards_updateSuccessTheme = "Тема успешно обновлена."
localization_addCards_updateSuccessAccount = "Пользователь успешно обновлен."
localization_addCards_ignoreAddCard = "Карточка пропущена."
localization_addCards_ignoreAddTheme = "Тема пропущена."
localization_addCards_ignoreAddAccount = "Пользователь пропущен."
localization_addCards_existCard = "Имеется карточка с id = {} primary_side = {}."
localization_addCards_existTheme = "Имеется тема с id = {} desc = \'{}\' или level = \'{}\'."
localization_addCards_existAccount = "Имеется пользователь с id = {} name = {}."
localization_addCards_addedAccount = "Пользователь (id = {}, name = {}) добавлен."
localization_addCards_addedTheme = "Тема (id = {}, Desc = {}, Level = {}) добавлена."
localization_addCards_addedCard = "Карточка (id = {}, PrimarySide = {}, SecondarySide = {}, Level = {}) добавлена."
localization_addCards_addedThemeCard = "Связь (theme_id = {}, card_id = {}) добавлена."
localization_addCards_addedAccountCard = "Связь (account_id = {}, card_id = {}) добавлена."
localization_addCards_emptyOneField = "Поле {} пустое."
localization_addCards_emptyTwoFields = "Поля {} и {} пустые."

localization_import_input_fileName = "Введите имя файла:"
localization_import_invalidFileName = "Файл {} не существует."

localization_export_menu_header = "Выберите действие:"
localization_export_menu_rewrite = "Перезаписать (все прежние данные очищаются)"
localization_export_menu_add = "Добавить в конец"
localization_export_menu_update = "Обновить (одинаковые карточки не дублируются)"

localization_export_allCards = "База карточек :"
localization_export_input_fileName = "Введите имя файла:"
localization_export_createNewFile = "Создаем файл {}."
localization_export_contentFile = "Содержимое файла {} :"
localization_export_rewriteFile = "Перезаписываем файл {}."
localization_export_addToFile = "Добавляем в конец файла {}."
localization_export_updateFile ="Обновляем файл {}."

localization_applyChanges_dataBaseEndChanges = "Изменения успешно отправлены."

localization_nothingCardTable = "Карточки отсутствуют."
localization_emptyFileName = "Введена пустая строка."

localization_input_pressAnyKey = "Нажмите любую клавишу..."

localization_tempImposible = "Временно не поддерживается"

def GetTableNames():
    return [
        'Themes',
        'Cards',
        'Accounts',
        'ThemeCards',
        'AccountCards',
        'Answers'
        ]

def GetSqlScripts():
    return {
        'CreateTable': {
            'Themes': 'Create table Themes(Theme_Id integer not null default 1 primary key, Theme_Desc text, Theme_Level text)',
            'Cards': 'Create table Cards(Card_Id integer not null default 1 primary key, Primary_Side text, Secondary_Side text, Card_Level text)',
            'Accounts': 'Create table Accounts(Account_Id integer not null default 1 primary key, Account_Name text)',
            'ThemeCards': 'Create table ThemeCards(Theme_Id integer not null, Card_Id integer not null)',
            'AccountCards': 'Create table AccountCards(Account_Id integer not null, Card_Id integer not null)',
            'Answers': 'Create table Answers(Card_Id integer not null, Account_Id integer not null, Result integer not null, History text)'
        },
        'DropTable': {
            'Themes': 'Drop table Themes',
            'Cards': 'Drop table Cards',
            'Accounts': 'Drop table Accounts',
            'ThemeCards': 'Drop table ThemeCards',
            'AccountCards': 'Drop table AccountCards',
            'Answers': 'Drop table Answers'
            },
        'DeleteFrom': {
            'Themes': 'Delete from Themes',
            'Cards': 'Delete from Cards',
            'Accounts': 'Delete from Accounts',
            'ThemeCards': 'Delete from ThemeCards',
            'AccountCards': 'Delete from AccountCards',
            'Answers': 'Delete from Answers'
            },
        'SelectAllFrom': {
            'Themes': 'Select * from Themes',
            'Cards': 'Select * from Cards',
            'Accounts': 'Select * from Accounts',
            'ThemeCards': 'Select * from ThemeCards',
            'AccountCards': 'Select * from AccountCards',
            'Answers': 'Select * from Answers'
            },
        'InsertInto': {
        'Themes': [
                "Insert into Themes(Theme_Id) values({})",
                "Insert into Themes(Theme_Id, Theme_Level) values({}, '{}')",
                "Insert into Themes(Theme_Id, Theme_Desc) values({}, '{}')",
                "Insert into Themes values({}, '{}', '{}')"
                ],
            'Cards': [
                "Insert into Cards(Card_Id) values({})",
                "Insert into Cards(Card_Id, Card_Level) values({}, '{}')",
                "Insert into Cards(Card_Id, Secondary_Side) values({}, '{}')",
                "Insert into Cards(Card_Id, Secondary_Side, Card_Level) values({}, '{}', '{}')",
                "Insert into Cards(Card_Id, Primary_Side) values({}, '{}')",
                "Insert into Cards(Card_Id, Primary_Side, Card_Level) values({}, '{}', '{}')",
                "Insert into Cards(Card_Id, Primary_Side, Secondary_Side) values({}, '{}', '{}')",
                "Insert into Cards values({}, '{}', '{}', '{}')"
            ],
            'Accounts': [
                "Insert into Accounts(Account_Id) values({})",
                "Insert into Accounts values({}, '{}')"
                ],
            'ThemeCards': [ "Insert into ThemeCards values({}, {})" ],
            'AccountCards': [ "Insert into AccountCards values({}, {})" ],
            'Answers' : [
                "Insert into Answers(Card_Id, Account_Id, Result) values({}, {}, {})"
                "Insert into Answers values({}, {}, {}, '{}')"
                ]
        },
        'Update': {
            'Themes': [ "Update Themes set Theme_Level = '{}' where Theme_Id = {}" ],
            'Cards': [
                "Update Cards set Card_Level = '{}' where Card_Id = {}",
                "Update Cards set Secondary_Side = '{}' where Card_Id = {}",
                "Update Cards set Secondary_Side = '{}', Card_Level = '{}' where Card_Id = {}"
                ],
        'Answers': [ "Update Answers set Result = {}, History = '{}' where Card_Id = {} and Account_Id = {}" ]
        },
        'Query': {
            'AllCards': """
                Select Primary_side, Secondary_side, Card_Level, Theme_desc, Theme_Level, Account_Name from Cards
                left outer join ThemeCards on ThemeCards.Card_Id = Cards.Card_Id
                left outer join Themes on ThemeCards.Theme_Id = Themes.Theme_Id
                left outer join AccountCards on AccountCards.Card_Id = Cards.Card_Id
                left outer join Accounts on ThemeCards.Card_Id = AccountCards.Card_Id and AccountCards.Account_Id = Accounts.Account_Id
                """
                }
        }

def GetSimpleTablesOperationHeaders():
    return {
        'CreateTable': 'Создаем все новые таблицы.',
        'DropTable': 'Удаляем все существующие таблицы.',
        'DeleteFrom': 'Очищаем все существующие таблицы.'
        }

def GetSimpleTableOperationsSuccessMessages():
    return {
        'CreateTable': "Создаем таблицу {}.",
        'DropTable': "Удаляем таблицу {}.",
        'DeleteFrom': "Очищаем таблицу {}."
        }

def GetMainMenuHeaders():
    return [
        "База данных: карточки",
        "====================="
        ]

def GetMainMenuItems():
    return [
        "Операции с таблицами",
        "Операции с карточками",
        "Отправить изменения в базу данных",
        "Пройти тестирование",
        ]

def GetTableMenuHeaders():
    return [
        "Операции с таблицами",
        "===================="
        ]

def GetTableMenuItems():
    return [
        "Создать все новые таблицы",
        "Удалить все существующие таблицы",
        "Очистить все существующие таблицы",
        "Показать содержимое всех таблиц",
        ]

def GetCardsMenuHeaders():
    return [
        "Операции с карточками",
        "====================="
        ]

def GetCardsMenuItems():
    return [
        "Показать карточки",
        "Добавить карточки вручную",
        "Импортировать карточки из текстового файла",
        "Экспортировать карточки в текстовый файл",
        ]

#---------------------------- Create/Drop/Delete Tables -----------------------

def RunSimpleTableOperation(operation, cursor):
    ClearScreen()
    InitCurrentLog()
    header = GetSimpleTablesOperationHeaders()[operation]
    scripts = GetSqlScripts()[operation]
    successTableMessage = GetSimpleTableOperationsSuccessMessages()[operation]
    AppendCurrentLog(header)
    for tableName in GetTableNames():
        try:
            cursor.execute(scripts[tableName])
        except Exception as e:
            ExceptError(cursor, e)
        else:
            AppendCurrentLog(successTableMessage.format(tableName))
    AppendCurrentLog(localization_input_pressAnyKey)
    PrintLines(currentLog)
    getch()

#------------------------------- Show Tables/Cards ----------------------------

def ShowAllTables(cursor):
    ClearScreen()
    InitCurrentLog()
    AppendCurrentLog(localization_header_showTables)
    ShowTable(GetTableRowsDescription('SelectAllFrom', 'Cards', cursor), 'Cards')
    ShowTable(GetTableRowsDescription('SelectAllFrom', 'Themes', cursor), 'Themes')
    ShowTable(GetTableRowsDescription('SelectAllFrom', 'Accounts', cursor), 'Accounts')
    ShowTable(GetTableRowsDescription('SelectAllFrom', 'ThemeCards', cursor), 'ThemeCards')
    ShowTable(GetTableRowsDescription('SelectAllFrom', 'AccountCards', cursor), 'AccountCards')
    ShowTable(GetTableRowsDescription('SelectAllFrom', 'Answers', cursor), 'Answers')
    AppendCurrentLog(localization_input_pressAnyKey)
    PrintLines(currentLog)
    getch()

def GetTableRowsDescription(operation, tableName, cursor):
    script = GetSqlScripts()[operation][tableName]
    rows =  GetTableRows(script, cursor)
    header = GetTableHeader(GetColumnNames(cursor))
    return { 'Script' : script, 'Rows' : rows, 'Header' : header }

def ShowTable(rowsDescription, tableName):
    ShowTableQuery(rowsDescription, "",
    localization_showTables_notEmptyTable.format(tableName),
    localization_showTables_emptyTable.format(tableName))

def ShowCardsQuery(cursor):
    ClearScreen()
    InitCurrentLog()
    ShowTableQuery(GetTableRowsDescription('Query', 'AllCards', cursor),
    localization_header_cardTable,
    localization_showCards_notEmptyTable,
    localization_showCards_emptyTable)
    AppendCurrentLog(localization_input_pressAnyKey)
    PrintLines(currentLog)
    getch()

def GetTableRows(script, cursor):
    if ExecuteScript(script, cursor):
        return cursor.fetchall()
    return None

def ShowTableQuery(tableRowsDescription, header, notEmptyTableHeader, emptyTableHeader):
    if header:
        AppendCurrentLog(header)
    ShowTableRows(tableRowsDescription, notEmptyTableHeader, emptyTableHeader)

def ShowTableRows(tableRowsDescription, notEmptyTableHeader, emptyTableHeader):
    rows = tableRowsDescription['Rows']
    if rows:
        AppendCurrentLog(notEmptyTableHeader)
        AppendCurrentLog(tableRowsDescription['Header'])
        [AppendCurrentLog(row) for row in rows]
    else:
        AppendCurrentLog(emptyTableHeader)
        AppendCurrentLog(tableRowsDescription['Header'])

#-------------------------------- Add Cards -----------------------------------
# (TODO : переделать AddCard:
# 1) сделать один большой запрос по всей таблице и сделать текстовые строки
# 2) калькуляцию вставляемых или обновляемых строк сделать на основе полученных текстовых строк
# 3) показать в логе сформированные sql-запросы для этого
# 4) запросы по базе по вставке/обновлению строк проводить в самом конце.
# Сейчас жесть - строки по одной вставляются в таблицы по запросам, которые вызываются сразу.

# test code-----------
#r = []
#r.append([3, 2, 1, 4, 5, 1])
#r.append([3, 2, 2, 3, 4, 2])
#r.append([2, 3, 3, 2, 3, 3])
#r.append([2, 3, 1, 4, 4, 2])
#r.append([3, 3, 1, 1, 5, 3])
#r.append([1, 2, 2, 4, 1, 2])
#r.append([1, 1, 3, 2, 4, 2])
#r.append([1, 1, 1, 3, 3, 2])
#r.append([2, 2, 2, 3, 5, 1])
#print("Input: ")
#print(r)
#sorted_r = sorted(r, key=itemgetter(5, 3, 4, 0, 1, 2))
#print("Sorted: ")
#print(sorted_r)
#group = {}
#for key, items in groupby(sorted_r, itemgetter(5)):
    #groupItems = GetGroupItems(key, 5, 1, items)
    #group_1 = {}
    #for key_1, items_1 in groupby(groupItems, itemgetter(3, 4)):
        #group_1[key_1] = GetGroupItems(key_1, 3, 2, items_1)
    #group[key] = group_1
#print("Group: ")
#print(group)
# ---------------

def AddCards(inputRows, hasUpdate, cursor):
    if not inputRows:
        AppendCurrentLog(localization_nothingCardTable)
        return

    inputCardInfos = CreateInputInfos(inputRows, 0)
    inputThemeInfos = CreateInputInfos(inputRows, 1)
    inputAccountInfos = CreateInputInfos(inputRows, 2)
    inputThemeCardInfos = CreateInputRelationInfos(inputRows, 1, 0)
    inputAccountCardInfos = CreateInputRelationInfos(inputRows, 2, 0)

    existingCardInfos = CreateExistingInfos('Cards', [1, 2, 3], cursor)
    existingThemeInfos = CreateExistingInfos('Themes', [1, 2], cursor)
    existingAccountInfos = CreateExistingInfos('Accounts', [1], cursor)
    existingThemeCardInfos = CreateExistingRelationInfos('ThemeCards', existingThemeInfos, existingCardInfos, cursor)
    existingAccountCardInfos = CreateExistingRelationInfos('AccountCards', existingAccountInfos, existingCardInfos, cursor)

    addedCardsInfos = CreateAddedInfos(inputCardInfos, existingCardInfos, hasUpdate, 3)
    addedThemeInfos = CreateAddedInfos(inputThemeInfos, existingThemeInfos, hasUpdate, 2)
    addedAccountInfos = CreateAddedInfos(inputAccountInfos, existingAccountInfos, hasUpdate, 1)
    addedThemeCardInfos = CreateAddedRelationInfos(inputThemeCardInfos, existingThemeCardInfos, addedThemeInfos, addedCardsInfos)
    addedAccountCardInfos = CreateAddedRelationInfos(inputAccountCardInfos, existingAccountCardInfos, addedAccountInfos, addedCardsInfos)

    addedScripts = []
    addedScripts.extend(CreateAddedCardScripts(addedCardsInfos))
    addedScripts.extend(CreateAddedThemeScripts(addedThemeInfos))
    addedScripts.extend(CreateAddedAccountScripts(addedAccountInfos))
    addedScripts.extend(CreateAddedRelationScripts(addedThemeCardInfos, 'ThemeCards'))
    addedScripts.extend(CreateAddedRelationScripts(addedAccountCardInfos, 'AccountCards'))

    AppendCurrentLog(localization_header_addCards)

    AppendCurrentLog("Подготовка скриптов: ")

    ShowInfos(inputRows, 'Введены строки:')
    ShowInfos(inputCardInfos, 'Введены карточки:')
    ShowInfos(inputThemeInfos, "Введены темы:")
    ShowInfos(inputAccountInfos, "Введены пользователи:")
    ShowInfos(inputThemeCardInfos, "Введены связи (theme, card): ")
    ShowInfos(inputAccountCardInfos, "Введены связи (account, card): ")

    ShowInfos(existingCardInfos, "Имеются Cards infos:")
    ShowInfos(existingThemeInfos, "Имеются Themes infos:")
    ShowInfos(existingAccountInfos, "Имеются Accounts infos:")
    ShowInfos(existingThemeCardInfos, "Имеются связи (theme, card): ")
    ShowInfos(existingAccountCardInfos, "Имеются связи (account, card): ")

    ShowInfos(addedCardsInfos, 'AddedCardsInfos:')
    ShowInfos(addedThemeInfos, 'addedThemeInfos:')
    ShowInfos(addedAccountInfos, 'AddedAccountInfos:')
    ShowInfos(addedThemeCardInfos, 'AddedThemeCardInfos:')
    ShowInfos(addedAccountCardInfos, 'AddedAccountCardInfos:')

    ShowInfos(addedScripts, "Получены скрипты:")

    ExecuteAddedScripts(addedScripts, cursor)

def CreateInputInfos(rows, index):
    result = []
    for row in rows:
        info = row[index]
        if result.count(info) == 0:
            result.append(info)
    return result

def CreateInputRelationInfos(rows, primaryIndex, groupItemIndex):
    def GetGroupItems(items, itemIndex):
        result = []
        for item in items:
            result.append(item[itemIndex])
        return result

    result = []
    for key, items in groupby(rows, itemgetter(primaryIndex)):
        groupItems = GetGroupItems(items, groupItemIndex)
        for item in groupItems:
            info = (key, item)
            if result.count(info) == 0:
                result.append(info)
    return result

def CreateExistingInfos(tableName, secondaryColumnIndexes, cursor):
    infos = []
    rows = GetTableRowsDescription('SelectAllFrom', tableName, cursor)['Rows']
    for row in rows:
        secondaryColumns = ()
        for index in secondaryColumnIndexes:
            secondaryColumns += (row[index], )
        infos.append((row[0], secondaryColumns))
    return infos

def CreateExistingRelationInfos(tableName, keyInfos, valueInfos, cursor):
    result = []
    rows = GetTableRowsDescription('SelectAllFrom', tableName, cursor)['Rows']
    for row in rows:
        keyValue = keyInfos[row[0]][1]
        valueValue =  valueInfos[row[1]][1]
        result.append((keyValue, valueValue))
    return result

def CreateAddedInfos(inputInfos, existingInfos, hasUpdate, columnCount):
    def GetMaxId(rows):
        result = 0
        for row in rows:
            id = row[0]
            if (id > result):
                result = id
        return result

    def CreateTuple(id, info, infoColumnCount, update):
        result = ()
        result += (id, )
        columns = ()
        for i in range(infoColumnCount):
            columns += (info[i], )
        result += (columns, )
        result += (update, )
        return result

    def GetFirstIndex(column, existingInfos):
        for existingInfo in existingInfos:
            if column == existingInfo[0]:
                return existingInfos.index(existingInfo)
        return -1

    startId = GetMaxId(existingInfos) + 1
    existingPartialInfos = [info[1] for info in existingInfos]
    result = []
    for inputInfo in inputInfos:
        existingIndex = GetFirstIndex(inputInfo[0], existingPartialInfos)
        if existingIndex == -1:
            result.append(CreateTuple(startId, inputInfo, columnCount, 'InsertInto'))
            startId += 1
        elif hasUpdate:
            existingInfo = existingInfos[existingIndex]
            result.append(CreateTuple(existingInfo[0], inputInfo, columnCount, ('Update', existingInfo)))
    return result

def CreateAddedRelationInfos(inputRelationInfos, existingRelationInfos, addedKeyInfos, addedValueInfos):
    def GetRelationId(infos, targetItem):
        for info in infos:
            id, compareItem, update = info
            if update == 'InsertInto' and compareItem == targetItem:
                return id
        return None

    result = []
    for inputInfo in inputRelationInfos:
        if existingRelationInfos.count(inputInfo) == 0:
            theme_id = GetRelationId(addedKeyInfos, inputInfo[0])
            if theme_id:
                card_id = GetRelationId(addedValueInfos, inputInfo[1])
                if card_id:
                    result.append((theme_id, card_id))
    return result

def CreateAddedCardScripts(addedInfos):
    result = []
    for info in addedInfos:
        id, inputInfo, update = info
        if update == 'InsertInto':
            script = CreateCardInsertIntoScript(id, inputInfo)
        else:
            script = CreateCardUpdateScript(id, inputInfo, update[1][1])
        result.append(script)
    return result

def CreateAddedThemeScripts(addedInfos):
    result = []
    for info in addedInfos:
        id, inputInfo, update = info
        if update == 'InsertInto':
            script = CreateThemeInsertIntoScript(id, inputInfo)
        else:
            script = CreateThemeUpdateScript(id, inputInfo, update[1][1])
        result.append(script)
    return result

def CreateAddedAccountScripts(addedInfos):
    result = []
    for info in addedInfos:
        id, inputInfo, update = info
        result.append(CreateAccountInsertIntoScript(id, inputInfo))
    return result

def CreateCardInsertIntoScript(id, inputInfo):
    if inputInfo[0] == "" and inputInfo[1] == "" and inputInfo[2] == "":
        return GetSqlScripts()['InsertInto']['Cards'][0].format(id)
    elif inputInfo[0] == "" and inputInfo[1] == "" and inputInfo[2] != "":
        return GetSqlScripts()['InsertInto']['Cards'][1].format(id, inputInfo[2])
    elif inputInfo[0] == "" and inputInfo[1] != "" and inputInfo[2] == "":
        return GetSqlScripts()['InsertInto']['Cards'][2].format(id, inputInfo[1])
    elif inputInfo[0] == "" and inputInfo[1] != "" and inputInfo[2] != "":
        return GetSqlScripts()['InsertInto']['Cards'][3].format(id, inputInfo[1], inputInfo[2])
    elif inputInfo[0] != "" and inputInfo[1] == "" and inputInfo[2] == "":
        return GetSqlScripts()['InsertInto']['Cards'][4].format(id, inputInfo[0])
    elif inputInfo[0] != "" and inputInfo[1] == "" and inputInfo[2] != "":
        return GetSqlScripts()['InsertInto']['Cards'][5].format(id, inputInfo[0], inputInfo[2])
    elif inputInfo[0] != "" and inputInfo[1] != "" and inputInfo[2] == "":
        return GetSqlScripts()['InsertInto']['Cards'][6].format(id, inputInfo[0], inputInfo[1])
    else:
        return GetSqlScripts()['InsertInto']['Cards'][7].format(id, inputInfo[0], inputInfo[1], inputInfo[2])

def CreateThemeInsertIntoScript(id, inputInfo):
    if inputInfo[0] == "" and inputInfo[1] == "":
        return GetSqlScripts()['InsertInto']['Themes'][0].format(id)
    elif inputInfo[0] == "" and inputInfo[1] != "":
        return GetSqlScripts()['InsertInto']['Themes'][1].format(id, inputInfo[1])
    elif inputInfo[0] != "" and inputInfo[1] == "":
        return GetSqlScripts()['InsertInto']['Themes'][2].format(id, inputInfo[0])
    else:
        return GetSqlScripts()['InsertInto']['Themes'][3].format(id, inputInfo[0], inputInfo[1])

def CreateAccountInsertIntoScript(id, inputInfo):
    if inputInfo[0] == "":
        return GetSqlScripts()['InsertInto']['Accounts'][0].format(id)
    else:
        return GetSqlScripts()['InsertInto']['Accounts'][1].format(id, inputInfo[0])

def CreateCardUpdateScript(id, inputInfo, existingInfo):
    if inputInfo[1] == existingInfo[1] and inputInfo[2] != existingInfo[2]:
        return GetSqlScripts()['Update']['Cards'][0].format(inputInfo[2], id)
    elif inputInfo[1] != existingInfo[1] and inputInfo[2] == existingInfo[2]:
        return GetSqlScripts()['Update']['Cards'][1].format(inputInfo[1], id)
    else:
        return GetSqlScripts()['Update']['Cards'][2].format(inputInfo[1], inputInfo[2], id)

def CreateThemeUpdateScript(id, inputInfo, existingInfo):
    return GetSqlScripts()['Update']['Cards'][0].format(inputInfo[1], id)

def CreateAddedRelationScripts(infos, tableName):
    return [GetSqlScripts()['InsertInto'][tableName][0].format(info[0], info[1]) for info in infos]

def ShowInfos(rows, header):
    AppendCurrentLog(header)
    [AppendCurrentLog(row) for row in rows]

def ShowScripts(scripts):
    [AppendCurrentLog(script) for script in scripts]

def ExecuteAddedScripts(addedScripts, cursor):
    resultExecuteScripts = [(script, ExecuteScript(script, cursor)) for script in addedScripts]
    AppendCurrentLog("Результаты выполнения скриптов: ")
    for script, result in resultExecuteScripts:
        AppendCurrentLog(script + " " + "Выполнен." if result else "Не выполнен.")

#------------------------- Add cards dialog -----------------------------------

def InputAddCards():
    def InputAccount():
        result = ()
        result += (input("Account : ").strip(), )
        return result

    def InputTheme():
        result = ()
        result += (input("Theme : ").strip(), )
        result += (input("Theme Level : "), )
        return result

    def InputCard():
        result = ()
        result += (input("Primary Side : ").strip(), )
        result += (input("Secondary Side : ").strip(), )
        result += (input("Card Level : ").strip(), )
        return result

    def InputAddCard(cardsList):
        cardInfo = InputCard()
        themeInfo = InputTheme()
        accountInfo = InputAccount()
        if cardInfo != ("", "", "") or themeInfo != ("", "") or accountInfo != "":
            cardsList.append((cardInfo,themeInfo,accountInfo))

    result = []
    InputAddCard(result)
    while True:
        if input("Ввести еще карточки (1 - да)?") == '1':
            InputAddCard(result)
        else:
            break
    return result

#-------------------- Import cards from file dialog ---------------------------

def InputImportCards():
    AppendCurrentLog(localization_header_importCards)
    fileName = input(localization_import_input_fileName)
    fileName = fileName.strip()
    if not fileName:
        AppendCurrentLog(localization_emptyFileName)
        return None
    if not exists(fileName):
        AppendCurrentLog(localization_import_invalidFileName.format(fileName))
        return None
    linesFromFile = GetLinesFromFile(fileName)
    result = []
    for line in linesFromFile:
        lineItems = line.split(',')
        cardInfo = (lineItems[0].strip(), lineItems[1].strip(), lineItems[2].strip())
        themeInfo = (lineItems[3].strip(), lineItems[4].strip())
        accountInfo = ()
        accountInfo += (lineItems[5].strip(), )
        result.append((cardInfo, themeInfo, accountInfo))
    return result

#---------------------------- Export Cards ------------------------------------

def ExportCards(fileName, exportType, cursor):
    AppendCurrentLog(localization_header_exportCards)
    try:
        cursor.execute(GetSqlScripts()['Query']['AllCards'])
    except Exception as e:
        ExceptError(cursor, e)
    else:
        rows = cursor.fetchall()
        if not rows:
            AppendCurrentLog(localization_nothingCardTable)
            return
        AppendCurrentLog(localization_export_allCards)
        columnNames = GetColumnNames(cursor)
        AppendCurrentLog(GetTableHeader(columnNames))
        linesFromRows = GetLinesFromRows(rows, len(columnNames), ", ", [0])
        for line in linesFromRows:
            AppendCurrentLog(line)
        fileName = fileName.strip()
        if not fileName:
            AppendCurrentLog(localization_emptyFileName)
            return
        if not exists(fileName):
            AppendCurrentLog(localization_export_createNewFile.format(fileName))
            ExportToNewTxtFile(fileName, linesFromRows)
            ShowTextFile(fileName);
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
            ExportToNewTxtFile(fileName, JoinLines(linesFromFile, linesFromRows, len(GetColumnNames(cursor)), ", "))
        ShowTextFile(fileName);

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

def ShowTextFile(file_name):
    AppendCurrentLog(localization_export_contentFile.format(file_name))
    for line in GetLinesFromFile(file_name):
        AppendCurrentLog(line)

#----------------------------- Commit Changes ---------------------------------

def CommitChanges(cursor):
    ClearScreen()
    InitCurrentLog()
    AppendCurrentLog(localization_header_applyChanges)
    cursor.commit()
    AppendCurrentLog(localization_applyChanges_dataBaseEndChanges)
    AppendCurrentLog(localization_input_pressAnyKey)
    PrintLines(currentLog)
    getch()

#----------------------------- Run Testing ------------------------------------

def RunTesting(cursor):
    ClearScreen()
    InitCurrentLog()
    AppendCurrentLog(localization_header_testing)
    AppendCurrentLog(localization_tempImposible)
    AppendCurrentLog(localization_input_pressAnyKey)
    PrintLines(currentLog)
    getch()

#----------------------------- Utils ------------------------------------------
# TODO : надо исправить JoinLines
# Сейчас мы добавляем только новые строки, оставляя старые,
# но не удаляем существующие, если они отличаются некоторыми колонками.

def ExecuteScript(script, cursor):
    if not script:
        return False;
    try:
        cursor.execute(script)
    except Exception as e:
        ExceptError(cursor, e)
        return False;
    else:
        return True

def GetLinesFromFile(file_name):
    txt_file = open(file_name, 'r')
    lines = [line.strip() for line in txt_file]
    txt_file.close()
    return lines

def GetLineColumns(line, delimeter):
    return [column.strip() for column in line.split(delimeter)]

def JoinLines(targetLines, addedLines, columnCount, delimeter):
    def ContainsLine(targetLines, line, columnCount, delimeter):
        lineItems = GetLineColumns(line, delimeter)
        for targetLine in targetLines:
            targetLineItems = GetLineColumns(targetLine, delimeter)
            hasDifferences = False
            for i in range(columnCount):
                if targetLineItems[i] != lineItems[i]:
                    hasDifferences = True
                    break;
            if not hasDifferences:
                return True
        return False

    result = []
    for targetLine in targetLines:
        result.append(targetLine)
    for addedLine in addedLines:
        if not ContainsLine(targetLines, addedLine, columnCount, delimeter):
            result.append(addedLine)
    return result

def ExceptError(cursor, e):

    def GetTableName(errorDecription):
        for tableName in GetTableNames():
            position = errorDecription.find(tableName)
            if position > 0 and errorDecription[position - 1] == "\'" and errorDecription[position + len(tableName)] == "\'":
                return tableName
        return ""

    if e.args[0] == '42S01':
        tableName = GetTableName(e.args[1])
        if tableName:
            AppendCurrentLog(localization_except_existTable.format(tableName))
            ShowTable(GetTableRowsDescription('SelectAllFrom', tableName, cursor), tableName)
        else:
            AppendCurrentLog(localization_except_invalidTable)
    elif e.args[0] == '42S02':
        tableName = GetTableName(e.args[1])
        if tableName:
            AppendCurrentLog(localization_except_nothingTable.format(tableName))
        else:
            AppendCurrentLog(localization_except_invalidTable)
    else:
        AppendCurrentLog(e.args[0] + " Неизвестная ошибка")

def InitCurrentLog():
    currentLog.clear()

def AppendCurrentLog(line):
    currentLog.append(line)

def GetLinesFromRows(rows, columnCount, delimeter, ignoreStrings):
    return [GetRowString(row, columnCount, delimeter, ignoreStrings) for row in rows]

def GetRowString(row, columnCount, delimeter, ignoreStrings):
    result = ""
    for i in range(columnCount):
        column = row[i]
        if ignoreStrings.count(i):
            result += GetFormatColumn(column, True)
        else:
            result += GetFormatColumn(column, False)
        if i < columnCount - 1:
            result += delimeter
    return result

def GetFormatColumn(column, ignoreFirstString):
    if ignoreFirstString:
        if column == 0:
            return f"{column}"
        elif column == "0":
            return f"{column}"
        elif column:
            return f"{column}"
    else:
        if column == 0:
            return f"\'{column}\'"
        elif column == "0":
            return f"\'{column}\'"
        elif column:
            return f"\'{column}\'"

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

def ClearScreen():
    os.system('cls' if os.name=='nt' else 'clear')

def PrintLines(lines):
    [print(line) for line in lines]

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

def CreateMenu(headers, items):
    result = []
    for header in headers:
        result.append(header)
    for i in range(len(items)):
        result.append("{} - {}".format(i + 1, items[i]))
    return result

#---------------------------- Main Menu ---------------------------------------

def GetMainMenuActionType(key):
    if key == b'\x1b':
        return 10
    if key == b'1':
        return 1
    if key == b'2':
        return 2
    if key == b'3':
        return 3
    if key == b'4':
        return 4
    return -1

def MainMenu(cursor):
    menu = CreateMenu(GetMainMenuHeaders(), GetMainMenuItems())
    while True:
        ClearScreen()
        PrintLines(menu)
        actionType = GetMainMenuActionType(getch())
        if actionType == 10:
            break
        if actionType == 1:
            TablesMenu(cursor)
        if actionType == 2:
            CardsMenu(cursor)
        if actionType == 3:
            CommitChanges(cursor)
        if actionType == 4:
            RunTesting(cursor)

#---------------------------- Tables Menu -------------------------------------

def GetTablesMenuActionType(key):
    if key == b'\x1b':
        return 10
    if key == b'1':
        return 1
    if key == b'2':
        return 2
    if key == b'3':
        return 3
    if key == b'4':
        return 4
    return -1

def TablesMenu(cursor):
    menu = CreateMenu(GetTableMenuHeaders(), GetTableMenuItems())
    while True:
        ClearScreen()
        PrintLines(menu)
        actionType = GetTablesMenuActionType(getch())
        if actionType == 10:
            break
        if actionType == 1:
            RunSimpleTableOperation('CreateTable', cursor)
        if actionType == 2:
            RunSimpleTableOperation('DropTable', cursor)
        if actionType == 3:
            RunSimpleTableOperation('DeleteFrom', cursor)
        if actionType == 4:
            ShowAllTables(cursor)

# --------------------------- Cards menu --------------------------------------

def GetCardsMenuActionType(key):
    if key == b'\x1b':
        return 10
    if key == b'1':
        return 1
    if key == b'2':
        return 2
    if key == b'3':
        return 3
    if key == b'4':
        return 4
    return -1

def CardsMenu(cursor):
    menu = CreateMenu(GetCardsMenuHeaders(), GetCardsMenuItems())
    while True:
        ClearScreen()
        PrintLines(menu)
        actionType = GetCardsMenuActionType(getch())
        if actionType == 10:
            break
        if actionType == 1:
            ShowCardsQuery(cursor)
        if actionType == 2:
            ClearScreen()
            InitCurrentLog()
            AddCards(InputAddCards(), True, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 3:
            ClearScreen()
            InitCurrentLog()
            AddCards(InputImportCards(), True, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()
        if actionType == 4:
            ClearScreen()
            InitCurrentLog()
            fileName = input(localization_export_input_fileName)
            PrintLines(exportMenu)
            exportType = GetExportType(getch())
            ExportCards(fileName, tableNames, exportType, cursor)
            AppendCurrentLog(localization_input_pressAnyKey)
            PrintLines(currentLog)
            getch()

#-------------------------------- Main ----------------------------------------

try:
    db_connection = pyodbc.connect(db_connection_string)
except Exception as e:
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
