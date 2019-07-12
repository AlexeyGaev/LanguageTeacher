import sqlite3
import pyodbc

from sys import argv
from msvcrt import getch

import localization
import menues

script = argv
serverMode = True
hasUpdate = True

print(localization.messages['Start'].format(script))
print(localization.messages['ShowOptions'])
print('ServerMode:', serverMode)
print('HasUpdate:', hasUpdate)

database = None
connection_string = None
if serverMode:
    driver = 'SQL Server'
    server = 'HOME\SQLEXPRESS'
    database = 'Cards'
    connection_string = 'DRIVER={};SERVER={};DATABASE={}'.format(driver, server, database)
    print('Connection string :', connection_string)
else:
    database = 'Cards.db'

print(localization.messages['DataBaseSync'])
print(localization.messages['PressAnyKey'])
getch()

try:
    if serverMode:
        connection = pyodbc.connect(connection_string)
    else:
        connection = sqlite3.connect(database)
except:
    print(localization.messages['MainException'])
    print(localization.messages['PressAnyKey'])
    getch()
else:
    menues.MainMenu(serverMode, hasUpdate, connection)
    connection.close()
