using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Primitives.Graphs.Layout
{
    public class DefaultGraphLayout2d<V> : IGraphLayout2d<V>
	{
        private readonly List<Tuple<V, Coord2d>> _values = new List<Tuple<V,Coord2d>>();

        /*
		public Maths.Coord2d getV(V v)
		{
            return new Maths.Coord2d();
		}

		public System.Collections.Generic.List<Maths.Coord2d> values()
		{
            return _values;
		}
        */

        public Coord2d VertexPosition
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Coord2d getV(V v)
        {
            throw new NotImplementedException();
        }

        public List<Coord2d> values()
        {
            throw new NotImplementedException();
        }
    }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
