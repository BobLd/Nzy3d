namespace Nzy3d.Maths
{
	public class Range : Scale
	{
		public Range(float min, float max)
			: base(min, max)
		{
		}

		public void Enlarge(float ratio)
		{
			float offset = (Max - Min) * ratio;
			if (offset == 0)
			{
				offset = 1;
			}
			Min -= offset;
			Max += offset;
		}

		public Range CreateEnlarge(float ratio)
		{
			float offset = (Max - Min) * ratio;
			if (offset == 0)
			{
				offset = 1;
			}
			return new Range(Min - offset, Max + offset);
		}
	}
}
