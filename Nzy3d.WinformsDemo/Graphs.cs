﻿using Nzy3d.Colors;
using Nzy3d.Colors.ColorMaps;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Builder.Concrete;
using Nzy3d.Plot3D.Primitives;
using Color = Nzy3d.Colors.Color;

namespace Nzy3d.WinformsDemo
{
    internal static class Graphs
    {
        /// <summary>
        /// Build a nice surface to display with cool alpha colors
        /// (alpha 0.8 for surface color and 0.5 for wireframe).
        /// </summary>
        public static Shape GetShapeGraph()
        {
            // Create a range for the graph generation
            var range = new Maths.Range(-150, 150);
            const int steps = 50;

            // Build a nice surface to display with cool alpha colors 
            // (alpha 0.8 for surface color and 0.5 for wireframe)
            var surface = Plot3D.Builder.Builder.buildOrthonomal(new OrthonormalGrid(range, steps, range, steps), new MyMapper());
            surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.zmin, surface.Bounds.zmax, new Color(1, 1, 1, 0.8));
            surface.FaceDisplayed = true;
            surface.WireframeDisplayed = true;
            surface.WireframeColor = Color.CYAN;
            surface.WireframeColor.mul(new Color(1, 1, 1, 0.5));
            return surface;
        }

        /// <summary>
        /// Build a nice scatter to display with cool alpha colors
        /// (alpha 0.25).
        /// </summary>
        public static Scatter GetScatterGraph()
        {
            int size = 500_000;
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
    }
}