using System;

namespace WinToolkit
{
	public static class BitMaskWrapper
	{
		public static bool IsSet<T>(UInt64 bitMask, T flagToCheck) where T : struct
		{
			return (bitMask & GetEquivalentBitMask(flagToCheck)) != 0;
		}

		public static void Set<T>(ref UInt64 bitMask, T flagToSet) where T : struct
		{
			bitMask |= GetEquivalentBitMask(flagToSet);
		}

		public static void Unset<T>(ref UInt64 bitMask, T flagToSet) where T : struct
		{
			bitMask &= ~GetEquivalentBitMask(flagToSet);
		}

		private static UInt64 GetEquivalentBitMask<T>(T biIndex) where T : struct
		{
			UInt64 flagEquivalentMask = 1;
			flagEquivalentMask <<= Convert.ToInt32(biIndex);
			if (Convert.ToInt32(biIndex) > 32)
				flagEquivalentMask <<= Convert.ToInt32(biIndex);
			return flagEquivalentMask;
		}
	}
}
