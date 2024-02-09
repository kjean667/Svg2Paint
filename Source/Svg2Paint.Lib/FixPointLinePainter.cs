using System;

namespace Svg2Paint.Lib
{
    public class FixPointLinePainter : IPainter
    {
        public static readonly int FixPointShift = 6;

        private readonly Line _line;

        public int StartFrame { get; }
        public int StepCount { get; }
        public double EndRestDistance { get; }

        private Vector2d _startPoint;
        private int _xStep;
        private int _yStep;

        public FixPointLinePainter(Line line, int startFrame, double stepDistance, double startSkipDistance)
        {
            _line = line;
            StartFrame = startFrame;

            var direction = _line.To - _line.From;
            var directionNormal = direction.Normalized();
            var length = direction.Length() - startSkipDistance;
            if (length < 0)
            {
                EndRestDistance = -length;
            }
            _startPoint = _line.From + directionNormal * startSkipDistance;

            var step = directionNormal * stepDistance;
            _xStep = (int)Math.Round(step.X * (1 << FixPointShift));
            _yStep = (int)Math.Round(step.Y * (1 << FixPointShift));

            StepCount = (int)Math.Floor(length / stepDistance) + 1;

            EndRestDistance = length - (StepCount - 1) * stepDistance;
        }

        public byte[] GetPaintInstructions()
        {
            int x = Math.Min(0x7fff, Math.Max(-0x7ffe, (int)_startPoint.X));
            int y = Math.Min(0xff, Math.Max(0, (int)_startPoint.Y));

            return new byte[]
            {
                // Start frame
                (byte)((StartFrame >> 8) & 0xff), (byte)(StartFrame & 0xff),
                // Animation length in frames
                (byte)StepCount,
                // Start X
                (byte)((x >> 8) & 0xff), (byte)(x & 0xff),
                // Start Y
                (byte)y,
                // Delta X per frame
                (byte)_xStep,
                // Delta Y per frame
                (byte)_yStep
            };
        }
    }
}
