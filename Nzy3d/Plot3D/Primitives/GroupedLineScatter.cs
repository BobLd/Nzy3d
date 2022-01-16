using Nzy3d.Colors;
using Nzy3d.Events;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;
using OpenTK.Graphics.OpenGL;

// From Keex0r's https://github.com/Keex0r/nzy3d-api/blob/master/nzy3d-api/Plot3D/Primitives/GroupedLineScatter.cs
namespace Nzy3d.Plot3D.Primitives
{
    public class GroupedLineScatter : AbstractDrawable, ISingleColorable
    {
        private Color[] _colors;

        /// <summary>
        /// Each list is rendered as an individual scatter plot with lines between the points
        /// </summary>
        private List<Coord3d[]> _coordinates;

        public GroupedLineScatter()
        {
            _bbox = new BoundingBox3d();
            PointWidth = 1;
            LineWidth = 1;
            Color = Color.BLACK;
        }

        /// <summary>
        /// Initializes a new GroupedLineScatter object
        /// </summary>
        /// <param name="coordinates">A list of coordinate arrays. Each list item is rendered as a separate series in a different color.</param>
        public GroupedLineScatter(List<Coord3d[]> coordinates) :
                this(coordinates, Color.BLACK)
        {
        }

        /// <summary>
        /// Initializes a new GroupedLineScatter object
        /// </summary>
        /// <param name="coordinates">A list of coordinate arrays. Each list item is rendered as a separate series in a different color.</param>
        /// <param name="rgb">The default color. It is recommended to use the <see cref="GroupedLineScatter.GroupedLineScatter(List{Coord3d[]}, Color[], float, float)"/> constructor and a provide a list of colors.</param>
        /// <param name="pointwidth">The size of the scatter markers</param>
        /// <param name="linewidth">The width of the line</param>
        public GroupedLineScatter(List<Coord3d[]> coordinates, Color rgb, float pointwidth = 8, float linewidth = 2)
        {
            _bbox = new BoundingBox3d();
            Data = coordinates;
            this.PointWidth = pointwidth;
            this.LineWidth = linewidth;
            Color = rgb;
        }

        /// <summary>
        /// Initializes a new GroupedLineScatter object
        /// </summary>
        /// <param name="coordinates">A list of coordinate arrays. Each list item is rendered as a separate series in a different color.</param>
        /// <param name="colors">A list of colors. The series are colored in order of this list with rollover if more series than colors are provided</param>
        /// <param name="pointwidth">The size of the scatter markers</param>
        /// <param name="linewidth">The width of the line</param>
        public GroupedLineScatter(List<Coord3d[]> coordinates, Color[] colors, float pointwidth = 8, float linewidth = 2)
        {
            _bbox = new BoundingBox3d();
            Data = coordinates;
            this.PointWidth = pointwidth;
            this.LineWidth = linewidth;
            Colors = colors;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _coordinates = null;
            _bbox.Reset();
        }

        /// <inheritdoc/>
        public override void Draw(Camera cam)
        {
            _transform?.Execute();

            if (_coordinates != null)
            {
                int k = 0;
                GL.PointSize(PointWidth);
                GL.LineWidth(LineWidth);
                //Enable in order to provide correct z-indexing of the series
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);
                //Render each set of coordinates
                foreach (Coord3d[] c in _coordinates)
                {
                    //Setup colors
                    if (_colors == null)
                    {
                        GL.Color4(Color.R, Color.G, Color.B, Color.A);
                    }

                    if (_colors != null)
                    {
                        GL.Color4(_colors[k].R, _colors[k].G, _colors[k].B, _colors[k].A);
                        k++;
                        if (k >= _colors.Length) k = 0; //Roll over the color list
                    }

                    //Draw points
                    if (PointWidth > 0)
                    {
                        GL.Begin(PrimitiveType.Points);
                        foreach (Coord3d p in c)
                        {
                            GL.Vertex3(p.X, p.Y, p.Z);
                        }
                        GL.End();
                    }

                    //Draw lines
                    if (LineWidth > 0)
                    {
                        GL.Begin(PrimitiveType.LineStrip);
                        foreach (Coord3d p in c)
                        {
                            GL.Vertex3(p.X, p.Y, p.Z);
                        }
                        GL.End();
                    }
                }
            }
        }

        public override Transform.Transform Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                UpdateBounds();
            }
        }

        private void UpdateBounds()
        {
            _bbox.Reset();
            // Iterate over all groups of points to update the bounds
            foreach (var s in _coordinates)
            {
                foreach (var c in s)
                {
                    _bbox.Add(c);
                }
            }
        }

        private List<Coord3d[]> Data
        {
            get => _coordinates;
            set
            {
                _coordinates = value;
                UpdateBounds();
            }
        }

        private Color[] Colors
        {
            get => _colors;
            set
            {
                _colors = value;
                FireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
            }
        }

        private float PointWidth { get; }

        private float LineWidth { get; }

        public Color Color { get; set; }
    }
}
