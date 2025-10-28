namespace MusgoEngine;

public class Hierarchy : GameComponent
{
    public Guid ParentId { get; set; }
    public List<Guid> ChildrenId { get; set; } = [];
}
