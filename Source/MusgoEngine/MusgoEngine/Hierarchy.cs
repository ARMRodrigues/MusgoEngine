namespace MusgoEngine;

public class Hierarchy : GameComponent
{
    public Guid ParentId { get; init; }

    public List<Guid> ChildrenId { get; set; } = [];
}
