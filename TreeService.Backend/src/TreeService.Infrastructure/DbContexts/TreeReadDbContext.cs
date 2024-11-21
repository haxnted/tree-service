using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TreeService.Application.Interfaces;
using TreeService.Contracts;

namespace TreeService.Infrastructure.DbContexts;

public class TreeReadDbContext : DbContext, ITreeReadDbContext 
{
    private readonly string _connectionString;
    
    public TreeReadDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IQueryable<NodeDto> Nodes => Set<NodeDto>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        
        // Disable change tracking
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
        {
            builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information)
                .AddConsole();
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TreeReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }
}
