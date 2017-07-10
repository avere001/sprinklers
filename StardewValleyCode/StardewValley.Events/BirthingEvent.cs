using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Events
{
	public class BirthingEvent : FarmEvent
	{
		private int behavior;

		private int timer;

		private string soundName;

		private string message;

		private string babyName;

		private bool playedSound;

		private bool showedMessage;

		private bool isMale;

		private bool getBabyName;

		private bool naming;

		private Vector2 targetLocation;

		private TextBox babyNameBox;

		private ClickableTextureComponent okButton;

		public bool setUp()
		{
			Random random = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			Utility.getHomeOfFarmer(Game1.player);
			NPC characterFromName = Game1.getCharacterFromName(Game1.player.spouse, false);
			Game1.player.CanMove = false;
			if (Game1.player.getNumberOfChildren() == 0)
			{
				this.isMale = (random.NextDouble() < 0.5);
			}
			else
			{
				this.isMale = (Game1.player.getChildren()[0].gender == 1);
			}
			if (characterFromName.isGaySpouse())
			{
				this.message = Game1.content.LoadString("Strings\\Events:BirthMessage_Adoption", new object[]
				{
					Lexicon.getGenderedChildTerm(this.isMale)
				});
			}
			else if (characterFromName.gender == 0)
			{
				this.message = Game1.content.LoadString("Strings\\Events:BirthMessage_PlayerMother", new object[]
				{
					Lexicon.getGenderedChildTerm(this.isMale)
				});
			}
			else
			{
				this.message = Game1.content.LoadString("Strings\\Events:BirthMessage_SpouseMother", new object[]
				{
					Lexicon.getGenderedChildTerm(this.isMale),
					characterFromName.displayName
				});
			}
			return false;
		}

		public void returnBabyName(string name)
		{
			this.babyName = name;
			Game1.exitActiveMenu();
		}

		public void afterMessage()
		{
			this.getBabyName = true;
		}

		public bool tickUpdate(GameTime time)
		{
			Game1.player.CanMove = false;
			this.timer += time.ElapsedGameTime.Milliseconds;
			Game1.fadeToBlackAlpha = 1f;
			if (this.timer > 1500 && !this.playedSound && !this.getBabyName)
			{
				if (this.soundName != null && !this.soundName.Equals(""))
				{
					Game1.playSound(this.soundName);
					this.playedSound = true;
				}
				if (!this.playedSound && this.message != null && !Game1.dialogueUp && Game1.activeClickableMenu == null)
				{
					Game1.drawObjectDialogue(this.message);
					Game1.afterDialogues = new Game1.afterFadeFunction(this.afterMessage);
				}
			}
			else if (this.getBabyName)
			{
				if (!this.naming)
				{
					Game1.activeClickableMenu = new NamingMenu(new NamingMenu.doneNamingBehavior(this.returnBabyName), Game1.content.LoadString(this.isMale ? "Strings\\Events:BabyNamingTitle_Male" : "Strings\\Events:BabyNamingTitle_Female", new object[0]), "");
					this.naming = true;
				}
				if (this.babyName != null && this.babyName != "" && this.babyName.Length > 0)
				{
					double num = Game1.player.spouse.Equals("Maru") ? 0.5 : 0.0;
					num += (Game1.player.hasDarkSkin() ? 0.5 : 0.0);
					bool isDarkSkinned = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed).NextDouble() < num;
					string text = this.babyName;
					using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.name.Equals(text))
							{
								text += " ";
								break;
							}
						}
					}
					Utility.getHomeOfFarmer(Game1.player).characters.Add(new Child(text, this.isMale, isDarkSkinned, Game1.player));
					Game1.playSound("smallSelect");
					Game1.player.getSpouse().daysAfterLastBirth = 5;
					Game1.player.getSpouse().daysUntilBirthing = -1;
					if (Game1.player.getChildren().Count == 2)
					{
						Game1.player.getSpouse().setNewDialogue((Game1.random.NextDouble() < 0.5) ? ((Game1.player.getSpouse().gender == 0) ? Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild1", new object[0]).Split(new char[]
						{
							'/'
						}).First<string>() : Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild1", new object[0]).Split(new char[]
						{
							'/'
						}).Last<string>()) : ((Game1.player.getSpouse().gender == 0) ? Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild2", new object[0]).Split(new char[]
						{
							'/'
						}).First<string>() : Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild2", new object[0]).Split(new char[]
						{
							'/'
						}).Last<string>()), false, false);
						Game1.getSteamAchievement("Achievement_FullHouse");
					}
					else if (Game1.player.getSpouse().isGaySpouse())
					{
						Game1.player.getSpouse().setNewDialogue((Game1.player.getSpouse().gender == 0) ? Game1.content.LoadString("Data\\ExtraDialogue:NewChild_Adoption", new object[]
						{
							this.babyName
						}).Split(new char[]
						{
							'/'
						}).First<string>() : Game1.content.LoadString("Data\\ExtraDialogue:NewChild_Adoption", new object[]
						{
							this.babyName
						}).Split(new char[]
						{
							'/'
						}).Last<string>(), false, false);
					}
					else
					{
						Game1.player.getSpouse().setNewDialogue((Game1.player.getSpouse().gender == 0) ? Game1.content.LoadString("Data\\ExtraDialogue:NewChild_FirstChild", new object[]
						{
							this.babyName
						}).Split(new char[]
						{
							'/'
						}).First<string>() : Game1.content.LoadString("Data\\ExtraDialogue:NewChild_FirstChild", new object[]
						{
							this.babyName
						}).Split(new char[]
						{
							'/'
						}).Last<string>(), false, false);
					}
					if (Game1.keyboardDispatcher != null)
					{
						Game1.keyboardDispatcher.Subscriber = null;
					}
					Game1.player.position = Utility.PointToVector2(Utility.getHomeOfFarmer(Game1.player).getBedSpot()) * (float)Game1.tileSize;
					Game1.globalFadeToClear(null, 0.02f);
					return true;
				}
			}
			return false;
		}

		public void draw(SpriteBatch b)
		{
		}

		public void makeChangesToLocation()
		{
		}

		public void drawAboveEverything(SpriteBatch b)
		{
		}
	}
}
