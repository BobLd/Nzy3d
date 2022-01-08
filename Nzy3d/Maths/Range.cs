namespace Nzy3d.Maths
{
	public class Range : Scale
	{
		public Range(double min, double max) : base(min, max)
		{
		}

		public void Enlarge(double ratio)
		{
			double offset = (Max - Min) * ratio;
			if (offset == 0)
			{
				offset = 1;
			}
			Min -= offset;
			Max += offset;
		}

		public Range CreateEnlarge(double ratio)
		{
			double offset = (Max - Min) * ratio;
			if (offset == 0)
			{
				offset = 1;
			}
			return new Range(Min - offset, Max + offset);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
