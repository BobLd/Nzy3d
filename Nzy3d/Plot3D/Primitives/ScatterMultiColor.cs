using Nzy3d.Colors;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Primitives
{
    public class ScatterMultiColor : AbstractDrawable, IMultiColorable
    {
        private Coord3d[] _coordinates;

        public ScatterMultiColor(Coord3d[] coordinates, ColorMapper mapper, float width = 1.0f)
        {
            _bbox = new BoundingBox3d();
            Data = coordinates;
            Width = width;
            ColorMapper = mapper;
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

            if (_coordinates != null)
            {
                foreach (Coord3d c in _coordinates)
                {
                    var color = ColorMapper.Color(c); // TODO: should store result in the point color
                    GL.Color4(color.r, color.g, color.b, color.a);
                    GL.Vertex3(c.X, c.Y, c.Z);
                }
            }
            GL.End();

            // doDrawBounds (MISSING)            
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

        public override Transform.Transform Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                UpdateBounds();
            }
        }

        private float Width { get; }

        public ColorMapper ColorMapper { get; set; }
    }
}