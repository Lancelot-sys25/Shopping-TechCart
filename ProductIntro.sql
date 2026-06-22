CREATE TABLE [accounts] (
    [account] varchar(20) NOT NULL,
    [pass] varchar(100) NOT NULL,
    [lastName] nvarchar(50) NULL,
    [firstName] nvarchar(30) NOT NULL,
    [birthday] datetime NULL,
    [gender] bit NULL DEFAULT CAST(1 AS bit),
    [phone] nvarchar(20) NULL,
    [isUse] bit NULL DEFAULT CAST(0 AS bit),
    [roleInSystem] int NULL DEFAULT 0,
    CONSTRAINT [PK__accounts__EA162E101BB06DD0] PRIMARY KEY ([account])
);
GO


CREATE TABLE [categories] (
    [typeId] int NOT NULL IDENTITY,
    [categoryName] nvarchar(88) NOT NULL,
    [memo] ntext NULL DEFAULT N'',
    CONSTRAINT [PK__categori__F04DF13A19F890C8] PRIMARY KEY ([typeId])
);
GO


CREATE TABLE [orders] (
    [orderId] int NOT NULL IDENTITY,
    [accountId] varchar(20) NOT NULL,
    [createdAt] datetime NULL,
    [totalAmount] int NOT NULL,
    [status] nvarchar(30) NOT NULL,
    [paymentMethod] nvarchar(30) NULL,
    [paidAt] datetime NULL,
    CONSTRAINT [PK_orders] PRIMARY KEY ([orderId]),
    CONSTRAINT [FK_orders_accounts_accountId] FOREIGN KEY ([accountId]) REFERENCES [accounts] ([account]) ON DELETE CASCADE
);
GO


CREATE TABLE [session_tokens] (
    [accountId] varchar(20) NOT NULL,
    [token] varchar(100) NOT NULL,
    [updatedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__session___F267251E1BDB293A] PRIMARY KEY ([accountId]),
    CONSTRAINT [FK__session_t__accou__3E52440B] FOREIGN KEY ([accountId]) REFERENCES [accounts] ([account])
);
GO


CREATE TABLE [products] (
    [productId] varchar(10) NOT NULL,
    [productName] nvarchar(500) NOT NULL,
    [productImage] varchar(max) NULL DEFAULT '',
    [brief] nvarchar(2000) NULL DEFAULT N'',
    [postedDate] datetime NULL DEFAULT ((getdate())),
    [typeId] int NOT NULL,
    [account] varchar(20) NOT NULL,
    [unit] nvarchar(32) NULL DEFAULT N'pcs',
    [price] int NULL DEFAULT 0,
    [discount] int NULL DEFAULT 0,
    CONSTRAINT [PK__products__2D10D16A83F1B25C] PRIMARY KEY ([productId]),
    CONSTRAINT [FK__products__accoun__3C69FB99] FOREIGN KEY ([account]) REFERENCES [accounts] ([account]),
    CONSTRAINT [FK__products__typeId__3D5E1FD2] FOREIGN KEY ([typeId]) REFERENCES [categories] ([typeId])
);
GO


CREATE TABLE [cart_items] (
    [id] int NOT NULL IDENTITY,
    [sessionId] varchar(100) NOT NULL,
    [productId] varchar(10) NOT NULL,
    [quantity] int NULL DEFAULT 1,
    [addedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__cart_ite__3213E83F6B201125] PRIMARY KEY ([id]),
    CONSTRAINT [FK__cart_item__produ__3B75D760] FOREIGN KEY ([productId]) REFERENCES [products] ([productId])
);
GO


CREATE TABLE [order_details] (
    [id] int NOT NULL IDENTITY,
    [orderId] int NOT NULL,
    [productId] varchar(10) NOT NULL,
    [quantity] int NOT NULL,
    [unitPrice] int NOT NULL,
    [discount] int NOT NULL,
    [lineTotal] int NOT NULL,
    CONSTRAINT [PK_order_details] PRIMARY KEY ([id]),
    CONSTRAINT [FK_order_details_orders_orderId] FOREIGN KEY ([orderId]) REFERENCES [orders] ([orderId]) ON DELETE CASCADE,
    CONSTRAINT [FK_order_details_products_productId] FOREIGN KEY ([productId]) REFERENCES [products] ([productId]) ON DELETE CASCADE
);
GO


CREATE TABLE [view_history] (
    [id] int NOT NULL IDENTITY,
    [accountId] varchar(20) NOT NULL,
    [productId] varchar(10) NOT NULL,
    [viewedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__view_his__3213E83F8D9074C9] PRIMARY KEY ([id]),
    CONSTRAINT [FK__view_hist__accou__3F466844] FOREIGN KEY ([accountId]) REFERENCES [accounts] ([account]),
    CONSTRAINT [FK__view_hist__produ__403A8C7D] FOREIGN KEY ([productId]) REFERENCES [products] ([productId])
);
GO


CREATE INDEX [IX_cart_items_productId] ON [cart_items] ([productId]);
GO


CREATE INDEX [IX_order_details_orderId] ON [order_details] ([orderId]);
GO


CREATE INDEX [IX_order_details_productId] ON [order_details] ([productId]);
GO


CREATE INDEX [IX_orders_accountId] ON [orders] ([accountId]);
GO


CREATE INDEX [IX_products_account] ON [products] ([account]);
GO


CREATE INDEX [IX_products_typeId] ON [products] ([typeId]);
GO


CREATE INDEX [IX_view_history_accountId] ON [view_history] ([accountId]);
GO


CREATE INDEX [IX_view_history_productId] ON [view_history] ([productId]);
GO

-- Seed Data (Tài khoản và Dữ liệu mẫu)
INSERT INTO [accounts] ([account], [pass], [lastName], [firstName], [birthday], [gender], [phone], [isUse], [roleInSystem])
VALUES 
('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', N'System', N'Admin', '1990-01-01', 1, '0987654321', 1, 1),
('user', '04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb', N'Regular', N'User', '1995-05-05', 0, '0123456789', 1, 0);
GO

SET IDENTITY_INSERT [categories] ON;
INSERT INTO [categories] ([typeId], [categoryName], [memo])
VALUES 
(1, N'Laptop', N'Công nghệ máy tính xách tay'),
(2, N'Smartphone', N'Điện thoại thông minh'),
(3, N'Tablet', N'Máy tính bảng');
SET IDENTITY_INSERT [categories] OFF;
GO

INSERT INTO [products] ([productId], [productName], [productImage], [brief], [postedDate], [typeId], [account], [unit], [price], [discount])
VALUES 
('P001', N'Laptop Asus ROG', '', N'Laptop gaming cấu hình cao', GETDATE(), 1, 'admin', N'pcs', 30000000, 1000000),
('P002', N'iPhone 14 Pro Max', '', N'Điện thoại flagship từ Apple', GETDATE(), 2, 'admin', N'pcs', 25000000, 500000),
('P003', N'iPad Air 5', '', N'Máy tính bảng mỏng nhẹ, mạnh mẽ', GETDATE(), 3, 'admin', N'pcs', 15000000, 300000);
GO



