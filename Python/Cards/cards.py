from pyodbc import connect
from sys import argv
from msvcrt import getch

import localization
import menues

script, connection_string = argv

print('Вы запустили программу {}.'.format(script))
print("Устанавливаю связь с базой данных...")
try:
    connection = connect(connection_string)
except Exception as e:
    print(localization.messages['MainException'])
    print(localization.messages['PressAnyKey'])
    getch()
else:
    cursor = connection.cursor()
    menues.MainMenu(cursor)
    cursor.close()
    connection.close()
