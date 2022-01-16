namespace Nzy3d.Glut
{
	internal sealed class StrokeRec
	{
		public int num_coords;

		public CoordRec[] coord;

		public StrokeRec(int num_coords, CoordRec[] coord)
		{
			this.num_coords = num_coords;
			this.coord = coord;
		}
	}
}
