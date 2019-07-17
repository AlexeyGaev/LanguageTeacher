import localization
import sql
import operations

def Error(cursor, e):
    if e.args[0] == '42S01':
        tableName = GetTableName(e.args[1], sql.tables)
        if tableName:
            TableError(tableName, True, localization.messages['HasTable'], cursor)
        else:
            print(localization.messages['Error'], e.args[1])
    elif e.args[0] == '42S02':
        tableName = GetTableName(e.args[1], sql.tables)
        if tableName:
            TableError(tableName, False, localization.messages['MissingTable'], cursor)
        else:
            print(localization.messages['Error'], e.args[1])
    else:
        tableName = GetTableName(e.args[0], sql.tables)
        if tableName:
            print(localization.messages['MissingTable'][tableName])
        else:
            print(localization.messages['Error'], e.args[0])

def TableError(tableName, shouldShowTable, message, cursor):
    if tableName:
        print(message[tableName])
        if shouldShowTable:
            operations.ShowTable(tableName, cursor)
    else:
        print(localization.messages['InvalidTable'])

def GetTableName(errorDecription, tableNames):
    for tableName in tableNames:
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
