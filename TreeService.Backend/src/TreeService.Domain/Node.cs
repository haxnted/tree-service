using CSharpFunctionalExtensions;
using TreeService.Domain.Shared;
using TreeService.Domain.ValueObjects;

namespace TreeService.Domain;

/// <summary>
/// Представляет узел дерева
/// </summary>
public class Node
{
    public Guid Id { get; private set; }
    public Guid? ParentId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Node? Parent { get; private set; }
    public List<Node> ChildrenList { get; private set; }

    /// <summary>
    /// Приватный конструктор для создания узла с заданным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор узла</param>
    private Node(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Конструктор для создания узла с полными данными
    /// </summary>
    /// <param name="id">Идентификатор узла</param>
    /// <param name="title">Заголовок узла</param>
    /// <param name="description">Описание узла</param>
    /// <param name="createdAt">Дата создания</param>
    /// <param name="childrenList">Список дочерних узлов</param>
    /// <param name="parentId">Идентификатор родительского узла</param>
    /// <param name="parent">Родительский узел</param>
    public Node(
        Guid id, Title title, Description description, DateTime createdAt, IEnumerable<Node> childrenList,
        Guid? parentId = null, Node? parent = null)
    {
        Id = id;
        Title = title;
        Description = description;
        ParentId = parentId;
        Parent = parent;
        CreatedAt = createdAt;
        ChildrenList = childrenList.ToList();
    }

    /// <summary>
    /// Создает новый узел
    /// </summary>
    /// <param name="id">Идентификатор узла</param>
    /// <param name="title">Заголовок узла</param>
    /// <param name="description">Описание узла</param>
    /// <param name="parentId">Идентификатор родительского узла</param>
    /// <param name="parent">Родительский узел</param>
    /// <param name="childrenList">Список дочерних узлов</param>
    /// <returns>Результат операции с узлом или ошибкой</returns>
    public static Result<Node, Error> Create(
        Guid id, Title title, Description description, Guid? parentId = null, Node? parent = null,
        IEnumerable<Node>? childrenList = null)
    {
        var tempChildrenList = childrenList ?? new List<Node>();
        return new Node(id, title, description, DateTime.UtcNow, tempChildrenList, parentId, parent);
    }

    /// <summary>
    /// Обновляет информацию о заголовке и описании узла
    /// </summary>
    /// <param name="title">Новый заголовок</param>
    /// <param name="description">Новое описание</param>
    public void UpdateInformation(Title title, Description description)
    {
        Title = title;
        Description = description;
    }
}