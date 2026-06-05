using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvoiceApp.Models.DomainModels.OrderDetailAggregates;

namespace InvoiceApp.Models.FluentConfigs
{
    public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od=>od.OrderDetailID);
            builder.HasIndex(od => new { od.OrderHeaderID, od.ProductID }).IsUnique();

            builder.Property(od => od.UnitPrice).IsRequired();
            builder.Property(od => od.Quantity).IsRequired();

            builder.Property(od => od.IsDeleted).IsRequired();

            builder.HasOne(od => od.OrderHeader)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(od => od.OrderHeaderID)
                .IsRequired();

            builder.HasOne(od => od.Product)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(od => od.ProductID)
                .IsRequired();
            builder.HasQueryFilter(od => !od.IsDeleted);
        }
    }
}