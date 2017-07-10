using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Projectiles;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Minigames
{
	public class TargetGame : IMinigame
	{
		public class Target
		{
			public static int width = 14 * Game1.pixelZoom;

			public static int spawnRightPosition = 15 * Game1.tileSize;

			public static int spawnLeftPosition = 0;

			public static int basicTarget = 0;

			public static int bonusTarget = 1;

			public static int deluxeTarget = 2;

			public static int mediumSpeed = 4;

			public static int slowSpeed = 2;

			public static int fastSpeed = 5;

			public static int nearLane = 7 * Game1.tileSize;

			public static int middleLane = 5 * Game1.tileSize;

			public static int farLane = 2 * Game1.tileSize;

			public static int superNearLane = 9 * Game1.tileSize;

			public static int behindLane = 13 * Game1.tileSize;

			public static int pauseFarRight = 13 * Game1.tileSize;

			public static int pauseRight = 11 * Game1.tileSize;

			public static int pauseMiddleRight = 9 * Game1.tileSize;

			public static int pauseMiddleLeft = 6 * Game1.tileSize;

			public static int pauseLeft = 4 * Game1.tileSize;

			public static int pauseFarLeft = 2 * Game1.tileSize;

			public Microsoft.Xna.Framework.Rectangle Position;

			private int targetType;

			private int countdownBeforeSpawn;

			private int xPausePosition;

			private int xPauseTime;

			private int speed;

			private bool spawned;

			private bool atPausePosition;

			private Microsoft.Xna.Framework.Rectangle sourceRect;

			public Target(int countdownBeforeSpawn, int whichLane, int type = 0, int speed = 4, bool spawnFromRight = true, int pauseAndReturn = -1, int pauseTime = -1)
			{
				this.countdownBeforeSpawn = countdownBeforeSpawn;
				this.targetType = type;
				this.speed = speed * (spawnFromRight ? -1 : 1);
				this.Position = new Microsoft.Xna.Framework.Rectangle(spawnFromRight ? TargetGame.Target.spawnRightPosition : TargetGame.Target.spawnLeftPosition, whichLane, TargetGame.Target.width, TargetGame.Target.width);
				this.xPausePosition = pauseAndReturn;
				this.xPauseTime = pauseTime;
				this.sourceRect = new Microsoft.Xna.Framework.Rectangle(289, 1184 + type * 16, 14, 14);
			}

			public bool update(GameTime time, GameLocation location)
			{
				if (this.countdownBeforeSpawn > 0)
				{
					this.countdownBeforeSpawn -= time.ElapsedGameTime.Milliseconds;
					if (this.countdownBeforeSpawn <= 0)
					{
						this.spawned = true;
					}
				}
				if (this.spawned)
				{
					if (this.atPausePosition)
					{
						this.xPauseTime -= time.ElapsedGameTime.Milliseconds;
						if (this.xPauseTime <= 0)
						{
							this.speed = -this.speed;
							this.atPausePosition = false;
							this.xPausePosition = -1;
						}
					}
					else
					{
						this.Position.X = this.Position.X + this.speed;
						if (this.xPausePosition != -1 && Math.Abs(this.xPausePosition - this.Position.X) <= Math.Abs(this.speed))
						{
							this.atPausePosition = true;
						}
					}
					if (this.Position.X < 0 || this.Position.Right > TargetGame.Target.spawnRightPosition + Game1.tileSize)
					{
						return true;
					}
					for (int i = location.projectiles.Count - 1; i >= 0; i--)
					{
						if (location.projectiles[i].getBoundingBox().Intersects(this.Position))
						{
							this.shatter(location, location.projectiles[i]);
							if (this.targetType != TargetGame.Target.basicTarget)
							{
								location.projectiles[i].behaviorOnCollisionWithOther(location);
								location.projectiles.RemoveAt(i);
							}
							return true;
						}
					}
				}
				return false;
			}

			public void shatter(GameLocation location, Projectile stone)
			{
				int num = 0;
				if (this.targetType == TargetGame.Target.basicTarget)
				{
					Game1.playSound("breakingGlass");
					num++;
				}
				if (this.targetType == TargetGame.Target.bonusTarget)
				{
					Game1.playSound("potterySmash");
					num += 2;
				}
				if (this.targetType == TargetGame.Target.deluxeTarget)
				{
					Game1.playSound("potterySmash");
					num += 5;
				}
				location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(304, 1183 + this.targetType * 16, 16, 16), 60f, 3, 0, new Vector2((float)(this.Position.X - Game1.pixelZoom), (float)(this.Position.Y - Game1.pixelZoom)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
				location.debris.Add(new Debris(num, new Vector2((float)this.Position.Center.X, (float)this.Position.Center.Y), new Color(255, 130, 0), 1f, null));
				TargetGame.score += num;
				if (stone is BasicProjectile && (stone as BasicProjectile).damageToFarmer > 0)
				{
					TargetGame.successShots++;
					(stone as BasicProjectile).damageToFarmer = -1;
				}
			}

			public void draw(SpriteBatch b)
			{
				if (this.spawned)
				{
					b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)this.Position.X, (float)(this.Position.Bottom + Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.Position), new Microsoft.Xna.Framework.Rectangle?(this.sourceRect), Color.White);
				}
			}
		}

		private GameLocation location;

		private LocalizedContentManager content;

		private Item tempItemStash;

		private int timerToStart = 1000;

		private int gameEndTimer = 61000;

		private int showResultsTimer = -1;

		private bool gameDone;

		private bool exit;

		public static int score;

		public static int shotsFired;

		public static int successShots;

		public static int accuracy = -1;

		public static int starTokensWon;

		public List<TargetGame.Target> targets;

		private float modifierBonus;

		public TargetGame()
		{
			TargetGame.score = 0;
			TargetGame.successShots = 0;
			TargetGame.shotsFired = 0;
			this.content = Game1.content.CreateTemporary();
			this.location = new GameLocation(this.content.Load<Map>("Maps\\TargetGame"), "tent");
			Slingshot slingshot = new Slingshot();
			slingshot.attachments[0] = new StardewValley.Object(390, 999, false, -1, 0);
			this.tempItemStash = Game1.player.addItemToInventory(slingshot, 0);
			Game1.player.CurrentToolIndex = 0;
			Game1.globalFadeToClear(null, 0.01f);
			this.location.Map.LoadTileSheets(Game1.mapDisplayDevice);
			Game1.player.position = new Vector2(8f, 13f) * (float)Game1.tileSize;
			this.changeScreenSize();
			this.gameEndTimer = 50000;
			this.targets = new List<TargetGame.Target>();
			this.addTargets();
		}

		public bool overrideFreeMouseMovement()
		{
			return false;
		}

		public bool tick(GameTime time)
		{
			this.location.UpdateWhenCurrentLocation(time);
			this.location.updateEvenIfFarmerIsntHere(time, false);
			Game1.player.Stamina = (float)Game1.player.MaxStamina;
			Game1.player.Update(time, this.location);
			if ((Game1.oldKBState.GetPressedKeys().Length == 0 || (Game1.oldKBState.GetPressedKeys().Length == 1 && Game1.options.doesInputListContain(Game1.options.runButton, Game1.oldKBState.GetPressedKeys()[0])) || !Game1.player.movedDuringLastTick()) && !Game1.player.UsingTool)
			{
				Game1.player.Halt();
			}
			if (this.timerToStart > 0)
			{
				this.timerToStart -= time.ElapsedGameTime.Milliseconds;
				if (this.timerToStart <= 0)
				{
					Game1.playSound("whistle");
					Game1.changeMusicTrack("tickTock");
				}
			}
			else if (this.showResultsTimer >= 0)
			{
				int arg_115_0 = this.showResultsTimer;
				this.showResultsTimer -= time.ElapsedGameTime.Milliseconds;
				if (arg_115_0 > 16000 && this.showResultsTimer <= 16000)
				{
					Game1.playSound("smallSelect");
				}
				if (arg_115_0 > 14000 && this.showResultsTimer <= 14000)
				{
					Game1.playSound("smallSelect");
					TargetGame.accuracy = (int)(Math.Round((double)((float)TargetGame.successShots / (float)(TargetGame.shotsFired - 1)), 2) * 100.0);
				}
				if (arg_115_0 > 11000 && this.showResultsTimer <= 11000)
				{
					if (TargetGame.accuracy >= 75)
					{
						Game1.playSound("newArtifact");
						float num = 1.5f;
						if (TargetGame.accuracy >= 85)
						{
							num = 2f;
						}
						if (TargetGame.accuracy >= 90)
						{
							num = 2.5f;
						}
						if (TargetGame.accuracy >= 95)
						{
							num = 3f;
						}
						if (TargetGame.accuracy >= 100)
						{
							num = 4f;
						}
						TargetGame.score = (int)((float)TargetGame.score * num);
						this.modifierBonus = num;
					}
					else
					{
						Game1.playSound("smallSelect");
					}
				}
				if (arg_115_0 > 9000 && this.showResultsTimer <= 9000)
				{
					TargetGame.score *= 2;
					if (TargetGame.score >= 80)
					{
						Game1.playSound("reward");
						TargetGame.starTokensWon = (int)((float)((TargetGame.score - 30) / 10) * 2.5f);
						if (TargetGame.starTokensWon > 140)
						{
							TargetGame.starTokensWon = 250;
						}
						Game1.player.festivalScore += TargetGame.starTokensWon;
					}
					else
					{
						Game1.playSound("fishEscape");
					}
				}
				if (this.showResultsTimer <= 0)
				{
					Game1.globalFadeToClear(null, 0.02f);
					Game1.player.position = new Vector2(24f, 63f) * (float)Game1.tileSize;
					return true;
				}
			}
			else if (!this.gameDone)
			{
				this.gameEndTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.gameEndTimer <= 0)
				{
					Game1.playSound("whistle");
					this.gameEndTimer = 1000;
					Game1.player.completelyStopAnimatingOrDoingAction();
					Game1.player.canMove = false;
					this.gameDone = true;
				}
				for (int i = this.targets.Count - 1; i >= 0; i--)
				{
					if (this.targets[i].update(time, this.location))
					{
						this.targets.RemoveAt(i);
					}
				}
			}
			else if (this.gameDone && this.gameEndTimer > 0)
			{
				this.gameEndTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.gameEndTimer <= 0)
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.gameDoneAfterFade), 0.01f);
					Game1.player.forceCanMove();
				}
			}
			return this.exit;
		}

		public void gameDoneAfterFade()
		{
			this.showResultsTimer = 16100;
			Game1.player.canMove = false;
			Game1.player.freezePause = 16100;
			Game1.player.position = new Vector2(24f, 63f) * (float)Game1.tileSize;
			Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle(24 * Game1.tileSize, 63 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			Game1.player.faceDirection(2);
		}

		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.showResultsTimer < 0)
			{
				Game1.pressUseToolButton();
				return;
			}
			if (this.showResultsTimer > 16000)
			{
				this.showResultsTimer = 16001;
				return;
			}
			if (this.showResultsTimer > 14000)
			{
				this.showResultsTimer = 14001;
				return;
			}
			if (this.showResultsTimer > 11000)
			{
				this.showResultsTimer = 11001;
				return;
			}
			if (this.showResultsTimer > 9000)
			{
				this.showResultsTimer = 9001;
				return;
			}
			if (this.showResultsTimer < 9000 && this.showResultsTimer > 1000)
			{
				this.showResultsTimer = 1500;
				Game1.player.freezePause = 1500;
				Game1.playSound("smallSelect");
			}
		}

		public void leftClickHeld(int x, int y)
		{
		}

		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public void releaseLeftClick(int x, int y)
		{
			int count = this.location.projectiles.Count;
			if (this.showResultsTimer < 0 && Game1.player.CurrentTool != null && Game1.player.CurrentTool.onRelease(this.location, x, y, Game1.player))
			{
				Game1.player.usingSlingshot = false;
				Game1.player.canReleaseTool = true;
				Game1.player.usingTool = false;
				Game1.player.CanMove = true;
				if (this.location.projectiles.Count > count)
				{
					TargetGame.shotsFired++;
				}
			}
		}

		public void releaseRightClick(int x, int y)
		{
		}

		public void receiveKeyPress(Keys k)
		{
			if (this.showResultsTimer > 0 || this.gameEndTimer > 0)
			{
				Game1.player.Halt();
				return;
			}
			if (Game1.player.movementDirections.Count < 2 && !Game1.player.UsingTool && this.timerToStart <= 0)
			{
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
				{
					Game1.player.setMoving(1);
				}
				if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
				{
					Game1.player.setMoving(2);
				}
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
				{
					Game1.player.setMoving(4);
				}
				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
				{
					Game1.player.setMoving(8);
				}
			}
			if (Game1.options.doesInputListContain(Game1.options.runButton, k))
			{
				Game1.player.setRunning(true, false);
			}
		}

		public void receiveKeyRelease(Keys k)
		{
			if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
			{
				Game1.player.setMoving(33);
			}
			if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
			{
				Game1.player.setMoving(34);
			}
			if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
			{
				Game1.player.setMoving(36);
			}
			if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
			{
				Game1.player.setMoving(40);
			}
			if (Game1.options.doesInputListContain(Game1.options.runButton, k))
			{
				Game1.player.setRunning(false, false);
			}
		}

		public void draw(SpriteBatch b)
		{
			if (this.showResultsTimer < 0)
			{
				b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				Game1.mapDisplayDevice.BeginScene(b);
				this.location.Map.GetLayer("Back").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(32f, 24f)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f - ((Game1.player.running || Game1.player.usingTool) ? ((float)Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.8f) : 0f), SpriteEffects.None, Math.Max(0f, (float)Game1.player.getStandingY() / 10000f + 0.00011f) - 1E-07f);
				this.location.Map.GetLayer("Buildings").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				Game1.mapDisplayDevice.EndScene();
				b.End();
				b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				this.location.draw(b);
				Game1.player.draw(b);
				if (!Game1.options.hardwareCursor)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
				}
				using (List<TargetGame.Target>.Enumerator enumerator = this.targets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
				b.End();
				b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				Game1.mapDisplayDevice.BeginScene(b);
				this.location.Map.GetLayer("Front").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				Game1.mapDisplayDevice.EndScene();
				if (!Game1.options.hardwareCursor)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
				}
				Game1.player.CurrentTool.draw(b);
				Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.10444", new object[]
				{
					TargetGame.score
				}), Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
				Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1514", new object[]
				{
					this.gameEndTimer / 1000
				}), Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize));
				if (TargetGame.shotsFired > 1)
				{
					Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:TargetGame.cs.12154", new object[]
					{
						(int)(Math.Round((double)((float)TargetGame.successShots / (float)(TargetGame.shotsFired - 1)), 2) * 100.0)
					}), Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 2)));
				}
				b.End();
				return;
			}
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			Vector2 vector = new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 2), (float)(Game1.viewport.Height / 2 - Game1.tileSize));
			if (this.showResultsTimer <= 16000)
			{
				Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.10444", new object[]
				{
					TargetGame.score
				}), Game1.textColor, (this.showResultsTimer <= 11000 && this.modifierBonus > 1f) ? Color.Lime : Color.White, vector);
			}
			if (this.showResultsTimer <= 14000)
			{
				vector.Y += (float)(Game1.tileSize * 3 / 4);
				Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:TargetGame.cs.12157", new object[]
				{
					TargetGame.accuracy,
					TargetGame.successShots,
					TargetGame.shotsFired
				}), Game1.textColor, Color.White, vector);
			}
			if (this.showResultsTimer <= 11000)
			{
				vector.Y += (float)(Game1.tileSize * 3 / 4);
				if (this.modifierBonus > 1f)
				{
					Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:TargetGame.cs.12161", new object[]
					{
						this.modifierBonus
					}), Game1.textColor, Color.Yellow, vector);
				}
				else
				{
					Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:TargetGame.cs.12163", new object[0]), Game1.textColor, Color.Red, vector);
				}
			}
			if (this.showResultsTimer <= 9000)
			{
				vector.Y += (float)Game1.tileSize;
				if (TargetGame.starTokensWon > 0)
				{
					float scale = Math.Min(1f, (float)(this.showResultsTimer - 2000) / 4000f);
					Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12013", new object[]
					{
						TargetGame.starTokensWon
					}), Game1.textColor * 0.2f * scale, Color.SkyBlue * 0.3f * scale, vector + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) * (float)Game1.pixelZoom * 2f, 0f, 1f, 1f);
					Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12013", new object[]
					{
						TargetGame.starTokensWon
					}), Game1.textColor * 0.2f * scale, Color.SkyBlue * 0.3f * scale, vector + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) * (float)Game1.pixelZoom * 2f, 0f, 1f, 1f);
					Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12013", new object[]
					{
						TargetGame.starTokensWon
					}), Game1.textColor * 0.2f * scale, Color.SkyBlue * 0.3f * scale, vector + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) * (float)Game1.pixelZoom * 2f, 0f, 1f, 1f);
					Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12013", new object[]
					{
						TargetGame.starTokensWon
					}), Game1.textColor, Color.SkyBlue, vector, 0f, 1f, 1f);
				}
				else
				{
					Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12021", new object[0]), Game1.textColor, Color.Red, vector);
				}
			}
			if (this.showResultsTimer <= 1000)
			{
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * (1f - (float)this.showResultsTimer / 1000f));
			}
			b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, Game1.tileSize * 2 + ((Game1.player.festivalScore > 999) ? (Game1.tileSize / 4) : 0), Game1.tileSize), Color.Black * 0.75f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(338, 400, 8, 8)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			Game1.drawWithBorder(string.Concat(Game1.player.festivalScore), Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2 + 10 * Game1.pixelZoom), (float)(Game1.tileSize / 3 + Game1.pixelZoom * 2)), 0f, 1f, 1f, false);
			b.End();
		}

		public static void startMe()
		{
			Game1.currentMinigame = new TargetGame();
			Game1.changeMusicTrack("none");
		}

		public void changeScreenSize()
		{
			Game1.viewport.X = this.location.Map.Layers[0].LayerWidth * Game1.tileSize / 2 - Game1.viewport.Width / 2;
			Game1.viewport.Y = this.location.Map.Layers[0].LayerHeight * Game1.tileSize / 2 - Game1.viewport.Height / 2;
		}

		public void unload()
		{
			Game1.player.addItemToInventory(this.tempItemStash, 0);
			Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
			Game1.player.forceCanMove();
			this.content.Unload();
			this.content.Dispose();
			this.content = null;
			Game1.changeMusicTrack("fallFest");
		}

		public void addTargets()
		{
			this.addRowOfTargetsOnLane(0, TargetGame.Target.middleLane, 1500, 5, TargetGame.Target.mediumSpeed, false, 0);
			this.addRowOfTargetsOnLane(4000, TargetGame.Target.nearLane, 1000, 5, TargetGame.Target.mediumSpeed, true, 0);
			this.addRowOfTargetsOnLane(8000, TargetGame.Target.farLane, 2000, 5, TargetGame.Target.mediumSpeed, false, TargetGame.Target.bonusTarget);
			this.addTwinPausers(8000, TargetGame.Target.superNearLane, TargetGame.Target.pauseMiddleLeft, TargetGame.Target.fastSpeed, 2000, TargetGame.Target.bonusTarget);
			this.addTwinPausers(15000, TargetGame.Target.superNearLane, TargetGame.Target.pauseFarLeft, TargetGame.Target.mediumSpeed, 4000, TargetGame.Target.bonusTarget);
			this.addRowOfTargetsOnLane(18000, TargetGame.Target.middleLane, 1500, 5, TargetGame.Target.mediumSpeed, false, 0);
			this.addRowOfTargetsOnLane(21000, TargetGame.Target.nearLane, 1000, 5, TargetGame.Target.mediumSpeed, true, 0);
			this.addTwinPausers(25000, TargetGame.Target.behindLane, TargetGame.Target.pauseFarLeft, TargetGame.Target.fastSpeed, 1500, TargetGame.Target.deluxeTarget);
			this.addRowOfTargetsOnLane(27000, TargetGame.Target.superNearLane, 500, 8, TargetGame.Target.slowSpeed, true, 0);
			this.addRowOfTargetsOnLane(28000, TargetGame.Target.nearLane, 500, 8, TargetGame.Target.slowSpeed, true, 0);
			this.addRowOfTargetsOnLane(29000, TargetGame.Target.middleLane, 500, 8, TargetGame.Target.slowSpeed, true, 0);
			this.addRowOfTargetsOnLane(30000, TargetGame.Target.farLane, 500, 8, TargetGame.Target.slowSpeed, true, 0);
			this.addTwinPausers(36000, TargetGame.Target.behindLane, TargetGame.Target.pauseFarLeft, TargetGame.Target.fastSpeed, 2000, TargetGame.Target.deluxeTarget);
			this.addRowOfTargetsOnLane(41000, TargetGame.Target.middleLane, 1500, 5, TargetGame.Target.mediumSpeed, false, 0);
			this.addRowOfTargetsOnLane(42000, TargetGame.Target.nearLane, 1000, 5, TargetGame.Target.mediumSpeed, true, 0);
			this.addRowOfTargetsOnLane(43000, TargetGame.Target.farLane, 1000, 4, TargetGame.Target.mediumSpeed, false, 0);
		}

		private void addTwinPausers(int initialDelay, int whichLane, int pauseArea, int speed, int pauseTime, int targetType)
		{
			int pauseAndReturn = -1;
			bool flag = false;
			if (pauseArea == TargetGame.Target.pauseFarLeft)
			{
				pauseAndReturn = TargetGame.Target.pauseFarRight;
				flag = true;
			}
			if (pauseArea == TargetGame.Target.pauseLeft)
			{
				pauseAndReturn = TargetGame.Target.pauseRight;
				flag = true;
			}
			if (pauseArea == TargetGame.Target.pauseMiddleLeft)
			{
				pauseAndReturn = TargetGame.Target.pauseMiddleRight;
				flag = true;
			}
			if (pauseArea == TargetGame.Target.pauseMiddleRight)
			{
				pauseAndReturn = TargetGame.Target.pauseMiddleLeft;
			}
			if (pauseArea == TargetGame.Target.pauseRight)
			{
				pauseAndReturn = TargetGame.Target.pauseLeft;
			}
			if (pauseArea == TargetGame.Target.pauseFarRight)
			{
				pauseAndReturn = TargetGame.Target.pauseFarLeft;
			}
			this.targets.Add(new TargetGame.Target(initialDelay, whichLane, targetType, speed, !flag, pauseArea, pauseTime));
			this.targets.Add(new TargetGame.Target(initialDelay, whichLane, targetType, speed, flag, pauseAndReturn, pauseTime));
		}

		private void addRowOfTargetsOnLane(int initialDelayBeforeStarting, int whichLane, int delayBetween, int numberOfTargets, int speed, bool spawnFromRight = true, int targetType = 0)
		{
			for (int i = 0; i < numberOfTargets; i++)
			{
				this.targets.Add(new TargetGame.Target(initialDelayBeforeStarting + i * delayBetween, whichLane, targetType, speed, spawnFromRight, -1, -1));
			}
		}

		public void receiveEventPoke(int data)
		{
		}

		public string minigameId()
		{
			return "TargetGame";
		}
	}
}
