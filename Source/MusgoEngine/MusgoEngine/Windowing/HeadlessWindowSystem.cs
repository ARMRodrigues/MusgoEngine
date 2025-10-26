namespace MusgoEngine.Windowing;

public class HeadlessWindowSystem : IWindowSystem
{
    private bool _open = true;

    public HeadlessWindowSystem()
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            _open = false;

            Console.Out.WriteLine("Closing Musgo Engine...");
        };
    }

    public void CreateWindow(WindowSettings windowSettings)
    {
        _open = true;
    }

    public IntPtr GetNativeHandle() => IntPtr.Zero;

    public void ShowWindow() { }

    public void HideWindow() { }

    public void CenterWindow() { }

    public bool IsWindowOpen() => _open;

    public void PollEvents()
    {
        Thread.Sleep(1);
    }

    public void DestroyWindow()
    {
        _open = false;
    }

    public void Dispose()
    {
        _open = false;
    }
}
