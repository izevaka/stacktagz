use staging

select (select COUNT(question.id) from question) as Questions, 
(select COUNT(tsresult.id)  from tsresult)as Results,
(select COUNT(TSPeriod.id)  from TSPeriod)as Periods

select * from log where Url like '%){%' order by id 
select * from log where IPAddress like '%24.%'



--select * from tsresult

--select * from log order by id
