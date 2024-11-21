using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TreeService.Contracts;

namespace TreeService.Infrastructure.Configurations.Read;

/// <summary>
/// Конфигурация для класса <see cref="NodeDto"/> для чтения
/// </summary>
public class NodeDtoConfiguration : IEntityTypeConfiguration<NodeDto>
{
    public void Configure(EntityTypeBuilder<NodeDto> builder)
    {
        builder.ToTable("nodes");

        builder.HasKey(n => n.Id)
            .HasName("id");

        builder.Property(n => n.ParentId)
            .HasColumnName("parent_id");

        builder.Property(n => n.Title)
            .HasColumnName("title");

        builder.Property(n => n.Description)
            .HasColumnName("description");

        builder.HasMany(n => n.ChildrenList)
            .WithOne(n => n.Parent)
            .HasForeignKey(n => n.ParentId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}