Select * from Themes;
Select * from Cards;
Select * from Accounts;
Select * from ThemeCards;
Select * from AccountCards;

Select Primary_side, Secondary_side, Card_Level, Theme_desc, Theme_Level, Account_Name
from Cards
left outer join ThemeCards on ThemeCards.Card_Id = Cards.Card_Id
left outer join Themes on ThemeCards.Theme_Id = Themes.Theme_Id
left outer join AccountCards on AccountCards.Card_Id = Cards.Card_Id
left outer join Accounts on ThemeCards.Card_Id = AccountCards.Card_Id and AccountCards.Account_Id = Accounts.Account_Id;

/*
q1, a1, 1, NULL, NULL, NULL
q2, a2, 2, t1, 1, a1
q3, a3, 3, t1, 1, NULL
q4, a4, 4, t2, 2, NULL
q5, a5, 5, t2, 2, NULL
*/

/*Select Cards.Card_Id from ThemeCards, Cards 
Select COUNT(Card_Id) from ThemeCards;
Select Theme_Id from Themes where Themes.Theme_desc like 'python';
Select 
Theme_desc as 'Theme description', 
Card_desc as 'Card Description', 
primary_side as 'Question', 
secondary_side as 'Answer'
from Themes, Cards
where Themes.Theme_id=cards.theme_id;
SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'themes';
SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'cards';
SELECT COUNT(*) FROM themes;
SELECT COUNT(*) FROM cards;
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME != 'sysdiagrams';
SELECT column_name, data_type, character_maximum_length, table_name, ordinal_position, is_nullable 
FROM information_schema.COLUMNS WHERE table_name LIKE 'themes'
ORDER BY ordinal_position;
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES*/
