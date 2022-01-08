using Nzy3d.Colors;
using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Primitives
{
    /// <summary>
    /// <para>
    /// A Parallelepiped is a parallelepiped rectangle that is Drawable
    /// and Wireframeable.
    /// A future version of Rectangle3d should consider it as a Composite3d.
    /// </para>
    /// <para>
    /// This class has been implemented for debugging purpose and inconsistency
    /// of its input w.r.t other primitives should not be considered
    /// (no setData function).
    /// </para>
    /// <para>@author Martin Pernollet</para>
    /// </summary>
    public class Parallelepiped : AbstractWireframeable, ISingleColorable, IMultiColorable
	{
		private ColorMapper _mapper;
		private List<Polygon> _quads;

		private Color _color;
		public Parallelepiped() : base()
		{
			_bbox = new BoundingBox3d();
			_quads = new List<Polygon>(6);
		}

		public Parallelepiped(BoundingBox3d b) : base()
		{
			_bbox = new BoundingBox3d();
			_quads = new List<Polygon>();
			SetData(b);
		}

		public override void Draw(Rendering.View.Camera cam)
		{
			foreach (Polygon quad in _quads)
			{
				quad.Draw(cam);
			}
		}

		/// <summary>
		/// Return the transform that was affected to this composite.
		/// </summary>
		public override Transform.Transform Transform
		{
			get { return _transform; }
			set
			{
				_transform = value;
				lock (_quads)
				{
					foreach (Polygon s in _quads)
					{
						if (s != null)
						{
							s.Transform = value;
						}
					}
				}
			}
		}

		public override Color WireframeColor
		{
			get { return base.WireframeColor; }
			set
			{
				base.WireframeColor = value;
				lock (_quads)
				{
					foreach (Polygon s in _quads)
					{
						if (s != null)
						{
							s.WireframeColor = value;
						}
					}
				}
			}
		}

		public override bool WireframeDisplayed
		{
			get { return base.WireframeDisplayed; }
			set
			{
				base.WireframeDisplayed = value;
				lock (_quads)
				{
					foreach (Polygon s in _quads)
					{
						if (s != null)
						{
							s.WireframeDisplayed = value;
						}
					}
				}
			}
		}

		public override float WireframeWidth
		{
			get { return base.WireframeWidth; }
			set
			{
				base.WireframeWidth = value;
				lock (_quads)
				{
					foreach (Polygon s in _quads)
					{
						if (s != null)
						{
							s.WireframeWidth = value;
						}
					}
				}
			}
		}

		public override bool FaceDisplayed
		{
			get { return base.FaceDisplayed; }
			set
			{
				base.FaceDisplayed = value;
				lock (_quads)
				{
					foreach (Polygon s in _quads)
					{
						if (s != null)
						{
							s.FaceDisplayed = value;
						}
					}
				}
			}
		}

		public void SetData(BoundingBox3d box)
		{
			_bbox.Reset();
			_bbox.Add(box);
			_quads = new List<Polygon>(6);

			// Add 6 polygons to list
			for (int i = 0; i <= 5; i++)
			{
				_quads.Add(new Polygon());
			}

			_quads[0].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMin, _bbox.ZMax)));
			_quads[0].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMin, _bbox.ZMin)));
			_quads[0].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMax, _bbox.ZMin)));
			_quads[0].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMax, _bbox.ZMax)));

			_quads[1].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMax, _bbox.ZMax)));
			_quads[1].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMax, _bbox.ZMin)));
			_quads[1].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMin, _bbox.ZMin)));
			_quads[1].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMin, _bbox.ZMax)));

			_quads[2].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMax, _bbox.ZMax)));
			_quads[2].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMax, _bbox.ZMin)));
			_quads[2].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMax, _bbox.ZMin)));
			_quads[2].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMax, _bbox.ZMax)));

			_quads[3].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMin, _bbox.ZMax)));
			_quads[3].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMin, _bbox.ZMin)));
			_quads[3].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMin, _bbox.ZMin)));
			_quads[3].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMin, _bbox.ZMax)));

			_quads[4].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMin, _bbox.ZMax)));
			_quads[4].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMin, _bbox.ZMax)));
			_quads[4].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMax, _bbox.ZMax)));
			_quads[4].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMax, _bbox.ZMax)));

			_quads[5].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMin, _bbox.ZMin)));
			_quads[5].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMin, _bbox.ZMin)));
			_quads[5].Add(new Point(new Coord3d(_bbox.XMin, _bbox.YMax, _bbox.ZMin)));
			_quads[5].Add(new Point(new Coord3d(_bbox.XMax, _bbox.YMax, _bbox.ZMin)));
		}

		public ColorMapper ColorMapper
		{
			get { return _mapper; }
			set
			{
				_mapper = value;
				lock (_quads)
				{
					foreach (Polygon s in _quads)
					{
						if (s != null)
						{
							s.ColorMapper = value;
						}
					}
				}
			}
		}

		public Color Color
		{
			get { return _color; }
			set
			{
				_color = value;
				lock (_quads)
				{
					foreach (Polygon s in _quads)
					{
						if (s != null)
						{
							s.Color = value;
						}
					}
				}
			}
		}
	}
}
