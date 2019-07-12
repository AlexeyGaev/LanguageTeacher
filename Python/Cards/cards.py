import pyodbc

from sys import argv
from msvcrt import getch

import localization
import menues

script, db_connection_string = argv

try:
    db_connection = pyodbc.connect(db_connection_string)
except Exception as e:
    print(localization.messages['MainException'])
    print(localization.messages['PressAnyKey'])
    getch()
else:
    db_cursor = db_connection.cursor()
    menues.MainMenu(db_cursor)
    db_cursor.close()
    db_connection.close()
