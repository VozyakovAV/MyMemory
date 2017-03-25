SELECT question
FROM [u460423_mymemory].[dbo].[mem_items]
GROUP BY question
HAVING COUNT(*) > 1