using System;

namespace SharpPlatform
{
	public class Hero
	{
		public Hero ()
		{
			//Player Movement
			if (keystate.IsKeyDown (Keys.Right)) {
				player.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Left)) {
				player.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Up)) {
				player.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Down)) {
				player.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
		}
	}
}

