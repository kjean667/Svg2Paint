using Svg2Paint.Lib;

namespace Svg2Paint.UnitTest
{
    [TestClass]
    public class LoaderTests
    {
        private readonly string _sampleSvg = """
                                         <svg><g>
                                         <path
                                            style="fill:none;stroke:#000000;stroke-width:0.2;stroke-linejoin:bevel"
                                            d="m 262.72027,256.09639 -4.01845,-16.0738 -10.95941,-12.05535 -23.38008,-12.05535 v 0"
                                            id="path1" />
                                         </g></svg>
                                         """;

        private readonly string _fullSvg = """
            <?xml version="1.0" encoding="UTF-8" standalone="no"?>
            <!-- Created with Inkscape (http://www.inkscape.org/) -->

            <svg
               width="320mm"
               height="256mm"
               viewBox="0 0 320 256"
               version="1.1"
               id="svg1"
               inkscape:version="1.3.1 (91b66b0783, 2023-11-16)"
               sodipodi:docname="svg2paintTest.svg"
               xmlns:inkscape="http://www.inkscape.org/namespaces/inkscape"
               xmlns:sodipodi="http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd"
               xmlns="http://www.w3.org/2000/svg"
               xmlns:svg="http://www.w3.org/2000/svg">
              <sodipodi:namedview
                 id="namedview1"
                 pagecolor="#ffffff"
                 bordercolor="#000000"
                 borderopacity="0.25"
                 inkscape:showpageshadow="2"
                 inkscape:pageopacity="0.0"
                 inkscape:pagecheckerboard="0"
                 inkscape:deskcolor="#d1d1d1"
                 inkscape:document-units="mm"
                 inkscape:zoom="0.73228114"
                 inkscape:cx="396.70556"
                 inkscape:cy="561.25984"
                 inkscape:window-width="1920"
                 inkscape:window-height="1018"
                 inkscape:window-x="-6"
                 inkscape:window-y="-6"
                 inkscape:window-maximized="1"
                 inkscape:current-layer="layer1" />
              <defs
                 id="defs1" />
              <g
                 inkscape:label="Lager 1"
                 inkscape:groupmode="layer"
                 id="layer1">
                <path
                   style="fill:none;stroke-width:0.264583;stroke:#000000;stroke-opacity:1"
                   d="m 185.35401,221.84671 -7.22628,-56.00365 -56.00365,-71.540143 -88.160576,20.594893 -6.142337,70.09489 69.372263,12.28467 10.83941,-59.61679 -46.248169,8.67154"
                   id="path1" />
                <path
                   style="fill:none;stroke:#000000;stroke-width:0.264583;stroke-opacity:1"
                   d="m 177.04379,165.12044 45.16424,-90.689784 64.31386,14.09124 2.89052,73.346714"
                   id="path2" />
              </g>
            </svg>
            """;

        [TestMethod]
        public void SinglePathSvg_Load_ParsedPathLines()
        {
            var loader = new SvgLoader();
            loader.LoadFromString(_sampleSvg);

            Assert.AreEqual(1, loader.Paths.Count);
            Assert.AreEqual(3, loader.Paths[0].Primitives.Count);
            var firstLine = loader.Paths[0].Primitives[0] as Line;
            Assert.IsNotNull(firstLine);
            Assert.AreEqual(262.72027, firstLine.From.X);
            Assert.AreEqual(256.09639, firstLine.From.Y);
            Assert.AreEqual(262.72027 - 4.01845, firstLine.To.X);
            Assert.AreEqual(256.09639 - 16.0738, firstLine.To.Y);
            var secondLine = loader.Paths[0].Primitives[1] as Line;
            Assert.IsNotNull(secondLine);
            Assert.AreEqual(262.72027 - 4.01845, secondLine.From.X);
            Assert.AreEqual(256.09639 - 16.0738, secondLine.From.Y);
            Assert.AreEqual(262.72027 - 4.01845 - 10.95941, secondLine.To.X);
            Assert.AreEqual(256.09639 - 16.0738 - 12.05535, secondLine.To.Y);
        }

        [TestMethod]
        public void LoadFullSvg()
        {
            var loader = new SvgLoader();
            loader.LoadFromString(_fullSvg);

            Assert.AreEqual(2, loader.Paths.Count);
        }
        }
    }