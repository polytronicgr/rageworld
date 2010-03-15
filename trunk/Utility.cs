using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace RageWorld
{
	public sealed class Window : Frame
	{
		public Vector2 BottomLeft { get; set; }
		public Vector2 TopRight { get; set; }
		public Color Color { get; set; }

		public Window(Vector2 bottomLeft, Vector2 topRight)
		{
			BottomLeft = bottomLeft;
			TopRight = topRight;
			Color = Color.FromArgb(33, 33, 33, 33);
		}

		public override void Render(double time)
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();

			GL.Ortho(0.0f, 1.0f, 0.0f, 1.0f, -1.0f, 1.0f);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();

			Matrix4 m;
			GL.GetFloat(GetPName.ModelviewMatrix, out m);
			m.Invert();
			GL.MultMatrix(ref m);

			GL.Begin(BeginMode.Quads);

			GL.Color4(Color);
			GL.Vertex3(BottomLeft.X, BottomLeft.Y, 0.99f);
			GL.Vertex3(TopRight.X, BottomLeft.Y, 0.99f);
			GL.Vertex3(TopRight.X, TopRight.Y, 0.99f);
			GL.Vertex3(BottomLeft.X, TopRight.Y, 0.99f);

			float x = WindowSize.Width * 0.00001f;
			float y = WindowSize.Height * 0.00001f;

			GL.Vertex3(BottomLeft.X + y, BottomLeft.Y + x, 0.999f);
			GL.Vertex3(TopRight.X - y, BottomLeft.Y + x, 0.999f);
			GL.Vertex3(TopRight.X - y, TopRight.Y - x, 0.999f);
			GL.Vertex3(BottomLeft.X + y, TopRight.Y - x, 0.999f);

			GL.End();

			GL.PopMatrix();

			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
			GL.MatrixMode(MatrixMode.Modelview);
		}
	}

	public sealed class Grid : Frame
	{
		float _step;
		float _size;

		public Grid(float step = 1, float size = 10)
		{
			_step = step;
			_size = size;
		}

		public override void Render(double time)
		{
			GL.PushMatrix();

			base.Render(time);

			GL.Begin(BeginMode.Lines);

			GL.Color4(0.33f, 0.33f, 0.33f, 0.33f);
			for (float i = _step; i <= _size; i += _step)
			{
				GL.Vertex3(-_size, 0, i);
				GL.Vertex3(_size, 0, i);
				GL.Vertex3(-_size, 0, -i);
				GL.Vertex3(_size, 0, -i);

				GL.Vertex3(i, 0, -_size);
				GL.Vertex3(i, 0, _size);
				GL.Vertex3(-i, 0, -_size);
				GL.Vertex3(-i, 0, _size);
			}

			GL.End();

			GL.PopMatrix();
		}
	}

	public sealed class Axis : Frame
	{
		public Axis()
		{
			GL.LineStipple(1, 0x00FF);
		}

		public override void Render(double time)
		{
			GL.PushMatrix();

			base.Render(time);

			GL.LineWidth(2.0f);
			GL.Enable(EnableCap.LineStipple);
			GL.Begin(BeginMode.Lines);

			GL.Color3(Color.Red);
			GL.Vertex3(-150.0f, 0.0f, 0.0f);
			GL.Vertex3(0.0f, 0.0f, 0.0f);
			GL.Color3(Color.Green);
			GL.Vertex3(0.0f, -150.0f, 0.0f);
			GL.Vertex3(0.0f, 0.0f, 0.0f);
			GL.Color3(Color.Blue);
			GL.Vertex3(0.0f, 0.0f, -150.0f);
			GL.Vertex3(0.0f, 0.0f, 0.0f);

			GL.End();
			GL.Disable(EnableCap.LineStipple);
			GL.Begin(BeginMode.Lines);

			GL.Color3(Color.Red);
			GL.Vertex3(0.0f, 0.0f, 0.0f);
			GL.Vertex3(150.0f, 0.0f, 0.0f);
			GL.Color3(Color.Green);
			GL.Vertex3(0.0f, 0.0f, 0.0f);
			GL.Vertex3(0.0f, 150.0f, 0.0f);
			GL.Color3(Color.Blue);
			GL.Vertex3(0.0f, 0.0f, 0.0f);
			GL.Vertex3(0.0f, 0.0f, 150.0f);

			GL.End();
			GL.LineWidth(1.0f);

			GL.PopMatrix();
		}
	}

	public sealed class Cube : Frame
	{
		private readonly Color _randomColor = Utility.GetRandomColor();

		public Cube(float x = 0.0f, float y = 0.0f, float z = 0.0f, float s = 1.0f)
		{
			Scale = new Vector3(s, s, s);
			Position = new Vector3(x, y, z);
		}

		public override void Render(double time)
		{
			GL.PushMatrix();

			base.Render(time);

			GL.Begin(BeginMode.Quads);

			// front
			GL.Color4(_randomColor);
			GL.Normal3(0.0f, 0.0f, 1.0f);
			GL.Vertex3(-1.0f, -1.0f, 1.0f);
			GL.Vertex3(1.0f, -1.0f, 1.0f);
			GL.Vertex3(1.0f, 1.0f, 1.0f);
			GL.Vertex3(-1.0f, 1.0f, 1.0f);

			// back
			GL.Color4(_randomColor);
			GL.Normal3(0.0f, 0.0f, -1.0f);
			GL.Vertex3(-1.0f, -1.0f, -1.0f);
			GL.Vertex3(-1.0f, 1.0f, -1.0f);
			GL.Vertex3(1.0f, 1.0f, -1.0f);
			GL.Vertex3(1.0f, -1.0f, -1.0f);

			// top
			GL.Color4(_randomColor);
			GL.Normal3(0.0f, 1.0f, 0.0f);
			GL.Vertex3(-1.0f, 1.0f, -1.0f);
			GL.Vertex3(-1.0f, 1.0f, 1.0f);
			GL.Vertex3(1.0f, 1.0f, 1.0f);
			GL.Vertex3(1.0f, 1.0f, -1.0f);

			// bottom
			GL.Color4(_randomColor);
			GL.Normal3(0.0f, -1.0f, 0.0f);
			GL.Vertex3(-1.0f, -1.0f, -1.0f);
			GL.Vertex3(1.0f, -1.0f, -1.0f);
			GL.Vertex3(1.0f, -1.0f, 1.0f);
			GL.Vertex3(-1.0f, -1.0f, 1.0f);

			// right
			GL.Color4(_randomColor);
			GL.Normal3(1.0f, 0.0f, 0.0f);
			GL.Vertex3(1.0f, -1.0f, -1.0f);
			GL.Vertex3(1.0f, 1.0f, -1.0f);
			GL.Vertex3(1.0f, 1.0f, 1.0f);
			GL.Vertex3(1.0f, -1.0f, 1.0f);

			// left
			GL.Color4(_randomColor);
			GL.Normal3(-1.0f, 0.0f, 0.0f);
			GL.Vertex3(-1.0f, -1.0f, -1.0f);
			GL.Vertex3(-1.0f, -1.0f, 1.0f);
			GL.Vertex3(-1.0f, 1.0f, 1.0f);
			GL.Vertex3(-1.0f, 1.0f, -1.0f);

			GL.End();

			GL.PopMatrix();
		}
	}

	public sealed class Triangle : Frame
	{
		public Triangle(float x = 0.0f, float y = 0.0f, float z = 0.0f, float s = 1.0f)
		{
			Position = new Vector3(x, y, z);
			Scale = new Vector3(s, s, s);
		}

		public override void Render(double time)
		{
			GL.PushMatrix();

			base.Render(time);

			GL.Begin(BeginMode.Triangles);

			GL.Color4(1.0f, 0.0f, 0.0f, 0.333f);
			GL.Vertex3(0.0f, 1.0f, 0.0f);
			GL.Color4(0.0f, 1.0f, 0.0f, 0.333f);
			GL.Vertex3(-1.0f, -1.0f, 0.0f);
			GL.Color4(0.0f, 0.0f, 1.0f, 0.333f);
			GL.Vertex3(1.0f, -1.0f, 0.0f);

			GL.End();

			GL.PopMatrix();
		}

		static float angle = 0.0f;

		public override void Update(double time)
		{
			base.Update(time);

			angle += 0.01f;
			if (angle > 360)
				angle = 0.0f;

			Orientation = Quaternion.FromAxisAngle(Up, angle);
		}
	}

	public static class Utility
	{
		private static Random _random = new Random();

		public static double GetRandomNumber(double minimum, double maximum)
		{
			return (maximum - minimum) * _random.NextDouble() + minimum;
		}

		public static Color GetRandomColor()
		{
			int color1 = _random.Next(55, 200);
			int color2 = _random.Next(55, 200);
			int color3 = _random.Next(55, 200);
			int color4 = _random.Next(55, 200);
			return Color.FromArgb(color1, color2, color3, color4);
		}
	}

	public static class MathExtensions
	{
		public static Vector3 Apply(this Quaternion quat, Vector3 vector)
		{
			Quaternion v = new Quaternion() { X = vector.X, Y = vector.Y, Z = vector.Z, W = 0 };
			Quaternion i = Quaternion.Invert(quat);
			Quaternion t = i * v;
			v = t * quat;

			return new Vector3(v.X, v.Y, v.Z);
		}
	
		public static Quaternion CreateRotationX(float angle)
		{
			return new Quaternion()
			{
				X = (float)Math.Cos(angle / 2),
				Y = (float)Math.Sin(angle / 2),
				Z = 0,
				W = 0
			};
		}

		public static Quaternion CreateRotationY(float angle)
		{
			return new Quaternion()
			{
				X = (float)Math.Cos(angle / 2),
				Y = 0,
				Z = (float)Math.Sin(angle / 2),
				W = 0
			};
		}

		public static Quaternion CreateRotationZ(float angle)
		{
			return new Quaternion()
			{
				X = (float)Math.Cos(angle / 2),
				Y = 0,
				Z = 0,
				W = (float)Math.Sin(angle / 2)
			};
		}
	}
}
