using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace shoppingTechCart.Entities;

public partial class ProductIntroContext : DbContext
{
    public ProductIntroContext()
    {
    }

    public ProductIntroContext(DbContextOptions<ProductIntroContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<SessionToken> SessionTokens { get; set; }

    public virtual DbSet<ViewHistory> ViewHistories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn!;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Account1).HasName("PK__accounts__EA162E101BB06DD0");

            entity.ToTable("accounts");

            entity.Property(e => e.Account1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("account");
            entity.Property(e => e.Birthday)
                .HasColumnType("datetime")
                .HasColumnName("birthday");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("firstName");
            entity.Property(e => e.Gender)
                .HasDefaultValue(true)
                .HasColumnName("gender");
            entity.Property(e => e.IsUse)
                .HasDefaultValue(false)
                .HasColumnName("isUse");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.Pass)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pass");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleInSystem)
                .HasDefaultValue(0)
                .HasColumnName("roleInSystem");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cart_ite__3213E83F6B201125");

            entity.ToTable("cart_items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("addedAt");
            entity.Property(e => e.ProductId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("productId");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.SessionId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sessionId");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cart_item__produ__3B75D760");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__categori__F04DF13A19F890C8");

            entity.ToTable("categories");

            entity.Property(e => e.TypeId).HasColumnName("typeId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(88)
                .HasColumnName("categoryName");
            entity.Property(e => e.Memo)
                .HasDefaultValue("")
                .HasColumnType("ntext")
                .HasColumnName("memo");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__2D10D16A83F1B25C");

            entity.ToTable("products");

            entity.Property(e => e.ProductId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("productId");
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("account");
            entity.Property(e => e.Brief)
                .HasMaxLength(2000)
                .HasDefaultValue("")
                .HasColumnName("brief");
            entity.Property(e => e.Discount)
                .HasDefaultValue(0)
                .HasColumnName("discount");
            entity.Property(e => e.PostedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("postedDate");
            entity.Property(e => e.Price)
                .HasDefaultValue(0)
                .HasColumnName("price");
            entity.Property(e => e.ProductImage)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("productImage");
            entity.Property(e => e.ProductName)
                .HasMaxLength(500)
                .HasColumnName("productName");
            entity.Property(e => e.TypeId).HasColumnName("typeId");
            entity.Property(e => e.Unit)
                .HasMaxLength(32)
                .HasDefaultValue("pcs")
                .HasColumnName("unit");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Account)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__products__accoun__3C69FB99");

            entity.HasOne(d => d.Type).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__products__typeId__3D5E1FD2");
        });

        modelBuilder.Entity<SessionToken>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__session___F267251E1BDB293A");

            entity.ToTable("session_tokens");

            entity.Property(e => e.AccountId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("accountId");
            entity.Property(e => e.Token)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("token");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Account).WithOne(p => p.SessionToken)
                .HasForeignKey<SessionToken>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__session_t__accou__3E52440B");
        });

        modelBuilder.Entity<ViewHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__view_his__3213E83F8D9074C9");

            entity.ToTable("view_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("accountId");
            entity.Property(e => e.ProductId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("productId");
            entity.Property(e => e.ViewedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("viewedAt");

            entity.HasOne(d => d.Account).WithMany(p => p.ViewHistories)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__view_hist__accou__3F466844");

            entity.HasOne(d => d.Product).WithMany(p => p.ViewHistories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__view_hist__produ__403A8C7D");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("orderId");

            entity.Property(e => e.AccountId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("accountId");

            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");

            entity.Property(e => e.TotalAmount).HasColumnName("totalAmount");

            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasColumnName("status");

            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(30)
                .HasColumnName("paymentMethod");

            entity.Property(e => e.PaidAt)
                .HasColumnType("datetime")
                .HasColumnName("paidAt");

            entity.HasOne(d => d.Account)
                .WithMany()
                .HasForeignKey(d => d.AccountId);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("order_details");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.OrderId).HasColumnName("orderId");

            entity.Property(e => e.ProductId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("productId");

            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.Property(e => e.UnitPrice).HasColumnName("unitPrice");

            entity.Property(e => e.Discount).HasColumnName("discount");

            entity.Property(e => e.LineTotal).HasColumnName("lineTotal");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

