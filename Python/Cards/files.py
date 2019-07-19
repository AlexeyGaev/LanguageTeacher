def ReadFile(file_name):
    try:
        txt_file = open(file_name, 'r')
        lines = [line.strip() for line in txt_file]
    except:
        txt_file.close()
        return None
    return lines

def WriteFile(file_name, rows):
    try:
        txt_file = open(file_name, 'w')
        [txt_file.write(line + "\n") for line in rows]
    except:
        txt_file.close()
        return False
    else:
        txt_file.close()
        return True
