using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace RageWorld
{
	public abstract class Frame
	{
		public Frame()
		{
			Reset();
		}
		
		public void Reset()
		{
			_orientation = Quaternion.Identity;
			_targetOrientation = Quaternion.Identity;
			_position = Vector3.Zero;
			_targetPosition = Vector3.Zero;
			_scale = Vector3.One;
			_targetScale = Vector3.One;
		}

		public virtual void Resize(Rectangle size)
		{
			_windowSize = size;

			for (int i = 0; i < _children.Count; i++)
				_children[i].Resize(size);
		}

		public virtual void Render(double time)
		{
			Matrix4 rotate = Matrix4.Rotate(_orientation);
			Matrix4 translate = Matrix4.CreateTranslation(_position);
			Matrix4 scale = Matrix4.Scale(_scale);
			Matrix4 m = scale * translate * rotate;
			GL.MultMatrix(ref m);

			for (int i = 0; i < _children.Count; i++)
				_children[i].Render(time);
		}

		public virtual void Update(double time)
		{
			//TODO prevent roll for first person camera
			if (_orientation != _targetOrientation)
				_orientation = Quaternion.Slerp(_orientation, _targetOrientation, (float)time);

			Vector3 delta = _position - _targetPosition;
			if ((delta.X > float.Epsilon) || (delta.Y > float.Epsilon) || (delta.Z > float.Epsilon))
				_position = Vector3.Lerp(_position, _targetPosition, (float)time);

			delta = _scale - _targetScale;
			if ((delta.X > float.Epsilon) || (delta.Y > float.Epsilon) || (delta.Z > float.Epsilon))
				_scale = Vector3.Lerp(_scale, _targetScale, (float)time);

			for (int i = 0; i < _children.Count; i++)
				_children[i].Update(time);
		}

		private List<Frame> _children = new List<Frame>();
		public List<Frame> Children { get { return _children; } }

		private Rectangle _windowSize = Rectangle.Empty;
		public Rectangle WindowSize { get { return _windowSize; } }

		public Vector3 Forward { get { return _orientation.Apply(Vector3.UnitZ); } }
		public Vector3 Right { get { return _orientation.Apply(Vector3.UnitX); } }
		public Vector3 Up { get { return _orientation.Apply(Vector3.UnitY); } }

		public Vector3 TargetForward { get { return _targetOrientation.Apply(Vector3.UnitZ); } }
		public Vector3 TargetRight { get { return _targetOrientation.Apply(Vector3.UnitX); } }
		public Vector3 TargetUp { get { return _targetOrientation.Apply(Vector3.UnitY); } }

		private Quaternion _orientation;
		public Quaternion Orientation
		{
			get { return _orientation; }
			set
			{
				_orientation = value;
				_targetOrientation = value;
			}
		}

		private Quaternion _targetOrientation;
		public Quaternion TargetOrientation
		{
			get { return _targetOrientation; }
			set { _targetOrientation = value; }
		}

		private Vector3 _position;
		public Vector3 Position
		{
			get { return _position; }
			set
			{
				_position = value;
				_targetPosition = value;
			}
		}

		private Vector3 _targetPosition;
		public Vector3 TargetPosition
		{
			get { return _targetPosition; }
			set { _targetPosition = value; }
		}

		private Vector3 _scale;
		public Vector3 Scale
		{
			get { return _scale; }
			set
			{
				_scale = value;
				_targetScale = value;
			}
		}

		private Vector3 _targetScale;
		public Vector3 TargetScale
		{
			get { return _targetScale; }
			set { _targetScale = value; }
		}
	}
}
