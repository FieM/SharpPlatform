using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace SharpPlatform
{
	class GUIElement
	{
		private Texture2D GUITexture;

		private Rectangle GUIRect;

		private string assetName;

		public string AssetName
		{
			get { return assetName; }
			set { assetName = value; }
		}

		public delegate void ElementClicked(string element);

		public event ElementClicked clickEvent;

		public GUIElement (string assetName)
		{
			this.assetName = assetName;
		}

		public void LoadContent(ContentManager content)
		{
			GUITexture = content.Load<Texture2D> (assetName);
			GUIRect = new Rectangle (0, 0, GUITexture.Width, GUITexture.Height);
		}

		public void Update()
		{
			if(GUIRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				clickEvent (assetName);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (GUITexture, GUIRect, Color.White);
		}

		public void CenterElement(int height, int width)
		{
			GUIRect = new Rectangle (
				(width / 2) - (this.GUITexture.Width / 2),
				(height / 2) - (this.GUITexture.Height / 2), 
				this.GUITexture.Width, 
				this.GUITexture.Height
			);
		}
		public void MoveElement(int x, int y)
		{
			GUIRect = new Rectangle (
				GUIRect.X += x,
				GUIRect.Y += y,
				GUIRect.Width,
				GUIRect.Height
			);
		}
	}
}

