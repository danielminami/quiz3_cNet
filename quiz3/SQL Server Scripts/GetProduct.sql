CREATE procedure dbo.GetProduct
(
--TODO: parameters for other search criteria
@prod_name varchar(25) NULL,
@prod_sale bit = NULL
)
AS
BEGIN
	SELECT [PRD_CATEGORY]
		  ,[PRD_NAME]
		  ,[PRD_DESCRIPTION]
		  ,[PRD_SALE]
		  ,[PRD_PRICE]
	  FROM [STORE_PRODUCTS].[dbo].[PRODUCTS]
	where 
		PRD_NAME like '%' + ISNULL(@prod_name, PRD_NAME) + '%'
		AND PRD_SALE = ISNULL(@prod_sale, PRD_SALE)
END
GO
