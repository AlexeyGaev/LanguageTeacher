headers = {
    'CreateTable': 'Создаем все новые таблицы.',
    'DropTable': 'Удаляем все существующие таблицы.',
    'DeleteFrom': 'Очищаем все существующие таблицы.',
    'ShowTables': 'Вывод содержимого таблиц.',
    'ShowCards': 'Вывод базы карточек.',
    'AddCards': 'Добавляем карточки в таблицы.',
    'ImportCards': 'Импорт карточек из текстового файла.',
    'ExportCards': 'Экспорт карточек в текстовый файл.',
    'ApplyChanges': 'Отправляем изменения в базу данных.',
    'Testing' : 'Тестирование.'
    }

messages = {
    'CreateTable': {
        'Themes': 'Создаем таблицу Themes.',
        'Cards': 'Создаем таблицу Cards Accounts.',
        'Accounts': 'Создаем таблицу Accounts.',
        'ThemeCards': 'Создаем таблицу ThemeCards.',
        'AccountCards': 'Создаем таблицу AccountCards.',
        'Answers': 'Создаем таблицу Answers.'
        },
    'DropTable': {
        'Themes': 'Удаляем таблицу Themes.',
        'Cards': 'Удаляем таблицу Cards Accounts.',
        'Accounts': 'Удаляем таблицу Accounts.',
        'ThemeCards': 'Удаляем таблицу ThemeCards.',
        'AccountCards': 'Удаляем таблицу AccountCards.',
        'Answers': 'Удаляем таблицу Answers.'
        },
    'DeleteFrom': {
        'Themes': 'Очищаем таблицу Themes.',
        'Cards': 'Очищаем таблицу Cards Accounts.',
        'Accounts': 'Очищаем таблицу Accounts.',
        'ThemeCards': 'Очищаем таблицу ThemeCards.',
        'AccountCards': 'Очищаем таблицу AccountCards.',
        'Answers': 'Очищаем таблицу Answers.'
        },
    'HasTable' : {
        'Themes': 'Таблица Themes существует.',
        'Cards': 'Таблица Cards существует.',
        'Accounts': 'Таблица Accounts существует.',
        'ThemeCards': 'Таблица ThemeCards существует.',
        'AccountCards': 'Таблица AccountCards существует.',
        'Answers': 'Таблица Answers существует.'
        },
    'MissingTable' : {
        'Themes': 'Таблица Themes не существует.',
        'Cards': 'Таблица Cards не существует.',
        'Accounts': 'Таблица Accounts не существует.',
        'ThemeCards': 'Таблица ThemeCards не существует.',
        'AccountCards': 'Таблица AccountCards не существует.',
        'Answers': 'Таблица Answers не существует.',
        'QueryAllCards': "Карточки отсутствуют."
        },
    'ContentTable' : {
        'Themes': 'Содержимое таблицы Themes:',
        'Cards': 'Содержимое таблицы Cards:',
        'Accounts': 'Содержимое таблицы Accounts:',
        'ThemeCards': 'Содержимое таблицы ThemeCards:',
        'AccountCards': 'Содержимое таблицы AccountCards:',
        'Answers': 'Содержимое таблицы Answers:',
        'QueryAllCards': 'Содержимое запроса по карточкам:'
        },
    'EmptyTable' : {
        'Themes': 'Таблица Themes пустая.',
        'Cards': 'Таблица Cards пустая.',
        'Accounts': 'Таблица Accounts пустая.',
        'ThemeCards': 'Таблица ThemeCards пустая.',
        'AccountCards': 'Таблица AccountCards пустая.',
        'Answers': 'Таблица Answers пустая.',
        'QueryAllCards': 'Таблица карточек пустая.'
        },

    'ApplyChanges': 'Изменения успешно отправлены.',

    'InputFileName' : 'Введите имя файла: ',
    'InvalidFile' : 'Файл {} не существует.',
    'CreateFile': 'Создаем файл {}.',
    'FileContent': 'Содержимое файла {} :',
    'ReWriteFile': 'Перезаписываем файл {}.',
    'EmptyFileName': 'Введена пустая строка.',

    'MainException' : 'Не могу связаться к базой данных.',
    'InvalidTable' : 'Проблема с доступом к таблице.',
    'Error' : "Неизвестная ошибка",

    'PressAnyKey' : 'Нажмите любую клавишу...',
    'TempImposible' : 'Временно не поддерживается'
}

menues = {
    'Main' : {
        'Headers' : [
            "База данных: карточки",
            "====================="
            ],
        'Items' : [
            (1, 'Операции с таблицами', b'1'),
            (2, 'Операции с карточками', b'2'),
            (3, 'Отправить изменения в базу данных', b'3'),
            (4, 'Пройти тестирование', b'4')
            ]
        },
    'Tables': {
        'Headers' : [
            "Операции с таблицами",
            "===================="
            ],
        'Items' : [
            (1, 'Создать все новые таблицы', b'1'),
            (2, 'Удалить все существующие таблицы', b'2'),
            (3, 'Очистить все существующие таблицы', b'3'),
            (4, 'Показать содержимое всех таблиц', b'4')
            ]
        },
    'Cards': {
        'Headers' : [
            "Операции с карточками",
            "====================="
            ],
        'Items' : [
            (1, 'Показать карточки', b'1'),
            (2, 'Добавить карточки вручную', b'2'),
            (3, 'Импортировать карточки из текстового файла', b'3'),
            (4, 'Экспортировать карточки в текстовый файл', b'4')
            ]
        }
    }


#localization_addCards_input_createAccount = "Создать пользователя (1 - да)?"
#localization_addCards_input_createTheme = "Создать тему (1 - да)?"
#localization_addCards_input_createCard = "Создать карточку (1 - да)?"
#"Ввести еще карточки (1 - да)?"
#"Результаты выполнения скриптов: "
#"Выполнен."
#"Не выполнен."
#"Подготовка скриптов: "
#"Получены скрипты:"

#'Введены строки:'
#'Введены карточки:'
#"Введены темы:"
#"Введены пользователи:"
#"Введены связи (theme, card): "
#"Введены связи (account, card): "

#"Имеются Cards infos:"
#"Имеются Themes infos:"
#"Имеются Accounts infos:"
#"Имеются связи (theme, card): "
#"Имеются связи (account, card): "

#ShowInfos(addedCardsInfos, 'AddedCardsInfos:'
#ShowInfos(addedThemeInfos, 'AddedThemeInfos:'
#ShowInfos(addedAccountInfos, 'AddedAccountInfos:'
#ShowInfos(addedThemeCardInfos, 'AddedThemeCardInfos:'
#ShowInfos(addedAccountCardInfos, 'AddedAccountCardInfos:'
