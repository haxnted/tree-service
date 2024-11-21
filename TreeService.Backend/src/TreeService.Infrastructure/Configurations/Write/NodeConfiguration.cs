using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TreeService.Domain;
using TreeService.Domain.ValueObjects;

namespace TreeService.Infrastructure.Configurations.Write;

/// <summary>
/// Конфигурация для класса <see cref="Node"/> для записи
/// </summary>
public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable("nodes");

        builder.HasKey(n => n.Id)
            .HasName("id");
        
        builder.Property(n => n.ParentId)
            .HasColumnName("parent_id")
            .IsRequired(false);
        
        builder.OwnsOne(n => n.Title, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("title")
                .IsRequired() 
                .HasMaxLength(Title.MAX_TITLE_LENGHT); 
        });

        // Для Description
        builder.OwnsOne(n => n.Description, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("description")
                .IsRequired() 
                .HasMaxLength(Description.MAX_DESCRIPTION_LENGHT); 
        });
        
        builder.HasMany(n => n.ChildrenList)
            .WithOne(n => n.Parent)
            .HasForeignKey(n => n.ParentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); 
        
        builder.HasIndex(n => n.ParentId)
            .HasDatabaseName("idx_parent_id");
    }
}
