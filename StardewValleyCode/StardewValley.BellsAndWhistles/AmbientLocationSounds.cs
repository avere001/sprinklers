using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.BellsAndWhistles
{
	public class AmbientLocationSounds
	{
		public const int sound_babblingBrook = 0;

		public const int sound_cracklingFire = 1;

		public const int sound_engine = 2;

		public const int sound_cricket = 3;

		public const int numberOfSounds = 4;

		public const float doNotPlay = 9999999f;

		private static Dictionary<Vector2, int> sounds = new Dictionary<Vector2, int>();

		private static int updateTimer = 100;

		private static int farthestSoundDistance = Game1.tileSize * 16;

		private static float[] shortestDistanceForCue;

		private static Cue babblingBrook;

		private static Cue cracklingFire;

		private static Cue engine;

		private static Cue cricket;

		private static float volumeOverrideForLocChange;

		public static void InitShared()
		{
			if (Game1.soundBank != null)
			{
				if (AmbientLocationSounds.babblingBrook == null)
				{
					AmbientLocationSounds.babblingBrook = Game1.soundBank.GetCue("babblingBrook");
					AmbientLocationSounds.babblingBrook.Play();
					AmbientLocationSounds.babblingBrook.Pause();
				}
				if (AmbientLocationSounds.cracklingFire == null)
				{
					AmbientLocationSounds.cracklingFire = Game1.soundBank.GetCue("cracklingFire");
					AmbientLocationSounds.cracklingFire.Play();
					AmbientLocationSounds.cracklingFire.Pause();
				}
				if (AmbientLocationSounds.engine == null)
				{
					AmbientLocationSounds.engine = Game1.soundBank.GetCue("heavyEngine");
					AmbientLocationSounds.engine.Play();
					AmbientLocationSounds.engine.Pause();
				}
				if (AmbientLocationSounds.cricket == null)
				{
					AmbientLocationSounds.cricket = Game1.soundBank.GetCue("cricketsAmbient");
					AmbientLocationSounds.cricket.Play();
					AmbientLocationSounds.cricket.Pause();
				}
			}
			AmbientLocationSounds.shortestDistanceForCue = new float[4];
		}

		public static void update(GameTime time)
		{
			if (AmbientLocationSounds.sounds.Count == 0)
			{
				return;
			}
			if (AmbientLocationSounds.volumeOverrideForLocChange < 1f)
			{
				AmbientLocationSounds.volumeOverrideForLocChange += (float)time.ElapsedGameTime.Milliseconds * 0.0003f;
			}
			AmbientLocationSounds.updateTimer -= time.ElapsedGameTime.Milliseconds;
			if (AmbientLocationSounds.updateTimer <= 0)
			{
				for (int i = 0; i < AmbientLocationSounds.shortestDistanceForCue.Length; i++)
				{
					AmbientLocationSounds.shortestDistanceForCue[i] = 9999999f;
				}
				Vector2 standingPosition = Game1.player.getStandingPosition();
				for (int j = 0; j < AmbientLocationSounds.sounds.Count; j++)
				{
					float num = Vector2.Distance(AmbientLocationSounds.sounds.ElementAt(j).Key, standingPosition);
					if (AmbientLocationSounds.shortestDistanceForCue[AmbientLocationSounds.sounds.ElementAt(j).Value] > num)
					{
						AmbientLocationSounds.shortestDistanceForCue[AmbientLocationSounds.sounds.ElementAt(j).Value] = num;
					}
				}
				if (AmbientLocationSounds.volumeOverrideForLocChange >= 0f)
				{
					for (int k = 0; k < AmbientLocationSounds.shortestDistanceForCue.Length; k++)
					{
						if (AmbientLocationSounds.shortestDistanceForCue[k] <= (float)AmbientLocationSounds.farthestSoundDistance)
						{
							float num2 = Math.Min(AmbientLocationSounds.volumeOverrideForLocChange, Math.Min(1f, 1f - AmbientLocationSounds.shortestDistanceForCue[k] / (float)AmbientLocationSounds.farthestSoundDistance));
							switch (k)
							{
							case 0:
								if (AmbientLocationSounds.babblingBrook != null)
								{
									AmbientLocationSounds.babblingBrook.SetVariable("Volume", num2 * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientLocationSounds.babblingBrook.Resume();
								}
								break;
							case 1:
								if (AmbientLocationSounds.cracklingFire != null)
								{
									AmbientLocationSounds.cracklingFire.SetVariable("Volume", num2 * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientLocationSounds.cracklingFire.Resume();
								}
								break;
							case 2:
								if (AmbientLocationSounds.engine != null)
								{
									AmbientLocationSounds.engine.SetVariable("Volume", num2 * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientLocationSounds.engine.Resume();
								}
								break;
							case 3:
								if (AmbientLocationSounds.cricket != null)
								{
									AmbientLocationSounds.cricket.SetVariable("Volume", num2 * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientLocationSounds.cricket.Resume();
								}
								break;
							}
						}
						else
						{
							switch (k)
							{
							case 0:
								if (AmbientLocationSounds.babblingBrook != null)
								{
									AmbientLocationSounds.babblingBrook.Pause();
								}
								break;
							case 1:
								if (AmbientLocationSounds.cracklingFire != null)
								{
									AmbientLocationSounds.cracklingFire.Pause();
								}
								break;
							case 2:
								if (AmbientLocationSounds.engine != null)
								{
									AmbientLocationSounds.engine.Pause();
								}
								break;
							case 3:
								if (AmbientLocationSounds.cricket != null)
								{
									AmbientLocationSounds.cricket.Pause();
								}
								break;
							}
						}
					}
				}
				AmbientLocationSounds.updateTimer = 100;
			}
		}

		public static void changeSpecificVariable(string variableName, float value, int whichSound)
		{
			if (whichSound == 2 && AmbientLocationSounds.engine != null)
			{
				AmbientLocationSounds.engine.SetVariable(variableName, value);
			}
		}

		public static void addSound(Vector2 tileLocation, int whichSound)
		{
			if (!AmbientLocationSounds.sounds.ContainsKey(tileLocation * (float)Game1.tileSize))
			{
				AmbientLocationSounds.sounds.Add(tileLocation * (float)Game1.tileSize, whichSound);
			}
		}

		public static void removeSound(Vector2 tileLocation)
		{
			if (AmbientLocationSounds.sounds.ContainsKey(tileLocation * (float)Game1.tileSize))
			{
				switch (AmbientLocationSounds.sounds[tileLocation * (float)Game1.tileSize])
				{
				case 0:
					if (AmbientLocationSounds.babblingBrook != null)
					{
						AmbientLocationSounds.babblingBrook.Pause();
					}
					break;
				case 1:
					if (AmbientLocationSounds.cracklingFire != null)
					{
						AmbientLocationSounds.cracklingFire.Pause();
					}
					break;
				case 2:
					if (AmbientLocationSounds.engine != null)
					{
						AmbientLocationSounds.engine.Pause();
					}
					break;
				case 3:
					if (AmbientLocationSounds.cricket != null)
					{
						AmbientLocationSounds.cricket.Pause();
					}
					break;
				}
				AmbientLocationSounds.sounds.Remove(tileLocation * (float)Game1.tileSize);
			}
		}

		public static void onLocationLeave()
		{
			AmbientLocationSounds.sounds.Clear();
			AmbientLocationSounds.volumeOverrideForLocChange = -0.5f;
			if (AmbientLocationSounds.babblingBrook != null)
			{
				AmbientLocationSounds.babblingBrook.Pause();
			}
			if (AmbientLocationSounds.cracklingFire != null)
			{
				AmbientLocationSounds.cracklingFire.Pause();
			}
			if (AmbientLocationSounds.engine != null)
			{
				AmbientLocationSounds.engine.SetVariable("Frequency", 100f);
				AmbientLocationSounds.engine.Pause();
			}
			if (AmbientLocationSounds.cricket != null)
			{
				AmbientLocationSounds.cricket.Pause();
			}
		}
	}
}
