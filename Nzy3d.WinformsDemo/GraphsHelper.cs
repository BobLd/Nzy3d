using Nzy3d.Colors;
using Nzy3d.Colors.ColorMaps;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Builder.Concrete;
using Nzy3d.Plot3D.Primitives;
using Color = Nzy3d.Colors.Color;

namespace Nzy3d.WinformsDemo
{
    internal static class GraphsHelper
    {
        #region Surface
        /// <summary>
        /// Build a nice surface to display with cool alpha colors
        /// (alpha 0.8 for surface color and 0.5 for wireframe).
        /// </summary>
        public static Shape GetSurfaceGraph()
        {
            // Create a range for the graph generation
            var range = new Maths.Range(-150, 150);
            const int steps = 50;

            // Build a nice surface to display with cool alpha colors 
            // (alpha 0.8 for surface color and 0.5 for wireframe)
            var surface = Plot3D.Builder.Builder.BuildOrthonomal(new OrthonormalGrid(range, steps, range, steps), new MyMapper());
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin, surface.Bounds.ZMax, new Color(1, 1, 1, 0.8));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = true;
            surface.WireframeColor = Color.CYAN;
            surface.WireframeColor.mul(new Color(1, 1, 1, 0.5));
            return surface;
        }
        #endregion

        #region GroupedLineScatter
        public static GroupedLineScatter GetGroupedLineScatter()
        {
            const float a = 0.50f;
            int size = 4;
            var points = new List<Coord3d[]>(size);
            var colors = new Color[]
            {
                new Color(1.0, 0.0, 0.0, a), // RED
                new Color(0.0, 1.0, 0.0, a), // GREEN
                new Color(0.0, 0.0, 1.0, a), // BLUE
                new Color(1.0, 1.0, 0.0, a), // YELLOW
                //new Color(1.0, 0.0, 1.0), // MAGENTA
                //new Color(0.0, 1.0, 1.0), // CYAN
            };

            float x, y, z;
            var r = new Random(0);

            for (int i = 0; i < size; i++)
            {
                int size2 = r.Next(150);

                var points2 = new Coord3d[size2];
                for (int j = 0; j < size2; j++)
                {
                    x = r.NextSingle() - 0.5f;
                    y = r.NextSingle() - 0.5f;
                    z = r.NextSingle() - 0.5f;
                    points2[j] = new Coord3d(x, y, z);
                }
                points.Add(points2);
                //colors[i] = new Color(x, y, z, a);
            }
            return new GroupedLineScatter(points, colors);
        }
        #endregion

        #region Scatter
        public static Scatter GetScatterGraph()
        {
            return GetScatterGraph(500_000);
        }

        /// <summary>
        /// Build a nice scatter to display with cool alpha colors
        /// (alpha 0.25).
        /// </summary>
        public static Scatter GetScatterGraph(int size)
        {
            var points = new Coord3d[size];
            var colors = new Color[size];

            float x, y, z;
            const float a = 0.25f;

            var r = new Random(0);
            for (int i = 0; i < size; i++)
            {
                x = r.NextSingle() - 0.5f;
                y = r.NextSingle() - 0.5f;
                z = r.NextSingle() - 0.5f;
                points[i] = new Coord3d(x, y, z);
                colors[i] = new Color(x, y, z, a);
            }
            return new Scatter(points, colors);
        }
        #endregion
    }
}
