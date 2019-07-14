Select Id, RTRIM(Themes.Name) as Name from Themes;
Select * from Cards;
Select Id, RTRIM(Accounts.Name) as Name from Accounts;
Select * from ThemeCards;
Select * from AccountCards;
Select * from Answers;

SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME != 'sysdiagrams';

SELECT table_name, column_name, data_type, character_maximum_length, ordinal_position, is_nullable 
FROM information_schema.COLUMNS
where table_name != 'sysdiagrams'
