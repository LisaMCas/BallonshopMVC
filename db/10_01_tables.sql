USE BalloonShop

GO

CREATE TABLE Orders(
	OrderID INT IDENTITY(1,1) NOT NULL,
	DateCreated SMALLDATETIME NOT NULL CONSTRAINT DF_Orders_DateCreated  DEFAULT (getdate()),
	DateShipped SMALLDATETIME NULL,
	Verified BIT NOT NULL CONSTRAINT DF_Orders_Verified DEFAULT ((0)),
	Completed BIT NOT NULL CONSTRAINT DF_Orders_Completed DEFAULT ((0)),
	Canceled BIT NOT NULL CONSTRAINT DF_Orders_Canceled DEFAULT ((0)),
	Comments NVARCHAR(1000) NULL,
	CustomerName NVARCHAR(50) NULL,
	CustomerEmail NVARCHAR(50) NULL,
	ShippingAddress NVARCHAR(500) NULL,
 CONSTRAINT PK_Orders PRIMARY KEY CLUSTERED(OrderID ASC)
)

GO

CREATE TABLE OrderDetail(
	OrderID INT NOT NULL,
	ProductID INT NOT NULL,
	ProductName NVARCHAR(50) NOT NULL,
	Quantity INT NOT NULL,
	UnitCost MONEY NOT NULL,
	Subtotal  AS (Quantity*UnitCost),
 CONSTRAINT PK_OrderDetail PRIMARY KEY CLUSTERED(OrderID ASC, ProductID ASC)
)
  
GO

ALTER TABLE OrderDetail WITH CHECK ADD CONSTRAINT FK_OrderDetail_Orders FOREIGN KEY(OrderID)
REFERENCES Orders (OrderID)
