using System;

namespace StardewValley.SDKs
{
	public interface SDKHelper
	{
		void EarlyInitialize();

		void Initialize();

		void GetAchievement(string achieve);

		void ResetAchievements();

		void Update();

		void Shutdown();

		void DebugInfo();

		string FilterDirtyWords(string words);
	}
}
