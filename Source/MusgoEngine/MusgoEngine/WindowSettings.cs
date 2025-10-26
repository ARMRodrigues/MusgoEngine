namespace MusgoEngine;

public struct WindowSettings()
{
    public int Width { get; private set; } = 1280;
    public int Height { get; private set; } = 720;
    public string Title { get; private set; } = "Musgo Engine";
    public bool Fullscreen { get; private set; } = false;
    public bool Minimized { get; private set; } = false;
    public bool Maximized { get; private set; } = false;
    public bool Vsync { get; private set; } = false;
    public GraphicApiType ApiType { get; set; } = GraphicApiType.EGL;
    public Platform Platform { get; set; } = Platform.Desktop;
}
