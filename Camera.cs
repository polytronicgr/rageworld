using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace RageWorld
{
	public enum CameraBehavior
	{
		Flight,
		FirstPerson
	}

	public sealed class Camera : Frame
	{
		public CameraBehavior Behavior { get; set; }
		private KeyboardDevice _keyboard;
		private MouseDevice _mouse;
		private bool _mouseLeftDrag = false;
		private bool _mouseRightDrag = false;
		private const float _rotateScale = 0.01f;
		private const float _translateScale = 0.1f;

		public Camera(KeyboardDevice k, MouseDevice m, CameraBehavior behavior = CameraBehavior.Flight)
		{
			Behavior = behavior;
			_keyboard = k;
			_mouse = m;
			_mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(OnMouseButtonDown);
			_mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(OnMouseButtonUp);
			_mouse.Move += new EventHandler<MouseMoveEventArgs>(OnMouseMove);
		}

		public override void Resize(Rectangle client)
		{
			GL.Viewport(client);
			GL.MatrixMode(MatrixMode.Projection);
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, client.Width / (float)client.Height, 1.0f, 3000.0f);
			GL.LoadMatrix(ref projection);

			base.Resize(client);
		}

		public override void Render(double time)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			base.Render(time);
		}

		public override void Update(double time)
		{
			Vector3 translate = Vector3.Zero;
			Vector3 rotate = Vector3.Zero;
			GetDirections(ref translate, ref rotate);

			if (rotate != Vector3.Zero)
				TargetOrientation *= Quaternion.FromAxisAngle(rotate, _rotateScale);
			if (translate != Vector3.Zero)
				TargetPosition += translate;

			base.Update(time);
		}

		private void GetDirections(ref Vector3 translate, ref Vector3 rotate)
		{
			if (_keyboard[Key.ShiftLeft] || _keyboard[Key.ShiftRight] ||
				_keyboard[Key.AltLeft] || _keyboard[Key.AltRight] ||
				_keyboard[Key.ControlLeft] || _keyboard[Key.ControlRight])
			{
				if (_keyboard[Key.Up] || _keyboard[Key.W])
					rotate += Right * _rotateScale;
				if (_keyboard[Key.Down] || _keyboard[Key.S])
					rotate += -Right * _rotateScale;
				if (_keyboard[Key.Left] || _keyboard[Key.A])
					rotate += -Up * _rotateScale;
				if (_keyboard[Key.Right] || _keyboard[Key.D])
					rotate += Up * _rotateScale;
				if (_keyboard[Key.PageUp] || _keyboard[Key.R])
					rotate += Forward * _rotateScale;
				if (_keyboard[Key.PageDown] || _keyboard[Key.F])
					rotate += -Forward * _rotateScale;
			}
			else
			{
				if (_keyboard[Key.Up] || _keyboard[Key.W])
					translate += Forward * _translateScale;
				if (_keyboard[Key.Down] || _keyboard[Key.S])
					translate += -Forward * _translateScale;
				if (_keyboard[Key.Left] || _keyboard[Key.A])
					translate += Right * _translateScale;
				if (_keyboard[Key.Right] || _keyboard[Key.D])
					translate += -Right * _translateScale;
				if (_keyboard[Key.PageUp] || _keyboard[Key.R])
					translate += -Up * _translateScale;
				if (_keyboard[Key.PageDown] || _keyboard[Key.F])
					translate += Up * _translateScale;
			}

			if (_keyboard[Key.Q])
				rotate += -Up * _rotateScale;
			if (_keyboard[Key.E])
				rotate += Up * _rotateScale;

			if (_mouse[MouseButton.Right] && _mouse[MouseButton.Left])
				translate += Forward * _translateScale;
		}

		private void OnMouseButtonDown(object s, MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
				_mouseLeftDrag = true;
			if (e.Button == MouseButton.Right)
				_mouseRightDrag = true;

			if (e.Button == MouseButton.Middle)
				Reset();
		}

		private void OnMouseButtonUp(object s, MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
				_mouseLeftDrag = false;
			if (e.Button == MouseButton.Right)
				_mouseRightDrag = false;
		}

		private void OnMouseMove(object s, MouseMoveEventArgs e)
		{
			if (_mouseLeftDrag || _mouseRightDrag)
				TargetOrientation *= Quaternion.FromAxisAngle(Right, e.YDelta * _rotateScale) * Quaternion.FromAxisAngle(Up, e.XDelta * _rotateScale);
		}
	}
}
