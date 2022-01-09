namespace Nzy3d.Maths
{
	public class Normal
	{
		public static Coord3d Compute(Coord3d p0, Coord3d p1, Coord3d p2)
		{
			var v1 = new Vector3d(p0, p1);
			var v2 = new Vector3d(p1, p2);
			var norm = v1.Cross(v2);
			float d = norm.Distance(Coord3d.ORIGIN);
			return norm.Divide(d);
		}
	}
}
