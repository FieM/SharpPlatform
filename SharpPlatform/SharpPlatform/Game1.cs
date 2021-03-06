﻿#region Using Statements
using System;
using System.IO;
using System.Text;
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
		const int SECTION_WIDTH = 2000;
		const int MAX_GRAVITY = 10;

		Camera camera; // Accessing the Camera class
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		MainMenu main = new MainMenu();

		// Background
		Texture2D backgroundTexture;
		Vector2 backgroundPosition;
		Random random = new Random ();
		//Making a list of all my list of gameObjects. This will be the entire map of the game, Sections will contain a list of gameObjects, which is the content of the level(enemies, grounds etc.)
		List<List<GameObject>>Sections = new List<List<GameObject>>();
		Dictionary<string, Texture2D> sprites = new Dictionary<string, Texture2D> ();

		Hero hero; // Accessing the Hero class
		int gravity = 0; // Variables for the gravity & antigravity
		bool jumping = false;

		//bool hasDevice = false; // Used for inventory.
		bool deviceActivated = false;

		DateTime lastAttack = DateTime.MinValue;

		//Screen size is 800px wide
		string directoryPath;
		int activeSection = 0;

		public Hero Hero
		{
			get { return hero; } // Calling the hero class.
		}

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = false;	// Making sure that the game does not open in fullscreen.
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 500;
			directoryPath = Path.Combine (Environment.CurrentDirectory, "Content");
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

			sprites.Add ("ground", Content.Load<Texture2D> ("groundTwo"));
			sprites.Add ("enemy", Content.Load<Texture2D> ("enemy"));
			sprites.Add ("goldcoin", Content.Load<Texture2D> ("goldcoinTwo"));
			sprites.Add ("silvercoin", Content.Load<Texture2D> ("silvercoinTwo"));
			sprites.Add ("coppercoin", Content.Load<Texture2D> ("coppercoinTwo"));

			Sections = LoadSections (directoryPath);

			hero = new Hero (new Rectangle(450, 50, 42, 56), Content.Load<Texture2D> ("hero")); //Using initializer to set property
			hero.Died += (sender, e) => GameOver(); // Endgame statement that allows for the player to have an endgame.

			backgroundTexture = Content.Load<Texture2D> ("BackgroundTwo"); // Loads the background picture.
			backgroundPosition = new Vector2 (-400, 0); // Sets the position of the background position.

			main.LoadContent (Content);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			var keystate = Keyboard.GetState ();

			bool touchingGround = false; // Variables to work with the jumping method
			activeSection = hero.X / SECTION_WIDTH;

			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keystate.IsKeyDown (Keys.Escape) || keystate.IsKeyDown (Keys.Back)) {
				Exit ();
			}

			hero.Color = Color.White;

			//GRAVITY AND ANTI-GRAVITY
			if (deviceActivated)
				hero.Y -= gravity;
			else
				hero.Y += gravity;

			gravity += 1;
			if (gravity > MAX_GRAVITY)
				gravity = MAX_GRAVITY;

			//Collision
			// Use LINQ to only select the game objects of Enemy type.
			for (int i = Math.Max (activeSection - 1, 0); i < Math.Min (activeSection + 2, Sections.Count); i++) {
				hero.Left -= SECTION_WIDTH * i;
				foreach (var gameObject in Sections[i].ToArray()) {
					if (gameObject is Ground) {
						var ground = (Ground)gameObject;
						if (hero.Intersects (ground)) {
							var intersect = Rectangle.Intersect (hero.Size, ground.Size);
							if (intersect.Width >= intersect.Height) {
								if (intersect.Top == hero.Top) {
									hero.Top = ground.Bottom;
									if (deviceActivated) {
										touchingGround = true;
									}
								} else {
									hero.Bottom = ground.Top;
									if (!deviceActivated) {
										touchingGround = true;
									}
								}
								gravity = 0;
								jumping = false;
							} else {
								if (intersect.Left == hero.Left)
									hero.Left = ground.Right;
								else
									hero.Right = ground.Left;
							}

						}
					} else if (gameObject is Enemy) {
						var enemy = (Enemy)gameObject;
						enemy.Color = Color.White;
						if (hero.Intersects (enemy)) {
							hero.Left += SECTION_WIDTH * i;
							hero.Defend (enemy);
							hero.Color = Color.Red; // Collision with the enemy, i.e sets the player's color to red if he collides with the enemy.
							hero.Left -= SECTION_WIDTH * i;
						}

						if (keystate.IsKeyDown (Keys.LeftControl) &&
						   (DateTime.Now - lastAttack).TotalMilliseconds >= 1000 &&
						   hero.AttackIntersects (enemy)) {
							lastAttack = DateTime.Now;
							enemy.Defend (hero);
							enemy.Color = Color.Red;
							if (enemy.Health <= 0) {
								EnemyDrop (enemy);
								Sections [activeSection].Remove (enemy);
							}
						}

						if (enemy.Right >= enemy.PosRight)
							enemy.MoveLeft = true;
						else if (enemy.Left <= enemy.PosLeft)
							enemy.MoveLeft = false;

						if (enemy.MoveLeft)
							enemy.X -= 2;
						else
							enemy.X += 2;

					} else if (gameObject is IItem && hero.Intersects (gameObject)) {
						if (gameObject is Coin) {
							var coin = (Coin)gameObject;
							hero.Money += coin.Value;
						} else if (gameObject is IInventoryItem) {
							var inventoryItem = (IInventoryItem)gameObject;
							hero.Inventory.Add (inventoryItem);
						}

						Sections [activeSection].Remove (gameObject);
					}
				}
				hero.Left += SECTION_WIDTH * i;
			}

			// Inverts device activated, whenever hero is touching ground, compared to which direction he is facing
			if (keystate.IsKeyDown (Keys.E) && touchingGround)
				deviceActivated = !deviceActivated;

			//Player Movement
			if (keystate.IsKeyDown (Keys.Right))
				hero.X += 5;
			if (keystate.IsKeyDown (Keys.Left))
				hero.X -= 5;

			//Saving map to files
			if (keystate.IsKeyDown (Keys.P))
				SaveSections (Sections, directoryPath);

			if (jumping)
			{
				if (gravity > MAX_GRAVITY)
					jumping = false;
			}
			else if (keystate.IsKeyDown(Keys.Space) && touchingGround) {
				jumping = true;
				gravity = -16;
			}

			if (hero.Bottom < -50 || hero.Top > 500)
				hero.Defend (new AttackObject{ AttackValue = Int32.MaxValue });

			main.Update ();
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
			spriteBatch.Draw (backgroundTexture, backgroundPosition, Color.White);
			spriteBatch.Draw (hero.Sprite, hero.Size, hero.Color);

			//TODO: lav foreach på alle gameobjects i section, på det index nummer i secions man er på... og på det forrige og efterfølgende
			for (int i = Math.Max(activeSection - 1, 0); i < Math.Min(activeSection + 2, Sections.Count); i++) {
				foreach (var gameObject in Sections[i])
					spriteBatch.Draw (gameObject.Sprite, new Rectangle(gameObject.X + SECTION_WIDTH * i, gameObject.Y, gameObject.Width, gameObject.Height), gameObject.Color);
			}
			main.Draw (spriteBatch);
			//spriteBatch.DrawString (gameFont, "Hello", new Vector2 (10, 10), Color.ForestGreen);
			spriteBatch.End ();

			base.Draw (gameTime);
		}

		public void GameOver()
		{
			gravity = 0;
			hero.X = 450;
			hero.Y = 50;
			hero.Health = 5;
			deviceActivated = false;
		}

		// Adds the coins to the game and allows for the player to pick up, using a switch statement.
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

			var coin = new Coin (value, new Rectangle ((int)position.X, (int)position.Y, 20, 20), Content.Load<Texture2D> (spriteType));

			Sections [activeSection].Add (coin);
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

		public void SaveSection(List<GameObject> section, String filePath)
		{
			//Using automatically dispose of the variables after the codeblock that follows
			using ( var stream = File.Open (filePath, FileMode.CreateNew, FileAccess.Write ) )
			using ( var writer = new StreamWriter ( stream ) )
			{
				var builder = new StringBuilder ();
				foreach (var gameObject in section)
				{
					builder.Clear ();
					builder.AppendFormat ("{0},{1},{2},{3},{4},{5}", gameObject.Color.ToHex(), gameObject.Sprite.Name, gameObject.X, gameObject.Y, gameObject.Size.Width, gameObject.Size.Height);
					if (gameObject is AttackObject)
					{
						var attackObject = (AttackObject)gameObject;
						builder.AppendFormat ("{0}", attackObject.AttackValue);
					}
					if (gameObject is Ground)
					{
						var ground = (Ground)gameObject;
						builder.Insert (0, "Ground,");
					}
					if (gameObject is Coin)
					{
						var coin = (Coin)gameObject;
						builder.Insert (0, "Coin,");
						builder.AppendFormat ("{0}", coin.Value);
					}
					if (gameObject is Character)
					{
						var character = (Character)gameObject;
						builder.AppendFormat ("{0}", character.Health);
					}
					if (gameObject is Enemy)
					{
						var enemy = (Enemy)gameObject;
						builder.Insert (0, "Enemy,");
					}
					writer.WriteLine (builder.ToString ());
				}
			}
		}

		public void SaveSections(List<List<GameObject>> sections, String directoryPath)
		{
			for (int i = 0; i < sections.Count; i++)
				SaveSection (sections [i], Path.Combine(directoryPath, i + ".sec"));
		}

		public List<GameObject> LoadSection(String filePath)
		{
			var section = new List<GameObject> ();
			//Using automatically dispose of the variables after the codeblock that follows
			using ( var stream = File.Open (filePath, FileMode.Open, FileAccess.Read ) )
			using ( var reader = new StreamReader ( stream ) )
			{
				var line = "";
				while (!String.IsNullOrWhiteSpace (line = reader.ReadLine ()))
				{
					GameObject gameObject;
					var index = 1;
					var values = line.Split (',');
					var color = values [index++].ToColor ();
					var sprite = sprites[ values [index++] ];
					var size = new Rectangle (int.Parse (values [index++]), int.Parse (values [index++]), int.Parse (values [index++]), int.Parse (values [index++]));
					switch (values[0].ToLower())
					{
						case "ground":
						gameObject = new Ground (size, sprite) { Color = color, AttackValue = int.Parse (values [index++]) };
							break;
						case "coin":
							gameObject = new Coin (int.Parse (values [index++]), size, sprite){ Color = color };
							break;
						case "enemy":
						gameObject = new Enemy (size, sprite, int.Parse(values [index++]), int.Parse(values [index++])){ Color = color, AttackValue = int.Parse (values [index++]) };
							break;
						default:
							continue;
					}

					section.Add (gameObject);
				}
			}

			return section;
		}

		public List<List<GameObject>> LoadSections(String directoryPath)
		{
			var sections = new List<List<GameObject>> ();
			foreach (var filePath in Directory.GetFiles(directoryPath, "*.sec"))
				sections.Add(LoadSection(filePath));

			return sections;
		}
	}
}

