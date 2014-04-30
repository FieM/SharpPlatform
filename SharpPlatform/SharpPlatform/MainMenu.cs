using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace SharpPlatform
{
	class MainMenu
	{
		enum GameState{mainMenu, guideLines, inGame}

		GameState gameState;

		List<GUIElement> main = new List<GUIElement>();
		List<GUIElement> guidelines = new List<GUIElement>();

		public MainMenu()
		{
			main.Add (new GUIElement ("Menu"));
			main.Add (new GUIElement ("Play"));
			main.Add (new GUIElement ("ControlsBtn"));

			guidelines.Add (new GUIElement ("Controls"));
			guidelines.Add (new GUIElement ("Back"));
		}

		public void LoadContent(ContentManager content)
		{
			foreach (GUIElement element in main) {
				element.LoadContent (content);
				element.CenterElement (500, 800);
				element.clickEvent += OnClick;
			}
			main.Find (x => x.AssetName == "Play").MoveElement (170, 0);
			main.Find (x => x.AssetName == "ControlsBtn").MoveElement (250, 90);
			main.Find (x => x.AssetName == "Menu").MoveElement (71, 0);

			foreach (GUIElement element in guidelines) 
			{
				element.LoadContent (content);
				element.CenterElement (500, 800);
				element.clickEvent += OnClick;
			}
			guidelines.Find (x => x.AssetName == "Back").MoveElement (250, 90);
			guidelines.Find (x => x.AssetName == "Controls").MoveElement (71, 0);


		}

		public void Update()
		{
			switch (gameState) 
			{
			case GameState.mainMenu:
				foreach (GUIElement element in main) 
				{
					element.Update ();
				}
				break;

			case GameState.guideLines:
				foreach (GUIElement element in guidelines)
				{
					element.Update ();
				}
				break;

			case GameState.inGame:
				break;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{

			switch (gameState) 
			{
			case GameState.mainMenu:
				foreach (GUIElement element in main) 
				{
					element.Draw (spriteBatch);
				}
				break;

			case GameState.guideLines:
				foreach (GUIElement element in guidelines) 
				{
					element.Draw (spriteBatch);
				}
				break;

			case GameState.inGame:
				break;
			}


		}

		public void OnClick(string button)
		{
			if (button == "Play") 
			{
				//play the game
				gameState = GameState.inGame;
			}
			if (button == "ControlsBtn") 
			{
				gameState = GameState.guideLines;
			}
			if (button == "Back") 
			{
				gameState = GameState.mainMenu;
			}
		}
	}
}

