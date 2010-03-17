using System;
using System.Drawing;
using System.Reflection;
using System.Timers;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace RageWorld
{
	public sealed class Game : GameWindow
	{
		private Camera _camera;
		private Window _window;
		private Timer _timer = new Timer();
		private bool _updateFps = false;
		private static readonly string name = "RageWorld";

		public Game()
			: base(800, 600, GraphicsMode.Default, name)
		{
			Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);

			Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(OnKeyDown);
			MouseLeave += (s, e) => _camera.StopDrag();

			_timer.Elapsed += (s, e) => { _updateFps = true; };
			_timer.Interval = 1000;
			_timer.Start();

			_window = new Window(new Vector2(0.02f, 0.02f), new Vector2(0.98f, 0.25f));

			_camera = new Camera(Keyboard, Mouse, CameraBehavior.Flight);
			_camera.Children.Add(new Axis());
			_camera.Children.Add(new Grid(1.0f, 25.0f));
			_camera.Children.Add(new Cube(5.0f, 0.0f, 5.0f, 2.0f));
			_camera.Children.Add(new Cube(-5.0f, 0.0f, -5.0f));
			_camera.Children.Add(new Triangle(5.0f));
			_camera.Children.Add(_window);
		}

		protected override void OnLoad(EventArgs e)
		{
			GL.ClearColor(Color.White);
			GL.ShadeModel(ShadingModel.Smooth);
			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			base.OnLoad(e);
		}

		protected override void OnResize(EventArgs e)
		{
			_camera.Resize(ClientRectangle);

			base.OnResize(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			_camera.Render(e.Time);

			SwapBuffers();

			base.OnRenderFrame(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			if (Keyboard[Key.Escape])
				Exit();

			_camera.Update(e.Time);

			if (Math.Abs((_camera.TargetPosition - _camera.Position).Length) < 0.001f)
				_window.Color = Color.FromArgb(33, 33, 33, 33);
			else
				_window.Color = Color.FromArgb(33, 200, 33, 33);

			if (_updateFps)
			{
				_updateFps = false;

				Title = String.Format("{0} ({1:F2})", name, 1.0f / e.Time);
			}

			base.OnUpdateFrame(e);
		}

		private void OnKeyDown(object o, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				WindowState = (WindowState == WindowState.Fullscreen) ? WindowState.Normal : WindowState.Fullscreen;
		}

		[STAThread]
		private static void Main()
		{
			using (Game game = new Game())
			{
				game.Run(60);
			}
		}
	}
}
