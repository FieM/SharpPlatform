using System;
using Microsoft.Xna.Framework;

namespace SharpPlatform
{
	public static class ExtensionMethods
	{
		public static String ToHex(this Color color)
		{
			return "#" + color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}

		public static Color ToColor(this string color)
		{
			return new Color (
				int.Parse (color.Substring (1, 2), System.Globalization.NumberStyles.HexNumber),
				int.Parse (color.Substring (3, 2), System.Globalization.NumberStyles.HexNumber),
				int.Parse (color.Substring (5, 2), System.Globalization.NumberStyles.HexNumber),
				int.Parse (color.Substring (7, 2), System.Globalization.NumberStyles.HexNumber));
		}
	}
}

