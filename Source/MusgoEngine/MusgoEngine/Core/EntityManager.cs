using System.Collections.Concurrent;

namespace MusgoEngine.Core
{
    public class EntityManager
    {
        private readonly HashSet<Entity> _entities = [];
        private readonly ConcurrentDictionary<Entity, ConcurrentDictionary<Type, GameComponent>> _components = new();
        private readonly ConcurrentQueue<Entity> _toRemove = new();

        public IReadOnlyCollection<Entity> Entities
        {
            get
            {
                lock (_entities)
                {
                    return new List<Entity>(_entities);
                }
            }
        }

        public Entity CreateEntity()
        {
            var entity = new Entity(Guid.NewGuid());

            lock (_entities)
            {
                _entities.Add(entity);
            }

            _components[entity] = new ConcurrentDictionary<Type, GameComponent>();
            return entity;
        }

        public void RemoveEntity(Entity entity)
        {
            _toRemove.Enqueue(entity);
        }

        public void ProcessRemovals()
        {
            while (_toRemove.TryDequeue(out var entity))
            {
                if (_components.TryRemove(entity, out var entityComponents))
                {
                    foreach (var component in entityComponents.Values)
                    {
                        if (component is IDisposable disposable)
                            disposable.Dispose();
                    }
                }

                lock (_entities)
                {
                    _entities.Remove(entity);
                }
            }
        }

        public void AddChild(Entity parent, Entity child)
        {
            if (!TryGetComponent(parent, out Hierarchy? existingHierarchy))
            {
                var hierarchy = new Hierarchy
                {
                    ParentId = parent.Id,
                    ChildrenId = [child.Id]
                };
                AddComponent(parent, hierarchy);
            }
            else
            {
                if (existingHierarchy == null) return;

                lock (existingHierarchy)
                {
                    existingHierarchy.ChildrenId.Add(child.Id);
                }
            }
        }

        public void AddComponent<T>(Entity entity, T component) where T : GameComponent
        {
            _components[entity][typeof(T)] = component;
        }

        public T GetComponent<T>(Entity entity) where T : GameComponent
        {
            if (_components.TryGetValue(entity, out var components) &&
                components.TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }

            throw new KeyNotFoundException($"Component {typeof(T).Name} not found for Entity {entity.Id}");
        }

        public IEnumerable<T> GetComponents<T>() where T : GameComponent
        {
            foreach (var kvp in _components)
            {
                var components = kvp.Value;

                if (!components.TryGetValue(typeof(T), out var component)) continue;

                if (component is T typedComponent)
                    yield return typedComponent;
            }
        }

        public bool TryGetComponent<T>(Entity entity, out T component) where T : GameComponent?
        {
            if (_components.TryGetValue(entity, out var comps) &&
                comps.TryGetValue(typeof(T), out var baseComp) &&
                baseComp is T typedComp)
            {
                component = typedComp;
                return true;
            }

            component = null!;
            return false;
        }
    }
}
