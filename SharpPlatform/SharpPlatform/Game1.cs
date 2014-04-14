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
		private Texture2D sprite;
		private Rectangle chara;
		int gravity = 0;

		bool jumping;
		int startY, jumpspeed = 0;

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

		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);
<<<<<<< HEAD

<<<<<<< HEAD
<<<<<<< HEAD
			//TODO: use this.Content to load your game content here 
			sprite = Content.Load<Texture2D> ("vehiconava");
			chara = new Rectangle (0, 0, 100, 100);
			startY = chara.Y;
			jumping = false;
			jumpspeed = 0;
=======
=======
>>>>>>> origin/Fie
			//TODO: use this.Content to load your game content here
=======
			//TODO: use this.Content to load your game content here 
			playerSprite = Content.Load<Texture2D> ("hero");
			enemySprite = Content.Load<Texture2D> ("hero");
			groundSprite = Content.Load<Texture2D> ("ground");
			
			backgroundTexture = Content.Load<Texture2D> ("Background");
			backgroundPosition = new Vector2 (-400, 0);

>>>>>>> parent of 8cbd8af... Creating Classes

>>>>>>> 8cbd8afa785ac1a46e5e7d73ca157c453035153f
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
<<<<<<< HEAD
			KeyboardState KeyBS = Keyboard.GetState ();
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keystate.IsKeyDown(Keys.Escape) || keystate.IsKeyDown (Keys.Back)) {
				Exit ();
			}
<<<<<<< HEAD
=======
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
>>>>>>> parent of 8cbd8af... Creating Classes

			if (KeyBS.IsKeyDown (Keys.Right))
				chara.X += 5;
			if (KeyBS.IsKeyDown (Keys.Left))
				chara.X -= 5;
			if (KeyBS.IsKeyDown (Keys.Down))
				chara.Y += 5;
			if (KeyBS.IsKeyDown (Keys.Up))
				chara.Y -= 5;

			chara.Y += gravity;
			gravity += 1;
			if (gravity > 2)
				gravity = 2;

			if (jumping) {
				chara.Y += jumpspeed;
				jumpspeed += 1;
				if (chara.Y >= startY) {
					chara.Y = startY;
					jumping = false;
				}
			} 
			else {
				if (KeyBS.IsKeyDown (Keys.Space)) {
					jumping = true;
					jumpspeed = -14;
				}
			}


=======
>>>>>>> origin/Fie
			// TODO: Add your update logic here			

			base.Update (gameTime);
		}
		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			//TODO: Add your drawing code here
<<<<<<< HEAD
<<<<<<< HEAD
            
			spriteBatch.Begin ();
			spriteBatch.Draw (sprite, chara, Color.White);
=======
			spriteBatch.Begin (SpriteSortMode.Deferred, 
				BlendState.AlphaBlend, 
				null, null, null, null,
				camera.transform);
			spriteBatch.Draw(backgroundTexture, backgroundPosition, Color.White);
			spriteBatch.Draw (playerSprite, player, playerColor);
			spriteBatch.Draw (enemySprite, enemy, enemyColor);
			spriteBatch.Draw (groundSprite, ground, Color.White);
>>>>>>> 8cbd8afa785ac1a46e5e7d73ca157c453035153f
			spriteBatch.End ();

=======
	
>>>>>>> origin/Fie
			base.Draw (gameTime);
		}
	}
}

