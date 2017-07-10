using Microsoft.Xna.Framework;
using System;

namespace StardewValley.Tools
{
	public class Raft : Tool
	{
		public Raft() : base("Raft", 0, 1, 1, false, 0)
		{
			this.upgradeLevel = 0;
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			this.instantUse = true;
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Raft.cs.14204", new object[0]);
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Raft.cs.14205", new object[0]);
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			if (!who.isRafting && location.doesTileHaveProperty(x / Game1.tileSize, y / Game1.tileSize, "Water", "Back") != null)
			{
				who.isRafting = true;
				Rectangle position = new Rectangle(x - Game1.tileSize / 2, y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize);
				if (location.isCollidingPosition(position, Game1.viewport, true))
				{
					who.isRafting = false;
					return;
				}
				who.xVelocity = ((who.facingDirection == 1) ? 3f : ((who.facingDirection == 3) ? -3f : 0f));
				who.yVelocity = ((who.facingDirection == 2) ? 3f : ((who.facingDirection == 0) ? -3f : 0f));
				who.position = new Vector2((float)(x - Game1.tileSize / 2), (float)(y - Game1.tileSize / 2 - Game1.tileSize / 2 - ((y < who.getStandingY()) ? Game1.tileSize : 0)));
				Game1.playSound("dropItemInWater");
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
		}
	}
}
