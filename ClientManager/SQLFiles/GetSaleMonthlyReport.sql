USE [ClientManager_v2]

Create PROCEDURE GetMonthlySalesReport
AS
Begin
  
  SELECT Datename(month,[SaleDate]) as mname, sum(NoOfFollowUps) as calls
  Into #temp1
  FROM [dbo].[SaleActivity]   
  group by Datename(month,[SaleDate]) order by mname
  
  SELECT Datename(month,[SaleDate]) as mname, Count([status]) Orders
  Into #temp2
  FROM [dbo].[SaleActivity] where [status] = 6
  group by Datename(month,[SaleDate])  order by mname 

  SELECT Datename(month,[SaleDate]) as mname, Count([status]) Cancels
  Into #temp3
  FROM [dbo].[SaleActivity] where [status] = 4
  group by Datename(month,[SaleDate])  order by mname 

  select t1.mname, calls, isnull(t2.Orders,0) as Orders, isnull(t3.Cancels,0) as Cancels  from #temp1 t1 left outer join #temp2 t2 on t1.mname = t2.mname
  left outer join #temp3 t3 on t1.mname = t3.mname

  IF OBJECT_ID('tempdb..#temp1') IS NOT NULL 
  DROP TABLE #temp1; 

  IF OBJECT_ID('tempdb..#temp2') IS NOT NULL 
  DROP TABLE #temp2; 

  IF OBJECT_ID('tempdb..#temp3') IS NOT NULL 
  DROP TABLE #temp3; 

  End