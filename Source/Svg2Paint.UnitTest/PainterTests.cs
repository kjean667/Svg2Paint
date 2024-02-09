using Svg2Paint.Lib;
using Path = Svg2Paint.Lib.Path;

namespace Svg2Paint.UnitTest
{
    [TestClass]
    public class PainterTests
    {
        [TestMethod]
        public void PaintLineSequence()
        {
            var path = new Path(new Vector2d(0, 0));
            path.Add(new Line(new Vector2d(0, 0), new Vector2d(10, 0)));
            path.Add(new Line(new Vector2d(10, 0), new Vector2d(10, 10)));
            var allPaths = new List<Path> { path };
            var painter = new Painter(allPaths);
            var stepDistance = 2.0; // pixels per frame
            var paintInstructions = painter.Paint(stepDistance).ToList();

            Assert.AreEqual(2, paintInstructions.Count);
            // Start frame for first line
            Assert.AreEqual(0, paintInstructions[0][1]);
            // Start frame for second line
            Assert.AreEqual(6, paintInstructions[1][1]);
        }

        [TestMethod]
        public void ConnectingLine_StartAtIntersection()
        {
            var path = new Path(new Vector2d(0, 0));
            path.Add(new Line(new Vector2d(0, 0), new Vector2d(10, 0)));
            path.Add(new Line(new Vector2d(10, 0), new Vector2d(10, 10)));
            var path2 = new Path(new Vector2d(10, 0));
            path2.Add(new Line(new Vector2d(10, 0), new Vector2d(20, 0)));
            var allPaths = new List<Path> { path, path2 };
            var painter = new Painter(allPaths);
            var stepDistance = 2.0; // pixels per frame
            var paintInstructions = painter.Paint(stepDistance).ToList();

            Assert.AreEqual(3, paintInstructions.Count);
            // Start frame for first line
            Assert.AreEqual(0, paintInstructions[0][1]);
            // Start frame for second line
            Assert.AreEqual(6, paintInstructions[1][1]);
            // Start frame for third connecting line
            Assert.AreEqual(6, paintInstructions[2][1]);
        }

        [TestMethod]
        public void ConnectingLine_StartAtEnd()
        {
            var path = new Path(new Vector2d(0, 0));
            path.Add(new Line(new Vector2d(0, 0), new Vector2d(10, 0)));
            path.Add(new Line(new Vector2d(10, 0), new Vector2d(10, 10)));
            var path2 = new Path(new Vector2d(10, 10));
            path2.Add(new Line(new Vector2d(10, 10), new Vector2d(20, 10)));
            var allPaths = new List<Path> { path, path2 };
            var painter = new Painter(allPaths);
            var stepDistance = 2.0; // pixels per frame
            var paintInstructions = painter.Paint(stepDistance).ToList();

            Assert.AreEqual(3, paintInstructions.Count);
            // Start frame for first line
            Assert.AreEqual(0, paintInstructions[0][1]);
            // Start frame for second line
            Assert.AreEqual(6, paintInstructions[1][1]);
            // Start frame for third connecting line
            Assert.AreEqual(12, paintInstructions[2][1]);
        }

        [TestMethod]
        public void MultipleRoots()
        {
            var path = new Path(new Vector2d(0, 0));
            path.Add(new Line(new Vector2d(0, 0), new Vector2d(10, 0)));
            var path2 = new Path(new Vector2d(0, 100));
            path2.Add(new Line(new Vector2d(0, 100), new Vector2d(10, 100)));
            var path3 = new Path(new Vector2d(10, 100));
            path3.Add(new Line(new Vector2d(10, 100), new Vector2d(20, 100)));
            var allPaths = new List<Path> { path, path2, path3 };
            var painter = new Painter(allPaths);
            var stepDistance = 2.0; // pixels per frame
            var paintInstructions = painter.Paint(stepDistance).ToList();

            Assert.AreEqual(3, paintInstructions.Count);
            // Start frame for first line
            Assert.AreEqual(0, paintInstructions[0][1]);
            // Start frame for second line
            Assert.AreEqual(0, paintInstructions[1][1]);
            // Start frame for third line
            Assert.AreEqual(6, paintInstructions[2][1]);
        }

        [TestMethod]
        public void SortedStartFrames()
        {
            var path = new Path(new Vector2d(0, 0));
            path.Add(new Line(new Vector2d(0, 0), new Vector2d(10, 0)));
            path.Add(new Line(new Vector2d(10, 0), new Vector2d(20, 0)));
            path.Add(new Line(new Vector2d(20, 0), new Vector2d(30, 0)));
            var path2 = new Path(new Vector2d(10, 0));
            path2.Add(new Line(new Vector2d(10, 0), new Vector2d(10, 10)));
            path2.Add(new Line(new Vector2d(10, 10), new Vector2d(10, 20)));
            var allPaths = new List<Path> { path, path2};
            var painter = new Painter(allPaths);
            var stepDistance = 2.0; // pixels per frame
            var paintInstructions = painter.Paint(stepDistance).ToList();

            // Start frame for lines
            Assert.AreEqual(5, paintInstructions.Count);
            Assert.AreEqual(0, paintInstructions[0][1]);
            Assert.AreEqual(6, paintInstructions[1][1]);
            Assert.AreEqual(6, paintInstructions[2][1]);
            Assert.AreEqual(12, paintInstructions[3][1]);
            Assert.AreEqual(12, paintInstructions[4][1]);
        }
    }
}
