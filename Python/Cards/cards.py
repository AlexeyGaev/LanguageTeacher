import sqlite3
import pyodbc

from sys import argv
from msvcrt import getch

import localization
import menues

def Connect():
    serverMode = True
    try:
        if serverMode:
            driver = 'SQL Server'
            server = 'HOME\SQLEXPRESS'
            database = 'Cards'
            connection_string = 'DRIVER={};SERVER={};DATABASE={}'.format(driver, server, database)
            connection = pyodbc.connect(connection_string)
            return serverMode, connection, connection_string
        else:
            database = 'Cards.db'
            connection = sqlite3.connect(database)
            return serverMode, connection, database
    except:
        return None



def DataBaseDialogs(connect, hasUpdate):
    print("Установлена связь с базой данных.")
    serverMode, connection, database = connect
    if serverMode:
        print('Connection string :', connection_string)
        print('ServerMode:', True)
    else:
        print('Local database :', database)
        print('ServerMode:', False)
    cursor = connection.cursor()
    print("Проверка содержимого базы данных.")
    if not sql.Execute(sql.scripts['GetAllTableNames']):
        print("Не могу получить список таблиц базы данных.")
        getch()
    menues.MainMenu(hasUpdate, cursor)
    print("Вы закончили работать с базой данных.")
    if input("Отправить изменения в базу данных (0 - нет)?") != '0':
        operations.CommitChanges(cursor if serverMode else connection)
    cursor.close()
    connection.close()

def LocalDataBaseDialogs(database, hasUpdate, connection):
    cursor = connection.cursor()
    print('Local database :', database)
    print('ServerMode:', False)
    print("Проверка содержимого базы данных.")
    if sql.Execute(sql.scripts['GetAllTableNames']) operations.ContainsAllTables():
        print("Не все таблицы пристутствуют в базе.")
    menues.MainMenu(hasUpdate, cursor)
    print("Вы закончили работать с базой данных.")
    if input("Отправить изменения в базу данных (0 - нет)?") == '0':
        cursor.close()
        connection.close()
    else:
        operations.CommitChanges(connection)
    cursor.close()

script = argv
hasUpdate = True

print(localization.messages['Start'].format(script))
print(localization.messages['ShowOptions'])
print('HasUpdate:', hasUpdate)

database = None
connection_string = None

print(localization.messages['DataBaseSync'])
print(localization.messages['PressAnyKey'])
getch()

connect = Connect()

if not connect:
    print(localization.messages['MainException'])
    print(localization.messages['PressAnyKey'])
    getch()
else:
    DataBaseDialogs(connect)
