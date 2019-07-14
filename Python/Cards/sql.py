tables = [ 'Themes', 'Cards', 'Accounts', 'ThemeCards', 'AccountCards', 'Answers' ]
views = ['AllCards' ] #'AllAnswers' ]

scripts = {
    'CreateTable': {
        'Themes': 'Create table Themes(Id integer not null primary key, Name char(255), Level integer)',
        'Cards': 'Create table Cards(Id integer not null primary key, Primary_Side text, Secondary_Side text, Level integer)',
        'Accounts': 'Create table Accounts(Id integer not null primary key, Name text)',
        'ThemeCards': 'Create table ThemeCards(Theme_Id integer not null, Card_Id integer not null)',
        'AccountCards': 'Create table AccountCards(Account_Id integer not null, Card_Id integer not null)',
        'Answers': 'Create table Answers(Card_Id integer not null, Side_Order bit not null, Result integer not null, Level integer)'
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
    'SelectAllFromTable': {
        'Themes': 'Select Id, RTRIM(Themes.Name) as Name from Themes',
        'Cards': 'Select * from Cards',
        'Accounts': 'Select Id, RTRIM(Accounts.Name) as Name from Accounts',
        'ThemeCards': 'Select * from ThemeCards',
        'AccountCards': 'Select * from AccountCards',
        'Answers': 'Select * from Answers'
        },
    'InsertInto': {
        'Themes': [
            "Insert into Themes(Id) values({})",
            "Insert into Themes(Id, Level) values({}, {})",
            "Insert into Themes(Id, Name) values({}, '{}')",
            "Insert into Themes values({}, '{}', {})"
            ],
        'Cards': [
            "Insert into Cards(Id) values({})",
            "Insert into Cards(Id, Level) values({}, {})",
            "Insert into Cards(Id, Secondary_Side) values({}, '{}')",
            "Insert into Cards(Id, Secondary_Side, Level) values({}, '{}', {})",
            "Insert into Cards(Id, Primary_Side) values({}, '{}')",
            "Insert into Cards(Id, Primary_Side, Level) values({}, '{}', {})",
            "Insert into Cards(Id, Primary_Side, Secondary_Side) values({}, '{}', '{}')",
            "Insert into Cards values({}, '{}', '{}', {})"
            ],
        'Accounts': [
            "Insert into Accounts(Id) values({})",
            "Insert into Accounts values({}, '{}')"
            ],
        'ThemeCards': [ "Insert into ThemeCards values({}, {})" ],
        'AccountCards': [ "Insert into AccountCards values({}, {})" ],
        'Answers' : [
            "Insert into Answers(Card_Id, Side_Order, Result) values({}, {}, {})"
            "Insert into Answers values({}, {}, {}, {})"
            ]
        },
        'Update': {
            'Themes': [ "Update Themes set Level = {} where Id = {}" ],
            'Cards': [
                "Update Cards set Level = '{}' where Id = {}",
                "Update Cards set Secondary_Side = '{}' where Id = {}",
                "Update Cards set Secondary_Side = '{}', Level = '{}' where Id = {}"
                ],
            'Answers': [ "Update Answers set Level = {} where Card_Id = {} and Side_Order = {} and Result = {}" ]
        },
    'CreateView' : {
        'AllCards': """
            Create view AllCards as
            Select Primary_Side, Secondary_Side, Cards.Level as Card_Level,
            RTRIM(Themes.Name) as Theme_Name, Themes.Level as Theme_Level,
            RTRIM(Accounts.Name) as Account_Name
            from Cards
            left join ThemeCards on ThemeCards.Card_Id = Cards.Id
            left join Themes on ThemeCards.Theme_Id = Themes.Id
            left join AccountCards on AccountCards.Card_Id = Cards.Id
            left join Accounts on AccountCards.Account_Id = Accounts.Id
            """
            },
    'DropView': {
         'AllCards': 'Drop view AllCards'
         },
    'SelectAllFromView': {
        'AllCards': 'Select * from AllCards',
        },
    }

import exceptions

def Execute(script, cursor):
    if not script:
        return False;
    try:
        cursor.execute(script)
    except Exception as e:
        exceptions.Error(cursor, e)
        return False;
    else:
        return True
