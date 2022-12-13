using System;
using Godot;

namespace Common;

public static class ColorExtensions
{
    public static float Brightness(this Color color)
    {
        return (color.r + color.g + color.b) / 3;
    }
}