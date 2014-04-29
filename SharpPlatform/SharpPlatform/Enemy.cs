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
		public int PosLeft {
			get;
			set;
		}

		public int PosRight {
			get;
			set;
		}

		public bool MoveLeft {
			get;
			set;
		}

		public Enemy(Rectangle size, Texture2D sprite, int left, int right)
		{
			PosLeft = left;
			PosRight = right;
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

