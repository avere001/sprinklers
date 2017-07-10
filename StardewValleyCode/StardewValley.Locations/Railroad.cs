using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class Railroad : GameLocation
	{
		public const int trainSoundDelay = 10000;

		[XmlIgnore]
		public Train train;

		private bool hasTrainPassed;

		private int trainTime = -1;

		[XmlIgnore]
		public int trainTimer;

		public static Cue trainLoop;

		public bool witchStatueGone;

		public Railroad()
		{
		}

		public Railroad(Map map, string name) : base(map, name)
		{
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.currentSong != null && Game1.currentSong.Name.ToLower().Contains("ambient"))
			{
				Game1.changeMusicTrack("none");
			}
			if (this.witchStatueGone || Game1.player.mailReceived.Contains("witchStatueGone"))
			{
				base.removeTile(54, 35, "Buildings");
				base.removeTile(54, 34, "Front");
			}
			if (!Game1.IsWinter)
			{
				AmbientLocationSounds.addSound(new Vector2(15f, 56f), 0);
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			if (Railroad.trainLoop != null)
			{
				Railroad.trainLoop.Stop(AudioStopOptions.Immediate);
			}
			Railroad.trainLoop = null;
		}

		public override void checkForMusic(GameTime time)
		{
			if (Game1.timeOfDay < 1800 && !Game1.isRaining && !Game1.eventUp)
			{
				string currentSeason = Game1.currentSeason;
				if (currentSeason == "summer" || currentSeason == "fall" || currentSeason == "spring")
				{
					Game1.changeMusicTrack(Game1.currentSeason + "_day_ambient");
					return;
				}
			}
			else if (Game1.timeOfDay >= 2000 && !Game1.isRaining && !Game1.eventUp)
			{
				string currentSeason = Game1.currentSeason;
				if (currentSeason == "summer" || currentSeason == "fall" || currentSeason == "spring")
				{
					Game1.changeMusicTrack("spring_night_ambient");
				}
			}
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				if (tileIndex == 287)
				{
					if (Game1.player.hasDarkTalisman)
					{
						Game1.player.freezePause = 7000;
						Game1.playSound("fireball");
						DelayedAction.playSoundAfterDelay("secret1", 2000);
						DelayedAction.removeTileAfterDelay(54, 35, 2000, this, "Buildings");
						DelayedAction.removeTileAfterDelay(54, 34, 2000, this, "Front");
						DelayedAction.removeTemporarySpriteAfterDelay(this, 9999f, 2000);
						this.witchStatueGone = true;
						who.mailReceived.Add("witchStatueGone");
						for (int i = 0; i < 22; i++)
						{
							DelayedAction.playSoundAfterDelay("batFlap", 2220 + 240 * i);
						}
						this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(576, 271, 28, 31), 60f, 3, 999, new Vector2(54f, 34f) * (float)Game1.tileSize + new Vector2(-2f, 1f) * (float)Game1.pixelZoom, false, false, (float)(34 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							xPeriodic = true,
							xPeriodicLoopTime = 8000f,
							xPeriodicRange = (float)(Game1.tileSize * 6),
							motion = new Vector2(-2f, 0f),
							acceleration = new Vector2(0f, -0.015f),
							pingPong = true,
							delayBeforeAnimationStart = 2000
						});
						this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 499, 10, 11), 50f, 7, 999, new Vector2(54f, 34f) * (float)Game1.tileSize + new Vector2(7f, 11f) * (float)Game1.pixelZoom, false, false, (float)(34 * Game1.tileSize) / 10000f + 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							xPeriodic = true,
							xPeriodicLoopTime = 8000f,
							xPeriodicRange = (float)(Game1.tileSize * 6),
							motion = new Vector2(-2f, 0f),
							acceleration = new Vector2(0f, -0.015f),
							delayBeforeAnimationStart = 2000
						});
						this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 499, 10, 11), 35.715f, 7, 8, new Vector2(54f, 34f) * (float)Game1.tileSize + new Vector2(3f, 10f) * (float)Game1.pixelZoom, false, false, (float)(36 * Game1.tileSize) / 10000f + 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							id = 9999f
						});
					}
					else
					{
						Game1.drawObjectDialogue("???");
					}
					return true;
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			this.hasTrainPassed = false;
			this.trainTime = -1;
			Random random = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed);
			if (random.NextDouble() < 0.2 && Game1.isLocationAccessible("Railroad"))
			{
				this.trainTime = random.Next(900, 1800);
				this.trainTime -= this.trainTime % 10;
			}
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			return (this.train != null && this.train.getBoundingBox().Intersects(position)) || base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		public void setTrainComing(int delay)
		{
			this.trainTimer = delay;
			if (Game1.currentLocation.isOutdoors && !Game1.isFestival())
			{
				Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:Railroad_TrainComing", new object[0]));
				if (Game1.soundBank != null)
				{
					Cue expr_4A = Game1.soundBank.GetCue("distantTrain");
					expr_4A.SetVariable("Volume", 100f);
					expr_4A.Play();
				}
			}
		}

		public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
		{
			base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
			if (Game1.timeOfDay == this.trainTime - this.trainTime % 10 && this.trainTimer == 0 && !Game1.isFestival() && this.train == null)
			{
				this.setTrainComing(10000);
			}
			if (this.trainTimer > 0)
			{
				this.trainTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.trainTimer <= 0)
				{
					this.train = new Train();
					if (Game1.currentLocation.Equals(this))
					{
						Game1.playSound("trainWhistle");
					}
				}
				if (this.trainTimer < 3500)
				{
					if (Game1.soundBank != null && (Railroad.trainLoop == null || Railroad.trainLoop.IsStopped))
					{
						Railroad.trainLoop = Game1.soundBank.GetCue("trainLoop");
						Railroad.trainLoop.SetVariable("Volume", 0f);
					}
					if (Game1.currentLocation.Equals(this) && Railroad.trainLoop != null && !Railroad.trainLoop.IsPlaying)
					{
						Railroad.trainLoop.Play();
					}
				}
			}
			if (this.train != null)
			{
				if (Railroad.trainLoop == null || !Railroad.trainLoop.IsPlaying)
				{
					if (Game1.soundBank != null && (Railroad.trainLoop == null || Railroad.trainLoop.IsStopped))
					{
						Railroad.trainLoop = Game1.soundBank.GetCue("trainLoop");
						Railroad.trainLoop.SetVariable("Volume", 0f);
					}
					if (Game1.currentLocation.Equals(this) && Railroad.trainLoop != null)
					{
						Railroad.trainLoop.Play();
					}
				}
				if (this.train.Update(time, this))
				{
					this.train = null;
				}
				if (Railroad.trainLoop != null && Railroad.trainLoop.GetVariable("Volume") < 100f)
				{
					Railroad.trainLoop.SetVariable("Volume", Railroad.trainLoop.GetVariable("Volume") + 0.5f);
					return;
				}
			}
			else if (Railroad.trainLoop != null && this.trainTimer <= 0)
			{
				Railroad.trainLoop.SetVariable("Volume", Railroad.trainLoop.GetVariable("Volume") - 0.15f);
				if (Railroad.trainLoop.GetVariable("Volume") <= 0f)
				{
					Railroad.trainLoop.Stop(AudioStopOptions.Immediate);
					Railroad.trainLoop = null;
					return;
				}
			}
			else if (this.trainTimer > 0 && Railroad.trainLoop != null)
			{
				Railroad.trainLoop.SetVariable("Volume", Railroad.trainLoop.GetVariable("Volume") + 0.15f);
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (this.train != null && !Game1.eventUp)
			{
				this.train.draw(b);
			}
		}
	}
}
