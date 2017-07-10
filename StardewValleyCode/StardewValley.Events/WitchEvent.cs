using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewValley.Events
{
	public class WitchEvent : FarmEvent
	{
		public const int identifier = 942069;

		private Vector2 witchPosition;

		private Building targetBuilding;

		private Farm f;

		private Random r;

		private int witchFrame;

		private int witchAnimationTimer;

		private int animationLoopsDone;

		private int timerSinceFade;

		private bool animateLeft;

		private bool terminate;

		public bool setUp()
		{
			this.f = (Game1.getLocationFromName("Farm") as Farm);
			this.r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			foreach (Building current in this.f.buildings)
			{
				if (current is Coop && !current.buildingType.Equals("Coop") && !(current.indoors as AnimalHouse).isFull() && current.indoors.objects.Count < 50 && this.r.NextDouble() < 0.8)
				{
					this.targetBuilding = current;
				}
			}
			if (this.targetBuilding == null)
			{
				foreach (Building current2 in this.f.buildings)
				{
					if (current2.buildingType.Equals("Slime Hutch") && current2.indoors.characters.Count > 0 && this.r.NextDouble() < 0.5 && current2.indoors.numberOfObjectsOfType(83, true) == 0)
					{
						this.targetBuilding = current2;
					}
				}
			}
			if (this.targetBuilding == null)
			{
				return true;
			}
			Game1.currentLightSources.Add(new LightSource(4, this.witchPosition, 2f, Color.Black, 942069));
			Game1.currentLocation = this.f;
			this.f.resetForPlayerEntry();
			Game1.fadeClear();
			Game1.nonWarpFade = true;
			Game1.timeOfDay = 2400;
			Game1.ambientLight = new Color(200, 190, 40);
			Game1.displayHUD = false;
			Game1.freezeControls = true;
			Game1.viewportFreeze = true;
			Game1.displayFarmer = false;
			Game1.viewport.X = Math.Max(0, Math.Min(this.f.map.DisplayWidth - Game1.viewport.Width, this.targetBuilding.tileX * Game1.tileSize - Game1.viewport.Width / 2));
			Game1.viewport.Y = Math.Max(0, Math.Min(this.f.map.DisplayHeight - Game1.viewport.Height, (this.targetBuilding.tileY - 3) * Game1.tileSize - Game1.viewport.Height / 2));
			this.witchPosition = new Vector2((float)(Game1.viewport.X + Game1.viewport.Width + Game1.tileSize * 2), (float)(this.targetBuilding.tileY * Game1.tileSize - Game1.tileSize));
			Game1.changeMusicTrack("nightTime");
			DelayedAction.playSoundAfterDelay("cacklingWitch", 3200);
			return false;
		}

		public bool tickUpdate(GameTime time)
		{
			if (this.terminate)
			{
				return true;
			}
			Game1.UpdateGameClock(time);
			this.f.UpdateWhenCurrentLocation(time);
			this.f.updateEvenIfFarmerIsntHere(time, false);
			Game1.UpdateOther(time);
			Utility.repositionLightSource(942069, this.witchPosition + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
			if (this.animationLoopsDone < 1)
			{
				this.timerSinceFade += time.ElapsedGameTime.Milliseconds;
			}
			if (this.witchPosition.X > (float)(this.targetBuilding.tileX * Game1.tileSize + Game1.tileSize * 3 / 2))
			{
				if (this.timerSinceFade < 2000)
				{
					return false;
				}
				this.witchPosition.X = this.witchPosition.X - (float)time.ElapsedGameTime.Milliseconds * 0.4f;
				this.witchPosition.Y = this.witchPosition.Y + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 512.0) * 1f;
			}
			else if (this.animationLoopsDone < 4)
			{
				this.witchPosition.Y = this.witchPosition.Y + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 512.0) * 1f;
				this.witchAnimationTimer += time.ElapsedGameTime.Milliseconds;
				if (this.witchAnimationTimer > 2000)
				{
					this.witchAnimationTimer = 0;
					if (!this.animateLeft)
					{
						this.witchFrame++;
						if (this.witchFrame == 1)
						{
							this.animateLeft = true;
							for (int i = 0; i < 75; i++)
							{
								this.f.temporarySprites.Add(new TemporaryAnimatedSprite(10, this.witchPosition + new Vector2((float)(2 * Game1.pixelZoom), (float)(Game1.tileSize * 3 / 2 - Game1.pixelZoom * 4)), (this.r.NextDouble() < 0.5) ? Color.Lime : Color.DarkViolet, 8, false, 100f, 0, -1, -1f, -1, 0)
								{
									motion = new Vector2((float)this.r.Next(-100, 100) / 100f, 1.5f),
									alphaFade = 0.015f,
									delayBeforeAnimationStart = i * 30,
									layerDepth = 1f
								});
							}
							Game1.playSound("debuffSpell");
						}
					}
					else
					{
						this.witchFrame--;
						this.animationLoopsDone = 4;
						DelayedAction.playSoundAfterDelay("cacklingWitch", 2500);
					}
				}
			}
			else
			{
				this.witchAnimationTimer += time.ElapsedGameTime.Milliseconds;
				this.witchFrame = 0;
				if (this.witchAnimationTimer > 1000 && this.witchPosition.X > -999999f)
				{
					this.witchPosition.Y = this.witchPosition.Y + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 256.0) * 2f;
					this.witchPosition.X = this.witchPosition.X - (float)time.ElapsedGameTime.Milliseconds * 0.4f;
				}
				if (this.witchPosition.X < (float)(Game1.viewport.X - Game1.tileSize * 2) || float.IsNaN(this.witchPosition.X))
				{
					if (!Game1.fadeToBlack && this.witchPosition.X != -999999f)
					{
						Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterLastFade), 0.02f);
						Game1.changeMusicTrack("none");
						this.timerSinceFade = 0;
						this.witchPosition.X = -999999f;
					}
					this.timerSinceFade += time.ElapsedGameTime.Milliseconds;
				}
			}
			return false;
		}

		public void afterLastFade()
		{
			this.terminate = true;
			Game1.globalFadeToClear(null, 0.02f);
		}

		public void draw(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.witchPosition), new Rectangle?(new Rectangle(277, 1886 + this.witchFrame * 29, 34, 29)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9999999f);
		}

		public void makeChangesToLocation()
		{
			if (this.targetBuilding.buildingType.Equals("Slime Hutch"))
			{
				using (List<NPC>.Enumerator enumerator = this.targetBuilding.indoors.characters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NPC current = enumerator.Current;
						if (current is GreenSlime)
						{
							(current as GreenSlime).color = new Color(40 + this.r.Next(10), 40 + this.r.Next(10), 40 + this.r.Next(10));
						}
					}
					return;
				}
			}
			for (int i = 0; i < 200; i++)
			{
				Vector2 vector = new Vector2((float)this.r.Next(2, this.targetBuilding.indoors.map.Layers[0].LayerWidth - 2), (float)this.r.Next(2, this.targetBuilding.indoors.map.Layers[0].LayerHeight - 2));
				if (this.targetBuilding.indoors.isTileLocationTotallyClearAndPlaceable(vector) || (this.targetBuilding.indoors.terrainFeatures.ContainsKey(vector) && this.targetBuilding.indoors.terrainFeatures[vector] is Flooring))
				{
					this.targetBuilding.indoors.objects.Add(vector, new StardewValley.Object(Vector2.Zero, 305, null, false, true, false, true));
					return;
				}
			}
		}

		public void drawAboveEverything(SpriteBatch b)
		{
		}
	}
}
