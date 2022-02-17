using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Users;

namespace DDDSample1.Infrastructure.Users{

    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>{


            public void Configure(EntityTypeBuilder<User> builder){
                //builder.ToTable("Products", SchemaNames.DDDSample1);
                builder.HasKey(b => b.Id);
                builder.OwnsOne(b => b._Email).HasIndex(b=>b._Email).IsUnique();
            }


    }

}