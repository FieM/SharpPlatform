#region Using Statements
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

#endregion
namespace SharpPlatform
{
	public class Game1 : Game
	{
		Camera camera;
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont gameFont;
		// Background
		Texture2D backgroundTexture;
		Vector2 backgroundPosition;
		Random random = new Random ();
		List<GameObject> gameObjects = new List<GameObject>();

		Hero hero;
		float moveSpeed = 500f;
		int gravity = 0;
		bool jumping = false;
		bool touchingGround = false;
		//	float gravity = 0.1f;
		//float startY, jumpspeed = 0;
		int startY, jumpspeed = 0;
		DateTime lastAttack = DateTime.MinValue;

		Rectangle[] groundSizes = new[] { new Rectangle (-50, 300, 300, 50) };
		Point[] enemyPositions = new[] { new Point (100, 100) };
		Point[] coinPositions = new[] { new Point (100, 200) };

		public Hero Hero
		{
			get { return hero; }
		}

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
			//playerRec = new Rectangle ((int)player.X, (int)player.Y, playerSprite.Width, playerSprite.Height);
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
			foreach (var size in groundSizes)
				AddGround (size);

			foreach (var position in enemyPositions)
				AddEnemy (position);

			foreach (var position in coinPositions)
				AddCoin (position);

			hero = new Hero (new Rectangle(0, 0, 50, 50), Content.Load<Texture2D> ("hero")); //Using initializer to set property
			hero.Died += (sender, e) => GameOver();

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
			var keystate = Keyboard.GetState ();
			startY = hero.Y;
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keystate.IsKeyDown (Keys.Escape) || keystate.IsKeyDown (Keys.Back)) {
				Exit ();
			}
			// TODO: Add your update logic here
			hero.Color = Color.White;

			//Player Movement
			/*if (keystate.IsKeyDown (Keys.Right)) {
				player.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Left)) {
				player.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Up)) {
				player.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.Down)) {
				player.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}*/
			if (keystate.IsKeyDown (Keys.Right))
				hero.X += 5;
			if (keystate.IsKeyDown (Keys.Left))
				hero.X -= 5;
			if (keystate.IsKeyDown (Keys.Down))
				hero.Y += 5;
			if (keystate.IsKeyDown (Keys.Up))
				hero.Y -= 5;

			//Adjusting playerRec, so that it follows player
			//playerRec.X = (int)player.X;
			//playerRec.Y = (int)player.Y;

			/*player.Y += gravity;
			gravity += 0.5f;
			if (gravity > 2f)
				gravity = 2f;
			*/
			hero.Y += gravity;
			gravity += 1;
			if (gravity > 2)
				gravity = 2;

			//Enemy movement
			/*if (keystate.IsKeyDown (Keys.D)) {
				enemy.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.A)) {
				enemy.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.W)) {
				enemy.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			} else if (keystate.IsKeyDown (Keys.S)) {
				enemy.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}*/

			if (jumping)
			{
				hero.Y += jumpspeed;
				jumpspeed += 1;
				//if (player.Y > player.Y + 5) {
				if (hero.Y >= startY)
				{
					hero.Y = startY;
					jumping = false;
					touchingGround = false;
				}
			} 
			else
			{
				if (keystate.IsKeyDown (Keys.Space) && touchingGround)
				{
					jumping = true;
					jumpspeed = -14;
				}
			}

			//Collision
			// Use LINQ to only select the game objects of Enemy type.
			foreach (var gameObject in gameObjects.ToArray())
			{
				if (gameObject is Ground)
				{
					var ground = (Ground)gameObject;
					if (hero.Intersects (ground))
					{
						gravity = 0;
						touchingGround = true;
						hero.Y = ground.Y - hero.Size.Height;
					}
				}
				else if (gameObject is Enemy)
				{
					var enemy = (Enemy)gameObject;
					enemy.Color = Color.White;
					if (hero.Intersects(enemy))
					{
						hero.Defend (enemy);
						hero.Color = Color.Red;
					}

					if (keystate.IsKeyDown (Keys.LeftControl) && 
						(DateTime.Now - lastAttack).TotalMilliseconds >= 1000 && 
						hero.AttackIntersects (enemy))
					{
						lastAttack = DateTime.Now;
						enemy.Defend (hero);
						enemy.Color = Color.Red;
					}
				}
				else if (gameObject is IItem && hero.Intersects(gameObject))
				{
					if (gameObject is Coin)
					{
						var coin = (Coin)gameObject;
						hero.Money += coin.Value;
					}
					else if (gameObject is IInventoryItem)
					{
						var inventoryItem = (IInventoryItem)gameObject;
						hero.Inventory.Add (inventoryItem);
					}

					gameObjects.Remove (gameObject);
				}
			}

			camera.Update (gameTime, this);
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
			spriteBatch.Begin (SpriteSortMode.Deferred, 
				BlendState.AlphaBlend, 
				null, null, null, null,
				camera.transform);
			spriteBatch.Draw(backgroundTexture, backgroundPosition, Color.White);
			spriteBatch.Draw (hero.Sprite, hero.Size, hero.Color);
			foreach (var gameObject in gameObjects)
				spriteBatch.Draw (gameObject.Sprite, gameObject.Size, gameObject.Color);
			//spriteBatch.DrawString (gameFont, "Hello", new Vector2 (10, 10), Color.ForestGreen);

			spriteBatch.End ();

			base.Draw (gameTime);
		}

		public void GameOver()
		{
			hero.X = 0;
			hero.Y = 0;
		}

		public void AddGround(Rectangle size)
		{
			var ground = new Ground (size, Content.Load<Texture2D> ("ground"));

			gameObjects.Add (ground);
		}

		public void AddEnemy(Point position)
		{
			var enemy = new Enemy(new Rectangle (position.X, position.Y, 50, 50), Content.Load<Texture2D> ("hero"));

			enemy.Died += (sender, e) =>
			{
				var enemyDied = sender as Enemy;
				if (enemyDied == null)
					return;

				EnemyDrop (enemyDied);
				gameObjects.Remove (enemyDied);
			};

			gameObjects.Add (enemy);
		}

		public void AddCoin(Point position)
		{
			string spriteType = null;
			int value = 0;
			switch (random.Next(0, 2))
			{
				case 0:
					spriteType = "CopperCoin";
					value = 1;
					break;
				case 1:
					spriteType = "SilverCoin";
					value = 3;
					break;
				case 2:
					spriteType = "GoldCoin";
					value = 5;
					break;
				default:
					throw new Exception ("The type of coin was not specified");
			}

			var coin = new Coin (value, new Rectangle ((int)position.X, (int)position.Y, 30, 30), Content.Load<Texture2D> (spriteType));

			gameObjects.Add (coin);
		}

		public void AddItem(int min, int max, Point position)
		{
			int ItemRate = random.Next(min, max);
			if (ItemRate < 10)
				AddEquipment (position);
			else if (ItemRate <= 30)
				AddCoin (position);
		}

		public IEquipment AddEquipment(Point position)
		{
			//TODO: CreateItem sword and shield, must be child of Item class
			return null;
		}

		public void EnemyDrop(Enemy enemy)
		{
			AddItem (10, 30, new Point(enemy.X, enemy.Y));
		}
	}
}

