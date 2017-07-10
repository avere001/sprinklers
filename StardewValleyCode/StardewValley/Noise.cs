using System;

namespace StardewValley
{
	public class Noise
	{
		private static byte[] perm = new byte[]
		{
			151,
			160,
			137,
			91,
			90,
			15,
			131,
			13,
			201,
			95,
			96,
			53,
			194,
			233,
			7,
			225,
			140,
			36,
			103,
			30,
			69,
			142,
			8,
			99,
			37,
			240,
			21,
			10,
			23,
			190,
			6,
			148,
			247,
			120,
			234,
			75,
			0,
			26,
			197,
			62,
			94,
			252,
			219,
			203,
			117,
			35,
			11,
			32,
			57,
			177,
			33,
			88,
			237,
			149,
			56,
			87,
			174,
			20,
			125,
			136,
			171,
			168,
			68,
			175,
			74,
			165,
			71,
			134,
			139,
			48,
			27,
			166,
			77,
			146,
			158,
			231,
			83,
			111,
			229,
			122,
			60,
			211,
			133,
			230,
			220,
			105,
			92,
			41,
			55,
			46,
			245,
			40,
			244,
			102,
			143,
			54,
			65,
			25,
			63,
			161,
			1,
			216,
			80,
			73,
			209,
			76,
			132,
			187,
			208,
			89,
			18,
			169,
			200,
			196,
			135,
			130,
			116,
			188,
			159,
			86,
			164,
			100,
			109,
			198,
			173,
			186,
			3,
			64,
			52,
			217,
			226,
			250,
			124,
			123,
			5,
			202,
			38,
			147,
			118,
			126,
			255,
			82,
			85,
			212,
			207,
			206,
			59,
			227,
			47,
			16,
			58,
			17,
			182,
			189,
			28,
			42,
			223,
			183,
			170,
			213,
			119,
			248,
			152,
			2,
			44,
			154,
			163,
			70,
			221,
			153,
			101,
			155,
			167,
			43,
			172,
			9,
			129,
			22,
			39,
			253,
			19,
			98,
			108,
			110,
			79,
			113,
			224,
			232,
			178,
			185,
			112,
			104,
			218,
			246,
			97,
			228,
			251,
			34,
			242,
			193,
			238,
			210,
			144,
			12,
			191,
			179,
			162,
			241,
			81,
			51,
			145,
			235,
			249,
			14,
			239,
			107,
			49,
			192,
			214,
			31,
			181,
			199,
			106,
			157,
			184,
			84,
			204,
			176,
			115,
			121,
			50,
			45,
			127,
			4,
			150,
			254,
			138,
			236,
			205,
			93,
			222,
			114,
			67,
			29,
			24,
			72,
			243,
			141,
			128,
			195,
			78,
			66,
			215,
			61,
			156,
			180,
			151,
			160,
			137,
			91,
			90,
			15,
			131,
			13,
			201,
			95,
			96,
			53,
			194,
			233,
			7,
			225,
			140,
			36,
			103,
			30,
			69,
			142,
			8,
			99,
			37,
			240,
			21,
			10,
			23,
			190,
			6,
			148,
			247,
			120,
			234,
			75,
			0,
			26,
			197,
			62,
			94,
			252,
			219,
			203,
			117,
			35,
			11,
			32,
			57,
			177,
			33,
			88,
			237,
			149,
			56,
			87,
			174,
			20,
			125,
			136,
			171,
			168,
			68,
			175,
			74,
			165,
			71,
			134,
			139,
			48,
			27,
			166,
			77,
			146,
			158,
			231,
			83,
			111,
			229,
			122,
			60,
			211,
			133,
			230,
			220,
			105,
			92,
			41,
			55,
			46,
			245,
			40,
			244,
			102,
			143,
			54,
			65,
			25,
			63,
			161,
			1,
			216,
			80,
			73,
			209,
			76,
			132,
			187,
			208,
			89,
			18,
			169,
			200,
			196,
			135,
			130,
			116,
			188,
			159,
			86,
			164,
			100,
			109,
			198,
			173,
			186,
			3,
			64,
			52,
			217,
			226,
			250,
			124,
			123,
			5,
			202,
			38,
			147,
			118,
			126,
			255,
			82,
			85,
			212,
			207,
			206,
			59,
			227,
			47,
			16,
			58,
			17,
			182,
			189,
			28,
			42,
			223,
			183,
			170,
			213,
			119,
			248,
			152,
			2,
			44,
			154,
			163,
			70,
			221,
			153,
			101,
			155,
			167,
			43,
			172,
			9,
			129,
			22,
			39,
			253,
			19,
			98,
			108,
			110,
			79,
			113,
			224,
			232,
			178,
			185,
			112,
			104,
			218,
			246,
			97,
			228,
			251,
			34,
			242,
			193,
			238,
			210,
			144,
			12,
			191,
			179,
			162,
			241,
			81,
			51,
			145,
			235,
			249,
			14,
			239,
			107,
			49,
			192,
			214,
			31,
			181,
			199,
			106,
			157,
			184,
			84,
			204,
			176,
			115,
			121,
			50,
			45,
			127,
			4,
			150,
			254,
			138,
			236,
			205,
			93,
			222,
			114,
			67,
			29,
			24,
			72,
			243,
			141,
			128,
			195,
			78,
			66,
			215,
			61,
			156,
			180
		};

		public static float Generate(float x)
		{
			int num = Noise.FastFloor(x);
			int num2 = num + 1;
			float num3 = x - (float)num;
			float num4 = num3 - 1f;
			float expr_21 = 1f - num3 * num3;
			float expr_23 = expr_21 * expr_21;
			float num5 = expr_23 * expr_23 * Noise.grad((int)Noise.perm[num & 255], num3);
			float expr_44 = 1f - num4 * num4;
			float expr_46 = expr_44 * expr_44;
			float num6 = expr_46 * expr_46 * Noise.grad((int)Noise.perm[num2 & 255], num4);
			return 0.395f * (num5 + num6);
		}

		public static float Generate(float x, float y)
		{
			float num = (x + y) * 0.3660254f;
			float arg_12_0 = x + num;
			float x2 = y + num;
			int arg_20_0 = Noise.FastFloor(arg_12_0);
			int num2 = Noise.FastFloor(x2);
			float num3 = (float)(arg_20_0 + num2) * 0.211324871f;
			float num4 = (float)arg_20_0 - num3;
			float num5 = (float)num2 - num3;
			float num6 = x - num4;
			float num7 = y - num5;
			int num8;
			int num9;
			if (num6 > num7)
			{
				num8 = 1;
				num9 = 0;
			}
			else
			{
				num8 = 0;
				num9 = 1;
			}
			float num10 = num6 - (float)num8 + 0.211324871f;
			float num11 = num7 - (float)num9 + 0.211324871f;
			float num12 = num6 - 1f + 0.422649741f;
			float num13 = num7 - 1f + 0.422649741f;
			int num14 = arg_20_0 % 256;
			int num15 = num2 % 256;
			float num16 = 0.5f - num6 * num6 - num7 * num7;
			float num17;
			if (num16 < 0f)
			{
				num17 = 0f;
			}
			else
			{
				num16 *= num16;
				num17 = num16 * num16 * Noise.grad((int)Noise.perm[num14 + (int)Noise.perm[num15]], num6, num7);
			}
			float num18 = 0.5f - num10 * num10 - num11 * num11;
			float num19;
			if (num18 < 0f)
			{
				num19 = 0f;
			}
			else
			{
				num18 *= num18;
				num19 = num18 * num18 * Noise.grad((int)Noise.perm[num14 + num8 + (int)Noise.perm[num15 + num9]], num10, num11);
			}
			float num20 = 0.5f - num12 * num12 - num13 * num13;
			float num21;
			if (num20 < 0f)
			{
				num21 = 0f;
			}
			else
			{
				num20 *= num20;
				num21 = num20 * num20 * Noise.grad((int)Noise.perm[num14 + 1 + (int)Noise.perm[num15 + 1]], num12, num13);
			}
			return 40f * (num17 + num19 + num21);
		}

		private static int FastFloor(float x)
		{
			if (x <= 0f)
			{
				return (int)x - 1;
			}
			return (int)x;
		}

		private static float grad(int hash, float x)
		{
			int num = hash & 15;
			float num2 = 1f + (float)(num & 7);
			if ((num & 8) != 0)
			{
				num2 = -num2;
			}
			return num2 * x;
		}

		private static float grad(int hash, float x, float y)
		{
			int num = hash & 7;
			float num2 = (num < 4) ? x : y;
			float num3 = (num < 4) ? y : x;
			return (((num & 1) != 0) ? (-num2) : num2) + (((num & 2) != 0) ? (-2f * num3) : (2f * num3));
		}

		private static float grad(int hash, float x, float y, float z)
		{
			int num = hash & 15;
			float num2 = (num < 8) ? x : y;
			float num3 = (num < 4) ? y : ((num == 12 || num == 14) ? x : z);
			return (((num & 1) != 0) ? (-num2) : num2) + (((num & 2) != 0) ? (-num3) : num3);
		}

		private static float grad(int hash, float x, float y, float z, float t)
		{
			int num = hash & 31;
			float num2 = (num < 24) ? x : y;
			float num3 = (num < 16) ? y : z;
			float num4 = (num < 8) ? z : t;
			return (((num & 1) != 0) ? (-num2) : num2) + (((num & 2) != 0) ? (-num3) : num3) + (((num & 4) != 0) ? (-num4) : num4);
		}
	}
}
