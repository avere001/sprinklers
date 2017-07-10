using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Menus;
using System;

namespace StardewValley.Events
{
	public class QuestionEvent : FarmEvent
	{
		public const int pregnancyQuestion = 1;

		public const int barnBirth = 2;

		private int whichQuestion;

		private AnimalHouse animalHouse;

		public FarmAnimal animal;

		public bool forceProceed;

		public QuestionEvent(int whichQuestion)
		{
			this.whichQuestion = whichQuestion;
		}

		public bool setUp()
		{
			int num = this.whichQuestion;
			if (num != 1)
			{
				if (num == 2)
				{
					FarmAnimal farmAnimal = null;
					foreach (Building current in Game1.getFarm().buildings)
					{
						if ((current.owner.Equals(Game1.uniqueIDForThisGame) || !Game1.IsMultiplayer) && current.buildingType.Contains("Barn") && !current.buildingType.Equals("Barn") && !(current.indoors as AnimalHouse).isFull() && Game1.random.NextDouble() < (double)(current.indoors as AnimalHouse).animalsThatLiveHere.Count * 0.0055)
						{
							farmAnimal = Utility.getAnimal((current.indoors as AnimalHouse).animalsThatLiveHere[Game1.random.Next((current.indoors as AnimalHouse).animalsThatLiveHere.Count)]);
							this.animalHouse = (current.indoors as AnimalHouse);
							break;
						}
					}
					if (farmAnimal != null && !farmAnimal.isBaby() && farmAnimal.allowReproduction)
					{
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Events:AnimalBirth", new object[]
						{
							farmAnimal.displayName,
							farmAnimal.shortDisplayType()
						}));
						Game1.messagePause = true;
						this.animal = farmAnimal;
						return false;
					}
				}
				return true;
			}
			Response[] answerChoices = new Response[]
			{
				new Response("Yes", Game1.content.LoadString("Strings\\Events:HaveBabyAnswer_Yes", new object[0])),
				new Response("Not", Game1.content.LoadString("Strings\\Events:HaveBabyAnswer_No", new object[0]))
			};
			if (!Game1.getCharacterFromName(Game1.player.spouse, false).isGaySpouse())
			{
				Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Events:HaveBabyQuestion", new object[]
				{
					Game1.player.name
				}), answerChoices, new GameLocation.afterQuestionBehavior(this.answerPregnancyQuestion), Game1.getCharacterFromName(Game1.player.spouse, false));
			}
			else
			{
				Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Events:HaveBabyQuestion_Adoption", new object[]
				{
					Game1.player.name
				}), answerChoices, new GameLocation.afterQuestionBehavior(this.answerPregnancyQuestion), Game1.getCharacterFromName(Game1.player.spouse, false));
			}
			Game1.messagePause = true;
			return false;
		}

		private void answerPregnancyQuestion(Farmer who, string answer)
		{
			if (answer.Equals("Yes"))
			{
				Game1.getCharacterFromName(who.spouse, false).daysUntilBirthing = 14;
				Game1.getCharacterFromName(who.spouse, false).isGaySpouse();
			}
			Game1.player.position = Utility.PointToVector2(Utility.getHomeOfFarmer(Game1.player).getBedSpot()) * (float)Game1.tileSize;
		}

		public bool tickUpdate(GameTime time)
		{
			if (this.forceProceed)
			{
				return true;
			}
			if (this.whichQuestion == 2 && !Game1.dialogueUp)
			{
				if (Game1.activeClickableMenu == null)
				{
					Game1.activeClickableMenu = new NamingMenu(new NamingMenu.doneNamingBehavior(this.animalHouse.addNewHatchedAnimal), (this.animal != null) ? Game1.content.LoadString("Strings\\Events:AnimalNamingTitle", new object[]
					{
						this.animal.displayType
					}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestionEvent.cs.6692", new object[0]), null);
				}
				return false;
			}
			return !Game1.dialogueUp;
		}

		public void draw(SpriteBatch b)
		{
		}

		public void drawAboveEverything(SpriteBatch b)
		{
		}

		public void makeChangesToLocation()
		{
			Game1.messagePause = false;
			Game1.player.position = Game1.player.mostRecentBed;
		}
	}
}
