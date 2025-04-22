-- Création de la base de données
CREATE DATABASE ArtGallery;
GO

USE ArtGallery;
GO

-- Table des utilisateurs (pour l'authentification)
CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    IsAdmin BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    LastLogin DATETIME NULL
);
GO

-- Table pour les catégories d'œuvres d'art
CREATE TABLE Category (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
GO

-- Table pour les œuvres d'art
CREATE TABLE Artwork (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CreationDate DATE NULL,
    TechniqueUsed NVARCHAR(500) NULL,
    ImagePath NVARCHAR(500) NOT NULL,
    Price DECIMAL(10, 2) NULL,
    Size NVARCHAR(100) NULL,
    IsForSale BIT NOT NULL DEFAULT 1,
    CategoryId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Artwork_Category FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);
GO

-- Table pour les images supplémentaires d'une œuvre
CREATE TABLE ArtworkImage (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ArtworkId INT NOT NULL,
    ImagePath NVARCHAR(500) NOT NULL,
    IsMainImage BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ArtworkImage_Artwork FOREIGN KEY (ArtworkId) REFERENCES Artwork(Id) ON DELETE CASCADE
);
GO

-- Table pour les expositions
CREATE TABLE Exhibition (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Location NVARCHAR(200) NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    ImagePath NVARCHAR(500) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Table pour associer les œuvres aux expositions
CREATE TABLE ExhibitionArtwork (
    ExhibitionId INT NOT NULL,
    ArtworkId INT NOT NULL,
    CONSTRAINT PK_ExhibitionArtwork PRIMARY KEY (ExhibitionId, ArtworkId),
    CONSTRAINT FK_ExhibitionArtwork_Exhibition FOREIGN KEY (ExhibitionId) REFERENCES Exhibition(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ExhibitionArtwork_Artwork FOREIGN KEY (ArtworkId) REFERENCES Artwork(Id) ON DELETE CASCADE
);
GO

-- Table pour les messages du livre d'or
CREATE TABLE GoldenBookEntry (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    Message NVARCHAR(MAX) NOT NULL,
    IsApproved BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Table pour les liens externes (blogs d'amis, lieux d'exposition...)
CREATE TABLE ExternalLink (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Url NVARCHAR(500) NOT NULL,
    Description NVARCHAR(500) NULL,
    Type NVARCHAR(50) NULL, -- 'artist', 'exhibition', etc.
    IsActive BIT NOT NULL DEFAULT 1,
    SortOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Table pour les clients (acheteurs)
CREATE TABLE Customer (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Phone NVARCHAR(20) NULL,
    Address NVARCHAR(300) NULL,
    City NVARCHAR(100) NULL,
    PostalCode NVARCHAR(20) NULL,
    Country NVARCHAR(100) NULL,
    UserId INT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Customer_User FOREIGN KEY (UserId) REFERENCES [User](Id)
);
GO

-- Table pour les commandes
CREATE TABLE [Order] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(10, 2) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Shipped, Delivered, Cancelled
    ShippingAddress NVARCHAR(500) NULL,
    ShippingCity NVARCHAR(100) NULL,
    ShippingPostalCode NVARCHAR(20) NULL,
    ShippingCountry NVARCHAR(100) NULL,
    Notes NVARCHAR(MAX) NULL,
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Order_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
);
GO

-- Table pour les détails de commande
CREATE TABLE OrderDetail (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ArtworkId INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderId) REFERENCES [Order](Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetail_Artwork FOREIGN KEY (ArtworkId) REFERENCES Artwork(Id)
);
GO

-- Table pour la biographie de l'artiste
CREATE TABLE Biography (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX) NOT NULL,
    ImagePath NVARCHAR(500) NULL,
    LastUpdated DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Insertion des données initiales

-- Création de l'utilisateur administrateur (l'artiste) password artiste123
INSERT INTO [User] (Username, Email, PasswordHash, IsAdmin)
VALUES ('admin', 'artiste@example.com', 'AQAAAAEAACcQAAAAEBZCeK2wFJzhNipt08hQylgkgVtXjT7nCSBvTL7vmwlvSGtUrr+sl5YxAGpvhgo8nw==', 1);
GO

-- Création des catégories de base
INSERT INTO Category (Name, Description)
VALUES 
('Peintures', 'Œuvres réalisées avec différentes techniques de peinture'),
('Sculptures', 'Sculptures en différents matériaux et techniques'),
('Home Déco', 'Objets de décoration pour la maison'),
('Dessins', 'Croquis et dessins');
GO

-- Création d'une biographie par défaut
INSERT INTO Biography (Content)
VALUES ('Biographie de l''artiste à compléter.');
GO

-- Création d'index pour améliorer les performances

-- Index sur les œuvres d'art par catégorie
CREATE INDEX IX_Artwork_CategoryId ON Artwork(CategoryId);
GO

-- Index sur les œuvres d'art à vendre
CREATE INDEX IX_Artwork_IsForSale ON Artwork(IsForSale);
GO

-- Index sur les expositions
CREATE INDEX IX_Exhibition_Dates ON Exhibition(StartDate, EndDate);
GO

-- Index sur les commandes
CREATE INDEX IX_Order_Status ON [Order](Status);
GO
CREATE INDEX IX_Order_CustomerId ON [Order](CustomerId);
GO

-- Index sur le livre d'or
CREATE INDEX IX_GoldenBookEntry_IsApproved ON GoldenBookEntry(IsApproved);
GO

-- Procédures stockées

-- Procédure pour approuver une commande
CREATE OR ALTER PROCEDURE ApproveOrder
    @OrderId INT
AS
BEGIN
    UPDATE [Order]
    SET Status = 'Approved', UpdatedAt = GETDATE()
    WHERE Id = @OrderId AND Status = 'Pending';
END;
GO

-- Procédure pour obtenir les statistiques des ventes
CREATE OR ALTER PROCEDURE GetSalesStatistics
AS
BEGIN
    SELECT 
        c.Name AS CategoryName,
        COUNT(od.ArtworkId) AS SoldItems,
        SUM(od.Price) AS TotalRevenue
    FROM OrderDetail od
    JOIN Artwork a ON od.ArtworkId = a.Id
    JOIN Category c ON a.CategoryId = c.Id
    JOIN [Order] o ON od.OrderId = o.Id
    WHERE o.Status IN ('Approved', 'Shipped', 'Delivered')
    GROUP BY c.Name
    ORDER BY TotalRevenue DESC;
END;
GO

-- Procédure pour obtenir les articles les plus populaires
CREATE OR ALTER PROCEDURE GetPopularArtworks
    @Limit INT = 10
AS
BEGIN
    SELECT TOP (@Limit)
        a.Id,
        a.Title,
        a.ImagePath,
        COUNT(od.Id) AS TimesSold
    FROM Artwork a
    JOIN OrderDetail od ON a.Id = od.ArtworkId
    JOIN [Order] o ON od.OrderId = o.Id
    WHERE o.Status IN ('Approved', 'Shipped', 'Delivered')
    GROUP BY a.Id, a.Title, a.ImagePath
    ORDER BY TimesSold DESC;
END;
GO