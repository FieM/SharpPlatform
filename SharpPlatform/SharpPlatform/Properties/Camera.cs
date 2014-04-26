using System;
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
	public class Camera
	{
		public Matrix transform;
		Viewport view;
		Vector2 centre;

		public Camera(Viewport newView)
		{
			view = newView;
		}

		public void Update(GameTime gameTime, Game1 game)
		{
			centre = new Vector2 (game.Hero.X + (game.Hero.Size.Width / 2) - 400, 0);
			transform = Matrix.CreateScale(new Vector3(1,1,0)) * 
				Matrix.CreateTranslation(new Vector3(-centre.X,-centre.Y,0));

		}
	}
}
