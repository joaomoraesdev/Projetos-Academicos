USE LojaN2;
go

--PROCS GENERALIZADAS
CREATE PROCEDURE [dbo].[spConsulta]
(
	@id int ,
	@tabela varchar(max)
)
AS
BEGIN
	 DECLARE @sql varchar(max);
	 SET @sql = 'select * from ' + @tabela +
		' where id = ' + cast(@id as varchar(max))
	 EXEC(@sql)
END
GO

CREATE PROCEDURE [dbo].[spDelete]
(
	@id int ,
	@tabela varchar(max)
)
AS
BEGIN
	 DECLARE @sql varchar(max);
	 SET @sql = ' delete ' + @tabela +
		' where id = ' + cast(@id as varchar(max))
	 EXEC(@sql)
END
GO

CREATE PROCEDURE [dbo].[spListagem]
(
	 @tabela varchar(max),
	 @ordem varchar(max))
AS
BEGIN
	EXEC('select * from ' + @tabela +
		' order by ' + @ordem)
END
GO

--PROCS ESPECIFICAS
CREATE PROCEDURE [dbo].[spInsert_Orders_Items]
(
	@Id int ,
	@Item_Id int,
	@Order_Id int,
	@Ordered_Quantity int
)
AS
BEGIN
	INSERT INTO Sales.Orders_Items (Item_Id, Order_Id, Ordered_Quantity) 
	VALUES (@Item_Id, @Order_Id, @Ordered_Quantity)
END

GO

ALTER PROCEDURE [dbo].[spInsert_Orders]
(
	@Id int,
	@DataCompra date
)
AS
BEGIN
	INSERT INTO Sales.Orders (DataCompra) VALUES (@DataCompra)
END

GO

CREATE PROCEDURE [dbo].[spInsert_Items_Categories]
(
	@Id int,
	@Category_Name varchar (50)
)
AS
BEGIN
	INSERT INTO Store.Items_Categories (Category_Name) VALUES (@Category_Name)
END

GO

CREATE PROCEDURE [dbo].[spUpdate_Items_Categories]
(
	@Id int,
	@Category_Name varchar (50)
)
AS
BEGIN
	UPDATE Store.Items_Categories SET
		Category_Name = @Category_Name
		WHERE Id = @Id;
END

GO

CREATE PROCEDURE [dbo].[spInsert_Items]
(
	@id int,
	@Item_Description varchar(max),
	@Item_Name varchar(100),
	@Price decimal(18,2),
	@Quantity int,
	@Category_Id int,
	@Imagem varbinary(max)
)
AS
BEGIN
	INSERT INTO Store.Items (Item_Description, Item_Name, Price, Quantity, Category_Id, Imagem) 
	VALUES (@Item_Description, @Item_Name, @Price, @Quantity, @Category_Id, @Imagem)
END
GO

CREATE PROCEDURE [dbo].[spUpdate_Items]
(
	@id int,
	@Item_Description varchar(max),
	@Item_Name varchar(100),
	@Price decimal(18,2),
	@Quantity int,
	@Category_Id int,
	@Imagem varbinary(max)
)
AS
BEGIN
	UPDATE Store.Items SET 
		   Item_Name = @Item_Name, 
		   Item_Description = @Item_Description, 
		   Price = @Price,
		   Quantity = @Quantity, 
		   Category_Id = @Category_Id,
		   Imagem = @Imagem
		   WHERE Id = @Id
END
GO

CREATE PROCEDURE [dbo].[spListagem_Items]
(
	 @tabela varchar(max),
	 @ordem varchar(max))
AS
BEGIN
	SELECT Store.Items.*, Store.Items_Categories.Category_Name [CategoryName] 
	FROM Store.Items
	INNER JOIN Store.Items_Categories ON Store.Items.Category_Id = Store.Items_Categories.Id
END
GO

CREATE PROCEDURE [dbo].[spInsert_Clients]
(
	@Id int,
	@First_Name varchar(50),
	@Last_Name varchar(50),
	@Email varchar(100),
	@Telephone varchar(20),
	@Localidade varchar (50),
	@Uf varchar(2),
	@Cep varchar(9),
	@Logradouro varchar(150),
	@Birth_Date Date,
	@Client_Password varchar(50)
)
AS
BEGIN
	INSERT INTO Store.Clients (First_Name, Last_Name, Email, 
		Telephone, Localidade, Uf, Cep, Logradouro, Birth_Date, Client_Password) 
	VALUES (@First_Name, @Last_Name, @Email, @Telephone,			
		@Localidade, @Uf, @Cep, @Logradouro, @Birth_Date, @Client_Password)
END
GO

CREATE PROCEDURE [dbo].[spUpdate_Clients]
(
	@Id int,
	@First_Name varchar(50),
	@Last_Name varchar(50),
	@Email varchar(100),
	@Telephone varchar(20),
	@Localidade varchar (50),
	@Uf varchar(2),
	@Cep varchar(9),
	@Logradouro varchar(150),
	@Birth_Date Date,
	@Client_Password varchar(50)
)
AS
BEGIN
	UPDATE Store.Clients SET 
		   First_Name = @First_Name, 
		   Last_Name = @Last_Name, 
		   Email = @Email,
		   Telephone = @Telephone, 
		   Localidade = @Localidade, 
		   Uf = @Uf,
		   Cep = @Cep,
		   Logradouro = @Logradouro,
		   Birth_Date = @Birth_Date, 
		   Client_Password = @Client_Password
		   WHERE Id = @Id
END
GO

CREATE PROCEDURE [dbo].[spInsert_Staffs]
(
	@Id int,
	@First_Name varchar(50),
	@Last_Name varchar(50),
	@Cpf varchar(14),
	@Staff_Password varchar(50)
)
AS
BEGIN
	INSERT INTO Store.Staffs (First_Name, Last_Name, Cpf, Staff_Password) 
	VALUES (@First_Name, @Last_Name, @Cpf, @Staff_Password)
END
GO

CREATE PROCEDURE [dbo].[spUpdate_Staffs]
(
	@Id int,
	@First_Name varchar(50),
	@Last_Name varchar(50),
	@Cpf varchar(14),
	@Staff_Password varchar(50)
)
AS
BEGIN
	UPDATE Store.Staffs SET 
		   First_Name = @First_Name, 
		   Last_Name = @Last_Name, 
		   Cpf = @Cpf,
		   Staff_Password = @Staff_Password
		   WHERE Id = @Id
END
GO

CREATE PROCEDURE [dbo].[spVerifica_Login]
(
	@Email varchar(100)
)
AS
BEGIN
	SELECT Store.Clients.* FROM Store.Clients 
		WHERE Store.Clients.Email = @Email
END

GO

CREATE PROCEDURE [dbo].[spVerifica_LoginFunc]
(
	@Cpf varchar(14)
)
AS
BEGIN
	SELECT Store.Staffs.* FROM Store.Staffs 
		WHERE Store.Staffs.Cpf = @Cpf
END
go

create proc dbo.spItens_Vendidos as
select si.Item_Name, soi.Ordered_Quantity from Sales.Orders_Items soi
inner join Store.Items si on si.Id = soi.Item_Id

SELECT Store.Clients.* FROM Store.Clients