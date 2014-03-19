#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

#endregion
namespace SharpPlatform
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Texture2D player, enemy;
		Vector2 playerPos, enemyPos = new Vector2(100,100);
		KeyboardState keystate;
		Color playerColor = Color.White;
		Color enemyColor = Color.Black;
		float moveSpeed = 500f;
		Rectangle playerRec, enemyRec;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = false;		
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			base.Initialize (); // Calls LoadContent, and therefore gets the width and height of enemy and player
			enemyRec = new Rectangle ((int)enemyPos.X, (int)enemyPos.Y, enemy.Width, enemy.Height);
			playerRec = new Rectangle ((int)playerPos.X, (int)playerPos.Y, player.Width, player.Height);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//TODO: use this.Content to load your game content here 
			player = Content.Load<Texture2D> ("hero");
			enemy = Content.Load<Texture2D> ("hero");
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keystate.IsKeyDown(Keys.Escape) || keystate.IsKeyDown (Keys.Back)) {
				Exit ();
			}
			// TODO: Add your update logic here			
			keystate = Keyboard.GetState ();

			//Player Movement
			if (keystate.IsKeyDown (Keys.Right)) {
				playerPos.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Left)) {
				playerPos.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Up)) {
				playerPos.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Down)) {
				playerPos.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			//Adjusting playerRec, so that it follows player
			playerRec.X = (int)playerPos.X;
			playerRec.Y = (int)playerPos.Y;

			//Enemy movement
			if (keystate.IsKeyDown (Keys.D)) {
				enemyPos.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.A)) {
				enemyPos.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.W)) {
				enemyPos.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.S)) {
				enemyPos.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			//Adjust enemyRec, so that it follows enemy
			enemyRec.X = (int)enemyPos.X;
			enemyRec.Y = (int)enemyPos.Y;

			//Collision checking - NEW METHOD
			if (playerRec.Intersects(enemyRec)) {
				//collision
				playerColor = Color.Red;
			} else {
				//No collision
				playerColor = Color.White;
			}

					//collision ();

			base.Update (gameTime);
		}
		/*OLD COLLISION METHOD	
		private void  collision()
			{
		
			if (playerPos.X + player.Width < enemyPos.X || playerPos.X > enemy.Width + enemyPos.X ||
				playerPos.Y + player.Width < enemyPos.Y || playerPos.Y > enemy.Width + enemyPos.Y) {
				//No collision, checking that player is not in the position that the enemy is
				playerColor = Color.White;
			} else {
				//Collision between player and enemy
				playerColor = Color.Red;
			}
		}
		*/

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			//TODO: Add your drawing code here
			spriteBatch.Begin ();
			spriteBatch.Draw (player, playerPos, playerColor);
			spriteBatch.Draw (enemy, enemyPos, enemyColor);
			spriteBatch.End ();

			base.Draw (gameTime);
		}
	}
}

