using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class AdventureGuild : GameLocation
	{
		private NPC Gil = new NPC(null, new Vector2(-1000f, -1000f), "AdventureGuild", 2, "Gil", false, null, Game1.content.Load<Texture2D>("Portraits\\Gil"));

		private bool talkedToGil;

		public AdventureGuild()
		{
		}

		public AdventureGuild(Map map, string name) : base(map, name)
		{
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			int num = (this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1;
			if (num <= 1292)
			{
				if (num != 1291 && num != 1292)
				{
					goto IL_91;
				}
			}
			else
			{
				if (num == 1306)
				{
					this.showMonsterKillList();
					return true;
				}
				switch (num)
				{
				case 1355:
				case 1356:
				case 1357:
				case 1358:
					break;
				default:
					goto IL_91;
				}
			}
			this.gil();
			return true;
			IL_91:
			return base.checkAction(tileLocation, viewport, who);
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			this.talkedToGil = false;
			if (!Game1.player.mailReceived.Contains("guildMember"))
			{
				Game1.player.mailReceived.Add("guildMember");
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (!Game1.player.mailReceived.Contains("checkedMonsterBoard"))
			{
				float num = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(8 * Game1.tileSize - 8), (float)(9 * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(10 * Game1.tileSize) / 10000f + 1E-06f + 0.0008f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(8 * Game1.tileSize + Game1.tileSize / 2), (float)(9 * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(175, 425, 12, 12)), Color.White * 0.75f, 0f, new Vector2(6f, 6f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(10 * Game1.tileSize) / 10000f + 1E-05f + 0.0008f);
			}
		}

		private string killListLine(string monsterType, int killCount, int target)
		{
			string text = Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_" + monsterType, new object[0]);
			if (killCount == 0)
			{
				return Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat_None", new object[]
				{
					killCount,
					target,
					text
				}) + "^";
			}
			if (killCount >= target)
			{
				return Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat_OverTarget", new object[]
				{
					killCount,
					target,
					text
				}) + "^";
			}
			return Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat", new object[]
			{
				killCount,
				target,
				text
			}) + "^";
		}

		public void showMonsterKillList()
		{
			if (!Game1.player.mailReceived.Contains("checkedMonsterBoard"))
			{
				Game1.player.mailReceived.Add("checkedMonsterBoard");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_Header", new object[0]).Replace('\n', '^') + "^");
			int killCount = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
			int killCount2 = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
			int killCount3 = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
			int killCount4 = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
			int killCount5 = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
			int monstersKilled = Game1.stats.getMonstersKilled("Duggy");
			int monstersKilled2 = Game1.stats.getMonstersKilled("Dust Spirit");
			stringBuilder.Append(this.killListLine("Slimes", killCount, 1000));
			stringBuilder.Append(this.killListLine("VoidSpirits", killCount2, 150));
			stringBuilder.Append(this.killListLine("Bats", killCount5, 200));
			stringBuilder.Append(this.killListLine("Skeletons", killCount3, 50));
			stringBuilder.Append(this.killListLine("CaveInsects", killCount4, 125));
			stringBuilder.Append(this.killListLine("Duggies", monstersKilled, 30));
			stringBuilder.Append(this.killListLine("DustSprites", monstersKilled2, 500));
			stringBuilder.Append(Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_Footer", new object[0]).Replace('\n', '^'));
			Game1.drawLetterMessage(stringBuilder.ToString());
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.changeMusicTrack("none");
		}

		public static bool areAllMonsterSlayerQuestsComplete()
		{
			int arg_146_0 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
			int num = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
			int num2 = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
			Game1.stats.getMonstersKilled("Rock Crab");
			Game1.stats.getMonstersKilled("Lava Crab");
			int num3 = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
			int num4 = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
			int monstersKilled = Game1.stats.getMonstersKilled("Duggy");
			Game1.stats.getMonstersKilled("Metal Head");
			Game1.stats.getMonstersKilled("Stone Golem");
			int monstersKilled2 = Game1.stats.getMonstersKilled("Dust Spirit");
			return arg_146_0 >= 1000 && num >= 150 && num2 >= 50 && num3 >= 125 && num4 >= 200 && monstersKilled >= 30 && monstersKilled2 >= 500;
		}

		public static bool willThisKillCompleteAMonsterSlayerQuest(string nameOfMonster)
		{
			int arg_144_0 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
			int num = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
			int num2 = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
			int num3 = Game1.stats.getMonstersKilled("Rock Crab") + Game1.stats.getMonstersKilled("Lava Crab");
			int num4 = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
			int num5 = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
			int monstersKilled = Game1.stats.getMonstersKilled("Duggy");
			int monstersKilled2 = Game1.stats.getMonstersKilled("Metal Head");
			int monstersKilled3 = Game1.stats.getMonstersKilled("Stone Golem");
			int monstersKilled4 = Game1.stats.getMonstersKilled("Dust Spirit");
			int num6 = arg_144_0 + ((nameOfMonster.Equals("Green Slime") || nameOfMonster.Equals("Frost Jelly") || nameOfMonster.Equals("Sludge")) ? 1 : 0);
			int num7 = num + ((nameOfMonster.Equals("Shadow Guy") || nameOfMonster.Equals("Shadow Shaman") || nameOfMonster.Equals("Shadow Brute")) ? 1 : 0);
			int num8 = num2 + ((nameOfMonster.Equals("Skeleton") || nameOfMonster.Equals("Skeleton Mage")) ? 1 : 0);
			if (!nameOfMonster.Equals("Rock Crab"))
			{
				nameOfMonster.Equals("Lava Crab");
			}
			int num9 = num4 + ((nameOfMonster.Equals("Grub") || nameOfMonster.Equals("Fly") || nameOfMonster.Equals("Bug")) ? 1 : 0);
			int num10 = num5 + ((nameOfMonster.Equals("Bat") || nameOfMonster.Equals("Frost Bat") || nameOfMonster.Equals("Lava Bat")) ? 1 : 0);
			int num11 = monstersKilled + (nameOfMonster.Equals("Duggy") ? 1 : 0);
			nameOfMonster.Equals("Metal Head");
			nameOfMonster.Equals("Stone Golem");
			int num12 = monstersKilled4 + (nameOfMonster.Equals("Dust Spirit") ? 1 : 0);
			return (arg_144_0 < 1000 && num6 >= 1000 && !Game1.player.mailReceived.Contains("Gil_Slime Charmer Ring")) || (num < 150 && num7 >= 150 && !Game1.player.mailReceived.Contains("Gil_Savage Ring")) || (num2 < 50 && num8 >= 50 && !Game1.player.mailReceived.Contains("Gil_Skeleton Mask")) || (num4 < 125 && num9 >= 125 && !Game1.player.mailReceived.Contains("Gil_Insect Head")) || (num5 < 200 && num10 >= 200 && !Game1.player.mailReceived.Contains("Gil_Vampire Ring")) || (monstersKilled < 30 && num11 >= 30 && !Game1.player.mailReceived.Contains("Gil_Hard Hat")) || (monstersKilled4 < 500 && num12 >= 500 && !Game1.player.mailReceived.Contains("Gil_Burglar's Ring"));
		}

		private void gil()
		{
			List<Item> list = new List<Item>();
			int arg_171_0 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
			int num = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
			int num2 = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
			int num3 = Game1.stats.getMonstersKilled("Goblin Warrior") + Game1.stats.getMonstersKilled("Goblin Wizard");
			int num4 = Game1.stats.getMonstersKilled("Rock Crab") + Game1.stats.getMonstersKilled("Lava Crab");
			int num5 = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
			int num6 = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
			int monstersKilled = Game1.stats.getMonstersKilled("Duggy");
			int monstersKilled2 = Game1.stats.getMonstersKilled("Metal Head");
			int monstersKilled3 = Game1.stats.getMonstersKilled("Stone Golem");
			int monstersKilled4 = Game1.stats.getMonstersKilled("Dust Spirit");
			if (arg_171_0 >= 1000 && !Game1.player.mailReceived.Contains("Gil_Slime Charmer Ring"))
			{
				list.Add(new Ring(520));
			}
			if (num >= 150 && !Game1.player.mailReceived.Contains("Gil_Savage Ring"))
			{
				list.Add(new Ring(523));
			}
			if (num2 >= 50 && !Game1.player.mailReceived.Contains("Gil_Skeleton Mask"))
			{
				list.Add(new Hat(8));
			}
			if (num3 >= 50)
			{
				Game1.player.specialItems.Contains(9);
			}
			if (num4 >= 60)
			{
				Game1.player.specialItems.Contains(524);
			}
			if (num5 >= 125 && !Game1.player.mailReceived.Contains("Gil_Insect Head"))
			{
				list.Add(new MeleeWeapon(13));
			}
			if (num6 >= 200 && !Game1.player.mailReceived.Contains("Gil_Vampire Ring"))
			{
				list.Add(new Ring(522));
			}
			if (monstersKilled >= 30 && !Game1.player.mailReceived.Contains("Gil_Hard Hat"))
			{
				list.Add(new Hat(27));
			}
			if (monstersKilled2 >= 50)
			{
				Game1.player.specialItems.Contains(519);
			}
			if (monstersKilled3 >= 50)
			{
				Game1.player.specialItems.Contains(517);
			}
			if (monstersKilled4 >= 500 && !Game1.player.mailReceived.Contains("Gil_Burglar's Ring"))
			{
				list.Add(new Ring(526));
			}
			foreach (Item current in list)
			{
				if (current is StardewValley.Object)
				{
					(current as StardewValley.Object).specialItem = true;
				}
				else if (!Game1.player.hasOrWillReceiveMail("Gil_" + current.Name))
				{
					Game1.player.mailReceived.Add("Gil_" + current.Name);
				}
			}
			if (list.Count > 0)
			{
				Game1.activeClickableMenu = new ItemGrabMenu(list);
				return;
			}
			if (this.talkedToGil)
			{
				Game1.drawDialogue(this.Gil, Game1.content.LoadString("Characters\\Dialogue\\Gil:Snoring", new object[0]));
			}
			else
			{
				Game1.drawDialogue(this.Gil, Game1.content.LoadString("Characters\\Dialogue\\Gil:ComeBackLater", new object[0]));
			}
			this.talkedToGil = true;
		}
	}
}
