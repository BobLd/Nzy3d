using Nzy3d.Colors;
using Nzy3d.Events;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Plot3D.Primitives
{
	/// <summary>
	/// <para>
	/// A Composite gathers several Drawable and provides default methods
	/// for rendering them all in one call.
	/// </para>
	/// <para>@author Martin Pernollet</para>
	/// </summary>
	public class AbstractComposite : AbstractWireframeable, ISingleColorable, IMultiColorable
	{
		internal List<AbstractDrawable> _components = new List<AbstractDrawable>();
		internal ColorMapper _mapper;
		internal Color _color;

		internal bool _detailedToString = false;
		public AbstractComposite() : base()
		{
		}

		/// <summary>
		/// Remove all drawables stored by this composite.
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
		public IEnumerable<AbstractDrawable> Drawables
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

		/// <summary>
		/// Delegate rendering iteratively to all Drawable of this composite.
		/// </summary>
		public override void Draw(Camera cam)
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
		/// Return the transform that was affected to this composite.
		/// </summary>
		public override Transform.Transform Transform
		{
			get { return _transform; }
			set
			{
				_transform = value;
				lock (_components)
				{
					foreach (AbstractDrawable s in _components)
					{
						if (s != null)
						{
							s.Transform = value;
						}
					}
				}
			}
		}

		/// <summary>
		/// Creates and return a BoundingBox3d that embed all available Drawable bounds
		/// </summary>
		public override BoundingBox3d Bounds
		{
			get
			{
				var box = new BoundingBox3d();
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
						if (c is AbstractWireframeable cWF)
						{
							cWF.WireframeColor = value;
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
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is AbstractWireframeable cWF)
						{
							cWF.WireframeDisplayed = value;
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
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is AbstractWireframeable cWF)
						{
							cWF.WireframeWidth = value;
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
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is AbstractWireframeable cWF)
						{
							cWF.FaceDisplayed = value;
						}
					}
				}
			}
		}

		public override bool Displayed
		{
			get { return base.Displayed; }
			set
			{
				base.Displayed = value;
				lock (_components)
				{
					foreach (AbstractDrawable c in _components)
					{
						if (c is AbstractWireframeable cWF)
						{
							cWF.Displayed = value;
						}
					}
				}
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
						if (c is IMultiColorable cIM)
						{
							cIM.ColorMapper = value;
						}
						else if (c is ISingleColorable cIC)
						{
							cIC.Color = value.Color(c.Barycentre);
						}
					}
				}
				FireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
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
						if (c is ISingleColorable cIC)
						{
							cIC.Color = value;
						}
					}
				}
				FireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
			}
		}

		/// <summary>
		/// Returns the string representation of this composite
		/// </summary>
		public override string ToString()
		{
			return ToString(0);
		}

		/// <inheritdoc/>
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
						if (c is AbstractComposite cAC)
						{
							output += "\r\n" + ((AbstractComposite)c).ToString(depth + 1);
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
