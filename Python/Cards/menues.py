import os
from msvcrt import getch

import dialogs
import localization
import operations

def MainMenu(cursor):
    while True:
        ClearScreen()
        PrintLines(CreateMenu('Main'))
        key = getch()
        if key == b'\x1b':
            break
        actionType = GetActionType('Main', key)
        if actionType == 1:
            ClearScreen()
            TablesMenu(cursor)
        if actionType == 2:
            ClearScreen()
            CardsMenu(cursor)
        if actionType == 3:
            ClearScreen()
            operations.CommitChanges(cursor)
            EndMenuAction()
        if actionType == 4:
            ClearScreen()
            operations.RunTesting(cursor)
            EndMenuAction()

#---------------------------- Tables Menu -------------------------------------

def TablesMenu(cursor):
    while True:
        ClearScreen()
        PrintLines(CreateMenu('Tables'))
        key = getch()
        if key == b'\x1b':
            break
        actionType = GetActionType('Tables', key)
        if actionType == 1:
            ClearScreen()
            operations.RunSimpleTableOperation('CreateTable', cursor)
            EndMenuAction()
        if actionType == 2:
            ClearScreen()
            operations.RunSimpleTableOperation('DropTable', cursor)
            EndMenuAction()
        if actionType == 3:
            ClearScreen()
            operations.RunSimpleTableOperation('DeleteFrom', cursor)
            EndMenuAction()
        if actionType == 4:
            ClearScreen()
            operations.ShowAllTables(cursor)
            EndMenuAction()

# --------------------------- Cards menu --------------------------------------

def CardsMenu(cursor):
    menu = CreateMenu('Cards')
    while True:
        ClearScreen()
        PrintLines(menu)
        key = getch()
        if key == b'\x1b':
            break
        actionType = GetActionType('Cards', key)
        if actionType == 1:
            ClearScreen()
            operations.QueryAllCards(cursor)
            EndMenuAction()
        if actionType == 2:
            ClearScreen()
            operations.AddCards(dialogs.InputAddCards(), True, cursor)
            EndMenuAction()
        if actionType == 3:
            ClearScreen()
            operations.AddCards(dialogs.InputImportCards(), True, cursor)
            EndMenuAction()
        if actionType == 4:
            ClearScreen()
            operations.ExportCards(dialogs.InputExportCards(), cursor)
            EndMenuAction()

#-------------------------------- Utils ---------------------------------------

def EndMenuAction():
    print(localization.messages['PressAnyKey'])
    getch()

def CreateMenu(description):
    result = []
    localizationMenu = localization.menues[description]
    for header in localizationMenu['Headers']:
        result.append(header)
    for item in localizationMenu['Items']:
        h, n, k = item
        result.append("{} - {}".format(h, n))
    return result

def GetActionType(description, key):
    actions = {}
    for item in localization.menues[description]['Items']:
        h, n, k = item
        if k == key:
            return h
    return -1

def PrintLines(lines):
    [print(line) for line in lines]

def ClearScreen():
    os.system('cls' if os.name=='nt' else 'clear')
