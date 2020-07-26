USE STORE_PRODUCTS
GO
CREATE PROCEDURE dbo.GetProduct
(
	@productName VARCHAR(25) NULL,
	@isOnSale BIT = NULL,
	@productCategory VARCHAR(25) = NULL,
	@productDescription VARCHAR(200) = NULL
)
AS
BEGIN
	SELECT [PRD_CATEGORY] 'Category'
			,[PRD_NAME] 'Product Name'
			,[PRD_DESCRIPTION] 'Description'
			,[PRD_SALE] 'Available'
			,[PRD_PRICE] 'Price'
	FROM [dbo].[PRODUCTS]
	WHERE 
		PRD_NAME LIKE '%' + ISNULL(@productName, PRD_NAME) + '%'
		AND PRD_SALE = ISNULL(@isOnSale, PRD_SALE)
		AND PRD_CATEGORY LIKE '%' + ISNULL(@productCategory, PRD_CATEGORY)  + '%'
		AND PRD_DESCRIPTION LIKE '%' + ISNULL(@productDescription, PRD_DESCRIPTION)  + '%'
END
GO