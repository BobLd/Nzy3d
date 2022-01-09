namespace Nzy3d.Maths.Algorithms.Interpolation.Bernstein
{
	/// <summary>
	/// Helper class for the spline3d classes in this namespace. Used to compute
	/// subdivision points of the curve.
	/// </summary>
	public class BernsteinPolynomial
	{
		public float[] b0;
		public float[] b1;
		public float[] b2;
		public float[] b3;

		public int resolution;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="res">Resolution : number of subdivision steps between each control point of the spline3d (must be greater than or equal to two)</param>
		public BernsteinPolynomial(int res)
		{
			if (res < 2)
			{
				throw new ArgumentException("Resolution must be at least 2", nameof(res));
			}
			resolution = res;
			b0 = new float[res];
			b1 = new float[res];
			b2 = new float[res];
			b3 = new float[res];
			float t = 0;
			float dt = 1 / (resolution - 1);
			for (int i = 0; i <= resolution - 1; i++)
			{
				float t1 = 1 - t;
				float t12 = t1 * t1;
				float t2 = t * t;
				b0[i] = t1 * t12;
				b1[i] = 3 * t * t12;
				b2[i] = 3 * t2 * t1;
				b3[i] = t * t2;
				t = +dt;
			}
		}
	}
}
