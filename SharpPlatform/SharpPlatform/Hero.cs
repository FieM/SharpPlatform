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

namespace SharpPlatform
{
	public class Hero : Character
	{
		private int AttackWidth = 25;
		public Rectangle AttackRange {
			get { return new Rectangle (Size.X - AttackWidth, Size.Y, Size.Width + AttackWidth * 2, Size.Height); }
		}

		public int Money {
			get;
			set;
		}	

		public List<IInventoryItem> Inventory {
			get;
			private set;
		}

		public IEquipment RightHand {
			get;
			private set;
		}

		public IEquipment LeftHand {
			get;
			private set;
		}

		public Hero (Rectangle size, Texture2D sprite)
		{
			Size = size;
			Sprite = sprite;
			Health = 5;
			Inventory = new List<IInventoryItem>();
		}

		public bool Intersects(Rectangle value) //overloading intersect method
		{
			return Size.Intersects (value);
		}

		public bool Intersects(GameObject gameObject) //overloading intersect method
		{
			return Intersects (gameObject.Size);
		}

		public bool AttackIntersects(Rectangle value) //overloading intersect method
		{
			return AttackRange.Intersects (value);
		}
		public bool AttackIntersects(Enemy enemy) //overloading intersect method
		{
			return AttackIntersects (enemy.Size);
		}

		public override void Defend (AttackObject attacker)
		{
			var defendValue = 0;
			var shield = RightHand as IShield;
			if (shield != null)
				defendValue += shield.DefendValue;
			shield = LeftHand as IShield;
			if (shield != null)
				defendValue += shield.DefendValue;
			Defend(defendValue, attacker.Attack());
		}

		public override int Attack()
		{
			var attackValue = AttackValue;
			var weapon = RightHand as IWeapon;
			if (weapon != null)
				attackValue += weapon.AttackValue;
			weapon = LeftHand as IWeapon;
			if (weapon != null)
				attackValue += weapon.AttackValue;

			return attackValue;
		}

		public void Equip(IEquipment equipment)
		{
			if (!Inventory.Contains((IInventoryItem)equipment))
				throw new Exception("Equipment not found in inventory");

			if (RightHand == null)
				RightHand = equipment;
			else if (LeftHand == null)
				LeftHand = equipment;
			else
				throw new Exception ("Both hand are equipt with equipments");
			
			Inventory.Remove ((IInventoryItem)equipment);
		}

		public void Unequip (IEquipment equipment)
		{
			if (RightHand == equipment)
				RightHand = null;
			else if (LeftHand == equipment)
				LeftHand = null;
			else
				throw new Exception ("There is no equipment equiped ");
		}
	}

	public interface IInventoryItem : IItem
	{
	}

	public interface IEquipment : IInventoryItem
	{
	}

	public interface IWeapon : IEquipment
	{
		int AttackValue { get; }
	}

	public  interface IShield : IEquipment
	{
		int DefendValue { get; }
	}

}