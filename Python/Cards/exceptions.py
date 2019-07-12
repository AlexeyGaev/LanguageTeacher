import localization
import sql

from operations import ShowTable

def Error(cursor, e):
    if e.args[0] == '42S01':
        TableError(GetTableName(e.args[1]), True, localization.messages['HasTable'], cursor)
    elif e.args[0] == '42S02':
        TableError(GetTableName(e.args[1]), False, localization.messages['MissingTable'], cursor)
    else:
        tableName = GetTableName(e.args[0])
        if tableName:
            print(localization.messages['MissingTable'][tableName])
        else:
            print(localization.messages['Error'], e.args[0])

def TableError(tableName, shouldShowTable, message, cursor):
    if tableName:
        print(message[tableName])
        if shouldShowTable:
            ShowTable(tableName, cursor)
    else:
        print(localization.messages['InvalidTable'])

def GetTableName(errorDecription):
    for tableName in sql.tables:
        position = errorDecription.find(tableName)
        if (position <= 0):
            continue
        beforePosition = position - 1
        if errorDecription[beforePosition] != "\'" and errorDecription[beforePosition] != ' ':
            continue
        nextPosition = position + len(tableName)
        if nextPosition >= len(errorDecription):
            return tableName
        if errorDecription[nextPosition] == "\'" or errorDecription[nextPosition] == ' ':
            return tableName
    return None
