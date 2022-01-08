namespace Nzy3d.Maths
{
	public class TicToc
	{
		internal DateTime tstart;

		internal DateTime tstop;
		public void tic()
		{
			tstart = DateTime.Now;
		}

		public double toc()
		{
			tstop = DateTime.Now;
			return elapsedSecond;
		}

		public TimeSpan elapsedTimeSpan
		{
			get { return tstop - tstart; }
		}

		public double elapsedMillisecond
		{
			get { return this.elapsedTimeSpan.TotalMilliseconds; }
		}

		public double elapsedSecond
		{
			get { return this.elapsedTimeSpan.TotalSeconds; }
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
