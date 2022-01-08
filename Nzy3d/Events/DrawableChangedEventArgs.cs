namespace Nzy3d.Events
{
	public class DrawableChangedEventArgs : ObjectEventArgs
	{
        public enum FieldChanged : byte
		{
			Data = 0,
			Transform = 1,
			Color = 2,
			Metadata = 3,
			Displayed = 4
		}

		public DrawableChangedEventArgs(object objectChanged, FieldChanged what) : base(objectChanged)
		{
			What = what;
		}

        public FieldChanged What { get; }
    }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
