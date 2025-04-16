USE LojaN2;
go

CREATE SCHEMA Store;
go

CREATE SCHEMA Sales;
go

CREATE TABLE Store.Clients
(
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	First_Name varchar(50),
	Last_Name varchar(50),
	Email varchar(100),
	Telephone varchar(20),
	Localidade varchar (50),
	Uf varchar(2),
	Cep varchar(9),
	Logradouro varchar(150),
	Birth_Date Date,
	Client_Password varchar(50)
)

GO

CREATE TABLE Store.Items
(
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Item_Name varchar(100),
	Item_Description varchar(max),	
	Price decimal(18,2),
	Quantity int,
	Category_Id int,
	Imagem varbinary(max),
	FOREIGN KEY(Category_Id) REFERENCES Store.Items_Categories (Id)
);

GO

CREATE TABLE Store.Staffs
(
	Id int PRIMARY KEY NOT NULL IDENTITY(1,1),
	First_Name varchar(50),
	Last_Name varchar(50),
	Cpf varchar(14),
	Staff_Password varchar(50)
)

GO

CREATE TABLE Sales.Orders
(
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	DataCompra date
)

CREATE TABLE Sales.Orders_Items
(
	Id int PRIMARY KEY NOT NULL IDENTITY(1,1) ,
	Item_Id int,
	Order_Id int,
	Ordered_Quantity int,
	FOREIGN KEY(Order_Id) REFERENCES Sales.Orders (Id),
	FOREIGN KEY(Item_Id) REFERENCES Store.Items (Id)
)

GO

CREATE TABLE Store.Items_Categories
(
	Id int PRIMARY KEY NOT NULL IDENTITY (1,1),
	Category_Name varchar (50)
)

--criar ordem de compra // 3 é o id do cliente q vc criar a cima, ou coloque qq outro id
insert into Sales.Orders values (3)

--criar relação entre ordem de compra e items
insert into Sales.Orders_Items values
--Id          Item_Id     Order_Id    Ordered_Quantity
(1,           5,           1,           49),
(2,           7,           1,           50),
(3 ,          6   ,        1    ,       54),
(4   ,        10       ,    1   ,        3 ),
(5   ,        17      ,     1       ,    5 ),
(6     ,      1    ,       1      ,     1 )

SELECT * FROM Sales.Orders
SELECT * FROM Sales.Orders_Items
SELECT * FROM Store.Items