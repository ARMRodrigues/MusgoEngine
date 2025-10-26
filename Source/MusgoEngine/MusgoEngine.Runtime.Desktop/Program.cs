using MusgoEngine;

var myWindowSettings = new WindowSettings()
{
    ApiType = GraphicApiType.EGL
};

var musgo = new MusgoApplication(myWindowSettings);

musgo.Start();
musgo.Run();
musgo.Stop();
