namespace MusgoEngine.Graphics;

public interface IGraphicApi
{
    public void CreateApi(IntPtr nativeWindowHandle);
    public void BeginDraw();
    public void Draw();
    public void EndDraw();
    public void DestroyApi();
}
