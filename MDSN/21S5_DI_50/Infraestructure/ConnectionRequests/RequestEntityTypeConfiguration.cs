using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.ConnectionRequests;

namespace DDDSample1.Infrastructure.ConnectionRequests
{
    internal class RequestEntityTypeConfiguration : IEntityTypeConfiguration<ConnectionRequest>
    {
        public void Configure(EntityTypeBuilder<ConnectionRequest> builder)
        {
            //builder.ToTable("CRequests", SchemaNames.DDDSample1);
            builder.HasKey(b => b.Id);
        }
    }
}