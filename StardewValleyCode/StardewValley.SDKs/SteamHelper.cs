using Steamworks;
using System;

namespace StardewValley.SDKs
{
	public class SteamHelper : SDKHelper
	{
		public Callback<GameOverlayActivated_t> m_GameOverlayActivated;

		public bool active;

		public void EarlyInitialize()
		{
		}

		public void Initialize()
		{
			try
			{
				this.active = SteamAPI.Init();
			}
			catch (Exception)
			{
			}
			if (this.active)
			{
				this.m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnGameOverlayActivated));
			}
		}

		private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
		{
			if (this.active)
			{
				if (pCallback.m_bActive != 0)
				{
					Game1.paused = !Game1.IsMultiplayer;
					return;
				}
				Game1.paused = false;
			}
		}

		public void GetAchievement(string achieve)
		{
			if (this.active && SteamAPI.IsSteamRunning())
			{
				if (achieve.Equals("0"))
				{
					achieve = "a0";
				}
				try
				{
					SteamUserStats.SetAchievement(achieve);
					SteamUserStats.StoreStats();
				}
				catch (Exception)
				{
				}
			}
		}

		public void ResetAchievements()
		{
			if (this.active && SteamAPI.IsSteamRunning())
			{
				try
				{
					SteamUserStats.ResetAllStats(true);
				}
				catch (Exception)
				{
				}
			}
		}

		public void Update()
		{
			if (this.active)
			{
				SteamAPI.RunCallbacks();
			}
		}

		public void Shutdown()
		{
			SteamAPI.Shutdown();
		}

		public void DebugInfo()
		{
			if (SteamAPI.IsSteamRunning())
			{
				Game1.debugOutput = "steam is running";
				if (SteamUser.BLoggedOn())
				{
					Game1.debugOutput += ", user logged on";
					return;
				}
			}
			else
			{
				Game1.debugOutput = "steam is not running";
				SteamAPI.Init();
			}
		}

		public string FilterDirtyWords(string words)
		{
			return words;
		}
	}
}
