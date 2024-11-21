using CSharpFunctionalExtensions;
using TreeService.Domain;
using TreeService.Domain.Shared;

namespace TreeService.Application.Interfaces;

public interface ITreeRepository
{
    public Task Add(Node node, CancellationToken cancellationToken = default);
    
    public void Save(Node node);

    public Task<Result<Node, Error>> GetById(Guid nodeId, CancellationToken cancellationToken = default);

    public void Delete(Node node);
}
