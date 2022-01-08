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

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
