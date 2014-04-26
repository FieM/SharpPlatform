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
	public class Coin : GameObject, IItem
	{
		public int Value {
			get;
			protected set;
		}
		public Coin(int value, Rectangle size, Texture2D sprite)
		{
			Value = value;
			Sprite = sprite;
			Size = size;
		}
	}
	public interface IItem
	{

	}
}

