namespace Nzy3d.Glut
{
	public class StrokeRec
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

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
