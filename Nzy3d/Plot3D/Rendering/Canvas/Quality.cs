namespace Nzy3d.Plot3D.Rendering.Canvas
{
	/// <summary>
	/// <para>
	/// Provides a structure for setting the rendering quality, i.e., the tradeoff
	/// between computation speed, and graphic quality. Following mode have an impact
	/// on the way the <see cref="View.View"/> makes its GL2 initialization.
	/// The <see cref="Quality"/> may also activate an <see cref="Ordering.AbstractOrderingStrategy"/> algorithm
	/// that enables clean alpha results.
	/// </para>
	/// <para>
	/// Fastest:
	/// - No transparency, no color shading, just handle depth buffer.
	/// </para>
	/// <para>
	/// Intermediate:
	/// - include Fastest mode abilities
	/// - Color shading, mainly usefull to have interpolated colors on polygons.
	/// </para>
	/// <para>
	/// Advanced:
	/// - include Intermediate mode abilities
	/// - Transparency (GL2 alpha blending + polygon ordering in scene graph)
	/// </para>
	/// <para>
	/// Nicest:
	/// - include Advanced mode abilities
	/// - Anti aliasing on wires
	/// </para>
	/// <para>
	/// Toggling rendering model: one may either choose to have a repaint-on-demand
	/// or repaint-continuously model. Setting isAnimated(false) will desactivate a
	/// the <see cref="Animator"/> updating the choosen <see cref="ICanvas"/> implementation.
	/// </para>
	/// <para>setAutoSwapBuffer(false) will equaly configure the <see cref="ICanvas"/>.</para>
	/// <para>@author Martin Pernollet</para>
	/// </summary>
	public class Quality
	{
		internal bool _disableDepthBufferWhenAlpha;
		internal bool _isAnimated = true;

		internal bool _isAutoSwapBuffer = true;

		/// <summary>
		/// Provides a structure for setting the rendering quality, i.e., the tradeoff
		/// between computation speed, and graphic quality. Following mode have an impact
		/// on the way the <see cref="View.View"/> makes its GL2 initialization.
		/// The <see cref="Quality"/> may also activate an <see cref="Ordering.AbstractOrderingStrategy"/> algorithm
		/// that enables clean alpha results.
		/// </summary>
		/// <param name="depthActivated">Depth activated.</param>
		/// <param name="alphaActivated">Alpha activated.</param>
		/// <param name="smoothColor">Smooth color.</param>
		/// <param name="smoothPoint">Smooth point.</param>
		/// <param name="smoothLine">Smooth line.</param>
		/// <param name="smoothPolygon">Smooth polygon.</param>
		/// <param name="disableDepth">Disable depth buffer when alpha.</param>
		public Quality(bool depthActivated, bool alphaActivated, bool smoothColor, bool smoothPoint, bool smoothLine, bool smoothPolygon, bool disableDepth)
		{
			DepthActivated = depthActivated;
			AlphaActivated = alphaActivated;
			SmoothColor = smoothColor;
			SmoothPoint = smoothPoint;
			SmoothLine = smoothLine;
			SmoothPolygon = smoothPolygon;
			DisableDepthBufferWhenAlpha = disableDepth;
		}

		public bool DepthActivated { get; set; }

		public bool AlphaActivated { get; set; }

		public bool SmoothColor { get; set; }

		public bool SmoothLine { get; set; }

		/// <summary>
		/// same as <see cref="SmoothLine"/>.
		/// </summary>
		public bool SmoothEdge
		{
			get { return SmoothLine; }
			set { SmoothLine = value; }
		}

		public bool SmoothPoint { get; set; }

		public bool SmoothPolygon { get; set; }

		public bool DisableDepthBufferWhenAlpha { get; set; }

		public bool IsAnimated
		{
			get { return _isAnimated; }
			set { _isAnimated = value; }
		}

		public bool IsAutoSwapBuffer
		{
			get { return _isAutoSwapBuffer; }
			set { _isAutoSwapBuffer = value; }
		}

		/// <summary>
		/// Enables alpha, color interpolation and antialiasing on lines, points, and polygons.
		/// </summary>
		public static readonly Quality Nicest = new Quality(true, true, true, true, true, true, false);

		/// <summary>
		/// Enables alpha and color interpolation.
		/// </summary>
		public static readonly Quality Advanced = new Quality(true, true, true, false, false, false, false);

		/// <summary>
		/// Enables color interpolation.
		/// </summary>
		public static readonly Quality Intermediate = new Quality(true, false, true, false, false, false, false);

		/// <summary>
		/// Minimal quality to allow fastest rendering (no alpha, interpolation or antialiasing).
		/// </summary>
		public static readonly Quality Fastest = new Quality(true, false, false, false, false, false, false);
	}
}
