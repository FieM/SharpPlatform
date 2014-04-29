using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SharpPlatform
{
	public class GameObject
	{
		public Color Color {
			get;
			set;
		}
		public Texture2D Sprite {
			get;
			set;
		}

		public Rectangle Size {
			get;
			protected set;
		}

		public Point Position {
			get { return new Point (Size.X, Size.Y); }
			set { Size = new Rectangle (value.X, value.Y, Size.Width, Size.Height); }
		}

		public int X {
			get { return Size.X; }
			set { Size = new Rectangle (value, Size.Y, Size.Width, Size.Height); }
		}

		public int Y {
			get { return Size.Y; }
			set { Size = new Rectangle (Size.X, value, Size.Width, Size.Height); }
		}

		public int Width {
			get { return Size.Width; }
			set { Size = new Rectangle (Size.X, Size.Y, value, Size.Height); }
		}

		public int Height {
			get { return Size.Height; }
			set { Size = new Rectangle (Size.X, Size.Y, Size.Width, value); }
		}

		public int Top {
			get { return Size.Y; }
			set { Size = new Rectangle(Size.X, value, Size.Width, Size.Height); }
		}

		public int Bottom {
			get { return Size.Y + Size.Height; }
			set { Size = new Rectangle(Size.X, value - Size.Height, Size.Width, Size.Height); }
		}

		public int Left {
			get { return Size.X; }
			set { Size = new Rectangle(value, Size.Y, Size.Width, Size.Height); }
		}

		public int Right {
			get { return Size.X + Size.Width; }
			set { Size = new Rectangle(value - Size.Width, Size.Y, Size.Width, Size.Height); }
		}

		public GameObject()
		{
			Color = Color.White;
		}
	}
	public class AttackObject : GameObject
	{
		public int AttackValue {
			get;
			set;
		}
		public virtual int Attack()
		{
			return AttackValue;
		}
	}
}

