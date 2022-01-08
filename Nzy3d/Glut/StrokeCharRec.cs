namespace Nzy3d.Glut
{
    public class StrokeCharRec
	{
		public int num_strokes;
		public StrokeRec[] stroke;
		public float center;

		public float right;

		public StrokeCharRec(int num_strokes, StrokeRec[] stroke, float center, float right)
		{
			this.num_strokes = num_strokes;
			this.stroke = stroke;
			this.center = center;
			this.right = right;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
