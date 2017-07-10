using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Events
{
	public class DiaryEvent : FarmEvent
	{
		public string NPCname;

		public bool setUp()
		{
			if (Game1.player.isMarried())
			{
				return true;
			}
			foreach (string current in Game1.player.mailReceived)
			{
				if (current.Contains("diary"))
				{
					string text = current.Split(new char[]
					{
						'_'
					})[1];
					if (!Game1.player.mailReceived.Contains("diary_" + text + "_finished"))
					{
						Convert.ToInt32(text.Split(new char[]
						{
							'/'
						})[1]);
						this.NPCname = text.Split(new char[]
						{
							'/'
						})[0];
						NPC characterFromName = Game1.getCharacterFromName(this.NPCname, false);
						int arg_B8_0 = characterFromName.gender;
						Game1.player.mailReceived.Add("diary_" + text + "_finished");
						string text2 = string.Concat(new object[]
						{
							Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6658", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6660", new object[0]),
							Environment.NewLine,
							Environment.NewLine,
							"-",
							Utility.capitalizeFirstLetter(Game1.CurrentSeasonDisplayName),
							" ",
							Game1.dayOfMonth,
							"-",
							Environment.NewLine,
							Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6664", new object[]
							{
								this.NPCname
							})
						});
						Response[] answerChoices = new Response[]
						{
							new Response("...We're", Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6667", new object[0])),
							new Response("...I", (characterFromName.gender == 0) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6669", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6670", new object[0])),
							new Response("(Write", Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6672", new object[0]))
						};
						Game1.currentLocation.createQuestionDialogue(Game1.parseText(text2), answerChoices, "diary");
						Game1.messagePause = true;
						return false;
					}
				}
			}
			return true;
		}

		public bool tickUpdate(GameTime time)
		{
			return !Game1.dialogueUp;
		}

		public void draw(SpriteBatch b)
		{
		}

		public void makeChangesToLocation()
		{
			Game1.messagePause = false;
		}

		public void drawAboveEverything(SpriteBatch b)
		{
		}
	}
}
