using Nzy3d.Colors;
using Nzy3d.Events;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.Legends;
using Nzy3d.Plot3D.Rendering.View;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Primitives
{
	/// <summary>
	/// <para>
	/// A <see cref="AbstractDrawable"/> defines objects that may be rendered into an OpenGL
	/// context provided by a <see cref="Rendering.Canvas.ICanvas"/>.
	/// <br/>
	/// A <see cref="AbstractDrawable"/> must basically provide a rendering function called draw()
	/// that receives a reference to a GL2 and a GLU context. It may also
	/// use a reference to a Camera in order to implement specific behaviors
	/// according to the Camera position.
	/// <br/>
	/// A <see cref="AbstractDrawable"/> provides services for setting the transformation factor
	/// that is used inside the draw function, as well as a getter of the
	/// object's BoundingBox3d. Note that the BoundingBox must be set by
	/// a concrete descendant of a <see cref="AbstractDrawable"/>.
	/// <br/>
	/// A good practice is to define a setData function for initializing a <see cref="AbstractDrawable"/>
	/// and building its polygons. Since each class may have its own inputs, setData
	/// is not part of the interface but should be used as a convention.
	/// When not defining a setData function, a <see cref="AbstractDrawable"/> may have its data loaded by
	/// an "add(Drawable)" function.
	/// <br/>
	/// Note: A <see cref="AbstractDrawable"/> may last provide the information whether it is displayed or not,
	/// according to a rendering into the FeedBack buffer. This is currently supported
	/// specifically for the <see cref="Axes.AxeBox"/> object but could be extended with some few more
	/// algorithm for referencing all GL2 polygons.
	/// </para>
	/// <para>@author Martin Pernollet</para>
	/// </summary>
	public abstract class AbstractDrawable : IGLRenderer, ISortableDraw
	{
		internal Transform.Transform _transform;
		internal BoundingBox3d _bbox;
		internal Legend _legend = null;
		internal List<IDrawableListener> _listeners = new List<IDrawableListener>();
		internal bool _displayed = true;

		internal bool _legendDisplayed = false;
		public void Dispose()
		{
			_listeners?.Clear();
		}

		public abstract void Draw(Camera cam);

		internal static void CallC(Color c)
		{
			GL.Color4(c.R, c.G, c.B, c.A);
		}

		internal static void CallC(Color c, float alpha)
		{
			GL.Color4(c.R, c.G, c.B, alpha);
		}

		internal static void CallWithAlphaFactor(Color c, float alpha)
		{
			GL.Color4(c.R, c.G, c.B, c.A * alpha);
		}

		/// <summary>
		/// Get / Set object's transformation that is applied at the
		/// beginning of a call to "draw()"
		/// </summary>
		public virtual Transform.Transform Transform
		{
			get { return _transform; }
			set
			{
				_transform = value;
				FireDrawableChanged(DrawableChangedEventArgs.FieldChanged.Transform);
			}
		}

		/// <summary>
		/// Return the BoundingBox of this object
		/// </summary>
		public virtual BoundingBox3d Bounds
		{
			get { return _bbox; }
		}

		/// <summary>
		/// Return the barycentre of this object, which is
		/// computed as the center of its bounding box. If the bounding
		/// box is not available, the returned value is <see cref=" Coord3d.INVALID"/>
		/// </summary>
		public virtual Coord3d Barycentre
		{
			get
			{
				return _bbox != null ? _bbox.GetCenter() : Coord3d.INVALID;
			}
		}

		/// <summary>
		/// Get / Set the display status of this object
		/// </summary>
		public virtual bool Displayed
		{
			get { return _displayed; }
			set
			{
				_displayed = value;
				FireDrawableChanged(DrawableChangedEventArgs.FieldChanged.Displayed);
			}
		}

		public virtual double GetDistance(Camera camera)
		{
			return Barycentre.Distance(camera.Eye);
		}

		public virtual double GetLongestDistance(Camera camera)
		{
			return GetDistance(camera);
		}

		public virtual double GetShortestDistance(Camera camera)
		{
			return GetDistance(camera);
		}

		public Legend Legend
		{
			get { return _legend; }
			set
			{
				_legend = value;
				_legendDisplayed = true;
				FireDrawableChanged(DrawableChangedEventArgs.FieldChanged.Metadata);
			}
		}

		public bool HasLegend
		{
			get { return _legend != null; }
		}

		public bool LegendDisplayed
		{
			get { return _legendDisplayed; }
			set { _legendDisplayed = value; }
		}

		public void AddDrawableListener(IDrawableListener listener)
		{
			_listeners.Add(listener);
		}

		public void RemoveDrawableListener(IDrawableListener listener)
		{
			_listeners.Remove(listener);
		}

		internal void FireDrawableChanged(DrawableChangedEventArgs.FieldChanged eventType)
		{
			FireDrawableChanged(new DrawableChangedEventArgs(this, eventType));
		}

		internal void FireDrawableChanged(DrawableChangedEventArgs e)
		{
			foreach (IDrawableListener listener in _listeners)
			{
				listener.DrawableChanged(e);
			}
		}

		/// <summary>
		/// Returns the string representation of this object
		/// </summary>
		public override string ToString()
		{
			return ToString(0);
		}

		public virtual string ToString(int depth)
		{
			return Utils.blanks(depth) + "(" + this.GetType().Name + ")";
		}
	}
}
