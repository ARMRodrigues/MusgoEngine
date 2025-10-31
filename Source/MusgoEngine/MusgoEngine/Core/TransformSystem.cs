using System.Numerics;

namespace MusgoEngine.Core;

public class TransformSystem(EntityManager entityManager) : GameSystem
{
    private readonly Dictionary<Guid, List<Entity>> _childrenCache = new();
    private readonly Dictionary<Guid, Guid?> _parentCache = new();
    private readonly List<Entity> _rootEntities = [];

    public override void Initialize()
    {
        RebuildHierarchyCache();
    }

    public override void Update()
    {
        if (entityManager.IsHierarchyChanged)
        {
            RebuildHierarchyCache();
            entityManager.IsHierarchyChanged = false;
        }

        foreach (var rootEntity in _rootEntities)
        {
            if (!entityManager.TryGetComponent(rootEntity, out Transform rootTransform)) continue;

            UpdateTransformHierarchy(rootEntity, rootTransform, Matrix4x4.Identity);
        }
    }

    private void RebuildHierarchyCache()
    {
        _childrenCache.Clear();
        _parentCache.Clear();
        _rootEntities.Clear();

        foreach (var entity in entityManager.Entities)
        {
            if (!entityManager.TryGetComponent(entity, out Transform? _))
                continue;

            if (!entityManager.TryGetComponent(entity, out Hierarchy hierarchy) || hierarchy.ParentId == Guid.Empty)
            {
                _rootEntities.Add(entity);
                _parentCache[entity.Id] = null;
                continue;
            }

            if (entityManager.HasEntity(hierarchy.ParentId))
            {
                _parentCache[entity.Id] = hierarchy.ParentId;

                if (!_childrenCache.TryGetValue(hierarchy.ParentId, out var list))
                {
                    list = [];
                    _childrenCache[hierarchy.ParentId] = list;
                }

                list.Add(entity);
            }
            else
            {
                _rootEntities.Add(entity);
                _parentCache[entity.Id] = null;
            }
        }
    }

    private void UpdateTransformHierarchy(Entity entity, Transform transform, Matrix4x4 parentWorld)
    {
        if (transform.HasChanged)
        {
            transform.RebuildMatrices();
        }

        transform.WorldMatrix = (transform.LocalMatrix * parentWorld);
        //transform.WorldMatrix = parentWorld * transform.LocalMatrix;

        if (!_childrenCache.TryGetValue(entity.Id, out var children)) return;

        foreach (var childEntity in children)
        {
            if (!entityManager.TryGetComponent(childEntity, out Transform childTransform)) continue;

            UpdateTransformHierarchy(childEntity, childTransform, transform.WorldMatrix);
        }
    }
}
