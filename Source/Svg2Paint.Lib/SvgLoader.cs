using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Svg2Paint.Lib;

public class SvgLoader
{
    public IList<Path> Paths { get; } = new List<Path>();

    public void LoadFromString(string svg)
    {
        var svgDocument = XDocument.Parse(svg);
        var svgElement = svgDocument.Elements().FirstOrDefault(x => x.Name.LocalName == "svg");
        if (svgElement != null)
        {
            LoadSubElements(svgElement);
        }
    }

    private void LoadSubElements(XElement element)
    {
        foreach (var subElement in element.Elements())
        {
            if (subElement.Name.LocalName == "g")
            {
                LoadSubElements(subElement);
            }
            else if (subElement.Name.LocalName == "path")
            {
                LoadPathElement(subElement);
            }
        }
    }

    private enum OperationMode
    {
        None,
        MoveTo,
        LineToRelative,
        HorizontalLineToRelative,
        VerticalLineToRelative
    }

    private void LoadPathElement(XElement element)
    {
        var drawCommand = element.Attribute("d");
        if (drawCommand != null)
        {
            // Parse draw commands for path.
            // See https://css-tricks.com/svg-path-syntax-illustrated-guide/

            var drawSteps = drawCommand.Value.Split(' ');
            var mode = OperationMode.None;
            Path? path = null;
            var lastPoint = new Vector2d(0, 0);
            foreach (var step in drawSteps)
            {
                if (step == "m" || step == "M")
                {
                    mode = OperationMode.MoveTo;
                    continue;
                }

                if (step == "v")
                {
                    mode = OperationMode.VerticalLineToRelative;
                    continue;
                }

                if (step == "h")
                {
                    mode = OperationMode.HorizontalLineToRelative;
                    continue;
                }

                switch (mode)
                {
                    case OperationMode.MoveTo:
                        var point = GetCoordinate(step);
                        path = new Path(point);
                        lastPoint = point;
                        mode = OperationMode.LineToRelative;
                        break;

                    case OperationMode.LineToRelative:
                        if (path != null)
                        {
                            var vector = GetCoordinate(step);
                            var nextPoint = lastPoint + vector;
                            path.Add(new Line(lastPoint, nextPoint));
                            lastPoint = nextPoint;
                        }
                        break;

                    case OperationMode.VerticalLineToRelative:
                        if (path != null)
                        {
                            var value = GetValue(step);
                            if (value != 0)
                            {
                                path.Add(new Line(lastPoint, lastPoint + new Vector2d(0, value)));
                            }
                        }
                        break;

                    case OperationMode.HorizontalLineToRelative:
                        if (path != null)
                        {
                            var value = GetValue(step);
                            if (value != 0)
                            {
                                path.Add(new Line(lastPoint, lastPoint + new Vector2d(value, 0)));
                            }
                        }
                        break;
                }
            }

            if (path != null)
            {
                AddPath(path);
            }
        }
    }

    private void AddPath(Path path)
    {
        Paths.Add(path);
    }

    private Vector2d GetCoordinate(string coordinateString)
    {
        double x = 0;
        double y = 0;
        var coordinates = coordinateString.Split(',');
        if (coordinates.Length >= 1)
        {
            x = GetValue(coordinates[0]);
        }
        if (coordinates.Length == 2)
        {
            y = GetValue(coordinates[1]);
        }

        return new Vector2d(x, y);
    }

    private double GetValue(string valueString)
    {
        return double.TryParse(valueString, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : 0.0;
    }

}
