using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TreeService.Domain;

namespace TreeService.Infrastructure.DbContexts;

public class TreeWriteDbContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<Node> Nodes => Set<Node>();

    public TreeWriteDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TreeReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }
    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
        {
            builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information)
                .AddConsole();
        });
    }
}
