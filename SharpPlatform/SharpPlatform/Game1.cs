﻿#region Using Statements
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

		Texture2D playerSprite, enemySprite, groundSprite;
		public Vector2 player, enemy = new Vector2(100,100);
		KeyboardState keystate;
		Color playerColor = Color.White;
		Color enemyColor = Color.Black;
		float moveSpeed = 500f;
		public Rectangle playerRec, enemyRec, ground;
		float gravity = 0.1f;
		bool patrick = false;

		bool andreas = true;

		bool jumping;
		float startY, jumpspeed = 0.0f;

		Camera camera;

		// Background
		Texture2D backgroundTexture;
		Vector2 backgroundPosition;

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
			camera = new Camera(GraphicsDevice.Viewport);
			// TODO: Add your initialization logic here
			base.Initialize (); // Calls LoadContent, and therefore gets the width and height of enemy and player
			enemyRec = new Rectangle ((int)enemy.X, (int)enemy.Y, enemySprite.Width, enemySprite.Height);
			playerRec = new Rectangle ((int)player.X, (int)player.Y, playerSprite.Width, playerSprite.Height);
			ground = new Rectangle (-50, 300, 300, 50);

			startY = player.Y;
			jumping = false;
			jumpspeed = 0;
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
			playerSprite = Content.Load<Texture2D> ("hero");
			enemySprite = Content.Load<Texture2D> ("hero");
			groundSprite = Content.Load<Texture2D> ("ground");

			backgroundTexture = Content.Load<Texture2D> ("Background");
			backgroundPosition = new Vector2 (-400, 0);


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
				player.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Left)) {
				player.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Up)) {
				player.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Down)) {
				player.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			//Adjusting playerRec, so that it follows player
			playerRec.X = (int)player.X;
			playerRec.Y = (int)player.Y;

			player.Y += gravity;
			gravity += 0.5f;
			if (gravity > 2f)
				gravity = 2f;

			if (playerRec.Intersects (ground))
				gravity = 0;
			else
				jumping = false;

			//Enemy movement
			if (keystate.IsKeyDown (Keys.D)) {
				enemy.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.A)) {
				enemy.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.W)) {
				enemy.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.S)) {
				enemy.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			//Adjust enemyRec, so that it follows enemy
			enemyRec.X = (int)enemy.X;
			enemyRec.Y = (int)enemy.Y;

			//Collision checking - NEW METHOD
			if (playerRec.Intersects(enemyRec)) {
				//collision
				playerColor = Color.Red;
			} else {
				//No collision
				playerColor = Color.White;
			}

			if (jumping) {
				player.Y += jumpspeed;
				jumpspeed += 1;
				if (player.Y > player.Y + 5) {
					jumping = false;
				}
			} 
			else {
				if (keystate.IsKeyDown (Keys.Space)) {
					jumping = true;
					jumpspeed = -50;
				}
			}

			//collision ();
			camera.Update (gameTime, this);
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
			spriteBatch.Begin (SpriteSortMode.Deferred, 
				BlendState.AlphaBlend, 
				null, null, null, null,
				camera.transform);
			spriteBatch.Draw(backgroundTexture, backgroundPosition, Color.White);
			spriteBatch.Draw (playerSprite, player, playerColor);
			spriteBatch.Draw (enemySprite, enemy, enemyColor);
			spriteBatch.Draw (groundSprite, ground, Color.White);
			spriteBatch.End ();

			base.Draw (gameTime);
		}
	}
}

