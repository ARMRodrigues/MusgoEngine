using MusgoEngine.Core;

namespace MusgoEngine.Windowing;

public interface IWindowSystem : IDisposable
{
    void CreateWindow(WindowSettings windowSettings);
    void ShowWindow();
    void HideWindow();
    bool IsWindowOpen();
    void PollEvents();
    void DestroyWindow();
}
