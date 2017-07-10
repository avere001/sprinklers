using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
	public class DescriptionElement
	{
		public string xmlKey;

		public List<object> param;

		public static implicit operator DescriptionElement(string key)
		{
			return new DescriptionElement(key);
		}

		public DescriptionElement()
		{
			this.xmlKey = string.Empty;
			this.param = new List<object>();
		}

		public DescriptionElement(string key)
		{
			this.xmlKey = key;
			this.param = new List<object>();
		}

		public DescriptionElement(string key, object param1)
		{
			this.xmlKey = key;
			this.param = new List<object>();
			this.param.Add(param1);
		}

		public DescriptionElement(string key, List<object> paramlist)
		{
			this.xmlKey = key;
			this.param = new List<object>();
			foreach (object current in paramlist)
			{
				this.param.Add(current);
			}
		}

		public DescriptionElement(string key, object param1, object param2)
		{
			this.xmlKey = key;
			this.param = new List<object>();
			this.param.Add(param1);
			this.param.Add(param2);
		}

		public DescriptionElement(string key, object param1, object param2, object param3)
		{
			this.xmlKey = key;
			this.param = new List<object>();
			this.param.Add(param1);
			this.param.Add(param2);
			this.param.Add(param3);
		}

		public string loadDescriptionElement()
		{
			DescriptionElement descriptionElement = new DescriptionElement(this.xmlKey, this.param);
			for (int i = 0; i < descriptionElement.param.Count; i++)
			{
				if (descriptionElement.param[i].GetType() == typeof(DescriptionElement))
				{
					DescriptionElement descriptionElement2 = descriptionElement.param[i] as DescriptionElement;
					descriptionElement.param[i] = descriptionElement2.loadDescriptionElement();
				}
				if (descriptionElement.param[i].GetType() == typeof(StardewValley.Object))
				{
					string text;
					Game1.objectInformation.TryGetValue((descriptionElement.param[i] as StardewValley.Object).parentSheetIndex, out text);
					descriptionElement.param[i] = text.Split(new char[]
					{
						'/'
					})[4];
				}
				if (descriptionElement.param[i].GetType() == typeof(Monster))
				{
					DescriptionElement descriptionElement3;
					if ((descriptionElement.param[i] as Monster).name.Equals("Frost Jelly"))
					{
						descriptionElement3 = new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13772");
						descriptionElement.param[i] = descriptionElement3.loadDescriptionElement();
					}
					else
					{
						descriptionElement3 = new DescriptionElement("Data\\Monsters:" + (descriptionElement.param[i] as Monster).name);
						descriptionElement.param[i] = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en) ? (descriptionElement3.loadDescriptionElement().Split(new char[]
						{
							'/'
						}).Last<string>() + "s") : descriptionElement3.loadDescriptionElement().Split(new char[]
						{
							'/'
						}).Last<string>());
					}
					descriptionElement.param[i] = descriptionElement3.loadDescriptionElement().Split(new char[]
					{
						'/'
					}).Last<string>();
				}
				if (descriptionElement.param[i].GetType() == typeof(NPC))
				{
					DescriptionElement descriptionElement4 = new DescriptionElement("Data\\NPCDispositions:" + (descriptionElement.param[i] as NPC).name);
					descriptionElement.param[i] = descriptionElement4.loadDescriptionElement().Split(new char[]
					{
						'/'
					}).Last<string>();
				}
			}
			if (descriptionElement.xmlKey == "")
			{
				return string.Empty;
			}
			switch (descriptionElement.param.Count)
			{
			case 0:
			{
				IL_29E:
				string text2 = Game1.content.LoadString(descriptionElement.xmlKey, new object[0]);
				if (this.xmlKey.Contains("Dialogue.cs.7") || this.xmlKey.Contains("Dialogue.cs.8"))
				{
					text2 = Game1.content.LoadString(descriptionElement.xmlKey, new object[0]).Replace("/", " ");
					text2 = ((text2[0] == ' ') ? text2.Substring(1) : text2);
					return text2;
				}
				return text2;
			}
			case 1:
			{
				string text2 = Game1.content.LoadString(descriptionElement.xmlKey, new object[]
				{
					descriptionElement.param[0]
				});
				return text2;
			}
			case 2:
			{
				string text2 = Game1.content.LoadString(descriptionElement.xmlKey, new object[]
				{
					descriptionElement.param[0],
					descriptionElement.param[1]
				});
				return text2;
			}
			case 3:
			{
				string text2 = Game1.content.LoadString(descriptionElement.xmlKey, new object[]
				{
					descriptionElement.param[0],
					descriptionElement.param[1],
					descriptionElement.param[2]
				});
				return text2;
			}
			case 4:
			{
				string text2 = Game1.content.LoadString(descriptionElement.xmlKey, new object[]
				{
					descriptionElement.param[0],
					descriptionElement.param[1],
					descriptionElement.param[2],
					descriptionElement.param[3]
				});
				return text2;
			}
			}
			goto IL_29E;
		}
	}
}
