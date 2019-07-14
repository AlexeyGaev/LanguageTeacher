Select * from AllCards

Select Primary_Side, Secondary_Side, Card_Level, Theme_Name, Theme_Level 
from AllCards 
where Account_Name is Null

Select Primary_Side, Secondary_Side, Card_Level, Theme_Name, Theme_Level 
from AllCards 
where Account_Name like 'a1'

Select Primary_Side, Secondary_Side, Card_Level, Theme_Name, Theme_Level 
from AllCards 
where Account_Name like 'a2' 

Select Primary_Side, Secondary_Side, Card_Level 
from AllCards 
where Theme_Name is Null

Select Primary_Side, Secondary_Side, Card_Level 
from AllCards 
where Theme_Name like 't1'

Select Primary_Side, Secondary_Side, Card_Level 
from AllCards 
where Theme_Name like 't2'


