using System;

namespace StardewValley
{
	internal static class NoiseGenerator
	{
		public static int Seed
		{
			get;
			set;
		}

		public static int Octaves
		{
			get;
			set;
		}

		public static double Amplitude
		{
			get;
			set;
		}

		public static double Persistence
		{
			get;
			set;
		}

		public static double Frequency
		{
			get;
			set;
		}

		static NoiseGenerator()
		{
			NoiseGenerator.Seed = new Random().Next(2147483647);
			NoiseGenerator.Octaves = 8;
			NoiseGenerator.Amplitude = 1.0;
			NoiseGenerator.Frequency = 0.015;
			NoiseGenerator.Persistence = 0.65;
		}

		public static double Noise(int x, int y)
		{
			double num = 0.0;
			double num2 = NoiseGenerator.Frequency;
			double num3 = NoiseGenerator.Amplitude;
			for (int i = 0; i < NoiseGenerator.Octaves; i++)
			{
				num += NoiseGenerator.Smooth((double)x * num2, (double)y * num2) * num3;
				num2 *= 2.0;
				num3 *= NoiseGenerator.Persistence;
			}
			if (num < -2.4)
			{
				num = -2.4;
			}
			else if (num > 2.4)
			{
				num = 2.4;
			}
			return num / 2.4;
		}

		public static double NoiseGeneration(int x, int y)
		{
			int num = x + y * 57;
			num = (num << 13 ^ num);
			return 1.0 - (double)(num * (num * num * 15731 + 789221) + NoiseGenerator.Seed & 2147483647) / 1073741824.0;
		}

		private static double Interpolate(double x, double y, double a)
		{
			double num = (1.0 - Math.Cos(a * 3.1415926535897931)) * 0.5;
			return x * (1.0 - num) + y * num;
		}

		private static double Smooth(double x, double y)
		{
			double arg_35_0 = NoiseGenerator.NoiseGeneration((int)x, (int)y);
			double y2 = NoiseGenerator.NoiseGeneration((int)x + 1, (int)y);
			double x2 = NoiseGenerator.NoiseGeneration((int)x, (int)y + 1);
			double y3 = NoiseGenerator.NoiseGeneration((int)x + 1, (int)y + 1);
			double arg_4D_0 = NoiseGenerator.Interpolate(arg_35_0, y2, x - (double)((int)x));
			double y4 = NoiseGenerator.Interpolate(x2, y3, x - (double)((int)x));
			return NoiseGenerator.Interpolate(arg_4D_0, y4, y - (double)((int)y));
		}
	}
}
