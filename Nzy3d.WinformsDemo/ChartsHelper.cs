using Nzy3d.Colors;
using Nzy3d.Colors.ColorMaps;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Builder.Concrete;
using Nzy3d.Plot3D.Primitives;
using Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Plot3D.Rendering.View.Modes;
using Nzy3d.Winforms;
using System.Globalization;
using Color = Nzy3d.Colors.Color;

namespace Nzy3d.WinformsDemo
{
    internal static class ChartsHelper
    {
        #region Surface
        /// <summary>
        /// Build a nice surface to display with cool alpha colors
        /// (alpha 0.8 for surface color and 0.5 for wireframe).
        /// </summary>
        public static Chart.Chart GetMapperSurface(Renderer3D renderer3D)
        {
            // Create the chart
            Chart.Chart chart = new Chart.Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;
            chart.View.CameraMode = CameraMode.PERSPECTIVE;
            chart.View.IncludingTextLabels = true;

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
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.5));

            // Add surface to chart
            chart.Scene.Graph.Add(surface);

            return chart;
        }

        public static Chart.Chart GetFRB_H15_dec_2021(Renderer3D renderer3D)
        {
            // Generate data
            var labels = new TickLabelMap();
            List<Coord3d> coords = new List<Coord3d>();
            bool isHeader = true;
            string[] header;
            foreach (var line in File.ReadAllLines("FRB_H15_dec_2021.csv"))
            {
                var data = line.Split('\u002C');
                if (isHeader)
                {
                    header = data;
                    for (int i = 1; i < data.Length; i++)
                    {
                        labels.Register(i, header[i]);
                    }
                    isHeader = false;
                }
                else
                {
                    var date = DateTime.ParseExact(data[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var dateL = date.ToOADate();
                    for (int i = 1; i < data.Length; i++)
                    {
                        coords.Add(new Coord3d(i, dateL, double.Parse(data[i])));
                    }
                }
            }

            // Create the chart
            Chart.Chart chart = new Chart.Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;
            chart.View.CameraMode = CameraMode.PERSPECTIVE;
            chart.View.IncludingTextLabels = true;
            chart.AxeLayout.YTickRenderer = new DateTickRenderer("dd/MM/yyyy");
            chart.AxeLayout.YAxeLabel = "Date";
            chart.AxeLayout.XTickRenderer = labels;
            chart.AxeLayout.XAxeLabel = "Maturity";
            chart.AxeLayout.ZAxeLabel = "Rate (%)";

            // Create surface
            var surface = Plot3D.Builder.Builder.BuildDelaunay(coords);
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin * 1.05, surface.Bounds.ZMax * 0.95, new Color(1, 1, 1, 0.9));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = true;
            surface.WireframeColor = Color.GREEN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.2));

            // Add surface to chart
            chart.Scene.Graph.Add(surface);

            return chart;
        }

        /// <summary>
        /// Build a nice surface to display with cool alpha colors
        /// (alpha 0.8 for surface color and 0.5 for wireframe).
        /// </summary>
        public static Chart.Chart GetDelaunaySurface(Renderer3D renderer3D)
        {
            // Create data
            const int size = 100;
            List<Coord3d> coords = new List<Coord3d>(size);
            float x, y, z;
            var r = new Random(0);

            for (int i = 0; i < size; i++)
            {
                x = r.NextSingle() - 0.5f;
                y = r.NextSingle() - 0.5f;
                z = r.NextSingle() - 0.5f;
                coords.Add(new Coord3d(x, y, z));
            }
            coords = coords.OrderBy(p => p.X).ThenBy(p => p.Y).ThenBy(p => p.Z).ToList();

            // Create chart
            Chart.Chart chart = new Chart.Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;
            chart.View.CameraMode = CameraMode.PERSPECTIVE;
            chart.View.IncludingTextLabels = true;

            // Create surface
            var surface = Plot3D.Builder.Builder.BuildDelaunay(coords);
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin, surface.Bounds.ZMax, new Color(1, 1, 1, 0.8));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = true;
            surface.WireframeColor = Color.CYAN;
            surface.WireframeColor.Mul(new Color(1, 1, 1, 0.5));

            // Add surface to chart
            chart.Scene.Graph.Add(surface);

            return chart;
        }
        #endregion

        #region GroupedLineScatter
        public static Chart.Chart GetGroupedLineScatter(Renderer3D renderer3D)
        {
            // Create data
            const float a = 0.50f;
            const int size = 4;
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
            }

            // Create chart
            Chart.Chart chart = new Chart.Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;
            chart.View.CameraMode = CameraMode.PERSPECTIVE;
            chart.View.IncludingTextLabels = true;

            // Create scatter
            var scatter = new GroupedLineScatter(points, colors);

            // Add surface to chart
            chart.Scene.Graph.Add(scatter);

            return chart;
        }
        #endregion

        #region Scatter
        public static Chart.Chart GetScatterGraph(Renderer3D renderer3D)
        {
            return GetScatterGraph(renderer3D, 500_000);
        }

        /// <summary>
        /// Build a nice scatter to display with cool alpha colors
        /// (alpha 0.25).
        /// </summary>
        public static Chart.Chart GetScatterGraph(Renderer3D renderer3D, int size)
        {
            // Create data
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

            // Create chart
            Chart.Chart chart = new Chart.Chart(renderer3D, Quality.Nicest);
            chart.View.Maximized = false;
            chart.View.CameraMode = CameraMode.PERSPECTIVE;
            chart.View.IncludingTextLabels = true;

            // Create scatter
            var scatter = new Scatter(points, colors);

            // Add surface to chart
            chart.Scene.Graph.Add(scatter);

            return chart;
        }
        #endregion
    }
}
