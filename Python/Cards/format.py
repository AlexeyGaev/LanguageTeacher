def GetLinesFromRows(rows, columnCount, delimeter):
    return [GetRowString(row, columnCount, delimeter) for row in rows]

def GetFormatColumn(column):
    if column == 0:
        return f'{column}'
    elif column == '0':
        return f'{column}'
    elif column:
        return f'{column}'
    else:
        return ''

def GetRowString(row, columnCount, delimeter):
    result = ""
    for i in range(columnCount):
        result += GetFormatColumn(row[i])
        if i < columnCount - 1:
            result += delimeter
    return result

def GetTableRowHeader(columnDescription):
    result = ""
    columnNames = [f'{i[0]}' for i in columnDescription]
    columnCount = len(columnNames)
    for i in range(columnCount):
        result += columnNames[i]
        if i < columnCount - 1:
            result += ", "
    return result
