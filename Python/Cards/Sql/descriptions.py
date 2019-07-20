tables = [ 'Themes', 'Cards', 'Accounts', 'ThemeCards', 'AccountCards', 'Answers', 'AllCards' ]
views = ['AllCards']
table_columns = {
    'Themes': [
        ('Id', 'int', None, 1, 'NO'),
        ('Name', 'char', 255, 2, 'YES'),
        ('Level', 'int', None, 3, 'YES')
    ],
    'Cards': [
        ('Id', 'int', None, 1, 'NO'),
        ('Primary_Side', 'text', 2147483647, 2, 'YES'),
        ('Secondary_Side', 'text', 2147483647, 3, 'YES'),
        ('Level', 'int', None, 4, 'YES')
        ],
    'Accounts': [
        ('Id', 'int', None, 1, 'NO'),
        ('Name', 'char', 255, 2, 'YES')
        ],
    'ThemeCards': [
        ('Theme_Id', 'int', None, 1, 'NO'),
        ('Card_Id', 'int', None, 2, 'NO')
        ],
    'AccountCards': [
        ('Account_Id', 'int', None, 1, 'NO'),
        ('Card_Id', 'int', None, 2, 'NO')
        ],
    'Answers': [
        ('Card_Id', 'int', None, 1, 'NO'),
        ('Side_Order', 'bit', None, 2, 'NO'),
        ('Result', 'int', None, 3, 'NO'),
        ('Level', 'int', None, 4, 'YES')
        ],
    'AllCards': [
        ('Primary_Side', 'text', 2147483647, 1, 'YES'),
        ('Secondary_Side', 'text', 2147483647, 2, 'YES'),
        ('Card_Level', 'int', None, 3, 'YES'),
        ('Theme_Name', 'varchar', 255, 4, 'YES'),
        ('Theme_Level', 'int', None, 5, 'YES'),
        ('Account_Name', 'varchar', 255, 6, 'YES')
        ]
    }
table_column_descriptions = [
    'Column_Name',
    'Data_Type',
    'Character_maximum_length',
    'Ordinal_position'
    'Is_nullable'
]
