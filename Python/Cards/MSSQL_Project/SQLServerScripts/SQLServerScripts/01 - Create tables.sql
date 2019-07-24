Create table Themes(Id integer not null primary key, Name char(255), Level integer);
Create table Cards(Id integer not null primary key, Primary_Side text, Secondary_Side text, Level integer);
Create table Accounts(Id integer not null primary key, Name char(255));
Create table ThemeCards(Theme_Id integer not null, Card_Id integer not null);
Create table AccountCards(Account_Id integer not null,Card_Id integer not null);
Create table Answers(Card_Id integer not null, Side_Order bit not null, Result integer not null, Level integer);
Create table CardAnswers(Card_Id integer not null);