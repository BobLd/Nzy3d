using Nzy3d.Chart.Controllers;

namespace Nzy3d.Events
{
	public class ControllerEventArgs : ObjectEventArgs
	{
        public enum FieldChanged : byte
        {
            Data = 0,
            Transform = 1,
            Color = 2,
            Metadata = 3,
            Displayed = 4
        }

        public ControllerEventArgs(object objectChanged, ControllerType type, object value) : base(objectChanged)
		{
			Type = type;
			Value = value;
		}

        public ControllerType Type { get; }

        public object Value { get; }

        /// <inheritdoc/>
        public override string ToString()
		{
			return "ControllerEvent(type,value): " + Type + ", " + Value;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
