namespace Nzy3d.Events
{
	public class DrawableChangedEventArgs : ObjectEventArgs
	{

		private FieldChanged _what;
		public enum FieldChanged : int
		{
			Data = 0,
			Transform = 1,
			Color = 2,
			Metadata = 3,
			Displayed = 4
		}

		public DrawableChangedEventArgs(object objectChanged, FieldChanged what) : base(objectChanged)
		{
			_what = what;
		}

		public FieldChanged What
		{
			get { return _what; }
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
