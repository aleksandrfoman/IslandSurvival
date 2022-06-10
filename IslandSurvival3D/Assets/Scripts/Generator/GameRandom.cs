using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameRandom
{
    public static float NextFloat(this System.Random seed, double min, double max)
    {
        double val = (seed.NextDouble() * (max - min) + min);
        return (float)val;
        //double range = max - min;
        //double sample = seed.NextDouble();
        //double scaled = (sample * range) + min;
        //return (float)scaled;
    }
}
