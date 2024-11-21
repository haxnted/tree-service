using FluentAssertions;
using TreeService.Domain.ValueObjects;

namespace TreeService.Domain.Tests;

public class NodeTests
{
    /// <summary>
    /// Проверяет создание узла с корректными параметрами.
    /// </summary>
    [Fact(DisplayName = "Create Node with valid parameters")]
    public void CreateNode_ShouldCreateNode_WhenValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = Title.Create("Valid Title").Value;
        var description = Description.Create("Valid Description").Value;
        var parentId = (Guid?)null;
        var parent = (Node?)null;
        var childrenList = new List<Node>();

        // Act
        var result = Node.Create(id, title, description, parentId, parent, childrenList);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(id);
        result.Value.Title.Should().Be(title);
        result.Value.Description.Should().Be(description);
        result.Value.Parent.Should().BeNull();
        result.Value.ChildrenList.Should().BeEmpty();
    }

    /// <summary>
    /// Проверяет создание узла без родителя.
    /// </summary>
    [Fact(DisplayName = "Create Node without Parent")]
    public void CreateNode_ShouldCreateNodeWithoutParent_WhenParentIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = Title.Create("Valid Title").Value;
        var description = Description.Create("Valid Description").Value;
        var createdAt = DateTime.UtcNow;
        var parentId = (Guid?)null;
        var parent = (Node?)null;
        var childrenList = new List<Node>();

        // Act
        var result = Node.Create(id, title, description, parentId, parent, childrenList);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.ParentId.Should().BeNull();
    }

    /// <summary>
    /// Проверяет создание узла с дочерними элементами.
    /// </summary>
    [Fact(DisplayName = "Create Node with Children")]
    public void CreateNode_ShouldCreateNodeWithChildren_WhenChildrenProvided()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = Title.Create("Root Title").Value;
        var description = Description.Create("Root Description").Value;
        var createdAt = DateTime.UtcNow;
        var parentId = (Guid?)null;
        var parent = (Node?)null;
        var childNode = new Node(Guid.NewGuid(), Title.Create("Child Title").Value,
            Description.Create("Child Description").Value,
            DateTime.UtcNow, new List<Node>(), id);
        var childrenList = new List<Node> { childNode };

        // Act
        var result = Node.Create(id, title, description, parentId, parent, childrenList);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.ChildrenList.Should().HaveCount(1);
        result.Value.ChildrenList.First().Title.Value.Should().Be("Child Title");
    }

    /// <summary>
    /// Проверяет обновление информации об узле.
    /// </summary>
    [Fact(DisplayName = "Update Node Information")]
    public void UpdateInformation_ShouldUpdateNode_WhenValidDataProvided()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = Title.Create("Old Title").Value;
        var description = Description.Create("Old Description").Value;
        var createdAt = DateTime.UtcNow;
        var parentId = (Guid?)null;
        var parent = (Node?)null;
        var childrenList = new List<Node>();

        var node = Node.Create(id, title, description, parentId, parent, childrenList).Value;

        var newTitle = Title.Create("New Title").Value;
        var newDescription = Description.Create("New Description").Value;

        // Act
        node.UpdateInformation(newTitle, newDescription);

        // Assert
        node.Title.Should().Be(newTitle);
        node.Description.Should().Be(newDescription);
    }

    /// <summary>
    /// Проверяет создание узла с родителем, где родитель существует.
    /// </summary>
    [Fact(DisplayName = "Create Node with Parent")]
    public void CreateNode_ShouldSetParent_WhenParentProvided()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var parent = new Node(parentId, Title.Create("Parent Title").Value,
            Description.Create("Parent Description").Value,
            DateTime.UtcNow, new List<Node>(), null);

        var childId = Guid.NewGuid();
        var childTitle = Title.Create("Child Title").Value;
        var childDescription = Description.Create("Child Description").Value;
        var createdAt = DateTime.UtcNow;

        // Act
        var result = Node.Create(childId, childTitle, childDescription, parentId, parent, new List<Node>());

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Parent.Should().NotBeNull();
    }

    /// <summary>
    /// Проверяет создание узла с дочерними узлами и родителем, где родитель уже существует.
    /// </summary>
    [Fact(DisplayName = "Create Tree Structure with Parent and Children")]
    public void CreateNode_ShouldAssignParentAndChildren_WhenValidTreeStructure()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var parent = new Node(parentId, Title.Create("Parent").Value, Description.Create("Parent Desc").Value,
            DateTime.UtcNow,
            new List<Node>(), null);

        var childNodeId = Guid.NewGuid();
        var childNode = new Node(childNodeId, Title.Create("Child").Value, Description.Create("Child Desc").Value,
            DateTime.UtcNow,
            new List<Node>(), parentId, parent);

        var childrenList = new List<Node> { childNode };

        // Act
        var result = Node.Create(parentId, Title.Create("Root").Value, Description.Create("Root Desc").Value,
            parentId,
            parent, childrenList);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.ChildrenList.Should().Contain(childNode);
        result.Value.Parent.Should().NotBeNull();
    }
}
