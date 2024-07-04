using UnityEngine;
using System;

public enum FlameColor
{
    Red,
    Blue,
    Purple,
}

public static class FlameColors
{
    public static Color GetColor(FlameColor fc)
    {
        switch (fc)
        {
            case FlameColor.Red:
                return Color.red;
            case FlameColor.Blue:
                return Color.blue;
            case FlameColor.Purple:
                return Color.magenta;
            default:
                throw new Exception(string.Format("Color: {0} not found", fc));
        }
    }

    public static Color Standard;
}