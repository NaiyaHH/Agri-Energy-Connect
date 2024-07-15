CREATE DATABASE POE2;
USE POE2;


CREATE TABLE UserAccount(
UserID VARCHAR(50) PRIMARY KEY NOT NULL,
Email VARCHAR(50) NOT NULL,
FullName VARCHAR(50) NOT NULL,
UserRole VARCHAR(10) NOT NULL,
FOREIGN KEY (UserRole)  REFERENCES [Roles](UserRole)
);

CREATE TABLE Roles(
UserRole VARCHAR(10) PRIMARY KEY NOT NULL
)

INSERT INTO Roles VALUES ('Farmer');
INSERT INTO Roles VALUES ('Admin');


CREATE TABLE PRODUCT(
ProductID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
ProductName VARCHAR(50) NOT NULL,
ProductPrice FLOAT NOT NULL,
ProdDate DATE NOT NULL,
CategoryID INT NOT NULL,
UserID VARCHAR(50) NOT NULL,
FOREIGN KEY (CategoryID)  REFERENCES [Category](CategoryID),
FOREIGN KEY (UserID)  REFERENCES [UserAccount](UserID)
);

CREATE TABLE Category (
CategoryID INT PRIMARY KEY IDENTITY(1,1),
CategoryName VARCHAR(50) NOT NULL,
CategoryImg VARCHAR(255) 
);

CREATE PROCEDURE [dbo].[CREATE_USER]
(   
 @UserID VARCHAR(50) = NULL,
 @Email nvarchar(50) = NULL,
 @FullName VARCHAR(50) = NULL,
 @UserRole VARCHAR(10) = NULL,
 @status varchar(15)
 
)
AS
    BEGIN
	BEGIN
    IF @status = 'Insert'
        INSERT INTO UserAccount(UserID, Email, FullName, UserRole)
        VALUES (@UserID, @Email, @FullName, @UserRole )
    END
	END;


INSERT INTO Roles VALUES ('Farmer');
INSERT INTO Roles VALUES ('Admin');
INSERT INTO Roles VALUES ('Requested');
INSERT INTO Roles VALUES ('Denied');

SELECT * FROM ROLES;

INSERT INTO UserAccount VALUES ('osndGL9628WUfnazJtV1JZT7T2Z2', 'yusraadnan25@gmail.com', 'Yusra Adnan', 'Farmer')
INSERT INTO UserAccount VALUES ('US2OulTVSTOybizCniJ60SiiXyr1', 'admin1@agrienergyconnect.com', 'Naiya Haribhai', 'Admin')
INSERT INTO UserAccount VALUES ('x8LJWViW6ja1k9eAFURUQ1cJgRA3', 'admin2@agrienergyconnect.com', 'Aditi Haribhai', 'Admin')
INSERT INTO UserAccount VALUES ('CYRusGPxC4SVpLf1U32tmRQ8Xa72',	'qezia@gmail.com',	'Qezia Mack',	'Farmer')
INSERT INTO UserAccount VALUES ('w8bMidZTGCPWkAF3QChJpFuveFq2',	'qwezi@gmail.com',	'Qwezi Al'	,'Denied')
INSERT INTO UserAccount VALUES ('vQByIvdkD3UminMBD2rMKOWDfRb2',	'admin3@agrienergyconnect.edu.za',	'Bamini Haribhai', 	'Admin')
INSERT INTO UserAccount VALUES ('D6d2vo3EGdNKsSvDDVhrMhlFHc02',	'naiyaharibhai@gmail.com',	'Naiya Haribhai',	'Requested')


SELECT * FROM UserAccount;

INSERT INTO Category(CategoryName,CategoryImg) VALUES ('Fruit','https://m.media-amazon.com/images/I/71Xm+WCvi-L._AC_UL480_FMwebp_QL65_.jpg');
INSERT INTO Category(CategoryName,CategoryImg) VALUES ('Vegetables','https://m.media-amazon.com/images/I/51cZnLNRCdL._AC_US100_.jpg');
INSERT INTO Category(CategoryName,CategoryImg) VALUES ('Fertilizer','https://m.media-amazon.com/images/I/91qFW26lk3L.__AC_SX300_SY300_QL70_FMwebp_.jpg');
INSERT INTO Category(CategoryName,CategoryImg) VALUES ('Automated Solutions', 'https://m.media-amazon.com/images/I/71Ca-eYeB2L._AC_SX679_.jpg');
INSERT INTO Category(CategoryName,CategoryImg) VALUES ('Semi-Automated Solutions','https://m.media-amazon.com/images/S/aplus-media/mg/88301ed2-8c8c-4467-81c3-4e7568e890c0._SR300,300_.jpg');
INSERT INTO Category(CategoryName,CategoryImg) VALUES ('Green Energy Solutions', 'https://m.media-amazon.com/images/I/61NwRhurLQL._AC_SX679_.jpg');


SELECT * FROM Category;

INSERT INTO Product(ProductName, ProductPrice, ProdDate, CategoryID, UserID) VALUES ('Apples', 40.45, '13 May 2024', 1,'osndGL9628WUfnazJtV1JZT7T2Z2');
INSERT INTO Product(ProductName, ProductPrice, ProdDate, CategoryID, UserID) VALUES ('Cauliflower', 80.45, '14 May 2024', 2,'osndGL9628WUfnazJtV1JZT7T2Z2');
INSERT INTO Product(ProductName, ProductPrice, ProdDate, CategoryID, UserID) VALUES ('Nutrient Solution', 3000, '13 May 2024', 3,'D6d2vo3EGdNKsSvDDVhrMhlFHc02');
INSERT INTO Product(ProductName, ProductPrice, ProdDate, CategoryID, UserID) VALUES ('AI Powered Hydroponics', 300000, '24 May 2024', 4,'D6d2vo3EGdNKsSvDDVhrMhlFHc02');
INSERT INTO Product(ProductName, ProductPrice, ProdDate, CategoryID, UserID) VALUES ('Tractor', 500000, '01 January 2024', 5,'osndGL9628WUfnazJtV1JZT7T2Z2');
INSERT INTO Product(ProductName, ProductPrice, ProdDate, CategoryID, UserID) VALUES ('Wind-powered electricity', 500000, '13 May 2024', 6,'CYRusGPxC4SVpLf1U32tmRQ8Xa72');

SELECT * FROM Product;
SELECT * FROM UserAccount
SELECT * FROM Product;
SELECT * FROM Category;

