namespace Nzy3d.Maths
{
	public sealed class TicToc
	{
		internal DateTime tstart;
		internal DateTime tstop;

		public void Tic()
		{
			tstart = DateTime.Now;
		}

		public double Toc()
		{
			tstop = DateTime.Now;
			return ElapsedSecond;
		}

		public TimeSpan ElapsedTimeSpan
		{
			get { return tstop - tstart; }
		}

		public double ElapsedMillisecond
		{
			get { return this.ElapsedTimeSpan.TotalMilliseconds; }
		}

		public double ElapsedSecond
		{
			get { return this.ElapsedTimeSpan.TotalSeconds; }
		}
	}
}
