using Microsoft.EntityFrameworkCore;
using InvoiceApp.Models.FluentConfigs;
using InvoiceApp.Models.DomainModels.ProductAggregates;
using InvoiceApp.Models.DomainModels.CustomerAggregates;
using InvoiceApp.Models.DomainModels.OrderDetailAggregates;
using InvoiceApp.Models.DomainModels.OrderHeaderAggregates;

namespace InvoiceApp.Models
{
    public class ProjectDbContext : DbContext
    {

        #region [- Ctor -]
        public ProjectDbContext(DbContextOptions options) : base(options)
        {

        }
        #endregion

        #region [- DbSet<> -]
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        #endregion

        #region [- OnModelCreating() -]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region [- ApplyConfiguration() -]
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new OrderHeaderConfig());
            modelBuilder.ApplyConfiguration(new OrderDetailConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig()); 
            #endregion

        } 
        #endregion

    }
}
