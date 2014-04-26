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
	public class Enemy : Character
	{
		public Enemy(Rectangle size, Texture2D sprite)
		{
			Size = size;
			Sprite = sprite;
			Color = Color.White;
		}

		public override void Defend(AttackObject attacker)
		{
			Defend(0, attacker.Attack());
		}
	}
}

