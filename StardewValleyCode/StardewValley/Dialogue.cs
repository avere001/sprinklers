using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StardewValley
{
	public class Dialogue
	{
		public delegate bool onAnswerQuestion(int whichResponse);

		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			public static readonly Dialogue.<>c <>9 = new Dialogue.<>c();

			public static LocalizedContentManager.LanguageChangedHandler <>9__64_0;

			public static Func<NPCDialogueResponse, Response> <>9__83_0;

			internal void ctor>b__64_0(LocalizedContentManager.LanguageCode code)
			{
				Dialogue.TranslateArraysOfStrings();
			}

			internal Response <getResponseOptions>b__83_0(NPCDialogueResponse x)
			{
				return x;
			}
		}

		public const string dialogueHappy = "$h";

		public const string dialogueSad = "$s";

		public const string dialogueUnique = "$u";

		public const string dialogueNeutral = "$neutral";

		public const string dialogueLove = "$l";

		public const string dialogueAngry = "$a";

		public const string dialogueEnd = "$e";

		public const string dialogueBreak = "$b";

		public const string dialogueKill = "$k";

		public const string dialogueChance = "$c";

		public const string dialogueDependingOnWorldState = "$d";

		public const string dialogueQuickResponse = "$y";

		public const string dialoguePrerequisite = "$p";

		public const string dialogueSingle = "$1";

		public const string dialogueQuestion = "$q";

		public const string dialogueResponse = "$r";

		public const string breakSpecialCharacter = "{";

		public const string playerNameSpecialCharacter = "@";

		public const string genderDialogueSplitCharacter = "^";

		public const string genderDialogueSplitCharacter2 = "¦";

		public const string quickResponseDelineator = "*";

		public const string randomAdjectiveSpecialCharacter = "%adj";

		public const string randomNounSpecialCharacter = "%noun";

		public const string randomPlaceSpecialCharacter = "%place";

		public const string spouseSpecialCharacter = "%spouse";

		public const string randomNameSpecialCharacter = "%name";

		public const string firstNameLettersSpecialCharacter = "%firstnameletter";

		public const string timeSpecialCharacter = "%time";

		public const string bandNameSpecialCharacter = "%band";

		public const string bookNameSpecialCharacter = "%book";

		public const string rivalSpecialCharacter = "%rival";

		public const string petSpecialCharacter = "%pet";

		public const string farmNameSpecialCharacter = "%farm";

		public const string favoriteThingSpecialCharacter = "%favorite";

		public const string eventForkSpecialCharacter = "%fork";

		public const string kid1specialCharacter = "%kid1";

		public const string kid2SpecialCharacter = "%kid2";

		private static bool nameArraysTranslated = false;

		public static string[] adjectives = new string[]
		{
			"Purple",
			"Gooey",
			"Chalky",
			"Green",
			"Plush",
			"Chunky",
			"Gigantic",
			"Greasy",
			"Gloomy",
			"Practical",
			"Lanky",
			"Dopey",
			"Crusty",
			"Fantastic",
			"Rubbery",
			"Silly",
			"Courageous",
			"Reasonable",
			"Lonely",
			"Bitter"
		};

		public static string[] nouns = new string[]
		{
			"Dragon",
			"Buffet",
			"Biscuit",
			"Robot",
			"Planet",
			"Pepper",
			"Tomb",
			"Hyena",
			"Lip",
			"Quail",
			"Cheese",
			"Disaster",
			"Raincoat",
			"Shoe",
			"Castle",
			"Elf",
			"Pump",
			"Crisp",
			"Wig",
			"Mermaid",
			"Drumstick",
			"Puppet",
			"Submarine"
		};

		public static string[] verbs = new string[]
		{
			"ran",
			"danced",
			"spoke",
			"galloped",
			"ate",
			"floated",
			"stood",
			"flowed",
			"smelled",
			"swam",
			"grilled",
			"cracked",
			"melted"
		};

		public static string[] positional = new string[]
		{
			"atop",
			"near",
			"with",
			"alongside",
			"away from",
			"too close to",
			"dangerously close to",
			"far, far away from",
			"uncomfortably close to",
			"way above the",
			"miles below",
			"on a different planet from",
			"in a different century than"
		};

		public static string[] places = new string[]
		{
			"Castle Village",
			"Basket Town",
			"Pine Mesa City",
			"Point Drake",
			"Minister Valley",
			"Grampleton",
			"Zuzu City",
			"a small island off the coast",
			"Fort Josa",
			"Chestervale",
			"Fern Islands",
			"Tanker Grove"
		};

		public static string[] colors = new string[]
		{
			"/crimson",
			"/green",
			"/tan",
			"/purple",
			"/deep blue",
			"/neon pink",
			"/pale/yellow",
			"/chocolate/brown",
			"/sky/blue",
			"/bubblegum/pink",
			"/blood/red",
			"/bright/orange",
			"/aquamarine",
			"/silvery",
			"/glimmering/gold",
			"/rainbow"
		};

		private List<string> dialogues = new List<string>();

		private List<NPCDialogueResponse> playerResponses;

		private List<string> quickResponses;

		private bool isLastDialogueInteractive;

		private bool quickResponse;

		public bool isCurrentStringContinuedOnNextScreen;

		private bool dialogueToBeKilled;

		private bool finishedLastDialogue;

		public bool showPortrait;

		public bool removeOnNextMove;

		public int currentDialogueIndex;

		private string currentEmotion;

		public string temporaryDialogue;

		public NPC speaker;

		public Dialogue.onAnswerQuestion answerQuestionBehavior;

		public string CurrentEmotion
		{
			get
			{
				return this.currentEmotion;
			}
			set
			{
				this.currentEmotion = value;
			}
		}

		private static void TranslateArraysOfStrings()
		{
			Dialogue.colors = new string[]
			{
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.795", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.796", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.797", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.798", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.799", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.800", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.801", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.802", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.803", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.804", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.805", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.806", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.807", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.808", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.809", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.810", new object[0])
			};
			Dialogue.adjectives = new string[]
			{
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.679", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.680", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.681", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.682", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.683", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.684", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.685", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.686", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.687", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.688", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.689", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.690", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.691", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.692", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.693", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.694", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.695", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.696", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.697", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.698", new object[0])
			};
			Dialogue.nouns = new string[]
			{
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.699", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.700", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.701", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.702", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.703", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.704", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.705", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.706", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.707", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.708", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.709", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.710", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.711", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.712", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.713", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.714", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.715", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.716", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.717", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.718", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.719", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.720", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.721", new object[0])
			};
			Dialogue.verbs = new string[]
			{
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.722", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.723", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.724", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.725", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.726", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.727", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.728", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.729", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.730", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.731", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.732", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.733", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.734", new object[0])
			};
			Dialogue.positional = new string[]
			{
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.735", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.736", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.737", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.738", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.739", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.740", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.741", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.742", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.743", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.744", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.745", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.746", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.747", new object[0])
			};
			Dialogue.places = new string[]
			{
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.748", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.749", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.750", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.751", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.752", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.753", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.754", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.755", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.756", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.757", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.758", new object[0]),
				Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.759", new object[0])
			};
			Dialogue.nameArraysTranslated = true;
		}

		public Dialogue(string masterDialogue, NPC speaker)
		{
			if (!Dialogue.nameArraysTranslated)
			{
				Dialogue.TranslateArraysOfStrings();
				LocalizedContentManager.LanguageChangedHandler arg_3C_0;
				if ((arg_3C_0 = Dialogue.<>c.<>9__64_0) == null)
				{
					arg_3C_0 = (Dialogue.<>c.<>9__64_0 = new LocalizedContentManager.LanguageChangedHandler(Dialogue.<>c.<>9.<.ctor>b__64_0));
				}
				LocalizedContentManager.OnLanguageChange += arg_3C_0;
			}
			this.speaker = speaker;
			this.parseDialogueString(masterDialogue);
			this.checkForSpecialDialogueAttributes();
		}

		public void setCurrentDialogue(string dialogue)
		{
			this.dialogues.Clear();
			this.currentDialogueIndex = 0;
			this.parseDialogueString(dialogue);
		}

		public void addMessageToFront(string dialogue)
		{
			this.currentDialogueIndex = 0;
			List<string> list = new List<string>();
			list.AddRange(this.dialogues);
			this.dialogues.Clear();
			this.parseDialogueString(dialogue);
			this.dialogues.AddRange(list);
			this.checkForSpecialDialogueAttributes();
		}

		public static string getRandomVerb()
		{
			if (!Dialogue.nameArraysTranslated)
			{
				Dialogue.TranslateArraysOfStrings();
			}
			return Dialogue.verbs[Game1.random.Next(Dialogue.verbs.Count<string>())];
		}

		public static string getRandomAdjective()
		{
			if (!Dialogue.nameArraysTranslated)
			{
				Dialogue.TranslateArraysOfStrings();
			}
			return Dialogue.adjectives[Game1.random.Next(Dialogue.adjectives.Count<string>())];
		}

		public static string getRandomNoun()
		{
			if (!Dialogue.nameArraysTranslated)
			{
				Dialogue.TranslateArraysOfStrings();
			}
			return Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Count<string>())];
		}

		public static string getRandomPositional()
		{
			if (!Dialogue.nameArraysTranslated)
			{
				Dialogue.TranslateArraysOfStrings();
			}
			return Dialogue.positional[Game1.random.Next(Dialogue.positional.Count<string>())];
		}

		public int getPortraitIndex()
		{
			string a = this.currentEmotion;
			if (a == "$neutral")
			{
				return 0;
			}
			if (a == "$h")
			{
				return 1;
			}
			if (a == "$s")
			{
				return 2;
			}
			if (a == "$u")
			{
				return 3;
			}
			if (a == "$l")
			{
				return 4;
			}
			if (a == "$a")
			{
				return 5;
			}
			int num;
			if (int.TryParse(this.currentEmotion.Substring(1), out num))
			{
				return Convert.ToInt32(this.currentEmotion.Substring(1));
			}
			return 0;
		}

		private void parseDialogueString(string masterString)
		{
			if (masterString == null)
			{
				masterString = "...";
			}
			this.temporaryDialogue = null;
			if (this.playerResponses != null)
			{
				this.playerResponses.Clear();
			}
			string[] array = masterString.Split(new char[]
			{
				'#'
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length >= 2)
				{
					array[i] = this.checkForSpecialCharacters(array[i]);
					string text;
					try
					{
						text = array[i].Substring(0, 2);
					}
					catch (Exception)
					{
						text = "     ";
					}
					if (!text.Equals("$e"))
					{
						if (text.Equals("$b"))
						{
							if (this.dialogues.Count > 0)
							{
								List<string> list = this.dialogues;
								int index = this.dialogues.Count - 1;
								list[index] += "{";
							}
						}
						else if (text.Equals("$k"))
						{
							this.dialogueToBeKilled = true;
						}
						else if (text.Equals("$1") && array[i].Split(new char[]
						{
							' '
						}).Length > 1)
						{
							string text2 = array[i].Split(new char[]
							{
								' '
							})[1];
							if (!Game1.player.mailReceived.Contains(text2))
							{
								array[i + 1] = this.checkForSpecialCharacters(array[i + 1]);
								this.dialogues.Add(text2 + "}" + array[i + 1]);
								i = 99999;
								return;
							}
							i += 3;
							array[i] = this.checkForSpecialCharacters(array[i]);
						}
						else if (text.Equals("$c") && array[i].Split(new char[]
						{
							' '
						}).Length > 1)
						{
							double num = Convert.ToDouble(array[i].Split(new char[]
							{
								' '
							})[1]);
							if (Game1.random.NextDouble() > num)
							{
								i++;
							}
							else
							{
								this.dialogues.Add(array[i + 1]);
								i += 2;
							}
						}
						else if (text.Equals("$q"))
						{
							if (this.dialogues.Count > 0)
							{
								List<string> list = this.dialogues;
								int index = this.dialogues.Count - 1;
								list[index] += "{";
							}
							string[] array2 = array[i].Split(new char[]
							{
								' '
							});
							string[] array3 = array2[1].Split(new char[]
							{
								'/'
							});
							bool flag = false;
							for (int j = 0; j < array3.Length; j++)
							{
								if (Game1.player.DialogueQuestionsAnswered.Contains(Convert.ToInt32(array3[j])))
								{
									flag = true;
									break;
								}
							}
							if (flag && Convert.ToInt32(array3[0]) != -1)
							{
								if (!array2[2].Equals("null"))
								{
									array = array.Take(i).ToArray<string>().Concat(this.speaker.Dialogue[array2[2]].Split(new char[]
									{
										'#'
									})).ToArray<string>();
									i--;
								}
							}
							else
							{
								this.isLastDialogueInteractive = true;
							}
						}
						else if (text.Equals("$r"))
						{
							string[] array4 = array[i].Split(new char[]
							{
								' '
							});
							if (this.playerResponses == null)
							{
								this.playerResponses = new List<NPCDialogueResponse>();
							}
							this.isLastDialogueInteractive = true;
							this.playerResponses.Add(new NPCDialogueResponse(Convert.ToInt32(array4[1]), Convert.ToInt32(array4[2]), array4[3], array[i + 1]));
							i++;
						}
						else if (text.Equals("$p"))
						{
							string[] array5 = array[i].Split(new char[]
							{
								' '
							});
							string[] array6 = array[i + 1].Split(new char[]
							{
								'|'
							});
							bool flag2 = false;
							for (int k = 1; k < array5.Length; k++)
							{
								if (Game1.player.DialogueQuestionsAnswered.Contains(Convert.ToInt32(array5[1])))
								{
									flag2 = true;
									break;
								}
							}
							if (flag2)
							{
								array = array6[0].Split(new char[]
								{
									'#'
								});
								i = -1;
							}
							else
							{
								array[i + 1] = array[i + 1].Split(new char[]
								{
									'|'
								}).Last<string>();
							}
						}
						else if (text.Equals("$d"))
						{
							string[] arg_474_0 = array[i].Split(new char[]
							{
								' '
							});
							string text3 = masterString.Substring(masterString.IndexOf('#') + 1);
							bool flag3 = false;
							string a = arg_474_0[1].ToLower();
							if (!(a == "joja"))
							{
								if (!(a == "cc") && !(a == "communitycenter"))
								{
									if (a == "bus")
									{
										flag3 = Game1.player.mailReceived.Contains("ccVault");
									}
								}
								else
								{
									flag3 = Game1.isLocationAccessible("CommunityCenter");
								}
							}
							else
							{
								flag3 = Game1.isLocationAccessible("JojaMart");
							}
							char c = text3.Contains('|') ? '|' : '#';
							if (flag3)
							{
								array = new string[]
								{
									text3.Split(new char[]
									{
										c
									})[0]
								};
							}
							else
							{
								array = new string[]
								{
									text3.Split(new char[]
									{
										c
									})[1]
								};
							}
							i--;
						}
						else if (text.Equals("$y"))
						{
							this.quickResponse = true;
							this.isLastDialogueInteractive = true;
							if (this.quickResponses == null)
							{
								this.quickResponses = new List<string>();
							}
							if (this.playerResponses == null)
							{
								this.playerResponses = new List<NPCDialogueResponse>();
							}
							string text4 = array[i].Substring(array[i].IndexOf('\'') + 1);
							text4 = text4.Substring(0, text4.Length - 1);
							string[] array7 = text4.Split(new char[]
							{
								'_'
							});
							this.dialogues.Add(array7[0]);
							for (int l = 1; l < array7.Length; l += 2)
							{
								this.playerResponses.Add(new NPCDialogueResponse(-1, -1, "quickResponse" + l, Game1.parseText(array7[l])));
								this.quickResponses.Add(array7[l + 1].Replace("*", "#$b#"));
							}
						}
						else if (array[i].Contains("^"))
						{
							if (Game1.player.IsMale)
							{
								this.dialogues.Add(array[i].Substring(0, array[i].IndexOf("^")));
							}
							else
							{
								this.dialogues.Add(array[i].Substring(array[i].IndexOf("^") + 1));
							}
						}
						else if (array[i].Contains("¦"))
						{
							if (Game1.player.IsMale)
							{
								this.dialogues.Add(array[i].Substring(0, array[i].IndexOf("¦")));
							}
							else
							{
								this.dialogues.Add(array[i].Substring(array[i].IndexOf("¦") + 1));
							}
						}
						else
						{
							this.dialogues.Add(array[i]);
						}
					}
				}
			}
		}

		public string getCurrentDialogue()
		{
			if (this.currentDialogueIndex >= this.dialogues.Count || this.finishedLastDialogue)
			{
				return "";
			}
			this.showPortrait = true;
			if (this.speaker.name.Equals("Dwarf") && !Game1.player.canUnderstandDwarves)
			{
				return Dialogue.convertToDwarvish(this.dialogues[this.currentDialogueIndex]);
			}
			if (this.temporaryDialogue != null)
			{
				return this.temporaryDialogue;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("}"))
			{
				Game1.player.mailReceived.Add(this.dialogues[this.currentDialogueIndex].Split(new char[]
				{
					'}'
				})[0]);
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Substring(this.dialogues[this.currentDialogueIndex].IndexOf("}") + 1);
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$k", "");
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains('['))
			{
				string text = this.dialogues[this.currentDialogueIndex].Substring(this.dialogues[this.currentDialogueIndex].IndexOf('[') + 1, this.dialogues[this.currentDialogueIndex].IndexOf(']') - this.dialogues[this.currentDialogueIndex].IndexOf('[') - 1);
				string[] array = text.Split(new char[]
				{
					' '
				});
				Game1.player.addItemToInventoryBool(new Object(Vector2.Zero, Convert.ToInt32(array[Game1.random.Next(array.Length)]), null, false, true, false, false), true);
				Game1.player.showCarrying();
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("[" + text + "]", "");
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$k"))
			{
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$k", "");
				this.dialogues.RemoveRange(this.currentDialogueIndex + 1, this.dialogues.Count - 1 - this.currentDialogueIndex);
				this.finishedLastDialogue = true;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Length > 1 && this.dialogues[this.currentDialogueIndex][0] == '%')
			{
				this.showPortrait = false;
				return this.dialogues[this.currentDialogueIndex].Substring(1);
			}
			if (this.dialogues.Count<string>() <= 0)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.792", new object[0]);
			}
			return this.dialogues[this.currentDialogueIndex].Replace("%time", Game1.getTimeOfDayString(Game1.timeOfDay));
		}

		public bool isItemGrabDialogue()
		{
			return this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains('[');
		}

		public bool isOnFinalDialogue()
		{
			return this.currentDialogueIndex == this.dialogues.Count - 1;
		}

		public bool isDialogueFinished()
		{
			return this.finishedLastDialogue;
		}

		public string checkForSpecialCharacters(string str)
		{
			str = str.Replace("@", Game1.player.Name);
			str = str.Replace("%adj", Dialogue.adjectives[Game1.random.Next(Dialogue.adjectives.Length)].ToLower());
			if (str.Contains("%noun"))
			{
				str = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.de) ? (str.Substring(0, str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)]) + str.Substring(str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)])) : (str.Substring(0, str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)].ToLower()) + str.Substring(str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)].ToLower())));
			}
			str = str.Replace("%place", Dialogue.places[Game1.random.Next(Dialogue.places.Length)]);
			str = str.Replace("%name", Dialogue.randomName());
			str = str.Replace("%firstnameletter", Game1.player.Name.Substring(0, Math.Max(0, Game1.player.Name.Length / 2)));
			str = str.Replace("%band", Game1.samBandName);
			str = str.Replace("%book", Game1.elliottBookName);
			if (!string.IsNullOrEmpty(str) && str.Contains("%spouse"))
			{
				string[] array = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions")[Game1.player.spouse].Split(new char[]
				{
					'/'
				});
				str = str.Replace("%spouse", array[array.Length - 1]);
			}
			str = str.Replace("%farm", Game1.player.farmName);
			str = str.Replace("%favorite", Game1.player.favoriteThing);
			int numberOfChildren = Game1.player.getNumberOfChildren();
			str = str.Replace("%kid1", (numberOfChildren > 0) ? Game1.player.getChildren()[0].displayName : Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.793", new object[0]));
			str = str.Replace("%kid2", (numberOfChildren > 1) ? Game1.player.getChildren()[1].displayName : Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.794", new object[0]));
			str = str.Replace("%pet", Game1.player.getPetDisplayName());
			if (str.Contains("¦"))
			{
				str = (Game1.player.IsMale ? str.Substring(0, str.IndexOf("¦")) : str.Substring(str.IndexOf("¦") + 1));
			}
			if (str.Contains("%fork"))
			{
				str = str.Replace("%fork", "");
				if (Game1.currentLocation.currentEvent != null)
				{
					Game1.currentLocation.currentEvent.specialEventVariable1 = true;
				}
			}
			str = str.Replace("%rival", Utility.getOtherFarmerNames()[0].Split(new char[]
			{
				' '
			})[1]);
			return str;
		}

		public static string randomName()
		{
			string text = "";
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja)
			{
				string[] array = new string[]
				{
					"ローゼン",
					"ミルド",
					"ココ",
					"ナミ",
					"こころ",
					"サルコ",
					"ハンゾー",
					"クッキー",
					"ココナツ",
					"せん",
					"ハル",
					"ラン",
					"オサム",
					"ヨシ",
					"ソラ",
					"ホシ",
					"まこと",
					"マサ",
					"ナナ",
					"リオ",
					"リン",
					"フジ",
					"うどん",
					"ミント",
					"さくら",
					"ボンボン",
					"レオ",
					"モリ",
					"コーヒー",
					"ミルク",
					"マロン",
					"クルミ",
					"サムライ",
					"カミ",
					"ゴロ",
					"マル",
					"チビ",
					"ユキダマ"
				};
				text = array[new Random().Next(0, array.Length)];
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh)
			{
				string[] array2 = new string[]
				{
					"雨果",
					"蛋挞",
					"小百合",
					"毛毛",
					"小雨",
					"小溪",
					"精灵",
					"安琪儿",
					"小糕",
					"玫瑰",
					"小黄",
					"晓雨",
					"阿江",
					"铃铛",
					"马琪",
					"果粒",
					"郁金香",
					"小黑",
					"雨露",
					"小江",
					"灵力",
					"萝拉",
					"豆豆",
					"小莲",
					"斑点",
					"小雾",
					"阿川",
					"丽丹",
					"玛雅",
					"阿豆",
					"花花",
					"琉璃",
					"滴答",
					"阿山",
					"丹麦",
					"梅西",
					"橙子",
					"花儿",
					"晓璃",
					"小夕",
					"山大",
					"咪咪",
					"卡米",
					"红豆",
					"花朵",
					"洋洋",
					"太阳",
					"小岩",
					"汪汪",
					"玛利亚",
					"小菜",
					"花瓣",
					"阳阳",
					"小夏",
					"石头",
					"阿狗",
					"邱洁",
					"苹果",
					"梨花",
					"小希",
					"天天",
					"浪子",
					"阿猫",
					"艾薇儿",
					"雪梨",
					"桃花",
					"阿喜",
					"云朵",
					"风儿",
					"狮子",
					"绮丽",
					"雪莉",
					"樱花",
					"小喜",
					"朵朵",
					"田田",
					"小红",
					"宝娜",
					"梅子",
					"小樱",
					"嘻嘻",
					"云儿",
					"小草",
					"小黄",
					"纳香",
					"阿梅",
					"茶花",
					"哈哈",
					"芸儿",
					"东东",
					"小羽",
					"哈豆",
					"桃子",
					"茶叶",
					"双双",
					"沫沫",
					"楠楠",
					"小爱",
					"麦当娜",
					"杏仁",
					"椰子",
					"小王",
					"泡泡",
					"小林",
					"小灰",
					"马格",
					"鱼蛋",
					"小叶",
					"小李",
					"晨晨",
					"小琳",
					"小慧",
					"布鲁",
					"晓梅",
					"绿叶",
					"甜豆",
					"小雪",
					"晓林",
					"康康",
					"安妮",
					"樱桃",
					"香板",
					"甜甜",
					"雪花",
					"虹儿",
					"美美",
					"葡萄",
					"薇儿",
					"金豆",
					"雪玲",
					"瑶瑶",
					"龙眼",
					"丁香",
					"晓云",
					"雪豆",
					"琪琪",
					"麦子",
					"糖果",
					"雪丽",
					"小艺",
					"小麦",
					"小圆",
					"雨佳",
					"小火",
					"麦茶",
					"圆圆",
					"春儿",
					"火灵",
					"板子",
					"黑点",
					"冬冬",
					"火花",
					"米粒",
					"喇叭",
					"晓秋",
					"跟屁虫",
					"米果",
					"欢欢",
					"爱心",
					"松子",
					"丫头",
					"双子",
					"豆芽",
					"小子",
					"彤彤",
					"棉花糖",
					"阿贵",
					"仙儿",
					"冰淇淋",
					"小彬",
					"贤儿",
					"冰棒",
					"仔仔",
					"格子",
					"水果",
					"悠悠",
					"莹莹",
					"巧克力",
					"梦洁",
					"汤圆",
					"静香",
					"茄子",
					"珍珠"
				};
				text = array2[new Random().Next(0, array2.Length)];
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru)
			{
				string[] array3 = new string[]
				{
					"Августина",
					"Альф",
					"Анфиса",
					"Ариша",
					"Афоня",
					"Баламут",
					"Балкан",
					"Бандит",
					"Бланка",
					"Бобик",
					"Боня",
					"Борька",
					"Буренка",
					"Бусинка",
					"Вася",
					"Гаврюша",
					"Глаша",
					"Гоша",
					"Дуня",
					"Дуся",
					"Зорька",
					"Ивонна",
					"Игнат",
					"Кеша",
					"Клара",
					"Кузя",
					"Лада",
					"Максимус",
					"Маня",
					"Марта",
					"Маруся",
					"Моня",
					"Мотя",
					"Мурзик",
					"Мурка",
					"Нафаня",
					"Ника",
					"Нюша",
					"Проша",
					"Пятнушка",
					"Сеня",
					"Сивка",
					"Тихон",
					"Тоша",
					"Фунтик",
					"Шайтан",
					"Юнона",
					"Юпитер",
					"Ягодка",
					"Яшка"
				};
				text = array3[new Random().Next(0, array3.Length)];
			}
			else
			{
				int num = Game1.random.Next(3, 6);
				string[] array4 = new string[]
				{
					"B",
					"Br",
					"J",
					"F",
					"S",
					"M",
					"C",
					"Ch",
					"L",
					"P",
					"K",
					"W",
					"G",
					"Z",
					"Tr",
					"T",
					"Gr",
					"Fr",
					"Pr",
					"N",
					"Sn",
					"R",
					"Sh",
					"St"
				};
				string[] array5 = new string[]
				{
					"ll",
					"tch",
					"l",
					"m",
					"n",
					"p",
					"r",
					"s",
					"t",
					"c",
					"rt",
					"ts"
				};
				string[] array6 = new string[]
				{
					"a",
					"e",
					"i",
					"o",
					"u"
				};
				string[] array7 = new string[]
				{
					"ie",
					"o",
					"a",
					"ers",
					"ley"
				};
				Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
				Dictionary<string, string[]> dictionary2 = new Dictionary<string, string[]>();
				dictionary.Add("a", new string[]
				{
					"nie",
					"bell",
					"bo",
					"boo",
					"bella",
					"s"
				});
				dictionary.Add("e", new string[]
				{
					"ll",
					"llo",
					"",
					"o"
				});
				dictionary.Add("i", new string[]
				{
					"ck",
					"e",
					"bo",
					"ba",
					"lo",
					"la",
					"to",
					"ta",
					"no",
					"na",
					"ni",
					"a",
					"o",
					"zor",
					"que",
					"ca",
					"co",
					"mi"
				});
				dictionary.Add("o", new string[]
				{
					"nie",
					"ze",
					"dy",
					"da",
					"o",
					"ver",
					"la",
					"lo",
					"s",
					"ny",
					"mo",
					"ra"
				});
				dictionary.Add("u", new string[]
				{
					"rt",
					"mo",
					"",
					"s"
				});
				dictionary2.Add("a", new string[]
				{
					"nny",
					"sper",
					"trina",
					"bo",
					"-bell",
					"boo",
					"lbert",
					"sko",
					"sh",
					"ck",
					"ishe",
					"rk"
				});
				dictionary2.Add("e", new string[]
				{
					"lla",
					"llo",
					"rnard",
					"cardo",
					"ffe",
					"ppo",
					"ppa",
					"tch",
					"x"
				});
				dictionary2.Add("i", new string[]
				{
					"llard",
					"lly",
					"lbo",
					"cky",
					"card",
					"ne",
					"nnie",
					"lbert",
					"nono",
					"nano",
					"nana",
					"ana",
					"nsy",
					"msy",
					"skers",
					"rdo",
					"rda",
					"sh"
				});
				dictionary2.Add("o", new string[]
				{
					"nie",
					"zzy",
					"do",
					"na",
					"la",
					"la",
					"ver",
					"ng",
					"ngus",
					"ny",
					"-mo",
					"llo",
					"ze",
					"ra",
					"ma",
					"cco",
					"z"
				});
				dictionary2.Add("u", new string[]
				{
					"ssie",
					"bbie",
					"ffy",
					"bba",
					"rt",
					"s",
					"mby",
					"mbo",
					"mbus",
					"ngus",
					"cky"
				});
				text += array4[Game1.random.Next(array4.Length - 1)];
				for (int i = 1; i < num - 1; i++)
				{
					if (i % 2 == 0)
					{
						text += array5[Game1.random.Next(array5.Length)];
					}
					else
					{
						text += array6[Game1.random.Next(array6.Length)];
					}
					if (text.Length >= num)
					{
						break;
					}
				}
				if (Game1.random.NextDouble() < 0.5 && !array6.Contains(text.ElementAt(text.Length - 1).ToString() ?? ""))
				{
					text += array7[Game1.random.Next(array7.Length)];
				}
				else if (array6.Contains(text.ElementAt(text.Length - 1).ToString() ?? ""))
				{
					if (Game1.random.NextDouble() < 0.8)
					{
						if (text.Length <= 3)
						{
							text += dictionary2[text.ElementAt(text.Length - 1).ToString() ?? ""].ElementAt(Game1.random.Next(dictionary2[text.ElementAt(text.Length - 1).ToString() ?? ""].Length - 1));
						}
						else
						{
							text += dictionary[text.ElementAt(text.Length - 1).ToString() ?? ""].ElementAt(Game1.random.Next(dictionary[text.ElementAt(text.Length - 1).ToString() ?? ""].Length - 1));
						}
					}
				}
				else
				{
					text += array6[Game1.random.Next(array6.Length)];
				}
				for (int j = text.Length - 1; j > 2; j--)
				{
					if (array6.Contains(text[j].ToString()) && array6.Contains(text[j - 2].ToString()))
					{
						char c = text[j - 1];
						if (c != 'c')
						{
							if (c != 'l')
							{
								if (c == 'r')
								{
									text = text.Substring(0, j - 1) + "k" + text.Substring(j);
									j--;
								}
							}
							else
							{
								text = text.Substring(0, j - 1) + "n" + text.Substring(j);
								j--;
							}
						}
						else
						{
							text = text.Substring(0, j) + "k" + text.Substring(j);
							j--;
						}
					}
				}
				if (text.Length <= 3 && Game1.random.NextDouble() < 0.1)
				{
					text = ((Game1.random.NextDouble() < 0.5) ? (text + text) : (text + "-" + text));
				}
				if (text.Length <= 2 && text.Last<char>() == 'e')
				{
					text += ((Game1.random.NextDouble() < 0.3) ? "m" : ((Game1.random.NextDouble() < 0.5) ? "p" : "b"));
				}
				if (text.ToLower().Contains("sex") || text.ToLower().Contains("taboo") || text.ToLower().Contains("fuck") || text.ToLower().Contains("rape") || text.ToLower().Contains("cock") || text.ToLower().Contains("willy") || text.ToLower().Contains("cum") || text.ToLower().Contains("goock") || text.ToLower().Contains("trann") || text.ToLower().Contains("gook") || text.ToLower().Contains("bitch") || text.ToLower().Contains("shit") || text.ToLower().Contains("pusie") || text.ToLower().Contains("kike") || text.ToLower().Contains("nigg") || text.ToLower().Contains("puss"))
				{
					text = ((Game1.random.NextDouble() < 0.5) ? "Bobo" : "Wumbus");
				}
			}
			return text;
		}

		public string exitCurrentDialogue()
		{
			if (this.temporaryDialogue != null)
			{
				return null;
			}
			bool arg_42_0 = this.isCurrentStringContinuedOnNextScreen;
			if (this.currentDialogueIndex < this.dialogues.Count - 1)
			{
				this.currentDialogueIndex++;
				this.checkForSpecialDialogueAttributes();
			}
			else
			{
				this.finishedLastDialogue = true;
			}
			if (arg_42_0)
			{
				return this.getCurrentDialogue();
			}
			return null;
		}

		private void checkForSpecialDialogueAttributes()
		{
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("{"))
			{
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("{", "");
				this.isCurrentStringContinuedOnNextScreen = true;
			}
			else
			{
				this.isCurrentStringContinuedOnNextScreen = false;
			}
			this.checkEmotions();
		}

		private void checkEmotions()
		{
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$h"))
			{
				this.currentEmotion = "$h";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$h", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$s"))
			{
				this.currentEmotion = "$s";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$s", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$u"))
			{
				this.currentEmotion = "$u";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$u", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$l"))
			{
				this.currentEmotion = "$l";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$l", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$a"))
			{
				this.currentEmotion = "$a";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$a", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$"))
			{
				int num = (this.dialogues[this.currentDialogueIndex].Length > this.dialogues[this.currentDialogueIndex].IndexOf("$") + 2 && char.IsDigit(this.dialogues[this.currentDialogueIndex][this.dialogues[this.currentDialogueIndex].IndexOf("$") + 2])) ? 2 : 1;
				string oldValue = this.dialogues[this.currentDialogueIndex].Substring(this.dialogues[this.currentDialogueIndex].IndexOf("$"), num + 1);
				this.currentEmotion = oldValue;
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace(oldValue, "");
				return;
			}
			this.currentEmotion = "$neutral";
		}

		public List<NPCDialogueResponse> getNPCResponseOptions()
		{
			return this.playerResponses;
		}

		public List<Response> getResponseOptions()
		{
			IEnumerable<NPCDialogueResponse> arg_25_0 = this.playerResponses;
			Func<NPCDialogueResponse, Response> arg_25_1;
			if ((arg_25_1 = Dialogue.<>c.<>9__83_0) == null)
			{
				arg_25_1 = (Dialogue.<>c.<>9__83_0 = new Func<NPCDialogueResponse, Response>(Dialogue.<>c.<>9.<getResponseOptions>b__83_0));
			}
			return new List<Response>(arg_25_0.Select(arg_25_1));
		}

		public bool isCurrentDialogueAQuestion()
		{
			return this.isLastDialogueInteractive && this.currentDialogueIndex == this.dialogues.Count - 1;
		}

		public bool chooseResponse(Response response)
		{
			int i = 0;
			while (i < this.playerResponses.Count)
			{
				if (this.playerResponses[i].responseKey != null && response.responseKey != null && this.playerResponses[i].responseKey.Equals(response.responseKey))
				{
					if (this.answerQuestionBehavior != null)
					{
						if (this.answerQuestionBehavior(i))
						{
							Game1.currentSpeaker = null;
						}
						this.isLastDialogueInteractive = false;
						this.finishedLastDialogue = true;
						this.answerQuestionBehavior = null;
						return true;
					}
					if (this.quickResponse)
					{
						this.isLastDialogueInteractive = false;
						this.finishedLastDialogue = true;
						this.isCurrentStringContinuedOnNextScreen = true;
						this.speaker.setNewDialogue(this.quickResponses[i], false, false);
						Game1.drawDialogue(this.speaker);
						this.speaker.faceTowardFarmerForPeriod(4000, 3, false, Game1.player);
						return true;
					}
					if (Game1.isFestival())
					{
						Game1.currentLocation.currentEvent.answerDialogueQuestion(this.speaker, this.playerResponses[i].responseKey);
						this.isLastDialogueInteractive = false;
						this.finishedLastDialogue = true;
						return false;
					}
					Game1.player.changeFriendship(this.playerResponses[i].friendshipChange, this.speaker);
					if (this.playerResponses[i].id != -1)
					{
						Game1.player.addSeenResponse(this.playerResponses[i].id);
					}
					this.isLastDialogueInteractive = false;
					this.finishedLastDialogue = false;
					this.parseDialogueString(this.speaker.Dialogue[this.playerResponses[i].responseKey]);
					this.isCurrentStringContinuedOnNextScreen = true;
					return false;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		public static string convertToDwarvish(string str)
		{
			string text = "";
			int i = 0;
			while (i < str.Length)
			{
				char c = str[i];
				if (c <= '?')
				{
					if (c <= '\'')
					{
						if (c != '\n')
						{
							switch (c)
							{
							case ' ':
							case '!':
							case '"':
								goto IL_2CB;
							default:
								if (c != '\'')
								{
									goto IL_2E3;
								}
								goto IL_2CB;
							}
						}
					}
					else if (c <= '5')
					{
						switch (c)
						{
						case ',':
						case '.':
							goto IL_2CB;
						case '-':
						case '/':
							goto IL_2E3;
						case '0':
							text += "Q";
							break;
						case '1':
							text += "M";
							break;
						default:
							if (c != '5')
							{
								goto IL_2E3;
							}
							text += "X";
							break;
						}
					}
					else if (c != '9')
					{
						if (c != '?')
						{
							goto IL_2E3;
						}
						goto IL_2CB;
					}
					else
					{
						text += "V";
					}
				}
				else if (c <= 'I')
				{
					if (c != 'A')
					{
						if (c != 'E')
						{
							if (c != 'I')
							{
								goto IL_2E3;
							}
							text += "E";
						}
						else
						{
							text += "U";
						}
					}
					else
					{
						text += "O";
					}
				}
				else if (c <= 'u')
				{
					if (c != 'O')
					{
						switch (c)
						{
						case 'U':
							text += "I";
							break;
						case 'V':
						case 'W':
						case 'X':
						case '[':
						case '\\':
						case ']':
						case '^':
						case '_':
						case '`':
						case 'b':
						case 'f':
						case 'j':
						case 'k':
						case 'l':
						case 'q':
						case 'r':
							goto IL_2E3;
						case 'Y':
							text += "Ol";
							break;
						case 'Z':
							text += "B";
							break;
						case 'a':
							text += "o";
							break;
						case 'c':
							text += "t";
							break;
						case 'd':
							text += "p";
							break;
						case 'e':
							text += "u";
							break;
						case 'g':
							text += "l";
							break;
						case 'h':
						case 'm':
						case 's':
							goto IL_2CB;
						case 'i':
							text += "e";
							break;
						case 'n':
						case 'p':
							break;
						case 'o':
							text += "a";
							break;
						case 't':
							text += "n";
							break;
						case 'u':
							text += "i";
							break;
						default:
							goto IL_2E3;
						}
					}
					else
					{
						text += "A";
					}
				}
				else if (c != 'y')
				{
					if (c != 'z')
					{
						goto IL_2E3;
					}
					text += "b";
				}
				else
				{
					text += "ol";
				}
				IL_30A:
				i++;
				continue;
				IL_2CB:
				text += str[i].ToString();
				goto IL_30A;
				IL_2E3:
				if (char.IsLetterOrDigit(str[i]))
				{
					text += (str[i] + '\u0002').ToString();
					goto IL_30A;
				}
				goto IL_30A;
			}
			return text.Replace("nhu", "doo");
		}
	}
}
