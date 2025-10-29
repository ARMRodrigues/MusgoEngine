﻿using MusgoEngine;
using MusgoEngine.Game;

var game = new Game();

var myWindowSettings = new WindowSettings()
{
    Title = game.WindowTitle,
    ApiType = GraphicApiType.GLES,
};

var musgo = new MusgoApplication(myWindowSettings, game);

musgo.Start();
musgo.Run();
musgo.Stop();
