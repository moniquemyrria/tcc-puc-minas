SELECT 
	CONVERT(datetime, CONVERT(varchar(4),SUBSTRING(periodo, 4,9))+Right(SUBSTRING(periodo, 0,3),2)+'01', 102) [dateInicial], 
	DATEADD(DD, - DAY (DATEADD(M, 1, CONVERT(datetime, CONVERT(varchar(4),SUBSTRING(periodo, 4,9))+Right(SUBSTRING(periodo, 0,3),2)+'01', 102))), DATEADD(M, 1, CONVERT(datetime, CONVERT(varchar(4),SUBSTRING(periodo, 4,9))+Right(SUBSTRING(periodo, 0,3),2)+'01', 102))) [dateFinal],
	periodo [period], isNull(col1,0) [expense], isNull(col2,0) [revenue], isNull((col2 - col1),0) [balance]
FROM (
	SELECT periodo, valor, 
	CASE 
		WHEN tipo = 'Receita'THEN 'Col2' -- + CAST(ROW_NUMBER() OVER(PARTITION BY periodo ORDER BY tipo) AS VARCHAR(10))
		WHEN tipo = 'Despesa'THEN 'Col1' -- + CAST(ROW_NUMBER() OVER(PARTITION BY periodo ORDER BY tipo) AS VARCHAR(10))
	END AS Coluna
	FROM (
		SELECT 
			(CASE WHEN DATEPART(MONTH, G.data) < 10 THEN '0' + CAST(DATEPART(MONTH, G.data) AS VARCHAR(2)) ELSE CAST(DATEPART(MONTH, G.data) AS VARCHAR(2)) END) + '-' + CAST(DATEPART(YEAR, G.data) AS VARCHAR(4)) 
				AS [periodo],
			SUM(G.value) 
				AS [valor],
			tipo
		FROM (	
			SELECT dateCreated [data], value, 'Receita' [tipo] FROM Revenue WITH(NOLOCK)
			WHERE revenueStatus = 'Recebido' AND isActive = 1
	
			UNION ALL
			SELECT paymentDate [data], value, 'Despesa' [tipo] from [Expense.Installments]
			INNER JOIN Expense ON (Expense.Id = [Expense.Installments].expenseId)
			WHERE paymentDate IS NOT NULL AND isActive = 1

		) AS G
		GROUP BY (CASE WHEN DATEPART(MONTH, G.data) < 10 THEN '0' + CAST(DATEPART(MONTH, G.data) AS VARCHAR(2)) ELSE CAST(DATEPART(MONTH, G.data) AS VARCHAR(2)) END) + '-' + CAST(DATEPART(YEAR, G.data) AS VARCHAR(4)), G.tipo
	) as T
) AS SourceTable
PIVOT (
    MAX(valor)
    FOR  coluna in (col1, col2)
) as P
WHERE SUBSTRING(P.periodo, 4,9) = '2023' 