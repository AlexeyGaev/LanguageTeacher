Select Id, RTRIM(Themes.Name) as Name from Themes;
Select * from Cards;
Select Id, RTRIM(Accounts.Name) as Name from Accounts;
Select * from ThemeCards;
Select * from AccountCards;
Select * from Answers;

Select table_name FROM information_schema.tables where table_name != 'sysdiagrams';

Select table_name, column_name, data_type, character_maximum_length, ordinal_position, is_nullable 
from information_schema.columns
where table_name != 'sysdiagrams'
