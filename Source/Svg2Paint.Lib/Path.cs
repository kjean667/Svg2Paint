using System.Collections;
using System.Collections.Generic;

namespace Svg2Paint.Lib;

public class Path
{
    public Vector2d StartPoint { get; }

    public IList<IPathPrimitive> Primitives { get; } = new List<IPathPrimitive>();

    public Path(Vector2d startPoint)
    {
        StartPoint = startPoint;
    }

    public void Add(Line line)
    {
        Primitives.Add(line);
    }
}
