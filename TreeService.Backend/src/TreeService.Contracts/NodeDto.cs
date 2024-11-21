namespace TreeService.Contracts;

/// <summary>
/// Класс для передачи ноды
/// </summary>
public record NodeDto
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public NodeDto? Parent { get; set; }
    public IEnumerable<NodeDto> ChildrenList { get; set; }
}