CREATE TABLE [dbo].[Country]
(
    [CountryID] NVARCHAR(2) NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [PK_Country] PRIMARY KEY ([CountryID]) 
)
