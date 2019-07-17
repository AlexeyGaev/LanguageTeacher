import sys
import msvcrt
import localization
import operations
import dialogs

script = sys.argv

print(localization.messages['Start'].format(script))
print(localization.messages['DataBaseSync'])
print(localization.messages['PressAnyKey'])
msvcrt.getch()
connect = operations.Connect()
if connect == None:
    print(localization.messages['MainException'])
    print(localization.messages['PressAnyKey'])
    msvcrt.getch()
else:
    dialogs.StartDialog(connect, True)
