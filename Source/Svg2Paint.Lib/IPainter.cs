namespace Svg2Paint.Lib;

public interface IPainter
{
    public int StartFrame { get; }
    public int StepCount { get; }
    public double EndRestDistance { get; }

    byte[] GetPaintInstructions();
}
