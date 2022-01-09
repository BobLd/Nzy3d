namespace Nzy3d.Maths.Algorithms
{
	public class OutlierRemover
	{
		public static int[] GetOutlierIndices(float[] values, int nVariance)
		{
			throw new NotImplementedException();
		}

		public static int[] GetInlierIndices(float[] values, int nVariance)
		{
			throw new NotImplementedException();
		}

		public static float[] GetOutlierValues(float[] values, int nVariance)
		{
			Scale bounds = GetInlierBounds(values, nVariance);
			return System.Array.FindAll(values, x => !bounds.Contains(x));
		}

		public static float[] GetInlierValues(float[] values, int nVariance)
		{
			Scale bounds = GetInlierBounds(values, nVariance);
			return System.Array.FindAll(values, x => bounds.Contains(x));
		}

		public static Scale GetInlierBounds(float[] values, int nVariance)
		{
			if (values.Length == 0)
			{
				return new Scale(float.NaN, float.NaN);
			}

			float[] dists = new float[values.Length];
			float med = Statistics.Median(values, true);

			for (int i = 0; i <= values.Length - 1; i++)
			{
				dists[i] = MathF.Abs(values[i] - med);
			}

			float mad = Statistics.Median(dists, true);
			float upperBound = med + mad * nVariance;
			float lowerBound = med - mad * nVariance;
			return new Scale(lowerBound, upperBound);
		}
	}
}
