using System;

namespace StardewValley.SDKs
{
	public class NullSDKHelper : SDKHelper
	{
		public void EarlyInitialize()
		{
		}

		public void Initialize()
		{
		}

		public void GetAchievement(string achieve)
		{
		}

		public void ResetAchievements()
		{
		}

		public void Update()
		{
		}

		public void Shutdown()
		{
		}

		public void DebugInfo()
		{
		}

		public string FilterDirtyWords(string words)
		{
			return words;
		}
	}
}
