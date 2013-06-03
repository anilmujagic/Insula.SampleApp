CREATE TABLE [dbo].[Customer]
(
    [CustomerID] INT NOT NULL IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [City] NVARCHAR(50) NULL, 
    [CountryID] NVARCHAR(2) NULL, 
    CONSTRAINT [PK_Customer] PRIMARY KEY ([CustomerID]), 
    CONSTRAINT [FK_Customer_Country] FOREIGN KEY ([CountryID]) REFERENCES [Country]([CountryID]) 
)
