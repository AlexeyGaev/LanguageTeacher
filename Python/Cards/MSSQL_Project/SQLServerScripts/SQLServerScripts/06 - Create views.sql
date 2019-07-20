Create view AllCards as 
Select 
Primary_Side, 
Secondary_Side, 
Cards.Level as Card_Level, 
RTRIM(Themes.Name) as Theme_Name, 
Themes.Level as Theme_Level, 
RTRIM(Accounts.Name) as Account_Name
from Cards
left join ThemeCards on ThemeCards.Card_Id = Cards.Id
left join Themes on ThemeCards.Theme_Id = Themes.Id
left join AccountCards on AccountCards.Card_Id = Cards.Id
left join Accounts on AccountCards.Account_Id = Accounts.Id
