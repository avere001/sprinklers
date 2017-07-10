using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;
using System.Collections.Generic;

namespace StardewValley
{
	public class Fence : Object
	{
		public const int debrisPieces = 4;

		public static int fencePieceWidth = Game1.tileSize / Game1.pixelZoom;

		public static int fencePieceHeight = Game1.tileSize * 2 / Game1.pixelZoom;

		public const int gateClosedPosition = 0;

		public const int gateOpenedPosition = 88;

		public const int sourceRectForSoloGate = 17;

		public const int globalHealthMultiplier = 2;

		public const int N = 1000;

		public const int E = 100;

		public const int S = 500;

		public const int W = 10;

		public new const int wood = 1;

		public new const int stone = 2;

		public const int steel = 3;

		public const int gate = 4;

		public new const int gold = 5;

		private Texture2D fenceTexture;

		public new float health;

		public float maxHealth;

		public int whichType;

		public int gatePosition;

		public int gateMotion;

		public static Dictionary<int, int> fenceDrawGuide;

		public bool isGate;

		public Fence(Vector2 tileLocation, int whichType, bool isGate)
		{
			this.whichType = whichType;
			switch (whichType)
			{
			case 1:
				this.health = 28f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Wood Fence";
				this.parentSheetIndex = -5;
				break;
			case 2:
				this.health = 60f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Stone Fence";
				this.parentSheetIndex = -6;
				break;
			case 3:
				this.health = 125f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Iron Fence";
				this.parentSheetIndex = -7;
				break;
			case 4:
				this.health = 100f;
				this.name = "Gate";
				this.parentSheetIndex = -9;
				break;
			case 5:
				this.health = 280f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Hardwood Fence";
				this.parentSheetIndex = -8;
				break;
			}
			this.health *= 2f;
			this.maxHealth = this.health;
			this.price = whichType;
			this.isGate = isGate;
			this.reloadSprite();
			if (Fence.fenceDrawGuide == null)
			{
				Fence.populateFenceDrawGuide();
			}
			this.tileLocation = tileLocation;
			this.canBeSetDown = true;
			this.canBeGrabbed = true;
			this.price = 1;
			if (isGate)
			{
				this.health *= 2f;
			}
			base.Type = "Crafting";
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		public Fence()
		{
			if (Fence.fenceDrawGuide == null)
			{
				Fence.populateFenceDrawGuide();
			}
			this.price = 1;
		}

		public void repair()
		{
			switch (this.whichType)
			{
			case 1:
				this.health = 28f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Wood Fence";
				this.parentSheetIndex = -5;
				return;
			case 2:
				this.health = 60f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Stone Fence";
				this.parentSheetIndex = -6;
				return;
			case 3:
				this.health = 125f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Iron Fence";
				this.parentSheetIndex = -7;
				return;
			case 4:
				this.health = 100f;
				this.name = "Gate";
				this.parentSheetIndex = -9;
				return;
			case 5:
				this.health = 280f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Hardwood Fence";
				this.parentSheetIndex = -8;
				return;
			default:
				return;
			}
		}

		public static void populateFenceDrawGuide()
		{
			Fence.fenceDrawGuide = new Dictionary<int, int>();
			Fence.fenceDrawGuide.Add(0, 5);
			Fence.fenceDrawGuide.Add(10, 9);
			Fence.fenceDrawGuide.Add(100, 10);
			Fence.fenceDrawGuide.Add(1000, 3);
			Fence.fenceDrawGuide.Add(500, 5);
			Fence.fenceDrawGuide.Add(1010, 8);
			Fence.fenceDrawGuide.Add(1100, 6);
			Fence.fenceDrawGuide.Add(1500, 3);
			Fence.fenceDrawGuide.Add(600, 0);
			Fence.fenceDrawGuide.Add(510, 2);
			Fence.fenceDrawGuide.Add(110, 7);
			Fence.fenceDrawGuide.Add(1600, 0);
			Fence.fenceDrawGuide.Add(1610, 4);
			Fence.fenceDrawGuide.Add(1510, 2);
			Fence.fenceDrawGuide.Add(1110, 7);
			Fence.fenceDrawGuide.Add(610, 4);
		}

		public override void updateWhenCurrentLocation(GameTime time)
		{
			this.gatePosition += this.gateMotion;
			if (this.gatePosition >= 88 || this.gatePosition <= 0)
			{
				this.gateMotion = 0;
			}
			if (this.heldObject != null)
			{
				this.heldObject.updateWhenCurrentLocation(time);
			}
		}

		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (!justCheckingForActivity && who != null && who.currentLocation.objects.ContainsKey(new Vector2((float)who.getTileX(), (float)(who.getTileY() - 1))) && who.currentLocation.objects.ContainsKey(new Vector2((float)who.getTileX(), (float)(who.getTileY() + 1))) && who.currentLocation.objects.ContainsKey(new Vector2((float)(who.getTileX() + 1), (float)who.getTileY())) && who.currentLocation.objects.ContainsKey(new Vector2((float)(who.getTileX() - 1), (float)who.getTileY())))
			{
				this.performToolAction(null);
			}
			if (this.health <= 1f)
			{
				return false;
			}
			if (this.isGate)
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				int num = 0;
				Vector2 tileLocation = this.tileLocation;
				tileLocation.X += 1f;
				if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
				{
					num += 100;
				}
				tileLocation.X -= 2f;
				if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
				{
					num += 10;
				}
				tileLocation.X += 1f;
				tileLocation.Y += 1f;
				if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
				{
					num += 500;
				}
				tileLocation.Y -= 2f;
				if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
				{
					num += 1000;
				}
				if (this.isGate)
				{
					if (num == 110 || num == 1500)
					{
						who.temporaryImpassableTile = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
						if (this.gatePosition == 0)
						{
							this.gatePosition = 88;
						}
						else
						{
							this.gatePosition = 0;
						}
						Game1.playSound("doorClose");
					}
					else
					{
						who.temporaryImpassableTile = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
						this.gatePosition = 0;
					}
				}
				return true;
			}
			else
			{
				if (justCheckingForActivity)
				{
					return false;
				}
				foreach (Vector2 current in Utility.getAdjacentTileLocations(this.tileLocation))
				{
					if (Game1.currentLocation.objects.ContainsKey(current) && Game1.currentLocation.objects[current].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[current]).isGate)
					{
						((Fence)Game1.currentLocation.objects[current]).checkForAction(who, false);
						return true;
					}
				}
				return this.health <= 0f;
			}
		}

		public override bool performToolAction(Tool t)
		{
			if (this.heldObject != null && t != null && !(t is MeleeWeapon) && t.isHeavyHitter())
			{
				this.heldObject.performRemoveAction(this.tileLocation, Game1.currentLocation);
				this.heldObject = null;
				Game1.playSound("axchop");
				return false;
			}
			if (this.isGate && t != null && (t.GetType() == typeof(Axe) || t is Pickaxe))
			{
				Game1.playSound("axchop");
				Game1.createObjectDebris(325, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.player.uniqueMultiplayerID, Game1.player.currentLocation);
				Game1.currentLocation.objects.Remove(this.tileLocation);
				Game1.createRadialDebris(Game1.currentLocation, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, 6, false, -1, false, -1);
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
			}
			if ((this.whichType == 1 || this.whichType == 5) && (t == null || t.GetType() == typeof(Axe)))
			{
				Game1.playSound("axchop");
				Game1.currentLocation.objects.Remove(this.tileLocation);
				for (int i = 0; i < 4; i++)
				{
					Game1.currentLocation.temporarySprites.Add(new CosmeticDebris(this.fenceTexture, new Vector2(this.tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), this.tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)), (float)Game1.random.Next(-5, 5) / 100f, (float)Game1.random.Next(-Game1.tileSize, Game1.tileSize) / 30f, (float)Game1.random.Next(-800, -100) / 100f, (int)((this.tileLocation.Y + 1f) * (float)Game1.tileSize), new Rectangle(32 + Game1.random.Next(2) * 16 / 2, 96 + Game1.random.Next(2) * 16 / 2, 8, 8), Color.White, (Game1.soundBank != null) ? Game1.soundBank.GetCue("shiny4") : null, null, 0, 200));
				}
				Game1.createRadialDebris(Game1.currentLocation, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, 6, false, -1, false, -1);
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
				if (this.maxHealth - this.health < 0.5f)
				{
					int num = this.whichType;
					if (num != 1)
					{
						if (num == 5)
						{
							Game1.currentLocation.debris.Add(new Debris(new Object(298, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
						}
					}
					else
					{
						Game1.currentLocation.debris.Add(new Debris(new Object(322, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
					}
				}
			}
			else if ((this.whichType == 2 || this.whichType == 3) && (t == null || t.GetType() == typeof(Pickaxe)))
			{
				Game1.playSound("hammer");
				Game1.currentLocation.objects.Remove(this.tileLocation);
				for (int j = 0; j < 4; j++)
				{
					Game1.currentLocation.temporarySprites.Add(new CosmeticDebris(this.fenceTexture, new Vector2(this.tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), this.tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)), (float)Game1.random.Next(-5, 5) / 100f, (float)Game1.random.Next(-Game1.tileSize, Game1.tileSize) / 30f, (float)Game1.random.Next(-800, -100) / 100f, (int)((this.tileLocation.Y + 1f) * (float)Game1.tileSize), new Rectangle(32 + Game1.random.Next(2) * 16 / 2, 96 + Game1.random.Next(2) * 16 / 2, 8, 8), Color.White, (Game1.soundBank != null) ? Game1.soundBank.GetCue("shiny4") : null, null, 0, 200));
				}
				Game1.createRadialDebris(Game1.currentLocation, 14, (int)this.tileLocation.X, (int)this.tileLocation.Y, 6, false, -1, false, -1);
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
				if (this.maxHealth - this.health < 0.5f)
				{
					int num = this.whichType;
					if (num != 2)
					{
						if (num == 3)
						{
							Game1.currentLocation.debris.Add(new Debris(new Object(324, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
						}
					}
					else
					{
						Game1.currentLocation.debris.Add(new Debris(new Object(323, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
					}
				}
			}
			return false;
		}

		public override bool minutesElapsed(int minutes, GameLocation l)
		{
			if (!Game1.getFarm().isBuildingConstructed("Gold Clock"))
			{
				this.health -= (float)minutes / 1440f;
				if (this.health <= -1f && (Game1.timeOfDay <= 610 || Game1.timeOfDay > 1800))
				{
					return true;
				}
			}
			return false;
		}

		public override void actionOnPlayerEntry()
		{
			base.actionOnPlayerEntry();
			if (this.heldObject != null)
			{
				this.heldObject.actionOnPlayerEntry();
				this.heldObject.isOn = true;
				this.heldObject.initializeLightSource(this.tileLocation);
			}
		}

		public override bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
		{
			if (dropIn.parentSheetIndex == 325)
			{
				if (probe)
				{
					return false;
				}
				if (!this.isGate)
				{
					Vector2 tileLocation = this.tileLocation;
					int num = 0;
					tileLocation.X += 1f;
					if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
					{
						num += 100;
					}
					tileLocation.X -= 2f;
					if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
					{
						num += 10;
					}
					tileLocation.X += 1f;
					tileLocation.Y += 1f;
					if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
					{
						num += 500;
					}
					tileLocation.Y -= 2f;
					if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
					{
						num += 1000;
					}
					if (num == 1500 || num == 110)
					{
						this.isGate = true;
						Game1.playSound("axe");
						return true;
					}
				}
			}
			else if (dropIn.parentSheetIndex == 93 && this.heldObject == null && !probe && !this.isGate)
			{
				Game1.playSound("axe");
				this.heldObject = new Torch(this.tileLocation, 93);
				this.heldObject.name = "Torch";
				this.heldObject.initializeLightSource(this.tileLocation);
				return true;
			}
			if (this.health <= 1f && !probe)
			{
				int parentSheetIndex = dropIn.parentSheetIndex;
				if (parentSheetIndex != 298)
				{
					switch (parentSheetIndex)
					{
					case 322:
						if (this.whichType == 1)
						{
							this.health = 28f + (float)Game1.random.Next(-500, 500) / 100f;
							Game1.playSound("axe");
							return true;
						}
						break;
					case 323:
						if (this.whichType == 2)
						{
							this.health = 60f + (float)Game1.random.Next(-500, 600) / 100f;
							Game1.playSound("stoneStep");
							return true;
						}
						break;
					case 324:
						if (this.whichType == 3)
						{
							this.health = 125f + (float)Game1.random.Next(-500, 700) / 100f;
							Game1.playSound("hammer");
							return true;
						}
						break;
					}
				}
				else if (this.whichType == 5)
				{
					this.health = 280f + (float)Game1.random.Next(-2000, 2000) / 100f;
					Game1.playSound("axe");
					return true;
				}
			}
			return base.performObjectDropInAction(dropIn, probe, who);
		}

		public override bool performDropDownAction(Farmer who)
		{
			Vector2 tileLocation = new Vector2((float)((int)(Game1.player.GetDropLocation().X / (float)Game1.tileSize)), (float)((int)(Game1.player.GetDropLocation().Y / (float)Game1.tileSize)));
			this.tileLocation = tileLocation;
			return false;
		}

		public override void reloadSprite()
		{
			this.fenceTexture = Game1.content.Load<Texture2D>("LooseSprites\\Fence" + Math.Max(1, this.isGate ? 1 : this.whichType));
		}

		public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
			spriteBatch.Draw(this.fenceTexture, objectPosition - new Vector2(0f, (float)Game1.tileSize), new Rectangle?(new Rectangle(5 * Fence.fencePieceWidth % this.fenceTexture.Bounds.Width, 5 * Fence.fencePieceWidth / this.fenceTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)(f.getStandingY() + 1) / 10000f);
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scale, float transparency, float layerDepth, bool drawStackNumber)
		{
			location.Y -= (float)Game1.tileSize * scale;
			int num = 0;
			Vector2 tileLocation = this.tileLocation;
			tileLocation.X += 1f;
			if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
			{
				num += 100;
			}
			tileLocation.X -= 2f;
			if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
			{
				num += 10;
			}
			tileLocation.X += 1f;
			tileLocation.Y += 1f;
			if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
			{
				num += 500;
			}
			tileLocation.Y -= 2f;
			if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
			{
				num += 1000;
			}
			int tilePosition = Fence.fenceDrawGuide[num];
			if (this.isGate)
			{
				if (num == 110)
				{
					spriteBatch.Draw(this.fenceTexture, location + new Vector2(6f, 6f), new Rectangle?(new Rectangle(0, 512, 88, 24)), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					return;
				}
				if (num == 1500)
				{
					spriteBatch.Draw(this.fenceTexture, location + new Vector2(6f, 6f), new Rectangle?(new Rectangle(112, 512, 16, 64)), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					return;
				}
			}
			spriteBatch.Draw(this.fenceTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scale, new Rectangle?(Game1.getArbitrarySourceRect(this.fenceTexture, Game1.tileSize, Game1.tileSize * 2, tilePosition)), Color.White * transparency, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scale, scale, SpriteEffects.None, layerDepth);
		}

		public bool countsForDrawing(int type)
		{
			return this.health > 1f && !this.isGate && (type == this.whichType || type == 4);
		}

		public override bool isPassable()
		{
			return this.isGate && this.gatePosition >= 88;
		}

		public override void draw(SpriteBatch b, int x, int y, float alpha = 1f)
		{
			int num = 1;
			if (this.health > 1f)
			{
				int num2 = 0;
				Vector2 tileLocation = this.tileLocation;
				tileLocation.X += 1f;
				if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
				{
					num2 += 100;
				}
				tileLocation.X -= 2f;
				if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
				{
					num2 += 10;
				}
				tileLocation.X += 1f;
				tileLocation.Y += 1f;
				if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
				{
					num2 += 500;
				}
				tileLocation.Y -= 2f;
				if (Game1.currentLocation.objects.ContainsKey(tileLocation) && Game1.currentLocation.objects[tileLocation].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[tileLocation]).countsForDrawing(this.whichType))
				{
					num2 += 1000;
				}
				num = Fence.fenceDrawGuide[num2];
				if (this.isGate)
				{
					if (num2 == 110)
					{
						b.Draw(this.fenceTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize - 4 * Game1.pixelZoom), (float)(y * Game1.tileSize - Game1.tileSize))), new Rectangle?(new Rectangle((this.gatePosition == 88) ? 24 : 0, 128, 24, 32)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize / 2 + 1) / 10000f);
						return;
					}
					if (num2 == 1500)
					{
						b.Draw(this.fenceTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + 5 * Game1.pixelZoom), (float)(y * Game1.tileSize - Game1.tileSize - 6 * Game1.pixelZoom))), new Rectangle?(new Rectangle((this.gatePosition == 88) ? 16 : 0, 160, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize - Game1.tileSize / 2 + 1) / 10000f);
						b.Draw(this.fenceTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + 5 * Game1.pixelZoom), (float)(y * Game1.tileSize - Game1.tileSize + 10 * Game1.pixelZoom))), new Rectangle?(new Rectangle((this.gatePosition == 88) ? 16 : 0, 176, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize * 3 / 2 - 1) / 10000f);
						return;
					}
					num = 17;
				}
				else if (this.heldObject != null)
				{
					this.heldObject.draw(b, x * Game1.tileSize + ((num2 == 100) ? (-Game1.tileSize / 3) : 0) + ((num2 == 10) ? (Game1.tileSize / 3 - Game1.pixelZoom * 2) : 0), (y - 1) * Game1.tileSize - Game1.pixelZoom * 4 + ((this.whichType == 2) ? (Game1.tileSize / 4) : 0), (float)(y * Game1.tileSize + Game1.tileSize) / 10000f, 1f);
				}
			}
			b.Draw(this.fenceTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize))), new Rectangle?(new Rectangle(num * Fence.fencePieceWidth % this.fenceTexture.Bounds.Width, num * Fence.fencePieceWidth / this.fenceTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize / 2) / 10000f);
		}
	}
}
