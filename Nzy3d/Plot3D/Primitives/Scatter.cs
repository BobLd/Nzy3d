using Nzy3d.Colors;
using Nzy3d.Events;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Primitives
{
    public class Scatter : AbstractDrawable, ISingleColorable
    {
        private Color[] _colors;
        private Coord3d[] _coordinates;

        public Scatter()
        {
            _bbox = new BoundingBox3d();
            Width = 1;
            Color = Color.BLACK;
        }

        public Scatter(Coord3d[] coordinates) :
                this(coordinates, Color.BLACK)
        {
        }

        public Scatter(Coord3d[] coordinates, Color rgb, float width = 1)
        {
            _bbox = new BoundingBox3d();
            Data = coordinates;
            Width = width;
            Color = rgb;
        }

        public Scatter(Coord3d[] coordinates, Color[] colors, float width = 1)
        {
            _bbox = new BoundingBox3d();
            Data = coordinates;
            Width = width;
            Colors = colors;
        }

        public void Clear()
        {
            _coordinates = null;
            _bbox.Reset();
        }

        public override void Draw(Camera cam)
        {
            _transform?.Execute();

            GL.PointSize(Width);
            GL.Begin(PrimitiveType.Points);
            if (_colors == null)
            {
                GL.Color4(Color.r, Color.g, Color.b, Color.a);
            }

            if (_coordinates != null)
            {
                int k = 0;
                foreach (Coord3d c in _coordinates)
                {
                    if (_colors != null)
                    {
                        GL.Color4(_colors[k].r, _colors[k].g, _colors[k].b, _colors[k].a);
                        k++;
                    }

                    GL.Vertex3(c.X, c.Y, c.Z);
                }
            }
            GL.End();

            // doDrawBounds (MISSING)
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
            foreach (var c in _coordinates)
            {
                _bbox.Add(c);
            }
        }

        private Coord3d[] Data
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

        private float Width { get; }

        public Color Color { get; set; }
    }
}