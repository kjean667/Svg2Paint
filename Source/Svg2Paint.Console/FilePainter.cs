using Svg2Paint.Lib;

namespace Svg2Paint.Console;

public class FilePainter
{
    public FileInfo? InputFile { get; set; }
    public FileInfo? OutputFile { get; set; }

    /// <summary>
    /// Animation speed, specified in pixel distance per frame.
    /// </summary>
    public double Speed { get; set; } = 1.0;

    public void Paint()
    {
        if (InputFile == null)
        {
            return;
        }

        var svg = System.IO.File.ReadAllText(InputFile.FullName);
        var svgLoader = new SvgLoader();
        svgLoader.LoadFromString(svg);

        var painter = new Painter(svgLoader.Paths);
        var paintCommands = painter.Paint(Speed);
        using var outputStream = OutputFile?.OpenWrite();
        foreach (var command in paintCommands)
        {
            if (outputStream != null)
            {
                outputStream.Write(command);
            }

            string hexString = string.Join(" ", command.Select(b => $"{b:X2}"));
            System.Console.WriteLine(hexString);
        }
    }
}
