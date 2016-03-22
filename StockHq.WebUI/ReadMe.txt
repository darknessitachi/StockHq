未开发计划
1.连续上涨天数
2.红绿颜色
3.按周统计
4.所属板块


http://hangfire.io/
https://github.com/aspnetboilerplate/aspnetboilerplate


http://www.aspnetboilerplate.com/Pages/Documents/Hangfire-Integration
http://www.jtable.org/GettingStarted?ref=homebuttons#Introduction


INSERT INTO Stocks (CODE, NAME) VALUES ('002055', '得润电子')

--获得上周周交易跌幅排序
SELECT *FROM Stocks AS A INNER JOIN StockHq AS B ON B.stockid=A.id WHERE B.[date] ='2016-03-18' AND B.[type]='week' order by B.change

--最近5个交易日中有三次收负，一次跌停，并且总跌幅大于12个点

DECLARE @dateTable TABLE (dealdate DATE)
INSERT @dateTable (dealdate)
    SELECT DISTINCT TOP 5 date
      FROM StockHq
      ORDER BY date DESC 
   

SELECT SUM(hq.change),MAX(hq.[open]),(SUM(hq.change)/MAX(hq.[open])),MAX(hq.[open]),hq.code,hq.name
  FROM StockHq (NOLOCK) hq
    INNER JOIN @dateTable da ON da.dealdate=hq.date 
    WHERE hq.chg<0
  GROUP BY hq.code,hq.name
  HAVING (SUM(hq.change)/MAX(hq.[open]))<=-0.12 AND MIN(chg)<=-10 AND COUNT(chg)>=3
  ORDER BY hq.code



//http://q.stock.sohu.com/hisHq?code=cn_002055&start=20160226&end=20160226&stat=1&order=D&period=d&callback=historySearchHandler&rt=jsonp