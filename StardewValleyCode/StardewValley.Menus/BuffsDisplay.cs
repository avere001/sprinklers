using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
	public class BuffsDisplay : IClickableMenu
	{
		public const int fullnessLength = 180000;

		public const int quenchedLength = 60000;

		public static int sideSpace = Game1.tileSize / 2;

		public new static int width = Game1.tileSize * 4 + Game1.tileSize / 2;

		private Dictionary<ClickableTextureComponent, Buff> buffs = new Dictionary<ClickableTextureComponent, Buff>();

		public Buff food;

		public Buff drink;

		public List<Buff> otherBuffs = new List<Buff>();

		public int fullnessLeft;

		public int quenchedLeft;

		public string hoverText = "";

		public BuffsDisplay() : base(Game1.viewport.Width - 320 - BuffsDisplay.sideSpace - BuffsDisplay.width, Game1.tileSize / 8, BuffsDisplay.width, Game1.tileSize, false)
		{
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		private void updatePosition()
		{
			int num = 320 + BuffsDisplay.sideSpace + BuffsDisplay.width;
			this.xPositionOnScreen = Game1.viewport.Width - num;
			Vector2 vector = new Vector2((float)this.xPositionOnScreen, 0f);
			Utility.makeSafe(ref vector, num, 0);
			this.xPositionOnScreen = (int)vector.X;
			this.syncIcons();
		}

		public override void performHoverAction(int x, int y)
		{
			this.updatePosition();
			this.hoverText = "";
			foreach (KeyValuePair<ClickableTextureComponent, Buff> current in this.buffs)
			{
				if (current.Key.containsPoint(x, y))
				{
					this.hoverText = current.Key.hoverText + Environment.NewLine + current.Value.getTimeLeft();
					current.Key.scale = Math.Min(current.Key.baseScale + 0.1f, current.Key.scale + 0.02f);
					break;
				}
			}
		}

		public void arrangeTheseComponentsInThisRectangle(int rectangleX, int rectangleY, int rectangleWidthInComponentWidthUnits, int componentWidth, int componentHeight, int buffer, bool rightToLeft)
		{
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<ClickableTextureComponent, Buff> current in this.buffs)
			{
				ClickableTextureComponent key = current.Key;
				if (rightToLeft)
				{
					key.bounds = new Rectangle(rectangleX + rectangleWidthInComponentWidthUnits * componentWidth - (num + 1) * (componentWidth + buffer), rectangleY + num2 * (componentHeight + buffer), componentWidth, componentHeight);
				}
				else
				{
					key.bounds = new Rectangle(rectangleX + num * (componentWidth + buffer), rectangleY + num2 * (componentHeight + buffer), componentWidth, componentHeight);
				}
				num++;
				if (num > rectangleWidthInComponentWidthUnits)
				{
					num2++;
					num %= rectangleWidthInComponentWidthUnits;
				}
			}
		}

		public void syncIcons()
		{
			this.buffs.Clear();
			if (this.food != null)
			{
				foreach (ClickableTextureComponent current in this.food.getClickableComponents())
				{
					this.buffs.Add(current, this.food);
				}
			}
			if (this.drink != null)
			{
				foreach (ClickableTextureComponent current2 in this.drink.getClickableComponents())
				{
					this.buffs.Add(current2, this.drink);
				}
			}
			foreach (Buff current3 in this.otherBuffs)
			{
				foreach (ClickableTextureComponent current4 in current3.getClickableComponents())
				{
					this.buffs.Add(current4, current3);
				}
			}
			this.arrangeTheseComponentsInThisRectangle(this.xPositionOnScreen, Game1.tileSize / 8, BuffsDisplay.width / Game1.tileSize, Game1.tileSize, Game1.tileSize, Game1.tileSize / 8, true);
		}

		public bool hasBuff(int which)
		{
			using (List<Buff>.Enumerator enumerator = this.otherBuffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.which == which)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool tryToAddFoodBuff(Buff b, int duration)
		{
			if (b.total > 0 && this.fullnessLeft <= 0)
			{
				if (this.food != null)
				{
					this.food.removeBuff();
				}
				this.food = b;
				this.food.addBuff();
				this.syncIcons();
				return true;
			}
			return false;
		}

		public bool tryToAddDrinkBuff(Buff b)
		{
			if (b.source.Contains("Beer") || b.source.Contains("Wine") || b.source.Contains("Mead") || b.source.Contains("Pale Ale"))
			{
				this.addOtherBuff(new Buff(17));
			}
			else if (b.source.Equals("Oil of Garlic"))
			{
				this.addOtherBuff(new Buff(23));
			}
			else if (b.source.Equals("Life Elixir"))
			{
				Game1.player.health = Game1.player.maxHealth;
			}
			else if (b.source.Equals("Muscle Remedy"))
			{
				Game1.player.exhausted = false;
			}
			if (b.total > 0 && this.quenchedLeft <= 0)
			{
				if (this.drink != null)
				{
					this.drink.removeBuff();
				}
				this.drink = b;
				this.drink.addBuff();
				this.syncIcons();
				return true;
			}
			return false;
		}

		public bool addOtherBuff(Buff buff)
		{
			if (buff.which != -1)
			{
				foreach (KeyValuePair<ClickableTextureComponent, Buff> current in this.buffs)
				{
					if (buff.which == current.Value.which)
					{
						current.Value.millisecondsDuration = buff.millisecondsDuration;
						current.Key.scale = current.Key.baseScale + 0.2f;
						return false;
					}
				}
			}
			this.otherBuffs.Add(buff);
			buff.addBuff();
			this.syncIcons();
			return true;
		}

		public new void update(GameTime time)
		{
			this.updatePosition();
			if (this.food != null && this.food.update(time))
			{
				this.food.removeBuff();
				this.food = null;
				this.syncIcons();
			}
			if (this.drink != null && this.drink.update(time))
			{
				this.drink.removeBuff();
				this.drink = null;
				this.syncIcons();
			}
			for (int i = this.otherBuffs.Count - 1; i >= 0; i--)
			{
				if (this.otherBuffs[i].update(time))
				{
					this.otherBuffs[i].removeBuff();
					this.otherBuffs.RemoveAt(i);
					this.syncIcons();
				}
			}
			foreach (KeyValuePair<ClickableTextureComponent, Buff> current in this.buffs)
			{
				ClickableTextureComponent key = current.Key;
				key.scale = Math.Max(key.baseScale, key.scale - 0.01f);
			}
		}

		public void clearAllBuffs()
		{
			this.otherBuffs.Clear();
			if (this.food != null)
			{
				this.food.removeBuff();
				this.food = null;
			}
			if (this.drink != null)
			{
				this.drink.removeBuff();
				this.drink = null;
			}
			this.buffs.Clear();
		}

		public override void draw(SpriteBatch b)
		{
			this.updatePosition();
			foreach (KeyValuePair<ClickableTextureComponent, Buff> current in this.buffs)
			{
				current.Key.draw(b);
			}
			if (this.hoverText.Length != 0 && this.isWithinBounds(Game1.getOldMouseX(), Game1.getOldMouseY()))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}
	}
}
