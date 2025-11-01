using System.Diagnostics;

namespace MusgoEngine;

public class GameTime
{
    private double _previousTime;
    private double _accumulator;

    public static float DeltaTime { get; private set; }
    public static float FixedDeltaTime { get; private set; }
    public static float TotalTime { get; private set; }
    public static int FrameCount { get; private set; }

    public GameTime(float fixedDeltaTime = 1f / 50f)
    {
        FixedDeltaTime = fixedDeltaTime;
        _previousTime = 0;
        _accumulator = 0;
    }

    /// <summary>
    /// Update GameTime using the frame start timestamp
    /// </summary>
    public void Update(long frameStartTimestamp)
    {
        // Convert ticks to seconds
        var currentTime = frameStartTimestamp / (double)Stopwatch.Frequency;

        DeltaTime = (float)(currentTime - _previousTime);
        _previousTime = currentTime;

        TotalTime = (float)currentTime;
        FrameCount++;
        _accumulator += DeltaTime;
    }

    /// <summary>
    /// Determines if a fixed update should run based on the fixed delta time
    /// </summary>
    public bool ShouldRunFixedUpdate()
    {
        if (!(_accumulator >= FixedDeltaTime)) return false;
        _accumulator -= FixedDeltaTime;
        return true;
    }
}
