using System;

namespace SharpPlatform
{
	public abstract class Character : AttackObject
	{
		private DateTime LastDefend;
		public event EventHandler Died;

		public int Health {
			get;
			set;
		}

		public abstract void Defend (AttackObject attacker);

		protected void Defend(int defendValue, int attackValue)
		{
			if ((DateTime.Now - LastDefend).TotalMilliseconds < 1000)
				return;

			LastDefend = DateTime.Now;
			Health -= Math.Max(attackValue - defendValue, 1); //Takes the maximum value between the two arguments
			if (Health <= 0 && Died != null)
				Died(this,new EventArgs());
		}

	}
}

