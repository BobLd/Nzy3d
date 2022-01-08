using Nzy3d.Colors;
using Nzy3d.Events;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Plot3D.Rendering.View;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Primitives
{
	/// <summary>
	/// <para>
	/// A <see cref="CompileableComposite"/> allows storage and subsequent faster execution of individual
	/// contained instances drawing routines in an OpenGL display list.
	/// </para>
	/// <para>
	/// Compiling the object take the time needed to render it as a standard <see cref="AbstractComposite"/>,
	/// and rendering it once it is compiled seems to take roughly half the time up to now.
	/// Since compilation occurs during a <see cref="Draw" />, the first call to <see cref="Draw" /> is supposed
	/// to be 1.5x longer than a standard <see cref="AbstractComposite"/>, while all next cycles would be 0.5x
	/// longer.
	/// </para>
	/// <para>
	/// Compilation occurs when the content or the display attributes of this Composite changes
	/// (then all add(), remove(), setColor(), setWireFrameDisplayed(), etc). One can also force
	/// rebuilding the object by calling recompile();
	/// </para>
	/// <para>
	/// IMPORTANT: for the moment, <see cref="CompileableComposite"/> should not be use in a charts using a
	/// <see cref="Quality"/> superior to Intermediate, in other word, you should not desire to have alpha
	/// enabled in your scene. Indeed, alpha requires ordering of polygons each time the viewpoint changes,
	/// which would require to recompile the object.
	/// </para>
	/// <para>@author Nils Hoffmann</para>
	/// </summary>
	public class CompileableComposite : AbstractWireframeable, ISingleColorable, IMultiColorable
	{
		private int _dlID = -1;
		private bool _resetDL;
		internal ColorMapper _mapper;
		internal Color _color;
		internal bool _detailedToString;

		internal List<AbstractDrawable> _components;
		public CompileableComposite() : base()
		{
			_components = new List<AbstractDrawable>();
		}

		/// <summary>
		/// Force the object to be rebuilt and stored as a display list at the next call to draw().
		/// </summary>
		/// <remarks>This operation does not rebuilt the object, but only marks it as "to be rebuilt" for new call to draw().</remarks>
		public void Recompile()
		{
			_resetDL = true;
		}

		/// <summary>
		/// Reset the object if required, compile the object if it is not compiled,
		/// and execute actual rendering.
		/// </summary>
		/// <param name="cam">Camera to draw for.</param>
		public override void Draw(Camera cam)
		{
			if (_resetDL)
			{
				this.Reset();
			}

			if (_dlID == -1)
			{
				this.Compile(cam);
			}
			this.Execute(cam);
		}

		/// <summary>
		/// If you call compile, the display list will be regenerated.
		/// </summary>
		internal void Compile(Camera cam)
		{
			this.Reset();
			// clear old list
			this.NullifyChildrenTransforms();
			_dlID = GL.GenLists(1);
			GL.NewList(_dlID, ListMode.Compile);
			this.DrawComponents(cam);
			GL.EndList();
		}

		internal void Execute(Camera cam)
		{
			_transform?.Execute();
			GL.CallList(_dlID);
		}

		internal void Reset()
		{
			if (_dlID != -1)
			{
				if (GL.IsList(_dlID))
				{
					GL.DeleteLists(_dlID, 1);
				}
				_dlID = -1;
			}
			_resetDL = false;
		}

		/// <summary>
		/// When a drawable has a null transform, no transform is applied at draw(...).
		/// </summary>
		internal void NullifyChildrenTransforms()
		{
			throw new NotImplementedException("CompileableComposite.NullifyChildrenTransforms: BobLd - need to check lock here");

			lock (_components)
			{
			}

			foreach (AbstractDrawable c in _components)
			{
				if (c != null)
				{
					c.Transform = null;
				}
			}
		}

		internal void DrawComponents(Camera cam)
		{
			lock (_components)
			{
				foreach (AbstractDrawable s in _components)
				{
					s?.Draw(cam);
				}
			}
		}

		/// <summary>
		/// Add all drawables stored by this composite.
		/// </summary>
		public void Add(List<AbstractDrawable> drawables)
		{
			this.Add(drawables);
		}

		/// <summary>
		/// Remove all drawables stored by this composite.
		/// </summary>
		public void Add(IEnumerable<AbstractDrawable> drawables)
		{
			lock (_components)
			{
				_components.AddRange(drawables);
				Recompile();
			}
		}

		/// <summary>
		/// Clear the list of drawables stored by this composite.
		/// </summary>
		public void Clear()
		{
			lock (_components)
			{
				_components.Clear();
				Recompile();
			}
		}

		/// <summary>
		/// Add a Drawable stored by this composite.
		/// </summary>
		public void Add(AbstractDrawable drawable)
		{
			lock (_components)
			{
				_components.Add(drawable);
				Recompile();
			}
		}

		/// <summary>
		/// Remove a Drawable stored by this composite.
		/// </summary>
		public void Remove(AbstractDrawable drawable)
		{
			lock (_components)
			{
				_components.Remove(drawable);
				Recompile();
			}
		}

		/// <summary>
		/// Get a Drawable stored by this composite.
		/// </summary>
		public AbstractDrawable GetDrawable(int p)
		{
			return _components[p];
		}

		/// <summary>
		/// Get an enumerator through the list of drawabless stored by this composite.
		/// </summary>
		public IEnumerable<AbstractDrawable> GetDrawables
		{
			get { return _components; }
		}

		/// <summary>
		/// Return the number of Drawable stored by this composite.
		/// </summary>
		public int Size
		{
			get { return _components.Count; }
		}

		public override BoundingBox3d Bounds
		{
			get
			{
				BoundingBox3d box = new BoundingBox3d();
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c?.Bounds != null)
						{
							box.Add(c.Bounds);
						}
					}
				}
				return box;
			}
		}

		public override Color WireframeColor
		{
			get { return base.WireframeColor; }
			set
			{
				base.WireframeColor = value;
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is AbstractWireframeable cWf)
						{
							cWf.WireframeColor = Color;
						}
					}
				}
				Recompile();
			}
		}

		public override bool WireframeDisplayed
		{
			get { return base.WireframeDisplayed; }
			set
			{
				base.WireframeDisplayed = value;
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is AbstractWireframeable cWf)
						{
							cWf.WireframeDisplayed = value;
						}
					}
				}
				Recompile();
			}
		}

		public override float WireframeWidth
		{
			get { return base.WireframeWidth; }
			set
			{
				base.WireframeWidth = value;
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is AbstractWireframeable cWf)
						{
							cWf.WireframeWidth = value;
						}
					}
				}
				Recompile();
			}
		}

		public override bool FaceDisplayed
		{
			get { return base.FaceDisplayed; }
			set
			{
				base.FaceDisplayed = value;
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is AbstractWireframeable cWf)
						{
							cWf.FaceDisplayed = value;
						}
					}
				}
				Recompile();
			}
		}

		public ColorMapper ColorMapper
		{
			get { return _mapper; }
			set
			{
				_mapper = value;
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is IMultiColorable cMC)
						{
							cMC.ColorMapper = value;
						}
						else if (c is ISingleColorable cSC)
						{
							cSC.Color = value.Color(c.Barycentre);
						}
					}
				}
				FireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
				Recompile();
			}
		}

		public Color Color
		{
			get { return _color; }
			set
			{
				_color = value;
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is ISingleColorable cSC)
						{
							cSC.Color = value;
						}
					}
				}
				FireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
				Recompile();
			}
		}

		/// <summary>
		/// Returns the string representation of this composite
		/// </summary>
		public override string ToString()
		{
			return base.ToString(0);
		}

		public override string ToString(int depth)
		{
			string output = Utils.Blanks(depth) + "(Composite3d) #elements:" + _components.Count + " | isDisplayed=" + this.Displayed;
			if (_detailedToString)
			{
				int k = 0;
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is AbstractComposite cAc)
						{
							output += "\r\n" + cAc.ToString(depth + 1);
						}
						else if (c != null)
						{
							output += "\r\n" + Utils.Blanks(depth + 1) + "Composite element[" + k + "]:" + c.ToString();
						}
						else
						{
							output += "\r\n" + Utils.Blanks(depth + 1) + "(null)";
						}
						k++;
					}
				}
			}
			return output;
		}

		/// <summary>
		/// Get / Set the property.
		/// When to true, the <see cref="CompileableComposite.toString"/> method will give the detail of each element
		/// of this composite object in a tree like layout.
		/// </summary>
		public bool DetailedToString
		{
			get { return _detailedToString; }
			set { _detailedToString = value; }
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
