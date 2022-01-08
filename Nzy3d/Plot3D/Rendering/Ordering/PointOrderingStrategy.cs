using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Plot3D.Rendering.Ordering
{
	public class PointOrderingStrategy : IComparer<Coord3d>
	{
		internal Camera _camera;
		public void Sort(List<Coord3d> points, Camera cam)
		{
			_camera = cam;
			points.Sort(this);
		}

		public int Compare(Coord3d o1, Coord3d o2)
		{
			if (_camera == null)
			{
				throw new Exception("No available camera for computing PointOrderingStrategy");
			}

			// Reflexivity
			if (o1.Equals(o2))
			{
				return 0;
			}

			double dist1 = _camera.Eye.Distance(o1);
			double dist2 = _camera.Eye.Distance(o2);
			if (dist1 == dist2)
			{
				return 0;
			}
			else if (dist1 < dist2)
			{
				return 1;
			}
			else
			{
				return -1;
			}
		}

		//
		// Operation must be:
		// symetric: compare(a,b)=-compare(b,a)
		// transitive: ((compare(x, y)>0) && (compare(y, z)>0)) implies compare(x, z)>0    true if all Drawables and the Camera don't change position!
		// consistency?: compare(x, y)==0  implies that sgn(compare(x, z))==sgn(compare(y, z))
		//
	}
}
