using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewValley.Locations
{
	public class Bus : GameLocation
	{
		private bool talkedToKid;

		private bool talkedToWoman;

		private bool talkedToMan;

		private bool foundChange;

		private bool talkedToOldLady;

		private bool talkedToHaley;

		private int timesBagAttempt;

		public Bus()
		{
		}

		public Bus(Map m, string name) : base(m, name)
		{
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
			if (tile != null && Game1.year == 1 && Game1.dayOfMonth == 0 && who.IsMainPlayer)
			{
				int tileIndex = tile.TileIndex;
				if (tileIndex <= 238)
				{
					if (tileIndex <= 225)
					{
						if (tileIndex != 221)
						{
							if (tileIndex == 225)
							{
								if (!this.talkedToHaley)
								{
									Game1.drawDialogue(Game1.getCharacterFromName("Haley", false), Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7152", new object[0])));
								}
								else
								{
									Game1.drawDialogue(Game1.getCharacterFromName("Haley", false), Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7154", new object[0])));
								}
								this.talkedToHaley = true;
							}
						}
						else
						{
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7130", new object[0]));
						}
					}
					else
					{
						switch (tileIndex)
						{
						case 229:
							if (!this.talkedToMan)
							{
								Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7134", new object[0])));
							}
							break;
						case 230:
						case 231:
							break;
						case 232:
							Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7135", new object[0])));
							break;
						case 233:
							if (!this.talkedToOldLady)
							{
								Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7136", new object[0])));
							}
							else
							{
								Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7137", new object[0])));
							}
							this.talkedToOldLady = true;
							break;
						default:
							if (tileIndex != 236)
							{
								if (tileIndex == 238)
								{
									if (this.talkedToHaley)
									{
										this.busEvent();
									}
									else
									{
										Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7150", new object[0])));
									}
								}
							}
							else
							{
								switch (this.timesBagAttempt)
								{
								case 0:
									Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7140", new object[0])));
									goto IL_4D6;
								case 1:
									Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7141", new object[0])));
									goto IL_4D6;
								case 2:
									Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7142", new object[0])));
									goto IL_4D6;
								case 3:
									Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7143", new object[0])));
									goto IL_4D6;
								case 4:
									Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7144", new object[0])));
									goto IL_4D6;
								case 5:
									Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7145", new object[0])));
									goto IL_4D6;
								case 10:
									Game1.multipleDialogues(new string[]
									{
										Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7146", new object[0])),
										Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7147", new object[0]))
									});
									Game1.player.money++;
									goto IL_4D6;
								}
								Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7148", new object[0])));
								IL_4D6:
								this.timesBagAttempt++;
							}
							break;
						}
					}
				}
				else if (tileIndex <= 270)
				{
					if (tileIndex != 265)
					{
						if (tileIndex != 266)
						{
							if (tileIndex == 270)
							{
								if (!this.talkedToWoman)
								{
									Game1.multipleDialogues(new string[]
									{
										Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7131", new object[0])),
										Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7132", new object[0]))
									});
								}
								else
								{
									Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7133", new object[0])));
								}
								this.talkedToWoman = true;
							}
						}
						else if (Game1.player.isMale)
						{
							Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7129", new object[0])));
						}
					}
					else
					{
						Game1.drawObjectDialogue((Game1.player.isMale || this.talkedToKid) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7127", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7128", new object[0]));
						this.talkedToKid = true;
					}
				}
				else if (tileIndex != 274)
				{
					if (tileIndex != 278)
					{
						if (tileIndex == 459)
						{
							Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7149", new object[0])));
						}
					}
					else
					{
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7139", new object[0]));
					}
				}
				else if (!this.foundChange)
				{
					Game1.player.money += 20;
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7138", new object[0])));
				}
				else
				{
					this.foundChange = true;
				}
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public void busEvent()
		{
			this.characters.Add(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Dobson"), 0, Game1.tileSize, Game1.tileSize * 2), new Vector2((float)(-1000 * Game1.tileSize), (float)(3 * Game1.tileSize - Game1.tileSize)), "Bus", 0, "Dobson", false, null, Game1.content.Load<Texture2D>("Portraits\\Dobson")));
			this.currentEvent = new Event(string.Concat(new string[]
			{
				"none/10 4/farmer 18 5 0 Dobson -100 -100 2/message \"",
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7156", new object[0]),
				"\"/pause 500/faceDirection farmer 3/pause 1000/playSound doorClose/warp Dobson 1 4/pause 500/speak Dobson \"",
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7157", new object[0]),
				"\"/move Dobson 3 0 1/speak Dobson \"",
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7158", new object[0]),
				"\"/move Dobson 5 0 1/pause 500/faceDirection Dobson 0/speak Dobson \"",
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7159", new object[0]),
				"\"/pause 500/faceDirection Dobson 3/speak Dobson \"",
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7160", new object[0]),
				"\"/pause 800/move Dobson 2 0 0/pause 800/showFrame Dobson 16/pause 800/speak Dobson \"",
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7161", new object[0]),
				"\"/showFrame Dobson 8/pause 400/move Dobson 5 0 1/pause 1000/showFrame Dobson 17/pause 1000/showFrame Dobson 4/pause 1000/faceDirection Dobson 3/speak Dobson \"",
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7162", new object[0]),
				"\"/message \"",
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7163", new object[0]),
				"\"/pause 800/changeMapTile Buildings 9 1 119/changeMapTile Buildings 9 2 141/pause 400/changeMapTile Buildings 9 1 185/changeMapTile Buildings 9 2 207/pause 400/changeMapTile Buildings 9 1 119/changeMapTile Buildings 9 2 141/pause 300/changeMapTile Buildings 9 1 119/changeMapTile Buildings 9 2 141/pause 400/changeMapTile Buildings 9 1 185/changeMapTile Buildings 9 2 207/pause 400/changeMapTile Buildings 9 1 119/changeMapTile Buildings 9 2 141/pause 300/message \"",
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Bus.cs.7164", new object[0]),
				"\"/pause 400/faceDirection farmer 0/pause 500/faceDirection Dobson 0/pause 1000/end busIntro"
			}), -1);
			Game1.eventUp = true;
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			this.map.Update((long)time.ElapsedGameTime.Milliseconds);
			if (this.currentEvent != null)
			{
				this.currentEvent.checkForNextCommand(this, time);
			}
		}
	}
}
