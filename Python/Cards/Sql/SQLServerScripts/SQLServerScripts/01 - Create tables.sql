Create table Themes(Theme_Id integer not null default 1 PRIMARY KEY, Theme_Desc text, Theme_Level text);
Create table Cards(Card_Id integer not null default 1 PRIMARY KEY,Primary_Side text,Secondary_Side text, Card_Level text);
Create table Accounts(Account_Id integer not null default 1 PRIMARY KEY,Account_Name text);
Create table Answers(Answer_Id integer not null default 1 PRIMARY KEY,BeginDateTime DateTime,EndDateTime DateTime,AnswerResult integer);
Create table ThemeCards(Theme_Id integer not null, Card_Id integer not null);
Create table AccountCards(Account_Id integer not null,Card_Id integer not null);
