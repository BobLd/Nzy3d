using Nzy3d.Maths;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Transform
{
	public sealed class Scale : ITransformer
	{
		private readonly Coord3d _scale;
		public Scale(Coord3d scale)
		{
			_scale = scale;
		}

		public Coord3d Compute(Coord3d input)
		{
			return input.Multiply(_scale);
		}

		public void Execute()
		{
			GL.Scale(_scale.X, _scale.Y, _scale.Z);
		}

		public override string ToString()
		{
			return "(Scale)" + _scale.ToString();
		}
	}
}
