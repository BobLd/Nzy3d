namespace Nzy3d.Maths.Graphs
{
    public class DefaultGraph<V, E> : IGraph<V, E> where E : class
	{
		internal List<V> vertices = new List<V>();
		internal List<E> edges = new List<E>();
		internal List<Tuple<E, V>> edgeStart = new List<Tuple<E, V>>();
		internal List<Tuple<E, V>> edgeStop = new List<Tuple<E, V>>();
		internal Random r = new Random();

		public void AddEdge(E edge, V v1, V v2)
		{
			edges.Add(edge);
			edgeStart.Add(new Tuple<E, V>(edge, v1));
			edgeStop.Add(new Tuple<E, V>(edge, v2));
		}

		public void AddVertex(V vertex)
		{
			vertices.Add(vertex);
		}

		public List<E> GetEdges()
		{
			return edges;
		}

		public V GetEdgeStartVertex(E e)
		{
			return edgeStart.Single(p => p.Item1 == e).Item2;
		}

		public V GetEdgeStopVertex(E e)
		{
			return edgeStop.Single(p => p.Item1 == e).Item2;
		}

		public V GetRandomVertex()
		{
			return GetVertex(r.Next(0, vertices.Count - 1));
		}

		public V GetVertex(int i)
		{
			return vertices[i];
		}

		public List<V> GetVertices()
		{
			return vertices;
		}
	}
}
