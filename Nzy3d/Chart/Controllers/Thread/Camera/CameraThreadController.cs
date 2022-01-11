using Nzy3d.Chart.Controllers.Camera;
using Nzy3d.Maths;

namespace Nzy3d.Chart.Controllers.Thread.Camera
{
	public class CameraThreadController : AbstractCameraController
	{
		private Coord2d _move;
		private Task _task;
		private CancellationTokenSource _cts;
		private readonly int _sleep = 1; // 1000/25; // nb milisecond wait between two frames

		public float MoveStep { get; set; } = 0.0005f;

		public CameraThreadController()
		{
		}

		public CameraThreadController(Chart chart) : this()
		{
			Register(chart);
		}

		public override void Dispose()
		{
			System.Diagnostics.Debug.WriteLine("CameraThreadController.Dispose");
			Stop();
			base.Dispose();
		}

		public void Start()
		{
			if (_task == null)
			{
				System.Diagnostics.Debug.WriteLine("CameraThreadController.Start");
				_cts = new CancellationTokenSource();
				_task = Task.Factory.StartNew(Run, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
			}
		}

		public void Stop()
		{
			if (_task != null)
			{
				System.Diagnostics.Debug.WriteLine("CameraThreadController.Stop");
				_cts.Cancel();
				_cts.Dispose();
				_cts = null;
				_task = null;
			}
		}

		public async Task Run()
		{
			System.Diagnostics.Debug.WriteLine("CameraThreadController.Run");
			_move = new Coord2d(MoveStep, 0);
			while (_task?.IsCanceled == false)
			{
				//System.Diagnostics.Debug.WriteLine($"CameraThreadController.Run: {Environment.CurrentManagedThreadId}");
				try
				{
					_cts.Token.ThrowIfCancellationRequested();
					await Rotate(_move).ConfigureAwait(false);
					await Task.Delay(_sleep).ConfigureAwait(false);
				}
				catch (OperationCanceledException)
                {
					_task = null;
				}
				catch (ThreadInterruptedException)
				{
					_task = null;
				}
			}
		}
	}
}
