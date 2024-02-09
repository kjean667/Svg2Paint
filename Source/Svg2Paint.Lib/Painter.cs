using System;
using System.Collections.Generic;
using System.Linq;

namespace Svg2Paint.Lib;

public class Painter(IEnumerable<Path> allPaths)
{
    private readonly IEnumerable<Path> _allPaths = allPaths;

    public IEnumerable<byte[]> Paint(double stepDistance)
    {
        // Sort by frame number contained as BigEndian in the first two bytes
        return PaintAllPaths(stepDistance).OrderBy(x => x[0] << 8 | x[1]);
    }
    private IEnumerable<byte[]> PaintAllPaths(double stepDistance)
    {
        var frame = 0;
        var startSkipDistance = 0.0;
        
        foreach (var rootPath in GetRootPaths(_allPaths))
        {
            foreach (var command in PaintPath(rootPath, frame, startSkipDistance, stepDistance))
            {
                yield return command;
            }

        }
    }

    private IEnumerable<Path> GetRootPaths(IEnumerable<Path> allPaths)
    {
        var allToPoints = allPaths.SelectMany(x => x.Primitives.Select(y => y.To)).ToList();
        foreach (var path in allPaths)
        {
            if (allToPoints.All(x => Math.Abs((x - path.StartPoint).Length()) > 1))
            {
                yield return path;
            }
        }
    }

    public IEnumerable<byte[]> PaintPath(Path path, int frame, double startSkipDistance, double stepDistance)
    {
        foreach (var primitive in path.Primitives)
        {
            // Paint any connecting paths
            var connectingPaths = GetConnectingPaths(primitive.From, _allPaths);
            if (ReferenceEquals(primitive, path.Primitives.Last()))
            {
                // Include end connections if this is the last primitive in the path
                connectingPaths.Concat(GetConnectingPaths(primitive.To, _allPaths));
            }
            foreach (var connectingPath in connectingPaths)
            {
                if (ReferenceEquals(connectingPath, path))
                {
                    // Don't connect path to itself
                    continue;
                }
                var paintCommands = PaintPath(connectingPath, frame, startSkipDistance, stepDistance);
                foreach (var command in paintCommands)
                {
                    yield return command;
                }
            }

            // Paint the path itself
            var painter = CreatePainter(primitive, frame, stepDistance, startSkipDistance);
            if (painter != null)
            {
                yield return painter.GetPaintInstructions();
                startSkipDistance = painter.EndRestDistance;
                frame += painter.StepCount;
            }

            // Paint any end connections if this is the last primitive in the path
            if (ReferenceEquals(primitive, path.Primitives.Last()))
            {
                var connectingEndPaths = GetConnectingPaths(primitive.To, _allPaths);
                foreach (var connectingPath in connectingEndPaths)
                {
                    var paintCommands = PaintPath(connectingPath, frame, startSkipDistance, stepDistance);
                    foreach (var command in paintCommands)
                    {
                        yield return command;
                    }
                }
            }
        }
    }

    private IEnumerable<Path> GetConnectingPaths(Vector2d connectionPoint, IEnumerable<Path> paths)
    {
        return paths.Where(x => Math.Abs((x.StartPoint - connectionPoint).Length()) <= 1);
    }

    private IPainter? CreatePainter(object primitive, int startFrame, double stepDistance, double startSkipDistance)
    {
        if (primitive is Line line)
        {
            return new FixPointLinePainter(line, startFrame, stepDistance, startSkipDistance);
        }

        return null;
    }
}
