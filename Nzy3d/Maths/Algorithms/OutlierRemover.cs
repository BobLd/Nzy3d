namespace Nzy3d.Maths.Algorithms
{
	public class OutlierRemover
	{
		public static int[] GetOutlierIndices(double[] values, int nVariance)
		{
			throw new NotImplementedException();
		}

		public static int[] GetInlierIndices(double[] values, int nVariance)
		{
			throw new NotImplementedException();
		}

		public static double[] GetOutlierValues(double[] values, int nVariance)
		{
			Scale bounds = GetInlierBounds(values, nVariance);
			return System.Array.FindAll(values, x => !bounds.Contains(x));
		}

		public static double[] GetInlierValues(double[] values, int nVariance)
		{
			Scale bounds = GetInlierBounds(values, nVariance);
			return System.Array.FindAll(values, x => bounds.Contains(x));
		}

		public static Scale GetInlierBounds(double[] values, int nVariance)
		{
			if (values.Length == 0)
			{
				return new Scale(double.NaN, double.NaN);
			}

			double[] dists = new double[values.Length];
			double med = Statistics.Median(values, true);

			for (int i = 0; i <= values.Length - 1; i++)
			{
				dists[i] = Math.Abs(values[i] - med);
			}

			double mad = Statistics.Median(dists, true);
			double upperBound = med + mad * nVariance;
			double lowerBound = med - mad * nVariance;
			return new Scale(lowerBound, upperBound);
		}
	}
}
