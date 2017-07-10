using System;

namespace StardewValley
{
	public class BloomSettings
	{
		public const int nightTimeLights = 7;

		public string Name;

		public float BloomThreshold;

		public float BlurAmount;

		public float BloomIntensity;

		public float BaseIntensity;

		public float BloomSaturation;

		public float BaseSaturation;

		public bool brightWhiteOnly;

		public static BloomSettings[] PresetSettings = new BloomSettings[]
		{
			new BloomSettings("Default", 0.25f, 4f, 1.25f, 1f, 1f, 1f, false),
			new BloomSettings("Soft", 0f, 3f, 1f, 1f, 1f, 1f, false),
			new BloomSettings("Desaturated", 0.5f, 8f, 2f, 1f, 0f, 1f, false),
			new BloomSettings("Saturated", 0.25f, 4f, 2f, 1f, 2f, 0f, false),
			new BloomSettings("RainyDay", 0f, 2f, 0.7f, 1f, 0.5f, 0.5f, false),
			new BloomSettings("SunnyDay", 0.6f, 4f, 0.7f, 1f, 1f, 1f, false),
			new BloomSettings("B&W", 0f, 0f, 0f, 1f, 0f, 0f, false),
			new BloomSettings("NightTimeLights", 0f, 3f, 7f, 1f, 4f, 1.2f, true)
		};

		public BloomSettings(string name, float bloomThreshold, float blurAmount, float bloomIntensity, float baseIntensity, float bloomSaturation, float baseSaturation, bool brightWhiteOnly = false)
		{
			this.Name = name;
			this.BloomThreshold = bloomThreshold;
			this.BlurAmount = blurAmount;
			this.BloomIntensity = bloomIntensity;
			this.BaseIntensity = baseIntensity;
			this.BloomSaturation = bloomSaturation;
			this.BaseSaturation = baseSaturation;
			this.brightWhiteOnly = brightWhiteOnly;
		}
	}
}
