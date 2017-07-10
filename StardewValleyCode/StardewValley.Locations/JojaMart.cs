using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;

namespace StardewValley.Locations
{
	public class JojaMart : GameLocation
	{
		public const int JojaMembershipPrice = 5000;

		public static NPC Morris;

		private Texture2D communityDevelopmentTexture;

		public JojaMart()
		{
		}

		public JojaMart(Map map, string name) : base(map, name)
		{
		}

		private bool signUpForJoja(int response)
		{
			if (response == 0)
			{
				base.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:JojaMart_SignUp", new object[0])), base.createYesNoResponses(), "JojaSignUp");
				return true;
			}
			Game1.dialogueUp = false;
			Game1.player.forceCanMove();
			Game1.playSound("smallSelect");
			Game1.currentSpeaker = null;
			Game1.dialogueTyping = false;
			return true;
		}

		public override bool answerDialogue(Response answer)
		{
			string a = this.lastQuestionKey.Split(new char[]
			{
				' '
			})[0] + "_" + answer.responseKey;
			if (a == "JojaSignUp_Yes")
			{
				if (Game1.player.Money >= 5000)
				{
					Game1.player.Money -= 5000;
					Game1.addMailForTomorrow("JojaMember", true, true);
					Game1.player.removeQuest(26);
					JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_PlayerSignedUp", new object[0]), false, false);
					Game1.drawDialogue(JojaMart.Morris);
				}
				else if (Game1.player.Money < 5000)
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1", new object[0]));
				}
				return true;
			}
			return base.answerDialogue(answer);
		}

		public override void cleanupBeforePlayerExit()
		{
			if (!Game1.isRaining)
			{
				Game1.changeMusicTrack("none");
			}
			base.cleanupBeforePlayerExit();
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				PropertyValue propertyValue = "";
				this.map.GetLayer("Buildings").Tiles[tileLocation].Properties.TryGetValue("Action", out propertyValue);
				if (propertyValue != null)
				{
					string a = propertyValue.ToString();
					if (!(a == "JojaShop") && a == "JoinJoja")
					{
						JojaMart.Morris.CurrentDialogue.Clear();
						if (Game1.player.mailForTomorrow.Contains("JojaMember%&NL&%"))
						{
							JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_ComeBackLater", new object[0]), false, false);
							Game1.drawDialogue(JojaMart.Morris);
						}
						else if (!Game1.player.mailReceived.Contains("JojaMember"))
						{
							if (!Game1.player.mailReceived.Contains("JojaGreeting"))
							{
								JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_Greeting", new object[0]), false, false);
								Game1.player.mailReceived.Add("JojaGreeting");
								Game1.drawDialogue(JojaMart.Morris);
							}
							else if (Game1.stats.DaysPlayed < 0u)
							{
								string path = (Game1.dayOfMonth % 7 == 0 || Game1.dayOfMonth % 7 == 6) ? "Data\\ExtraDialogue:Morris_WeekendGreeting" : "Data\\ExtraDialogue:Morris_FirstGreeting";
								JojaMart.Morris.setNewDialogue(Game1.content.LoadString(path, new object[0]), false, false);
								Game1.drawDialogue(JojaMart.Morris);
							}
							else
							{
								string str = (Game1.dayOfMonth % 7 == 0 || Game1.dayOfMonth % 7 == 6) ? "Data\\ExtraDialogue:Morris_WeekendGreeting" : "Data\\ExtraDialogue:Morris_FirstGreeting";
								if (!Game1.IsMultiplayer || Game1.IsServer)
								{
									JojaMart.Morris.setNewDialogue(Game1.content.LoadString(str + "_MembershipAvailable", new object[]
									{
										5000
									}), false, false);
									JojaMart.Morris.CurrentDialogue.Peek().answerQuestionBehavior = new Dialogue.onAnswerQuestion(this.signUpForJoja);
								}
								else
								{
									JojaMart.Morris.setNewDialogue(str + "_SecondPlayer", false, false);
								}
								Game1.drawDialogue(JojaMart.Morris);
							}
						}
						else
						{
							if (Game1.player.mailForTomorrow.Contains("jojaFishTank%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaPantry%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaCraftsRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaBoilerRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaVault%&NL&%"))
							{
								JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_StillProcessingOrder", new object[0]), false, false);
							}
							else
							{
								if (Game1.player.isMale)
								{
									JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_CommunityDevelopmentForm_PlayerMale", new object[0]), false, false);
								}
								else
								{
									JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_CommunityDevelopmentForm_PlayerFemale", new object[0]), false, false);
								}
								JojaMart.Morris.CurrentDialogue.Peek().answerQuestionBehavior = new Dialogue.onAnswerQuestion(this.viewJojaNote);
							}
							Game1.drawDialogue(JojaMart.Morris);
						}
					}
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		private bool viewJojaNote(int response)
		{
			if (response == 0)
			{
				Game1.activeClickableMenu = new JojaCDMenu(this.communityDevelopmentTexture);
			}
			Game1.dialogueUp = false;
			Game1.player.forceCanMove();
			Game1.playSound("smallSelect");
			Game1.currentSpeaker = null;
			Game1.dialogueTyping = false;
			return true;
		}

		public override void resetForPlayerEntry()
		{
			this.communityDevelopmentTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\JojaCDForm");
			JojaMart.Morris = new NPC(null, Vector2.Zero, "JojaMart", 2, "Morris", false, null, Game1.temporaryContent.Load<Texture2D>("Portraits\\Morris"));
			base.resetForPlayerEntry();
		}
	}
}
