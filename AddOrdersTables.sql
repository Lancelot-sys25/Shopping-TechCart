-- Script to create missing tables 'orders' and 'order_details' for ProductIntro database.

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'orders')
BEGIN
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

    CREATE INDEX [IX_orders_accountId] ON [orders] ([accountId]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'order_details')
BEGIN
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

    CREATE INDEX [IX_order_details_orderId] ON [order_details] ([orderId]);
    CREATE INDEX [IX_order_details_productId] ON [order_details] ([productId]);
END
GO
