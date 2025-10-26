namespace MusgoEngine.Windowing;

public interface IWindowSystem : IDisposable
{
    void CreateWindow(WindowSettings windowSettings);
    IntPtr GetNativeHandle();
    void ShowWindow();
    void HideWindow();
    void CenterWindow();
    bool IsWindowOpen();
    void PollEvents();
    void DestroyWindow();
}
