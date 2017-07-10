using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class MapPage : IClickableMenu
	{
		public const int region_desert = 1001;

		public const int region_farm = 1002;

		public const int region_backwoods = 1003;

		public const int region_busstop = 1004;

		public const int region_wizardtower = 1005;

		public const int region_marnieranch = 1006;

		public const int region_leahcottage = 1007;

		public const int region_samhouse = 1008;

		public const int region_haleyhouse = 1009;

		public const int region_townsquare = 1010;

		public const int region_harveyclinic = 1011;

		public const int region_generalstore = 1012;

		public const int region_blacksmith = 1013;

		public const int region_saloon = 1014;

		public const int region_manor = 1015;

		public const int region_museum = 1016;

		public const int region_elliottcabin = 1017;

		public const int region_sewer = 1018;

		public const int region_graveyard = 1019;

		public const int region_trailer = 1020;

		public const int region_alexhouse = 1021;

		public const int region_sciencehouse = 1022;

		public const int region_tent = 1023;

		public const int region_mines = 1024;

		public const int region_adventureguild = 1025;

		public const int region_quarry = 1026;

		public const int region_jojamart = 1027;

		public const int region_fishshop = 1028;

		public const int region_spa = 1029;

		public const int region_secretwoods = 1030;

		public const int region_ruinedhouse = 1031;

		public const int region_communitycenter = 1032;

		public const int region_sewerpipe = 1033;

		public const int region_railroad = 1034;

		private string descriptionText = "";

		private string hoverText = "";

		private string playerLocationName;

		private Texture2D map;

		private int mapX;

		private int mapY;

		private Vector2 playerMapPosition;

		public List<ClickableComponent> points = new List<ClickableComponent>();

		public ClickableTextureComponent okButton;

		public MapPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.okButton = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11059", new object[0]), new Rectangle(this.xPositionOnScreen + width + Game1.tileSize, this.yPositionOnScreen + height - IClickableMenu.borderWidth - Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.map = Game1.content.Load<Texture2D>("LooseSprites\\map");
			Vector2 topLeftPositionForCenteringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(this.map.Bounds.Width * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0);
			this.mapX = (int)topLeftPositionForCenteringOnScreen.X;
			this.mapY = (int)topLeftPositionForCenteringOnScreen.Y;
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX, this.mapY, 292, 152), Game1.player.mailReceived.Contains("ccVault") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11062", new object[0]) : "???")
			{
				myID = 1001,
				rightNeighborID = 1003,
				downNeighborID = 1030
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 324, this.mapY + 252, 188, 132), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11064", new object[]
			{
				Game1.player.farmName
			}))
			{
				myID = 1002,
				leftNeighborID = 1005,
				upNeighborID = 1003,
				rightNeighborID = 1004,
				downNeighborID = 1006
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 360, this.mapY + 96, 188, 132), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11065", new object[0]))
			{
				myID = 1003,
				downNeighborID = 1002,
				leftNeighborID = 1001,
				rightNeighborID = 1022,
				upNeighborID = 1029
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 516, this.mapY + 224, 76, 100), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11066", new object[0]))
			{
				myID = 1004,
				leftNeighborID = 1002,
				upNeighborID = 1003,
				downNeighborID = 1006,
				rightNeighborID = 1011
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 196, this.mapY + 352, 36, 76), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11067", new object[0]))
			{
				myID = 1005,
				upNeighborID = 1001,
				downNeighborID = 1031,
				rightNeighborID = 1006,
				leftNeighborID = 1030
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 420, this.mapY + 392, 76, 40), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11068", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11069", new object[0]))
			{
				myID = 1006,
				leftNeighborID = 1005,
				downNeighborID = 1007,
				upNeighborID = 1002,
				rightNeighborID = 1008
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 452, this.mapY + 436, 32, 24), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11070", new object[0]))
			{
				myID = 1007,
				upNeighborID = 1006,
				downNeighborID = 1033,
				leftNeighborID = 1005,
				rightNeighborID = 1008
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 612, this.mapY + 396, 36, 52), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11071", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11072", new object[0]))
			{
				myID = 1008,
				leftNeighborID = 1006,
				upNeighborID = 1010,
				rightNeighborID = 1009
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 652, this.mapY + 408, 40, 36), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11073", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11074", new object[0]))
			{
				myID = 1009,
				leftNeighborID = 1008,
				upNeighborID = 1010,
				rightNeighborID = 1018
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 672, this.mapY + 340, 44, 60), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11075", new object[0]))
			{
				myID = 1010,
				leftNeighborID = 1008,
				downNeighborID = 1009,
				rightNeighborID = 1014,
				upNeighborID = 1011
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 680, this.mapY + 304, 16, 32), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11076", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11077", new object[0]))
			{
				myID = 1011,
				leftNeighborID = 1004,
				rightNeighborID = 1012,
				downNeighborID = 1010,
				upNeighborID = 1032
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 696, this.mapY + 296, 28, 40), string.Concat(new string[]
			{
				Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11078", new object[0]),
				Environment.NewLine,
				Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11079", new object[0]),
				Environment.NewLine,
				Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11080", new object[0])
			}))
			{
				myID = 1012,
				leftNeighborID = 1011,
				downNeighborID = 1014,
				rightNeighborID = 1021,
				upNeighborID = 1032
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 852, this.mapY + 388, 80, 36), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11081", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11082", new object[0]))
			{
				myID = 1013,
				upNeighborID = 1027,
				rightNeighborID = 1016,
				downNeighborID = 1017,
				leftNeighborID = 1015
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 716, this.mapY + 352, 28, 40), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11083", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11084", new object[0]))
			{
				myID = 1014,
				leftNeighborID = 1010,
				rightNeighborID = 1020,
				downNeighborID = 1019,
				upNeighborID = 1012
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 768, this.mapY + 388, 44, 56), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11085", new object[0]))
			{
				myID = 1015,
				leftNeighborID = 1019,
				upNeighborID = 1020,
				rightNeighborID = 1013,
				downNeighborID = 1017
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 892, this.mapY + 416, 32, 28), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11086", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11087", new object[0]))
			{
				myID = 1016,
				downNeighborID = 1017,
				leftNeighborID = 1013,
				upNeighborID = 1027,
				rightNeighborID = 99989
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 824, this.mapY + 564, 28, 20), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11088", new object[0]))
			{
				myID = 1017,
				downNeighborID = 1028,
				upNeighborID = 1015,
				rightNeighborID = 99989
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 696, this.mapY + 448, 24, 20), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11089", new object[0]))
			{
				myID = 1018,
				downNeighborID = 1017,
				rightNeighborID = 1019,
				upNeighborID = 1014,
				leftNeighborID = 1009
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 724, this.mapY + 424, 40, 32), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11090", new object[0]))
			{
				myID = 1019,
				leftNeighborID = 1018,
				upNeighborID = 1014,
				rightNeighborID = 1015,
				downNeighborID = 1017
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 780, this.mapY + 360, 24, 20), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11091", new object[0]))
			{
				myID = 1020,
				upNeighborID = 1021,
				leftNeighborID = 1014,
				downNeighborID = 1015,
				rightNeighborID = 1027
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 748, this.mapY + 316, 36, 36), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11092", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11093", new object[0]))
			{
				myID = 1021,
				rightNeighborID = 1027,
				downNeighborID = 1020,
				leftNeighborID = 1012,
				upNeighborID = 1032
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 732, this.mapY + 148, 48, 32), string.Concat(new string[]
			{
				Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11094", new object[0]),
				Environment.NewLine,
				Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11095", new object[0]),
				Environment.NewLine,
				Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11096", new object[0])
			}))
			{
				myID = 1022,
				downNeighborID = 1032,
				leftNeighborID = 1003,
				upNeighborID = 1034,
				rightNeighborID = 1023
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 784, this.mapY + 128, 12, 16), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11097", new object[0]))
			{
				myID = 1023,
				leftNeighborID = 1034,
				downNeighborID = 1022,
				rightNeighborID = 1024
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 880, this.mapY + 96, 16, 24), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11098", new object[0]))
			{
				myID = 1024,
				leftNeighborID = 1023,
				rightNeighborID = 1025,
				downNeighborID = 1027
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 900, this.mapY + 108, 32, 36), (Game1.stats.DaysPlayed >= 5u) ? (Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11099", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11100", new object[0])) : "???")
			{
				myID = 1025,
				leftNeighborID = 1024,
				rightNeighborID = 1026,
				downNeighborID = 1027
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 968, this.mapY + 116, 88, 76), Game1.player.mailReceived.Contains("ccCraftsRoom") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11103", new object[0]) : "???")
			{
				myID = 1026,
				leftNeighborID = 1025,
				downNeighborID = 1027
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 872, this.mapY + 280, 52, 52), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11105", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11106", new object[0]))
			{
				myID = 1027,
				upNeighborID = 1025,
				leftNeighborID = 1021,
				downNeighborID = 1013
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 844, this.mapY + 608, 36, 40), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11107", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11108", new object[0]))
			{
				myID = 1028,
				upNeighborID = 1017,
				rightNeighborID = 99989
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 576, this.mapY + 60, 48, 36), Game1.isLocationAccessible("Railroad") ? (Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11110", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11111", new object[0])) : "???")
			{
				myID = 1029,
				rightNeighborID = 1034,
				downNeighborID = 1003,
				leftNeighborID = 1001
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX, this.mapY + 272, 196, 176), Game1.player.mailReceived.Contains("beenToWoods") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11114", new object[0]) : "???")
			{
				myID = 1030,
				upNeighborID = 1001,
				rightNeighborID = 1005
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 260, this.mapY + 572, 20, 20), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11116", new object[0]))
			{
				myID = 1031,
				rightNeighborID = 1033,
				upNeighborID = 1005
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 692, this.mapY + 204, 44, 36), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11117", new object[0]))
			{
				myID = 1032,
				downNeighborID = 1012,
				upNeighborID = 1022,
				leftNeighborID = 1004
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 380, this.mapY + 596, 24, 32), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11118", new object[0]))
			{
				myID = 1033,
				leftNeighborID = 1031,
				rightNeighborID = 1017,
				upNeighborID = 1007
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 644, this.mapY + 64, 16, 8), Game1.isLocationAccessible("Railroad") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11119", new object[0]) : "???")
			{
				myID = 1034,
				leftNeighborID = 1029,
				rightNeighborID = 1023,
				downNeighborID = 1022
			});
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 728, this.mapY + 652, 28, 28), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11122", new object[0])));
			this.setUpPlayerMapPosition();
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(1002);
			this.snapCursorToCurrentSnappedComponent();
		}

		public void setUpPlayerMapPosition()
		{
			this.playerMapPosition = new Vector2(-999f, -999f);
			string text = Game1.player.currentLocation.Name;
			string name = Game1.player.currentLocation.Name;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 2026102357u)
			{
				if (num <= 1078463463u)
				{
					if (num <= 278567071u)
					{
						if (num != 144182059u)
						{
							if (num != 263498407u)
							{
								if (num != 278567071u)
								{
									goto IL_7C7;
								}
								if (!(name == "HarveyRoom"))
								{
									goto IL_7C7;
								}
							}
							else
							{
								if (!(name == "BathHouse_Pool"))
								{
									goto IL_7C7;
								}
								goto IL_64D;
							}
						}
						else
						{
							if (!(name == "WizardHouseBasement"))
							{
								goto IL_7C7;
							}
							goto IL_632;
						}
					}
					else if (num <= 746089795u)
					{
						if (num != 437214172u)
						{
							if (num != 746089795u)
							{
								goto IL_7C7;
							}
							if (!(name == "ScienceHouse"))
							{
								goto IL_7C7;
							}
							goto IL_6BD;
						}
						else
						{
							if (!(name == "Desert"))
							{
								goto IL_7C7;
							}
							goto IL_53E;
						}
					}
					else if (num != 807500499u)
					{
						if (num != 1078463463u)
						{
							goto IL_7C7;
						}
						if (!(name == "Temp"))
						{
							goto IL_7C7;
						}
						if (Game1.player.currentLocation.Map.Id.Contains("Town"))
						{
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
							goto IL_7C7;
						}
						goto IL_7C7;
					}
					else if (!(name == "Hospital"))
					{
						goto IL_7C7;
					}
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11076", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11077", new object[0]);
					goto IL_7C7;
				}
				if (num <= 1446049731u)
				{
					if (num <= 1253908523u)
					{
						if (num != 1167876998u)
						{
							if (num != 1253908523u)
							{
								goto IL_7C7;
							}
							if (!(name == "JoshHouse"))
							{
								goto IL_7C7;
							}
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11092", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11093", new object[0]);
							goto IL_7C7;
						}
						else
						{
							if (!(name == "ManorHouse"))
							{
								goto IL_7C7;
							}
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11085", new object[0]);
							goto IL_7C7;
						}
					}
					else if (num != 1428365440u)
					{
						if (num != 1446049731u)
						{
							goto IL_7C7;
						}
						if (!(name == "CommunityCenter"))
						{
							goto IL_7C7;
						}
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11117", new object[0]);
						goto IL_7C7;
					}
					else
					{
						if (!(name == "SeedShop"))
						{
							goto IL_7C7;
						}
						text = string.Concat(new string[]
						{
							Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11078", new object[0]),
							Environment.NewLine,
							Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11079", new object[0]),
							Environment.NewLine,
							Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11080", new object[0])
						});
						goto IL_7C7;
					}
				}
				else if (num <= 1840909614u)
				{
					if (num != 1807680626u)
					{
						if (num != 1840909614u)
						{
							goto IL_7C7;
						}
						if (!(name == "SandyHouse"))
						{
							goto IL_7C7;
						}
					}
					else if (!(name == "SandyShop"))
					{
						goto IL_7C7;
					}
				}
				else if (num != 1919215024u)
				{
					if (num != 2026102357u)
					{
						goto IL_7C7;
					}
					if (!(name == "UndergroundMine"))
					{
						goto IL_7C7;
					}
					goto IL_6A2;
				}
				else
				{
					if (!(name == "ElliottHouse"))
					{
						goto IL_7C7;
					}
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11088", new object[0]);
					goto IL_7C7;
				}
			}
			else
			{
				if (num > 3095702198u)
				{
					if (num <= 3848897750u)
					{
						if (num <= 3647688262u)
						{
							if (num != 3188665151u)
							{
								if (num != 3647688262u)
								{
									goto IL_7C7;
								}
								if (!(name == "BathHouse_WomensLocker"))
								{
									goto IL_7C7;
								}
								goto IL_64D;
							}
							else if (!(name == "Railroad"))
							{
								goto IL_7C7;
							}
						}
						else if (num != 3653788295u)
						{
							if (num != 3848897750u)
							{
								goto IL_7C7;
							}
							if (!(name == "Mine"))
							{
								goto IL_7C7;
							}
							goto IL_6A2;
						}
						else
						{
							if (!(name == "SkullCave"))
							{
								goto IL_7C7;
							}
							goto IL_53E;
						}
					}
					else if (num <= 3978811393u)
					{
						if (num != 3924195856u)
						{
							if (num != 3978811393u)
							{
								goto IL_7C7;
							}
							if (!(name == "AnimalShop"))
							{
								goto IL_7C7;
							}
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11068", new object[0]);
							goto IL_7C7;
						}
						else
						{
							if (!(name == "BathHouse_MensLocker"))
							{
								goto IL_7C7;
							}
							goto IL_64D;
						}
					}
					else if (num != 3979572909u)
					{
						if (num != 4002456539u)
						{
							goto IL_7C7;
						}
						if (!(name == "WitchWarpCave"))
						{
							goto IL_7C7;
						}
					}
					else
					{
						if (!(name == "Club"))
						{
							goto IL_7C7;
						}
						goto IL_53E;
					}
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11119", new object[0]);
					goto IL_7C7;
				}
				if (num <= 2706464810u)
				{
					if (num <= 2204429310u)
					{
						if (num != 2028543928u)
						{
							if (num != 2204429310u)
							{
								goto IL_7C7;
							}
							if (!(name == "SebastianRoom"))
							{
								goto IL_7C7;
							}
							goto IL_6BD;
						}
						else
						{
							if (!(name == "Backwoods"))
							{
								goto IL_7C7;
							}
							goto IL_7C7;
						}
					}
					else if (num != 2478616111u)
					{
						if (num != 2706464810u)
						{
							goto IL_7C7;
						}
						if (!(name == "WizardHouse"))
						{
							goto IL_7C7;
						}
						goto IL_632;
					}
					else
					{
						if (!(name == "BathHouse_Entry"))
						{
							goto IL_7C7;
						}
						goto IL_64D;
					}
				}
				else if (num <= 2844260897u)
				{
					if (num != 2708986271u)
					{
						if (num != 2844260897u)
						{
							goto IL_7C7;
						}
						if (!(name == "Woods"))
						{
							goto IL_7C7;
						}
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11114", new object[0]);
						goto IL_7C7;
					}
					else
					{
						if (!(name == "ArchaeologyHouse"))
						{
							goto IL_7C7;
						}
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11086", new object[0]);
						goto IL_7C7;
					}
				}
				else if (num != 3006626703u)
				{
					if (num != 3095702198u)
					{
						goto IL_7C7;
					}
					if (!(name == "AdventureGuild"))
					{
						goto IL_7C7;
					}
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11099", new object[0]);
					goto IL_7C7;
				}
				else
				{
					if (!(name == "FishShop"))
					{
						goto IL_7C7;
					}
					text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11107", new object[0]);
					goto IL_7C7;
				}
			}
			IL_53E:
			text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11062", new object[0]);
			goto IL_7C7;
			IL_632:
			text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11067", new object[0]);
			goto IL_7C7;
			IL_64D:
			text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11110", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11111", new object[0]);
			goto IL_7C7;
			IL_6A2:
			text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11098", new object[0]);
			goto IL_7C7;
			IL_6BD:
			text = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11094", new object[0]) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11095", new object[0]);
			IL_7C7:
			foreach (ClickableComponent current in this.points)
			{
				if (current.name.Equals(text) || current.name.Replace(" ", "").Equals(text) || (current.name.Contains(Environment.NewLine) && current.name.Substring(0, current.name.IndexOf(Environment.NewLine)).Equals(text.Substring(0, text.Contains(Environment.NewLine) ? text.IndexOf(Environment.NewLine) : text.Length))))
				{
					this.playerMapPosition = new Vector2((float)current.bounds.Center.X, (float)current.bounds.Center.Y);
					this.playerLocationName = (current.name.Contains(Environment.NewLine) ? current.name.Substring(0, current.name.IndexOf(Environment.NewLine)) : current.name);
					return;
				}
			}
			int tileX = Game1.player.getTileX();
			int tileY = Game1.player.getTileY();
			name = Game1.player.currentLocation.name;
			num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 2151182681u)
			{
				if (num > 1078463463u)
				{
					if (num <= 1684694008u)
					{
						if (num != 1667813495u)
						{
							if (num != 1684694008u)
							{
								return;
							}
							if (!(name == "Coop"))
							{
								return;
							}
							goto IL_D4F;
						}
						else if (!(name == "Tunnel"))
						{
							return;
						}
					}
					else if (num != 1972213674u)
					{
						if (num != 2028543928u)
						{
							if (num != 2151182681u)
							{
								return;
							}
							if (!(name == "Farm"))
							{
								return;
							}
							goto IL_D4F;
						}
						else if (!(name == "Backwoods"))
						{
							return;
						}
					}
					else
					{
						if (!(name == "Big Coop"))
						{
							return;
						}
						goto IL_D4F;
					}
					this.playerMapPosition = new Vector2((float)(this.mapX + 109 * Game1.pixelZoom), (float)(this.mapY + 47 * Game1.pixelZoom));
					this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11180", new object[0]);
					return;
				}
				if (num <= 411937663u)
				{
					if (num != 109565081u)
					{
						if (num != 411937663u)
						{
							return;
						}
						if (!(name == "Shed"))
						{
							return;
						}
					}
					else if (!(name == "Slime Hutch"))
					{
						return;
					}
				}
				else if (num != 784782095u)
				{
					if (num != 846075854u)
					{
						if (num != 1078463463u)
						{
							return;
						}
						if (!(name == "Temp"))
						{
							return;
						}
						if (!Game1.player.currentLocation.Map.Id.Contains("Town"))
						{
							return;
						}
						if (tileX > 84 && tileY < 68)
						{
							this.playerMapPosition = new Vector2((float)(this.mapX + 225 * Game1.pixelZoom), (float)(this.mapY + 81 * Game1.pixelZoom));
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
							return;
						}
						if (tileX > 80 && tileY >= 68)
						{
							this.playerMapPosition = new Vector2((float)(this.mapX + 220 * Game1.pixelZoom), (float)(this.mapY + 108 * Game1.pixelZoom));
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
							return;
						}
						if (tileY <= 42)
						{
							this.playerMapPosition = new Vector2((float)(this.mapX + 178 * Game1.pixelZoom), (float)(this.mapY + 64 * Game1.pixelZoom));
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
							return;
						}
						if (tileY > 42 && tileY < 76)
						{
							this.playerMapPosition = new Vector2((float)(this.mapX + 175 * Game1.pixelZoom), (float)(this.mapY + 88 * Game1.pixelZoom));
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
							return;
						}
						this.playerMapPosition = new Vector2((float)(this.mapX + 182 * Game1.pixelZoom), (float)(this.mapY + 109 * Game1.pixelZoom));
						this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
						return;
					}
					else if (!(name == "Big Barn"))
					{
						return;
					}
				}
				else if (!(name == "FarmHouse"))
				{
					return;
				}
			}
			else if (num <= 3014964069u)
			{
				if (num <= 2503779456u)
				{
					if (num != 2233558176u)
					{
						if (num != 2503779456u)
						{
							return;
						}
						if (!(name == "Forest"))
						{
							return;
						}
						if (tileY > 51)
						{
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11186", new object[0]);
							this.playerMapPosition = new Vector2((float)(this.mapX + 70 * Game1.pixelZoom), (float)(this.mapY + 135 * Game1.pixelZoom));
							return;
						}
						if (tileX < 58)
						{
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11186", new object[0]);
							this.playerMapPosition = new Vector2((float)(this.mapX + 63 * Game1.pixelZoom), (float)(this.mapY + 104 * Game1.pixelZoom));
							return;
						}
						this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11188", new object[0]);
						this.playerMapPosition = new Vector2((float)(this.mapX + 109 * Game1.pixelZoom), (float)(this.mapY + 107 * Game1.pixelZoom));
						return;
					}
					else if (!(name == "Greenhouse"))
					{
						return;
					}
				}
				else if (num != 2601855023u)
				{
					if (num != 2909376585u)
					{
						if (num != 3014964069u)
						{
							return;
						}
						if (!(name == "Town"))
						{
							return;
						}
						if (tileX > 84 && tileY < 68)
						{
							this.playerMapPosition = new Vector2((float)(this.mapX + 225 * Game1.pixelZoom), (float)(this.mapY + 81 * Game1.pixelZoom));
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
							return;
						}
						if (tileX > 80 && tileY >= 68)
						{
							this.playerMapPosition = new Vector2((float)(this.mapX + 220 * Game1.pixelZoom), (float)(this.mapY + 108 * Game1.pixelZoom));
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
							return;
						}
						if (tileY <= 42)
						{
							this.playerMapPosition = new Vector2((float)(this.mapX + 178 * Game1.pixelZoom), (float)(this.mapY + 64 * Game1.pixelZoom));
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
							return;
						}
						if (tileY > 42 && tileY < 76)
						{
							this.playerMapPosition = new Vector2((float)(this.mapX + 175 * Game1.pixelZoom), (float)(this.mapY + 88 * Game1.pixelZoom));
							this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
							return;
						}
						this.playerMapPosition = new Vector2((float)(this.mapX + 182 * Game1.pixelZoom), (float)(this.mapY + 109 * Game1.pixelZoom));
						this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190", new object[0]);
						return;
					}
					else
					{
						if (!(name == "Saloon"))
						{
							return;
						}
						this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11172", new object[0]);
						return;
					}
				}
				else if (!(name == "Deluxe Barn"))
				{
					return;
				}
			}
			else if (num <= 3308967874u)
			{
				if (num != 3183088828u)
				{
					if (num != 3308967874u)
					{
						return;
					}
					if (!(name == "Mountain"))
					{
						return;
					}
					if (tileX < 38)
					{
						this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11176", new object[0]);
						this.playerMapPosition = new Vector2((float)(this.mapX + 185 * Game1.pixelZoom), (float)(this.mapY + 36 * Game1.pixelZoom));
						return;
					}
					if (tileX < 96)
					{
						this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11177", new object[0]);
						this.playerMapPosition = new Vector2((float)(this.mapX + 220 * Game1.pixelZoom), (float)(this.mapY + 38 * Game1.pixelZoom));
						return;
					}
					this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11178", new object[0]);
					this.playerMapPosition = new Vector2((float)(this.mapX + 253 * Game1.pixelZoom), (float)(this.mapY + 40 * Game1.pixelZoom));
					return;
				}
				else if (!(name == "Barn"))
				{
					return;
				}
			}
			else if (num != 3333348840u)
			{
				if (num != 3627096444u)
				{
					if (num != 3734277467u)
					{
						return;
					}
					if (!(name == "Deluxe Coop"))
					{
						return;
					}
				}
				else if (!(name == "FarmCave"))
				{
					return;
				}
			}
			else
			{
				if (!(name == "Beach"))
				{
					return;
				}
				this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11174", new object[0]);
				this.playerMapPosition = new Vector2((float)(this.mapX + 202 * Game1.pixelZoom), (float)(this.mapY + 141 * Game1.pixelZoom));
				return;
			}
			IL_D4F:
			this.playerMapPosition = new Vector2((float)(this.mapX + 96 * Game1.pixelZoom), (float)(this.mapY + 72 * Game1.pixelZoom));
			this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11064", new object[]
			{
				Game1.player.farmName
			});
		}

		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (Game1.options.doesInputListContain(Game1.options.mapButton, key))
			{
				base.exitThisMenu(true);
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.okButton.containsPoint(x, y))
			{
				this.okButton.scale -= 0.25f;
				this.okButton.scale = Math.Max(0.75f, this.okButton.scale);
				(Game1.activeClickableMenu as GameMenu).changeTab(0);
			}
			foreach (ClickableComponent current in this.points)
			{
				if (current.containsPoint(x, y))
				{
					string name = current.name;
					if (name == "Lonely Stone")
					{
						Game1.playSound("stoneCrack");
					}
				}
			}
			if (Game1.activeClickableMenu != null)
			{
				(Game1.activeClickableMenu as GameMenu).changeTab(0);
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			foreach (ClickableComponent current in this.points)
			{
				if (current.containsPoint(x, y))
				{
					this.hoverText = current.name;
					return;
				}
			}
			if (this.okButton.containsPoint(x, y))
			{
				this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f);
				return;
			}
			this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
		}

		public override void draw(SpriteBatch b)
		{
			Game1.drawDialogueBox(this.mapX - Game1.pixelZoom * 8, this.mapY - Game1.pixelZoom * 24, (this.map.Bounds.Width + 16) * Game1.pixelZoom, 212 * Game1.pixelZoom, false, true, null, false);
			b.Draw(this.map, new Vector2((float)this.mapX, (float)this.mapY), new Rectangle?(new Rectangle(0, 0, 300, 180)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.86f);
			switch (Game1.whichFarm)
			{
			case 1:
				b.Draw(this.map, new Vector2((float)this.mapX, (float)(this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 180, 131, 61)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
				break;
			case 2:
				b.Draw(this.map, new Vector2((float)this.mapX, (float)(this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(131, 180, 131, 61)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
				break;
			case 3:
				b.Draw(this.map, new Vector2((float)this.mapX, (float)(this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 241, 131, 61)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
				break;
			case 4:
				b.Draw(this.map, new Vector2((float)this.mapX, (float)(this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(131, 241, 131, 61)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
				break;
			}
			Game1.player.FarmerRenderer.drawMiniPortrat(b, this.playerMapPosition - new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 0.00011f, 4f, 2, Game1.player);
			if (this.playerLocationName != null)
			{
				SpriteText.drawStringWithScrollCenteredAt(b, this.playerLocationName, this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + this.height + Game1.tileSize / 2 + Game1.pixelZoom * 4, "", 1f, -1, 0, 0.88f, false);
			}
			this.okButton.draw(b);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}
	}
}
