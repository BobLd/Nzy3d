using Nzy3d.Maths;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Transform
{
    public sealed class Transform
	{
		private readonly List<ITransformer> _sequence;

		public Transform()
		{
			_sequence = new List<ITransformer>();
		}

		public Transform(ITransformer transformer)
		{
			_sequence = new List<ITransformer>();
			_sequence.Add(transformer);
		}

		public Transform(Transform transform)
		{
			_sequence = new List<ITransformer>();
			foreach (ITransformer nextT in transform.Sequence)
			{
				_sequence.Add(nextT);
			}
		}

		public IEnumerable<ITransformer> Sequence
		{
			get { return _sequence; }
		}

		public void Add(ITransformer nextT)
		{
			_sequence.Add(nextT);
		}

		public void Add(Transform transform)
		{
			foreach (ITransformer nextT in transform.Sequence)
			{
				_sequence.Add(nextT);
			}
		}

		public void Execute()
		{
			// Do nothing
		}

		public void Execute(bool loadIdentity)
		{
			if (loadIdentity)
			{
				GL.LoadIdentity();
			}
			foreach (ITransformer nextT in _sequence)
			{
				nextT.Execute();
			}
		}

		public Coord3d Compute(Coord3d input)
		{
			Coord3d output = input.Clone();
			foreach (ITransformer nextT in _sequence)
			{
				output = nextT.Compute(output);
			}
			return output;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			string txt = "";
			foreach (ITransformer nextT in _sequence)
			{
				txt += " * " + nextT.ToString();
			}
			return txt;
		}
	}
}
