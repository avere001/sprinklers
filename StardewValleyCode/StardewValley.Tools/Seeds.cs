using System;

namespace StardewValley.Tools
{
	public class Seeds : Stackable
	{
		private string seedType;

		private int numberInStack;

		public new int NumberInStack
		{
			get
			{
				return this.numberInStack;
			}
			set
			{
				this.numberInStack = value;
			}
		}

		public string SeedType
		{
			get
			{
				return this.seedType;
			}
			set
			{
				this.seedType = value;
			}
		}

		public Seeds()
		{
		}

		public Seeds(string seedType, int numberInStack) : base("Seeds", 0, 0, 0, true)
		{
			this.seedType = seedType;
			this.numberInStack = numberInStack;
			this.setCurrentTileIndexToSeedType();
			this.indexOfMenuItemView = base.CurrentParentTileIndex;
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Seeds.cs.14209", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Seeds.cs.14210", new object[0]);
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			who.Stamina -= 2f - (float)who.FarmingLevel * 0.1f;
			this.numberInStack--;
			this.setCurrentTileIndexToSeedType();
			Game1.playSound("seeds");
		}

		private void setCurrentTileIndexToSeedType()
		{
			string text = this.seedType;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 2309904358u)
			{
				if (num <= 1020152658u)
				{
					if (num <= 137760495u)
					{
						if (num != 100164663u)
						{
							if (num != 121410417u)
							{
								if (num != 137760495u)
								{
									return;
								}
								if (!(text == "Garlic"))
								{
									return;
								}
								base.CurrentParentTileIndex = 4;
								return;
							}
							else
							{
								if (!(text == "Beet"))
								{
									return;
								}
								base.CurrentParentTileIndex = 62;
								return;
							}
						}
						else
						{
							if (!(text == "Rhubarb"))
							{
								return;
							}
							base.CurrentParentTileIndex = 6;
							return;
						}
					}
					else if (num <= 321905522u)
					{
						if (num != 252262714u)
						{
							if (num != 321905522u)
							{
								return;
							}
							if (!(text == "Potato"))
							{
								return;
							}
							base.CurrentParentTileIndex = 3;
							return;
						}
						else
						{
							if (!(text == "Spring Mix"))
							{
								return;
							}
							base.CurrentParentTileIndex = 63;
							return;
						}
					}
					else if (num != 795997688u)
					{
						if (num != 1020152658u)
						{
							return;
						}
						if (!(text == "Summer Mix"))
						{
							return;
						}
						base.CurrentParentTileIndex = 64;
						return;
					}
					else
					{
						if (!(text == "Radish"))
						{
							return;
						}
						base.CurrentParentTileIndex = 12;
						return;
					}
				}
				else if (num <= 1418370675u)
				{
					if (num != 1026502717u)
					{
						if (num != 1155325948u)
						{
							if (num != 1418370675u)
							{
								return;
							}
							if (!(text == "Corn"))
							{
								return;
							}
							base.CurrentParentTileIndex = 15;
							return;
						}
						else
						{
							if (!(text == "Melon"))
							{
								return;
							}
							base.CurrentParentTileIndex = 7;
							return;
						}
					}
					else
					{
						if (!(text == "Starfruit"))
						{
							return;
						}
						base.CurrentParentTileIndex = 14;
						return;
					}
				}
				else if (num <= 1787800187u)
				{
					if (num != 1651195363u)
					{
						if (num != 1787800187u)
						{
							return;
						}
						if (!(text == "Eggplant"))
						{
							return;
						}
						base.CurrentParentTileIndex = 56;
						return;
					}
					else
					{
						if (!(text == "Tomato"))
						{
							return;
						}
						base.CurrentParentTileIndex = 8;
						return;
					}
				}
				else if (num != 2051905485u)
				{
					if (num != 2309904358u)
					{
						return;
					}
					if (!(text == "Kale"))
					{
						return;
					}
					base.CurrentParentTileIndex = 5;
					return;
				}
				else
				{
					if (!(text == "Cranberries"))
					{
						return;
					}
					base.CurrentParentTileIndex = 61;
					return;
				}
			}
			else if (num <= 3388885286u)
			{
				if (num <= 2510277530u)
				{
					if (num != 2401363451u)
					{
						if (num != 2427154736u)
						{
							if (num != 2510277530u)
							{
								return;
							}
							if (!(text == "Winter Mix"))
							{
								return;
							}
							base.CurrentParentTileIndex = 66;
							return;
						}
						else
						{
							if (!(text == "Wheat"))
							{
								return;
							}
							base.CurrentParentTileIndex = 11;
							return;
						}
					}
					else
					{
						if (!(text == "Yellow Pepper"))
						{
							return;
						}
						base.CurrentParentTileIndex = 10;
						return;
					}
				}
				else if (num <= 2981019750u)
				{
					if (num != 2837304215u)
					{
						if (num != 2981019750u)
						{
							return;
						}
						if (!(text == "Yam"))
						{
							return;
						}
						base.CurrentParentTileIndex = 60;
						return;
					}
					else
					{
						if (!(text == "Blueberry"))
						{
							return;
						}
						base.CurrentParentTileIndex = 9;
						return;
					}
				}
				else if (num != 3106606342u)
				{
					if (num != 3388885286u)
					{
						return;
					}
					if (!(text == "Bok Choy"))
					{
						return;
					}
					base.CurrentParentTileIndex = 59;
					return;
				}
				else
				{
					if (!(text == "Green Bean"))
					{
						return;
					}
					base.CurrentParentTileIndex = 1;
					return;
				}
			}
			else if (num <= 3607993668u)
			{
				if (num != 3468995526u)
				{
					if (num != 3517106355u)
					{
						if (num != 3607993668u)
						{
							return;
						}
						if (!(text == "Parsnip"))
						{
							return;
						}
						base.CurrentParentTileIndex = 0;
						return;
					}
					else
					{
						if (!(text == "Red Cabbage"))
						{
							return;
						}
						base.CurrentParentTileIndex = 13;
						return;
					}
				}
				else
				{
					if (!(text == "Fall Mix"))
					{
						return;
					}
					base.CurrentParentTileIndex = 65;
					return;
				}
			}
			else if (num <= 3782215428u)
			{
				if (num != 3772079609u)
				{
					if (num != 3782215428u)
					{
						return;
					}
					if (!(text == "Cauliflower"))
					{
						return;
					}
					base.CurrentParentTileIndex = 2;
					return;
				}
				else
				{
					if (!(text == "Ancient Fruit"))
					{
						return;
					}
					base.CurrentParentTileIndex = 72;
					return;
				}
			}
			else if (num != 3998561503u)
			{
				if (num != 4181074131u)
				{
					return;
				}
				if (!(text == "Artichoke"))
				{
					return;
				}
				base.CurrentParentTileIndex = 57;
				return;
			}
			else
			{
				if (!(text == "Pumpkin"))
				{
					return;
				}
				base.CurrentParentTileIndex = 58;
				return;
			}
		}
	}
}
