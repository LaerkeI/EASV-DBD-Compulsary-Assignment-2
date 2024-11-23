CREATE DATABASE BookStoreDb;
GO

USE BookStoreDb;
GO

CREATE TABLE Customers (
    CustomerId INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE Books (
    ISBN NVARCHAR(30) PRIMARY KEY NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    StockLevel INT NOT NULL
);
GO

CREATE TABLE Orders (
    Id INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
    CustomerId INT NOT NULL,
    BookISBN NVARCHAR(30) NOT NULL,
    Quantity INT NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    FOREIGN KEY (BookISBN) REFERENCES Books(ISBN)
);
GO

INSERT INTO Customers (Name, Email)
VALUES
('Alice Johnson', 'alice@example.com'),
('Bob Smith', 'bob@example.com'),
('Charlie Brown', 'charlie@example.com');
GO

INSERT INTO Books (ISBN, Title, Price, StockLevel)
VALUES 
('111-1-11-111111-1', 'The Pragmatic Programmer', 39.99, 50),
('111-1-11-111111-2', 'Clean Code', 49.99, 40),
('111-1-11-111111-3', 'The Mythical Man-Month', 29.99, 30),
('111-1-11-111111-4', 'Introduction to Algorithms', 89.99, 20);
GO

INSERT INTO Orders (CustomerId, BookISBN, Quantity, TotalAmount)
VALUES
(1, '111-1-11-111111-1', 2, 79.98),
(2, '111-1-11-111111-3', 1, 30),
(3, '111-1-11-111111-4', 3, 269.97);
GO

ALTER LOGIN sa ENABLE;
ALTER LOGIN sa WITH PASSWORD = 'MyStrong!Passw0rd';
GO
