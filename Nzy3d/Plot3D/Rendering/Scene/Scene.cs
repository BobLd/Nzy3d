using Nzy3d.Plot3D.Primitives;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Plot3D.Rendering.Lights;
using Nzy3d.Plot3D.Rendering.Scene;

namespace Nzy3d.Plot3D.Rendering.View
{
	/// <summary>
	/// <para>
	/// A Scene holds a <see cref="Graph"/> to be rendered by a list
	/// <see cref="View"/>s.
	/// </para>
	/// <para>
	/// The creation of Views is not of user concern, since it is handled
	/// during the registration of the Scene by a <see cref="ICanvas"/>.
	/// The newView() is thus Friend because it is supposed to be called
	/// by a Canvas3d or a View only.
	/// </para>
	/// <para>
	/// The Scene is called by the <see cref="Renderer3d"/> to provide the effective
	/// (Friend) GL2 calls for initialization (List and Texture loading),
	/// clearing of window, and current view rendering.
	/// </para>
	/// <para>@author Martin Pernollet</para>
	/// </summary>
	public class Scene
	{
		internal List<View> _views;
		internal Graph _graph;

		internal LightSet _lighSet;
		public Scene() : this(false)
		{
		}

		public Scene(bool graphsort)
		{
			_graph = new Graph(this, Factories.OrderingStrategyFactory.GetInstance(), graphsort);
			_lighSet = new LightSet();
			_views = new List<View>();
		}

		public Scene(Graph graph)
		{
			_graph = graph;
			_lighSet = new LightSet();
			_views = new List<View>();
		}

		/// <summary>
		/// Handles disposing of the Graph as well as all views pointing to this Graph.
		/// </summary>
		public void Dispose()
		{
			_graph.Dispose();
			foreach (View v in _views)
			{
				v.Dispose();
			}
			_views.Clear();
		}

		/// <summary>
		/// Get/Set the scene graph attached to this scene
		/// </summary>
		public Graph Graph
		{
			get { return _graph; }
			set { _graph = value; }
		}

		/// <summary>
		/// Get/Set the light set attached to this scene
		/// </summary>
		public LightSet LightSet
		{
			get { return _lighSet; }
			set { _lighSet = value; }
		}

		public IEnumerable<View> Views
		{
			get { return _views; }
		}

		/// <summary>
		/// Add a list of drawable to the scene
		/// </summary>
		public void Add(List<AbstractDrawable> drawables)
		{
			_graph.Add(drawables);
		}

		/// <summary>
		/// Add a drawable to the scene
		/// </summary>
		public void Add(AbstractDrawable drawable)
		{
			_graph.Add(drawable);
		}

		/// <summary>
		/// Add a drawable to the scene and refresh on demand.
		/// </summary>
		public void Add(AbstractDrawable drawable, bool updateViews)
		{
			_graph.Add(drawable, updateViews);
		}

		/// <summary>
		/// Remove a drawable from the scene
		/// </summary>
		public void Remove(AbstractDrawable drawable)
		{
			_graph.Remove(drawable);
		}

		/// <summary>
		/// Remove a drawable from the scene and refresh on demand.
		/// </summary>
		public void Remove(AbstractDrawable drawable, bool updateViews)
		{
			_graph.Remove(drawable, updateViews);
		}

		/// <summary>
		/// Add a light to the scene
		/// </summary>
		public void Add(Light light)
		{
			_lighSet.Add(light);
		}

		/// <summary>
		/// Remove a light from the scene
		/// </summary>
		public void Remove(Light light)
		{
			_lighSet.Remove(light);
		}

		/// <summary>
		/// Instantiate a View attached to the given Canvas, and return its reference
		/// </summary>
		public virtual View NewView(ICanvas canvas, Quality quality)
		{
			View view = new View(this, canvas, quality);
			_views.Add(view);
			return view;
		}

		public virtual void ClearView(View view)
		{
			_views.Remove(view);
			view.Dispose();
		}

		/// <summary>
		/// Return the scene graph string representation
		/// </summary>
		public override string ToString()
		{
			return _graph.ToString();
		}
	}
}
