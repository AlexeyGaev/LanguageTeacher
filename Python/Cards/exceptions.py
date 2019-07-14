import localization
import sql

from operations import ShowTable

def Error(cursor, e):
    if e.args[0] == '42S01':
        tableName = GetQueryName(e.args[1], sql.tables)
        if tableName:
            QueryError(tableName, True, False, localization.messages['HasTable'], cursor)
            return
        viewName = GetQueryName(e.args[1], sql.views)
        if viewName:
            QueryError(viewName, False, True, localization.messages['HasView'], cursor)
            return
        print(localization.messages['Error'], e.args[1])
    elif e.args[0] == '42S02':
        tableName = GetQueryName(e.args[1], sql.tables)
        if tableName:
            QueryError(tableName, False, False, localization.messages['MissingTable'], cursor)
            return
        viewName = GetQueryName(e.args[1], sql.views)
        if viewName:
            QueryError(viewName, False, False, localization.messages['MissingView'], cursor)
            return
        print(localization.messages['Error'], e.args[1])
    else:
        tableName = GetQueryName(e.args[0], sql.tables)
        if tableName:
            print(localization.messages['MissingTable'][tableName])
            return
        viewName = GetQueryName(e.args[0], sql.views)
        if viewName:
            print(localization.messages['MissingView'][viewName])
            return
        print(localization.messages['Error'], e.args[0])

def QueryError(queryName, shouldShowTable, shouldViewTable, message, cursor):
    if queryName:
        print(message[queryName])
        if shouldShowTable:
            ShowTable(queryName, cursor)
        if shouldViewTable:
            ShowView(queryName, cursor)
    else:
        print(localization.messages['InvalidTable'])

def GetQueryName(errorDecription, queryNames):
    for queryName in queryNames:
        position = errorDecription.find(queryName)
        if (position <= 0):
            continue
        beforePosition = position - 1
        if errorDecription[beforePosition] != "\'" and errorDecription[beforePosition] != ' ':
            continue
        nextPosition = position + len(queryName)
        if nextPosition >= len(errorDecription):
            return queryName
        if errorDecription[nextPosition] == "\'" or errorDecription[nextPosition] == ' ':
            return queryName
    return None
