using System;

namespace Svg2Paint.Lib;

public class Vector2d(double x, double y)
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;

    public static Vector2d operator +(Vector2d first, Vector2d second)
    {
        return new Vector2d(first.X + second.X, first.Y + second.Y);
    }

    public static Vector2d operator -(Vector2d first, Vector2d second)
    {
        return new Vector2d(first.X - second.X, first.Y - second.Y);
    }

    public static Vector2d operator *(Vector2d v, double factor)
    {
        return new Vector2d(v.X * factor, v.Y * factor);
    }

    public double Length()
    {
        return Math.Sqrt(X * X + Y * Y);
    }

    public Vector2d Normalized()
    {
        var length = Length();
        return new Vector2d(X / length, Y / length);
    }
}
