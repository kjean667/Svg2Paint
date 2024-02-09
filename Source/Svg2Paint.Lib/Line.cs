namespace Svg2Paint.Lib;

public class Line(Vector2d from, Vector2d to) : IPathPrimitive
{
    public Vector2d From { get; } = from;
    public Vector2d To { get; } = to;
}
