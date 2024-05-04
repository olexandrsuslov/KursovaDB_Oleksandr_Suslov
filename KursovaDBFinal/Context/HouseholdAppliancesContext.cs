using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KursovaDBFinal.Models;

public partial class HouseholdAppliancesContext : DbContext
{
    public HouseholdAppliancesContext()
    {
    }

    public HouseholdAppliancesContext(DbContextOptions<HouseholdAppliancesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appliance> Appliances { get; set; }

    public virtual DbSet<ApplianceCategory> ApplianceCategories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<InventoryItem> InventoryItems { get; set; }

    public virtual DbSet<InventoryLocation> InventoryLocations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<SalesPromotion> SalesPromotions { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Top3RevenueByCategory> Top3RevenueByCategories { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=HouseholdAppliancesStore;Username=postgres;Password=suslik05;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appliance>(entity =>
        {
            entity.HasKey(e => e.ApplianceId).HasName("Appliances_pkey");

            entity.Property(e => e.ApplianceId)
                .HasDefaultValueSql("nextval('\"Products_product_id_seq\"'::regclass)")
                .HasColumnName("appliance_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.DateAdded)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_added");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhotoUrl)
                .HasColumnType("character varying")
                .HasColumnName("photo_url");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");

            entity.HasOne(d => d.Category).WithMany(p => p.Appliances)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk_category");
        });

        modelBuilder.Entity<ApplianceCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("Appliance_Categories_pkey");

            entity.ToTable("Appliance_Categories");

            entity.Property(e => e.CategoryId)
                .HasDefaultValueSql("nextval('\"Product_Categories_category_id_seq\"'::regclass)")
                .HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("Customers_pkey");

            entity.HasIndex(e => e.UserId, "user_id_fk_uc").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("user_id_fk");
        });

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.InventoryItemId).HasName("Inventory_pkey");

            entity.ToTable("Inventory_Items");

            entity.HasIndex(e => e.ApplianceId, "appliance_id_fk_uc").IsUnique();

            entity.Property(e => e.InventoryItemId)
                .HasDefaultValueSql("nextval('\"Stock_stock_id_seq\"'::regclass)")
                .HasColumnName("inventory_item_id");
            entity.Property(e => e.ApplianceId).HasColumnName("appliance_id");
            entity.Property(e => e.LastUpdated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_updated");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

            entity.HasOne(d => d.Appliance).WithOne(p => p.InventoryItem)
                .HasForeignKey<InventoryItem>(d => d.ApplianceId)
                .HasConstraintName("appliance_id_fk");

            entity.HasOne(d => d.Supplier).WithMany(p => p.InventoryItems)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("supplier_id_fk");
        });

        modelBuilder.Entity<InventoryLocation>(entity =>
        {
            entity.HasKey(e => e.InventoryLocationId).HasName("Inventory_Locations_pkey");

            entity.ToTable("Inventory_Locations");

            entity.HasIndex(e => e.InventoryItemId, "inventory_item_uc").IsUnique();

            entity.Property(e => e.InventoryLocationId).HasColumnName("inventory_location_id");
            entity.Property(e => e.InventoryItemId).HasColumnName("inventory_item_id");
            entity.Property(e => e.Row).HasColumnName("row");
            entity.Property(e => e.Shelf).HasColumnName("shelf");

            entity.HasOne(d => d.InventoryItem).WithOne(p => p.InventoryLocation)
                .HasForeignKey<InventoryLocation>(d => d.InventoryItemId)
                .HasConstraintName("inventory_item_id_fk");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("Orders_pkey");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.ManagerUserId).HasColumnName("manager_user_id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TotalSum)
                .HasPrecision(10, 2)
                .HasColumnName("total_sum");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("orders_ibfk_1");

            entity.HasOne(d => d.ManagerUser).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ManagerUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_manager");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_ibfk_2");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("Order_Details_pkey");

            entity.ToTable("Order_Details");

            entity.Property(e => e.OrderDetailId)
                .HasDefaultValueSql("nextval('\"Order_Items_order_item_id_seq\"'::regclass)")
                .HasColumnName("order_detail_id");
            entity.Property(e => e.ApplianceId).HasColumnName("appliance_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Appliance).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ApplianceId)
                .HasConstraintName("appliance_id_fk");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_id_fk");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("Order_Statuses_pkey");

            entity.ToTable("Order_Statuses");

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("status_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("Payment_Methods_pkey");

            entity.ToTable("Payment_Methods");

            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SalesPromotion>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("Sales_Promotions_pkey");

            entity.ToTable("Sales_Promotions");

            entity.HasIndex(e => e.ProductId, "appliance_id_fk_unique_constraint").IsUnique();

            entity.Property(e => e.SaleId)
                .HasDefaultValueSql("nextval('\"Discounts_Promotions_discount_id_seq\"'::regclass)")
                .HasColumnName("sale_id");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");

            entity.HasOne(d => d.Product).WithOne(p => p.SalesPromotion)
                .HasForeignKey<SalesPromotion>(d => d.ProductId)
                .HasConstraintName("appliance_id_fk");
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.HasKey(e => e.ShippingId).HasName("Shipping_pkey");

            entity.HasIndex(e => e.OrderId, "shipping_ibfk_uc").IsUnique();

            entity.Property(e => e.ShippingId)
                .HasDefaultValueSql("nextval('\"Shipping_Information_shipping_id_seq\"'::regclass)")
                .HasColumnName("shipping_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address");
            entity.Property(e => e.ShippingMethodId).HasColumnName("shipping_method_id");
            entity.Property(e => e.TrackingNumber)
                .HasMaxLength(100)
                .HasColumnName("tracking_number");

            entity.HasOne(d => d.Order).WithOne(p => p.Shipping)
                .HasForeignKey<Shipping>(d => d.OrderId)
                .HasConstraintName("shipping_ibfk_1");

            entity.HasOne(d => d.ShippingMethod).WithMany(p => p.Shippings)
                .HasForeignKey(d => d.ShippingMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shipping_method");
        });

        modelBuilder.Entity<ShippingMethod>(entity =>
        {
            entity.HasKey(e => e.ShippingMethodId).HasName("Shipping_Methods_pkey");

            entity.ToTable("Shipping_Methods");

            entity.Property(e => e.ShippingMethodId).HasColumnName("shipping_method_id");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("Suppliers_pkey");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.ContactName)
                .HasMaxLength(50)
                .HasColumnName("contact_name");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .HasColumnName("contact_phone");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Top3RevenueByCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("top_3_revenue_by_category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryRevenue).HasColumnName("category_revenue");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("Transactions_pkey");

            entity.HasIndex(e => e.OrderId, "transactions_ibfk_uc").IsUnique();

            entity.Property(e => e.TransactionId)
                .HasDefaultValueSql("nextval('\"Payment_Transactions_transaction_id_seq\"'::regclass)")
                .HasColumnName("transaction_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.TotalSum)
                .HasPrecision(10, 2)
                .HasColumnName("total_sum");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("transaction_date");

            entity.HasOne(d => d.Order).WithOne(p => p.Transaction)
                .HasForeignKey<Transaction>(d => d.OrderId)
                .HasConstraintName("transactions_ibfk_1");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_method");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_pkey");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("role_id_fk");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("User_Roles_pkey");

            entity.ToTable("User_Roles");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.Role)
                .HasColumnType("character varying")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
