namespace Nzy3d.Maths.Algorithms
{
	public class ScaleFinder
	{
		/// <summary>
		/// Apply an outlier remover on input data (<see cref="OutlierRemover.GetInlierValues"/>)
		/// and retrieve the min and max values of the non-rejected values.
		/// </summary>
		public static Scale GetFilteredScale(float[] values, int nvariance)
		{
			return GetMinMaxScale(OutlierRemover.GetInlierValues(values, nvariance));
		}

		/// <summary>
		/// Simply returns the min and max values of the input array into
		/// a Scale object.
		/// </summary>
		public static Scale GetMinMaxScale(float[] values)
		{
			if (values.Length == 0)
			{
				return new Scale(float.NaN, float.NaN);
			}

			return new Scale(Statistics.Min(values), Statistics.Max(values));
		}
	}
}
