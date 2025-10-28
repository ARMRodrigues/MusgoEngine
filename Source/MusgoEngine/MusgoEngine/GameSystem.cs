namespace MusgoEngine;

public abstract class GameSystem
{
    // Global
    public virtual void Initialize() { }
    public virtual void Shutdown() { }

    // Frame cycle
    public virtual void BeginFrame() { }
    public virtual void Update() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Render() { }
    public virtual void EndFrame() { }
}
