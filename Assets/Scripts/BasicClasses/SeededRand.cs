using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SeededRand : MonoBehaviour
{
    public static double state = 12345;
    public const double MOD = 233280;
    public const double OFFSET = 49297;
    public const double MULTIPLIER = 9301;

    public static double next()
    {
        state = (state * MULTIPLIER + OFFSET) % MOD;
        return state / MOD;
    }

    public static void seedWithDate(DateTime date)
    {
        state = (double)((date.Year - 1900) * date.Day + 1000 * (date.Month-1) + 100000 * date.Day);
    }

    // get a number between min and max (both inclusive)
    public static int intBetween(double min, double max)
    {
        return (int)Math.Floor((max - min + 1) * next() + min);
    }

    // Pick an element from an array
    public static T FromList<T>(List<T> list)
    {
        return list[intBetween(0, list.Count - 1)];
    }
}
