namespace Nzy3d.Events
{
	public class ObjectEventArgs : EventArgs
	{
		public ObjectEventArgs(object objectChanged) : base()
		{
			ObjectChanged = objectChanged;
		}

		public object ObjectChanged { get; }
	}
}
