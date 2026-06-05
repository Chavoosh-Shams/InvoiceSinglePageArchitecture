using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvoiceApp.Models.DomainModels.OrderHeaderAggregates;

namespace InvoiceApp.Models.FluentConfigs
{
    public class OrderHeaderConfig : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.HasKey(oh => oh.OrderHeaderID);
            builder.Property(oh=>oh.OrderDate).IsRequired();
            builder.Property(oh => oh.ShipCity).IsRequired().HasMaxLength(50);
            builder.Property(oh => oh.ShipAddress).IsRequired().HasMaxLength(200);
            builder.Property(oh => oh.IsDeleted).IsRequired();
            builder.HasOne(oh => oh.Customer)
                .WithMany(oh => oh.OrderHeaders)
                .HasForeignKey(oh => oh.CustomerID)
                .IsRequired();
            builder.HasQueryFilter(oh => !oh.IsDeleted);
        }
    }
}