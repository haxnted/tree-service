using TreeService.Contracts;
using TreeService.Domain;

namespace TreeService.Application.Interfaces;

public interface ITreeReadDbContext
{
    IQueryable<NodeDto> Nodes { get; }
}
