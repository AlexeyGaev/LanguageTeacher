import localization
import sql

from operations import ShowTable

def Error(cursor, e):
    if e.args[0] == '42S01':
        TableError(e, True, localization.messages['HasTable'], cursor)
    elif e.args[0] == '42S02':
        TableError(e, False, localization.messages['MissingTable'], cursor)
    else:
        print(e.args[0], localization.messages['Error'])

def TableError(e, shouldShowTable, message, cursor):
    tableName = GetTableName(e.args[1])
    if tableName:
        print(localization.messages['HasTable'][tableName])
        ShowTable(tableName, cursor)
    else:
        print(localization.messages['InvalidTable'])

def GetTableName(errorDecription):
    for tableName in sql.tables:
        position = errorDecription.find(tableName)
        if position > 0 and errorDecription[position - 1] == "\'" and errorDecription[position + len(tableName)] == "\'":
            return tableName
    return ""
