namespace Nzy3d.Maths.Algorithms.Interpolation.Bernstein
{
	/// <summary>
	/// Helper class for the spline3d classes in this namespace. Used to compute
	/// subdivision points of the curve.
	/// </summary>
	public sealed class BernsteinPolynomial
	{
		public double[] b0;
		public double[] b1;
		public double[] b2;
		public double[] b3;

		public int resolution;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="res">Resolution : number of subdivision steps between each control point of the spline3d (must be greater than or equal to two)</param>
		public BernsteinPolynomial(int res)
		{
			if (res < 2)
			{
				throw new ArgumentException("Resolution must be at least 2.", nameof(res));
			}

			resolution = res;
			b0 = new double[res];
			b1 = new double[res];
			b2 = new double[res];
			b3 = new double[res];
			double t = 0;
			double dt = 1 / (resolution - 1);
			for (int i = 0; i <= resolution - 1; i++)
			{
				double t1 = 1 - t;
				double t12 = t1 * t1;
				double t2 = t * t;
				b0[i] = t1 * t12;
				b1[i] = 3 * t * t12;
				b2[i] = 3 * t2 * t1;
				b3[i] = t * t2;
				t = +dt;
			}
		}
	}
}
