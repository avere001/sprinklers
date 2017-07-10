using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley
{
	public class CraftingRecipe
	{
		public string name;

		public string DisplayName;

		private string description;

		public static Dictionary<string, string> craftingRecipes;

		public static Dictionary<string, string> cookingRecipes;

		private Dictionary<int, int> recipeList = new Dictionary<int, int>();

		private List<int> itemToProduce = new List<int>();

		public bool bigCraftable;

		public bool isCookingRecipe;

		public int timesCrafted;

		public int numberProducedPerCraft;

		public static void InitShared()
		{
			CraftingRecipe.craftingRecipes = Game1.content.Load<Dictionary<string, string>>("Data//CraftingRecipes");
			CraftingRecipe.cookingRecipes = Game1.content.Load<Dictionary<string, string>>("Data//CookingRecipes");
		}

		public CraftingRecipe(string name, bool isCookingRecipe)
		{
			this.isCookingRecipe = isCookingRecipe;
			this.name = name;
			string text = (isCookingRecipe && CraftingRecipe.cookingRecipes.ContainsKey(name)) ? CraftingRecipe.cookingRecipes[name] : (CraftingRecipe.craftingRecipes.ContainsKey(name) ? CraftingRecipe.craftingRecipes[name] : null);
			if (text == null)
			{
				this.name = "Torch";
				name = "Torch";
				text = CraftingRecipe.craftingRecipes[name];
			}
			string[] array = text.Split(new char[]
			{
				'/'
			});
			string[] array2 = array[0].Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array2.Length; i += 2)
			{
				this.recipeList.Add(Convert.ToInt32(array2[i]), Convert.ToInt32(array2[i + 1]));
			}
			string[] array3 = array[2].Split(new char[]
			{
				' '
			});
			for (int j = 0; j < array3.Length; j += 2)
			{
				this.itemToProduce.Add(Convert.ToInt32(array3[j]));
				this.numberProducedPerCraft = ((array3.Length > 1) ? Convert.ToInt32(array3[j + 1]) : 1);
			}
			this.bigCraftable = (!isCookingRecipe && Convert.ToBoolean(array[3]));
			try
			{
				this.description = (this.bigCraftable ? Game1.bigCraftablesInformation[this.itemToProduce[0]].Split(new char[]
				{
					'/'
				})[4] : Game1.objectInformation[this.itemToProduce[0]].Split(new char[]
				{
					'/'
				})[5]);
			}
			catch (Exception)
			{
				this.description = "";
			}
			this.timesCrafted = (Game1.player.craftingRecipes.ContainsKey(name) ? Game1.player.craftingRecipes[name] : 0);
			if (name.Equals("Crab Pot") && Game1.player.professions.Contains(7))
			{
				this.recipeList = new Dictionary<int, int>();
				this.recipeList.Add(388, 25);
				this.recipeList.Add(334, 2);
			}
			if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
			{
				this.DisplayName = array[array.Length - 1];
				return;
			}
			this.DisplayName = name;
		}

		public int getIndexOfMenuView()
		{
			if (this.itemToProduce.Count <= 0)
			{
				return -1;
			}
			return this.itemToProduce[0];
		}

		public bool doesFarmerHaveIngredientsInInventory(List<Item> extraToCheck = null)
		{
			foreach (KeyValuePair<int, int> current in this.recipeList)
			{
				if (!Game1.player.hasItemInInventory(current.Key, current.Value, 5) && (extraToCheck == null || !Game1.player.hasItemInList(extraToCheck, current.Key, current.Value, 5)))
				{
					return false;
				}
			}
			return true;
		}

		public void drawMenuView(SpriteBatch b, int x, int y, float layerDepth = 0.88f, bool shadow = true)
		{
			if (this.bigCraftable)
			{
				Utility.drawWithShadow(b, Game1.bigCraftableSpriteSheet, new Vector2((float)x, (float)y), Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.getIndexOfMenuView(), 16, 32), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, layerDepth, -1, -1, 0.35f);
				return;
			}
			Utility.drawWithShadow(b, Game1.objectSpriteSheet, new Vector2((float)x, (float)y), Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIndexOfMenuView(), 16, 16), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, layerDepth, -1, -1, 0.35f);
		}

		public Item createItem()
		{
			int num = this.itemToProduce.ElementAt(Game1.random.Next(this.itemToProduce.Count));
			if (this.bigCraftable)
			{
				if (this.name.Equals("Chest"))
				{
					return new Chest(true);
				}
				return new Object(Vector2.Zero, num, false);
			}
			else
			{
				if (this.name.Equals("Torch"))
				{
					return new Torch(Vector2.Zero, this.numberProducedPerCraft);
				}
				if (num >= 516 && num <= 534)
				{
					return new Ring(num);
				}
				return new Object(Vector2.Zero, num, this.numberProducedPerCraft);
			}
		}

		public void consumeIngredients()
		{
			for (int i = this.recipeList.Count - 1; i >= 0; i--)
			{
				int value = this.recipeList[this.recipeList.Keys.ElementAt(i)];
				bool flag = false;
				for (int j = Game1.player.items.Count - 1; j >= 0; j--)
				{
					if (Game1.player.items[j] != null && Game1.player.items[j] is Object && !(Game1.player.items[j] as Object).bigCraftable && (((Object)Game1.player.items[j]).parentSheetIndex == this.recipeList.Keys.ElementAt(i) || ((Object)Game1.player.items[j]).category == this.recipeList.Keys.ElementAt(i)))
					{
						int num = this.recipeList[this.recipeList.Keys.ElementAt(i)];
						Dictionary<int, int> dictionary = this.recipeList;
						int key = this.recipeList.Keys.ElementAt(i);
						dictionary[key] -= Game1.player.items[j].Stack;
						Game1.player.items[j].Stack -= num;
						if (Game1.player.items[j].Stack <= 0)
						{
							Game1.player.items[j] = null;
						}
						if (this.recipeList[this.recipeList.Keys.ElementAt(i)] <= 0)
						{
							this.recipeList[this.recipeList.Keys.ElementAt(i)] = value;
							flag = true;
							break;
						}
					}
				}
				if (this.isCookingRecipe && !flag)
				{
					FarmHouse homeOfFarmer = Utility.getHomeOfFarmer(Game1.player);
					if (homeOfFarmer != null)
					{
						for (int k = homeOfFarmer.fridge.items.Count - 1; k >= 0; k--)
						{
							if (homeOfFarmer.fridge.items[k] != null && homeOfFarmer.fridge.items[k] is Object && (((Object)homeOfFarmer.fridge.items[k]).parentSheetIndex == this.recipeList.Keys.ElementAt(i) || ((Object)homeOfFarmer.fridge.items[k]).category == this.recipeList.Keys.ElementAt(i)))
							{
								int num2 = this.recipeList[this.recipeList.Keys.ElementAt(i)];
								Dictionary<int, int> dictionary = this.recipeList;
								int key = this.recipeList.Keys.ElementAt(i);
								dictionary[key] -= homeOfFarmer.fridge.items[k].Stack;
								homeOfFarmer.fridge.items[k].Stack -= num2;
								if (homeOfFarmer.fridge.items[k].Stack <= 0)
								{
									homeOfFarmer.fridge.items[k] = null;
								}
								if (this.recipeList[this.recipeList.Keys.ElementAt(i)] <= 0)
								{
									this.recipeList[this.recipeList.Keys.ElementAt(i)] = value;
									break;
								}
							}
						}
					}
				}
			}
		}

		public int getDescriptionHeight(int width)
		{
			return (int)(Game1.smallFont.MeasureString(Game1.parseText(this.description, Game1.smallFont, width)).Y + (float)(this.getNumberOfIngredients() * (Game1.tileSize / 2 + Game1.pixelZoom)) + (float)((int)Game1.smallFont.MeasureString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.567", new object[0])).Y) + (float)(Game1.tileSize / 3));
		}

		public void drawRecipeDescription(SpriteBatch b, Vector2 position, int width)
		{
			b.Draw(Game1.staminaRect, new Rectangle((int)(position.X + 8f), (int)(position.Y + (float)(Game1.tileSize / 2) + Game1.smallFont.MeasureString("Ing").Y) - Game1.pixelZoom - 2, width - Game1.tileSize / 2, Game1.pixelZoom / 2), Game1.textColor * 0.35f);
			Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.567", new object[0]), Game1.smallFont, position + new Vector2(8f, (float)(Game1.tileSize / 2 - Game1.pixelZoom)), Game1.textColor * 0.75f, 1f, -1f, -1, -1, 1f, 3);
			for (int i = 0; i < this.recipeList.Count; i++)
			{
				Color color = Game1.player.hasItemInInventory(this.recipeList.Keys.ElementAt(i), this.recipeList.Values.ElementAt(i), 8) ? Game1.textColor : Color.Red;
				if (this.isCookingRecipe && Game1.player.hasItemInList(Utility.getHomeOfFarmer(Game1.player).fridge.items, this.recipeList.Keys.ElementAt(i), this.recipeList.Values.ElementAt(i), 8))
				{
					color = Game1.textColor;
				}
				b.Draw(Game1.objectSpriteSheet, new Vector2(position.X, position.Y + (float)Game1.tileSize + (float)(i * Game1.tileSize / 2) + (float)(i * 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getSpriteIndexFromRawIndex(this.recipeList.Keys.ElementAt(i)), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom / 2f, SpriteEffects.None, 0.86f);
				Utility.drawTinyDigits(this.recipeList.Values.ElementAt(i), b, new Vector2(position.X + (float)(Game1.tileSize / 2) - Game1.tinyFont.MeasureString(string.Concat(this.recipeList.Values.ElementAt(i))).X, position.Y + (float)Game1.tileSize + (float)(i * Game1.tileSize / 2) + (float)(i * 4) + (float)(Game1.tileSize / 3)), (float)Game1.pixelZoom / 2f, 0.87f, Color.AntiqueWhite);
				Utility.drawTextWithShadow(b, this.getNameFromIndex(this.recipeList.Keys.ElementAt(i)), Game1.smallFont, new Vector2(position.X + (float)(Game1.tileSize / 2) + 8f, position.Y + (float)Game1.tileSize + (float)(i * Game1.tileSize / 2) + (float)(i * 4) + 4f), color, 1f, -1f, -1, -1, 1f, 3);
			}
			b.Draw(Game1.staminaRect, new Rectangle((int)position.X + 8, (int)position.Y + Game1.tileSize + Game1.pixelZoom + this.recipeList.Count * (Game1.tileSize / 2 + 4), width - Game1.tileSize / 2, Game1.pixelZoom / 2), Game1.textColor * 0.35f);
			Utility.drawTextWithShadow(b, Game1.parseText(this.description, Game1.smallFont, width - 8), Game1.smallFont, position + new Vector2(0f, (float)(Game1.tileSize + Game1.pixelZoom * 3 + this.recipeList.Count * (Game1.tileSize / 2 + 4))), Game1.textColor * 0.75f, 1f, -1f, -1, -1, 1f, 3);
		}

		public int getNumberOfIngredients()
		{
			return this.recipeList.Count;
		}

		public int getSpriteIndexFromRawIndex(int index)
		{
			switch (index)
			{
			case -6:
				return 184;
			case -5:
				return 176;
			case -4:
				return 145;
			case -3:
				return 24;
			case -2:
				return 80;
			case -1:
				return 20;
			default:
				return index;
			}
		}

		public string getNameFromIndex(int index)
		{
			if (index >= 0)
			{
				string result = Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.575", new object[0]);
				if (Game1.objectInformation.ContainsKey(index))
				{
					result = Game1.objectInformation[index].Split(new char[]
					{
						'/'
					})[4];
				}
				return result;
			}
			switch (index)
			{
			case -6:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.573", new object[0]);
			case -5:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.572", new object[0]);
			case -4:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.571", new object[0]);
			case -3:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.570", new object[0]);
			case -2:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.569", new object[0]);
			case -1:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.568", new object[0]);
			default:
				return "???";
			}
		}
	}
}
