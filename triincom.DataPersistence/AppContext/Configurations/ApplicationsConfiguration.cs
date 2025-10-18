
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using triincom.Core.Entities;

namespace triincom.DataPersistence.AppContext.Configurations
{
    internal class ApplicationsConfiguration : IEntityTypeConfiguration<ApplicationEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
               .HasConversion<string>()
               .IsRequired();
        }
    }
}
