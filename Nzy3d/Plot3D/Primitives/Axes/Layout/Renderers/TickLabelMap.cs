namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers
{
	/// <summary>
	/// An <see cref="ITickRenderer"/> that can store a list of labels for given axis values.
	/// </summary>
	/// <author>Martin Pernollet</author>
	public class TickLabelMap : ITickRenderer
	{
		internal Dictionary<double, string> _tickvalues = new Dictionary<double, string>();
		public void Register(double value, string label)
		{
			_tickvalues.Add(value, label);
		}

		public bool Contains(double value)
		{
			return _tickvalues.ContainsKey(value);
		}

		public bool Contains(string label)
		{
			return _tickvalues.ContainsValue(label);
		}

		public string Format(double value)
		{
			if (Contains(value))
			{
				return _tickvalues[value];
			}
			else
			{
				return "";
			}
		}
	}
}
