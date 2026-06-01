using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvoiceApp.Models.DomainModels.ProductAggregates;

namespace InvoiceApp.Models.FluentConfigs
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p=>p.ProductID);
            builder.Property(p=>p.ProductName).IsRequired().HasMaxLength(50);
            builder.Property(p=>p.UnitPrice).IsRequired();

        }
    }
}
