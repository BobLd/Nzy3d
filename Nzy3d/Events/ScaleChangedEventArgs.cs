using Nzy3d.Maths;

namespace Nzy3d.Events
{
	public class ScaleChangedEventArgs : ObjectEventArgs
	{
		public ScaleChangedEventArgs(object objectChanged, Scale scaling, int scaleID) : base(objectChanged)
		{
			Scaling = scaling;
			ScaleId = scaleID;
		}

		public ScaleChangedEventArgs(object objectChanged, Scale scaling) : this(objectChanged, scaling, -1)
		{
		}

		public Scale Scaling { get; }

		public int ScaleId { get; }

		/// <inheritdoc/>
		public override string ToString()
		{
			return "ScaleChangeEventArgs:id" + ScaleId + ", scale=" + Scaling.ToString();
		}
	}
}
