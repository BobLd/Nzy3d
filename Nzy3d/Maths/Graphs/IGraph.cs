namespace Nzy3d.Maths.Graphs
{
	public interface IGraph<V, E>
	{
		void AddVertex(V vertex);

		void AddEdge(E edge, V v1, V v2);

		V GetVertex(int i);

		V GetRandomVertex();

		List<V> GetVertices();

		List<E> GetEdges();

		V GetEdgeStartVertex(E e);

		V GetEdgeStopVertex(E e);
	}
}
