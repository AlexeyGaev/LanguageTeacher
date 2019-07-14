#import os
from os.path import exists

import localization
import files

#------------------------- Add cards dialog -----------------------------------

def InputAddCards():
    result = []
    InputAddCard(result)
    while True:
        if input(localization.dialogs['InputContinueCreateCard']) != '1':
            break
        InputAddCard(result)
    return result

def InputAccount():
    result = ()
    result += (input("Account : ").strip(), )
    return result

def InputTheme():
    result = ()
    result += (input("Theme : ").strip(), )
    result += (StringToInt(input("Theme Level : ")), )
    return result

def InputCard():
    result = ()
    result += (input("Primary Side : ").strip(), )
    result += (input("Secondary Side : ").strip(), )
    result += (StringToInt(input("Card Level : ")), )
    return result

def InputAddCard(cardsList):
    cardInfo = InputCard()
    themeInfo = InputTheme()
    accountInfo = InputAccount()
    if cardInfo != ("", "", "") or themeInfo != ("", "") or accountInfo != "":
        cardsList.append((cardInfo,themeInfo,accountInfo))

#-------------------- Import cards from file dialog ---------------------------

def InputImportCards():
    print(localization.headers['ImportCards'])
    file_name = input(localization.dialogs['InputFileName'])
    file_name = file_name.strip()
    if not file_name:
        print(localization.files['EmptyFileName'])
        return None
    if not exists(file_name):
        print(localization.files['InvalidFile'].format(file_name))
        return None
    linesFromFile = files.ReadFile(file_name)
    result = []
    for line in linesFromFile:
        lineItems = line.split(',')
        cardInfo = (lineItems[0].strip(), lineItems[1].strip(), StringToInt(lineItems[2].strip()))
        themeInfo = (lineItems[3].strip(), StringToInt(lineItems[4].strip()))
        accountInfo = ()
        accountInfo += (lineItems[5].strip(), )
        result.append((cardInfo, themeInfo, accountInfo))
    return result

def StringToInt(value):
    if not value:
        return None
    elif value == '':
        return 0
    else:
        return int(value)

#--------------------- Export to file dialog ----------------------------------

def InputExportCards():
    print(localization.headers['ExportCards'])
    file_name = input(localization.dialogs['InputFileName'])
    return file_name.strip()
