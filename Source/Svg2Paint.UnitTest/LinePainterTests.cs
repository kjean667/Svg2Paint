using Svg2Paint.Lib;

namespace Svg2Paint.UnitTest
{
    [TestClass]
    public class LinePainterTests
    {
        [TestMethod]
        public void HorizontalLine_FullCoverage()
        {
            var line = new Line(new Vector2d(0, 0), new Vector2d(2, 0));
            var startFrame = 0;
            var stepDistance = 1;
            var startSkipDistance = 0;
            var painter = new FixPointLinePainter(line, startFrame, stepDistance, startSkipDistance);
            var paintData = painter.GetPaintInstructions();
            Assert.IsNotNull(paintData);
            Assert.AreEqual(8, paintData.Length);
            var i = 0;
            // Start frame
            Assert.AreEqual(0, paintData[i++]);
            Assert.AreEqual(0, paintData[i++]);
            // Animation length in frames
            Assert.AreEqual(3, paintData[i++]);
            // Start X
            Assert.AreEqual(0, paintData[i++]);
            Assert.AreEqual(0, paintData[i++]);
            // Start Y
            Assert.AreEqual(0, paintData[i++]);
            // Delta X per frame
            Assert.AreEqual(1 << FixPointLinePainter.FixPointShift, paintData[i++]);
            // Delta Y per frame
            Assert.AreEqual(0 << FixPointLinePainter.FixPointShift, paintData[i++]);

            Assert.AreEqual(0, painter.EndRestDistance);
        }

        [TestMethod]
        public void HorizontalLine_NegativeDirection()
        {
            var line = new Line(new Vector2d(0, 0), new Vector2d(-2, 0));
            var startFrame = 0;
            var stepDistance = 1;
            var startSkipDistance = 0;
            var painter = new FixPointLinePainter(line, startFrame, stepDistance, startSkipDistance);
            var paintData = painter.GetPaintInstructions();
            Assert.IsNotNull(paintData);
            Assert.AreEqual(8, paintData.Length);
            var i = 0;
            // Start frame
            Assert.AreEqual(0, paintData[i++]);
            Assert.AreEqual(0, paintData[i++]);
            // Animation length in frames
            Assert.AreEqual(3, paintData[i++]);
            // Start X
            Assert.AreEqual(0, paintData[i++]);
            Assert.AreEqual(0, paintData[i++]);
            // Start Y
            Assert.AreEqual(0, paintData[i++]);
            // Delta X per frame
            Assert.AreEqual((byte)(-1 << FixPointLinePainter.FixPointShift), paintData[i++]);
            // Delta Y per frame
            Assert.AreEqual(0 << FixPointLinePainter.FixPointShift, paintData[i++]);

            Assert.AreEqual(0, painter.EndRestDistance);
        }


        [TestMethod]
        public void VerticalLine_HalfStepRest()
        {
            var line = new Line(new Vector2d(0, 0), new Vector2d(0, 2.5));
            var startFrame = 0;
            var stepDistance = 1;
            var startSkipDistance = 0;
            var painter = new FixPointLinePainter(line, startFrame, stepDistance, startSkipDistance);
            var paintData = painter.GetPaintInstructions();
            Assert.IsNotNull(paintData);
            Assert.AreEqual(8, paintData.Length);
            var i = 0;
            // Start frame
            Assert.AreEqual(0, paintData[i++]);
            Assert.AreEqual(0, paintData[i++]);
            // Animation length in frames
            Assert.AreEqual(3, paintData[i++]);
            // Start X
            Assert.AreEqual(0, paintData[i++]);
            Assert.AreEqual(0, paintData[i++]);
            // Start Y
            Assert.AreEqual(0, paintData[i++]);
            // Delta X per frame
            Assert.AreEqual(0 << FixPointLinePainter.FixPointShift, paintData[i++]);
            // Delta Y per frame
            Assert.AreEqual(1 << FixPointLinePainter.FixPointShift, paintData[i++]);

            Assert.AreEqual(0.5, painter.EndRestDistance);
        }

        [TestMethod]
        public void HorizontalLine_HalfStepStartOfFset()
        {
            var line = new Line(new Vector2d(0, 0), new Vector2d(10, 0));
            var startFrame = 0;
            var stepDistance = 2;
            var startSkipDistance = 1;
            var painter = new FixPointLinePainter(line, startFrame, stepDistance, startSkipDistance);
            var paintData = painter.GetPaintInstructions();
            Assert.IsNotNull(paintData);
            Assert.AreEqual(8, paintData.Length);
            var i = 0;
            // Start frame
            Assert.AreEqual(0, paintData[i++]);
            Assert.AreEqual(0, paintData[i++]);
            // Animation length in frames
            Assert.AreEqual(5, paintData[i++]);
            // Start X
            Assert.AreEqual(0, paintData[i++]);
            Assert.AreEqual(1, paintData[i++]);
            // Start Y
            Assert.AreEqual(0, paintData[i++]);
            // Delta X per frame
            Assert.AreEqual(2 << FixPointLinePainter.FixPointShift, paintData[i++]);
            // Delta Y per frame
            Assert.AreEqual(0 << FixPointLinePainter.FixPointShift, paintData[i++]);

            Assert.AreEqual(1, painter.EndRestDistance);
        }

        [TestMethod]
        public void StartFrame()
        {
            var line = new Line(new Vector2d(0, 0), new Vector2d(2, 0));

            var startFrame = 258;
            var stepDistance = 1;
            var startSkipDistance = 0;
            var painter = new FixPointLinePainter(line, startFrame, stepDistance, startSkipDistance);
            var paintData = painter.GetPaintInstructions();
            Assert.IsNotNull(paintData);
            Assert.AreEqual(8, paintData.Length);
            var i = 0;
            // Start frame
            Assert.AreEqual(1, paintData[i++]);
            Assert.AreEqual(2, paintData[i++]);
        }

    }
}
