namespace MusgoEngine.Core;

public class EntitiesCountGameSystem(EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;

    public override void Update()
    {
        var entities = _entityManager.Entities;
        Console.WriteLine($"Entities count: {entities.Count}");
    }
}
