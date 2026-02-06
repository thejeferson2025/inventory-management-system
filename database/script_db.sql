-- Crear Base de Datos InventoryDB
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'InventoryDB')
BEGIN
    CREATE DATABASE InventoryDB;
END
GO

USE InventoryDB;
GO

-- 1. Tabla de Productos (Microservicio Products)
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL DROP TABLE dbo.Products;
GO

CREATE TABLE Products (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Category NVARCHAR(50),
    ImageUrl NVARCHAR(MAX),
    Price DECIMAL(18, 2) NOT NULL,
    Stock INT NOT NULL CHECK (Stock >= 0), 
    CreatedAt DATETIME DEFAULT GETDATE() 
);
GO

-- 2. Tabla de Transacciones (Microservicio Transactions)
IF OBJECT_ID('dbo.Transactions', 'U') IS NOT NULL DROP TABLE dbo.Transactions;
GO

CREATE TABLE Transactions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    Date DATETIME DEFAULT GETDATE(),
    Type NVARCHAR(20) NOT NULL CHECK (Type IN ('Purchase', 'Sale')), 
    
    ProductId UNIQUEIDENTIFIER NOT NULL, 
    
    Quantity INT NOT NULL CHECK (Quantity > 0), 
    UnitPrice DECIMAL(18, 2) NOT NULL,
    TotalPrice DECIMAL(18, 2) NOT NULL,
    Detail NVARCHAR(MAX),
    
     -- Relación referencial
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
GO

-- 3. Creacion de indices 
CREATE INDEX IX_Transactions_ProductId_Date 
ON Transactions (ProductId, Date);
GO