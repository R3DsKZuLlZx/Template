using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Infrastructure.Database.Entities;

namespace Template.Infrastructure.Database.Configurations;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Username).HasMaxLength(50).IsRequired();
        builder.Property(x => x.FullName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
        builder.Property(x => x.DateOfBirth).IsRequired();

        builder.HasKey(x => x.Id);
    }
}
