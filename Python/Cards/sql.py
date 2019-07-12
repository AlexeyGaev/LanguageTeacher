tables = [ 'Themes', 'Cards', 'Accounts', 'ThemeCards', 'AccountCards', 'Answers' ]

scripts = {
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
