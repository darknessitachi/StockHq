--SQL

CREATE TABLE [dbo].[Stocks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](20) NOT NULL,
	[name] [varchar](20) NULL,
	[lowchg] [decimal](18, 2) NULL,
	[lowclose] [decimal](18, 2) NULL,
	[lowrate] [decimal](18, 2) NULL,
	[lowvolume] [int] NULL,
	[lowturnover] [decimal](18, 2) NULL,
	[lowbegin] [varchar](20) NULL,
	[lowend] [varchar](20) NULL,
	[lowdays] [int] NULL,
	[highchg] [decimal](18, 2) NULL,
	[highclose] [decimal](18, 2) NULL,
	[highrate] [decimal](18, 2) NULL,
	[highvolume] [int] NULL,
	[highturnover] [decimal](18, 2) NULL,
	[highbegin] [varchar](20) NULL,
	[highend] [varchar](20) NULL,
	[highdays] [int] NULL,
 CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[StockHq](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[stockid] [int] NULL,
	[date] [varchar](10) NULL,
	[open] [decimal](18, 2) NULL,
	[close] [decimal](18, 2) NULL,
	[highest] [decimal](18, 2) NULL,
	[lowest] [decimal](18, 2) NULL,
	[change] [decimal](18, 2) NULL,
	[chg] [decimal](18, 2) NULL,
	[volume] [int] NULL,
	[turnover] [decimal](18, 2) NULL,
	[rate] [decimal](18, 2) NULL,
	[type] [varchar](20) NULL,
 CONSTRAINT [PK_StockHq] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'涨跌额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockHq', @level2type=N'COLUMN',@level2name=N'change'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'涨跌幅 %' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockHq', @level2type=N'COLUMN',@level2name=N'chg'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成交量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockHq', @level2type=N'COLUMN',@level2name=N'volume'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成交金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockHq', @level2type=N'COLUMN',@level2name=N'turnover'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'换手率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockHq', @level2type=N'COLUMN',@level2name=N'rate'
GO




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