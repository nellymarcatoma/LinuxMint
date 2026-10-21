-- ============================================================================
-- NORTHWIND POINT OF SALE (POS) - DATABASE SETUP SCRIPT
-- Solution: NorthWind Point of Sale System
-- Database: NorthWindDB
-- ============================================================================

USE NorthWindDB;
GO

-- 1. UPDATE Customers TABLE
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Customers' AND COLUMN_NAME = 'FirstName')
BEGIN
    ALTER TABLE Customers ADD FirstName NVARCHAR(50) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Customers' AND COLUMN_NAME = 'LastName')
BEGIN
    ALTER TABLE Customers ADD LastName NVARCHAR(50) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Customers' AND COLUMN_NAME = 'Dni')
BEGIN
    ALTER TABLE Customers ADD Dni NVARCHAR(20) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Customers' AND COLUMN_NAME = 'Email')
BEGIN
    ALTER TABLE Customers ADD Email NVARCHAR(100) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Customers' AND COLUMN_NAME = 'Phone')
BEGIN
    ALTER TABLE Customers ADD Phone NVARCHAR(20) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Customers' AND COLUMN_NAME = 'Address')
BEGIN
    ALTER TABLE Customers ADD Address NVARCHAR(150) NULL;
END
GO

-- Populate existing Customers with complete POS data (using valid Ecuadorian DNI / Cédulas)
UPDATE Customers 
SET FirstName = 'Alfreds', 
    LastName = 'Futterkiste', 
    Dni = '1710034065', 
    Email = 'alfreds@northwind.com', 
    Phone = '0991234567', 
    Address = 'Av. Amazonas y Colón'
WHERE Id = 'ALFKI';

UPDATE Customers 
SET FirstName = 'Ana', 
    LastName = 'Trujillo', 
    Dni = '1803923455', 
    Email = 'ana.trujillo@gmail.com', 
    Phone = '0987654321', 
    Address = 'Av. Cevallos 12-34, Ambato'
WHERE Id = 'ANATR';

UPDATE Customers 
SET FirstName = 'Antonio', 
    LastName = 'Moreno', 
    Dni = '0102554789', 
    Email = 'antonio.moreno@hotmail.com', 
    Phone = '0978912345', 
    Address = 'Calle Bolívar 4-56'
WHERE Id = 'ANTON';

-- Insert additional seed Customers with valid Ecuadorian DNI / Cédulas
IF NOT EXISTS (SELECT * FROM Customers WHERE Id = 'C0001')
    INSERT INTO Customers (Id, Name, CurrentBalance, FirstName, LastName, Dni, Email, Phone, Address)
    VALUES ('C0001', 'Julia Salazar', 0, 'Julia', 'Salazar', '1804567897', 'julia@gmail.com', '0998877665', 'Av. Los Chasquis y Río Guayllabamba, Ambato');

IF NOT EXISTS (SELECT * FROM Customers WHERE Id = 'C0002')
    INSERT INTO Customers (Id, Name, CurrentBalance, FirstName, LastName, Dni, Email, Phone, Address)
    VALUES ('C0002', 'Carlos Mendoza', 0, 'Carlos', 'Mendoza', '1715632483', 'carlos.mendoza@outlook.com', '0995544332', 'Av. 12 de Octubre y Patria, Quito');

IF NOT EXISTS (SELECT * FROM Customers WHERE Id = 'C0003')
    INSERT INTO Customers (Id, Name, CurrentBalance, FirstName, LastName, Dni, Email, Phone, Address)
    VALUES ('C0003', 'Lucía Fernández', 0, 'Lucía', 'Fernández', '0923456784', 'lucia.fernandez@yahoo.com', '0981122334', 'Av. 9 de Octubre 500, Guayaquil');

IF NOT EXISTS (SELECT * FROM Customers WHERE Id = 'C0004')
    INSERT INTO Customers (Id, Name, CurrentBalance, FirstName, LastName, Dni, Email, Phone, Address)
    VALUES ('C0004', 'Pedro Morales', 0, 'Pedro', 'Morales', '1801234566', 'pedro.morales@uta.edu.ec', '0994433221', 'Cdla. Universitaria Huachi, Ambato');
GO

-- 2. UPDATE Products TABLE
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'Stock')
BEGIN
    ALTER TABLE Products ADD Stock INT NULL;
END
GO

UPDATE Products SET Stock = UnitsInStock WHERE Stock IS NULL;
GO

ALTER TABLE Products ALTER COLUMN Stock INT NOT NULL;
GO

-- Insert additional diverse tech products for Point of Sale
IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Laptop Dell Inspiron 15')
    INSERT INTO Products (Name, UnitsInStock, UnitPrice, Stock)
    VALUES ('Laptop Dell Inspiron 15', 15, 750.00, 15);

IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Teclado Mecanico RGB')
    INSERT INTO Products (Name, UnitsInStock, UnitPrice, Stock)
    VALUES ('Teclado Mecanico RGB', 30, 45.00, 30);

IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Mouse Inalambrico Logitech')
    INSERT INTO Products (Name, UnitsInStock, UnitPrice, Stock)
    VALUES ('Mouse Inalambrico Logitech', 40, 22.50, 40);

IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Monitor LG 24 Pulgadas FHD')
    INSERT INTO Products (Name, UnitsInStock, UnitPrice, Stock)
    VALUES ('Monitor LG 24 Pulgadas FHD', 20, 165.00, 20);

IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Auriculares Bluetooth Sony')
    INSERT INTO Products (Name, UnitsInStock, UnitPrice, Stock)
    VALUES ('Auriculares Bluetooth Sony', 25, 59.99, 25);

IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Memoria USB Kingston 64GB')
    INSERT INTO Products (Name, UnitsInStock, UnitPrice, Stock)
    VALUES ('Memoria USB Kingston 64GB', 0, 8.50, 0); -- Stock 0 for testing requirement 8

IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Impresora Epson EcoTank L3250')
    INSERT INTO Products (Name, UnitsInStock, UnitPrice, Stock)
    VALUES ('Impresora Epson EcoTank L3250', 8, 210.00, 8);
GO

-- 3. CREATE SalesOrders TABLE (POS Header)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SalesOrders')
BEGIN
    CREATE TABLE SalesOrders (
        OrderId INT IDENTITY(1,1) PRIMARY KEY,
        CustomerId NCHAR(5) NOT NULL,
        OrderDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        Subtotal DECIMAL(18, 2) NOT NULL,
        Tax DECIMAL(18, 2) NOT NULL,
        TotalAmount DECIMAL(18, 2) NOT NULL,
        UserId NVARCHAR(100) NOT NULL,
        CONSTRAINT FK_SalesOrders_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
    );
END
GO

-- 4. CREATE SalesOrderDetails TABLE (POS Details)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SalesOrderDetails')
BEGIN
    CREATE TABLE SalesOrderDetails (
        OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        UnitPrice DECIMAL(18, 2) NOT NULL,
        Quantity INT NOT NULL,
        Subtotal DECIMAL(18, 2) NOT NULL,
        CONSTRAINT FK_SalesOrderDetails_SalesOrders FOREIGN KEY (OrderId) REFERENCES SalesOrders(OrderId) ON DELETE CASCADE,
        CONSTRAINT FK_SalesOrderDetails_Products FOREIGN KEY (ProductId) REFERENCES Products(Id),
        CONSTRAINT UQ_SalesOrderDetails_Order_Product UNIQUE (OrderId, ProductId)
    );
END
GO

-- 5. STORED PROCEDURE FOR POS TRANSACTION
CREATE OR ALTER PROCEDURE sp_CreateSalesOrderWithStockReduction
    @CustomerId NCHAR(5),
    @UserId NVARCHAR(100),
    @Subtotal DECIMAL(18, 2),
    @Tax DECIMAL(18, 2),
    @TotalAmount DECIMAL(18, 2),
    @OrderId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO SalesOrders (CustomerId, OrderDate, Subtotal, Tax, TotalAmount, UserId)
        VALUES (@CustomerId, GETDATE(), @Subtotal, @Tax, @TotalAmount, @UserId);
        
        SET @OrderId = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

PRINT 'NorthWind Point of Sale database setup completed successfully.';
GO
