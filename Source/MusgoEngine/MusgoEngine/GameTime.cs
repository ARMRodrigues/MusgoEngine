using System.Diagnostics;

namespace MusgoEngine;

public class GameTime(float fixedDeltaTime = 1f / 60f)
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    private float _previousTime;
    private float _accumulator;

    public static float DeltaTime { get; private set; }

    public float FixedDeltaTime { get; } = fixedDeltaTime;
    public float TotalTime { get; private set; }
    public int FrameCount { get; private set; }

    public void Update()
    {
        float currentTime = (float)_stopwatch.Elapsed.TotalSeconds;
        DeltaTime = currentTime - _previousTime;
        _previousTime = currentTime;
        TotalTime = currentTime;
        FrameCount++;
        _accumulator += DeltaTime;
    }

    public bool ShouldRunFixedUpdate()
    {
        if (_accumulator >= FixedDeltaTime)
        {
            _accumulator -= FixedDeltaTime;
            return true;
        }
        return false;
    }
}

