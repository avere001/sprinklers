using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace StardewValley
{
	public class StartupPreferences
	{
		public const int windowed_borderless = 0;

		public const int windowed = 1;

		public const int fullscreen = 2;

		public static string filename = "startup_preferences";

		public static XmlSerializer serializer = new XmlSerializer(typeof(StartupPreferences));

		public bool startMuted;

		public bool levelTenFishing;

		public bool levelTenMining;

		public bool levelTenForaging;

		public bool levelTenCombat;

		public bool skipWindowPreparation;

		public int timesPlayed;

		public int windowMode;

		public LocalizedContentManager.LanguageCode languageCode;

		public void Init()
		{
			this.ensureFolderStructureExists();
			LocalizedContentManager.OnLanguageChange += new LocalizedContentManager.LanguageChangedHandler(this.OnLanguageChange);
		}

		private void OnLanguageChange(LocalizedContentManager.LanguageCode code)
		{
			this.savePreferences();
		}

		private void ensureFolderStructureExists()
		{
			FileInfo fileInfo = new FileInfo(Path.Combine(new string[]
			{
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley", "placeholder")
			}));
			if (!fileInfo.Directory.Exists)
			{
				fileInfo.Directory.Create();
			}
		}

		public void savePreferences()
		{
			Task task = new Task(delegate
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				this._savePreferences();
			});
			task.Start();
			task.Wait();
			if (task.IsFaulted)
			{
				throw task.Exception.GetBaseException();
			}
		}

		private void _savePreferences()
		{
			string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), StartupPreferences.filename);
			try
			{
				this.ensureFolderStructureExists();
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				using (FileStream fileStream = File.Create(path))
				{
					this.writeSettings(fileStream);
				}
			}
			catch (Exception arg_4C_0)
			{
				Game1.debugOutput = Game1.parseText(arg_4C_0.Message);
			}
		}

		private void writeSettings(Stream stream)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings
			{
				CloseOutput = true
			}))
			{
				xmlWriter.WriteStartDocument();
				this.languageCode = LocalizedContentManager.CurrentLanguageCode;
				StartupPreferences.serializer.Serialize(xmlWriter, this);
				xmlWriter.WriteEndDocument();
				xmlWriter.Flush();
			}
		}

		public void loadPreferences()
		{
			this.Init();
			Task task = new Task(delegate
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				this._loadPreferences();
			});
			task.Start();
			task.Wait();
			if (task.IsFaulted)
			{
				throw task.Exception.GetBaseException();
			}
		}

		private void _loadPreferences()
		{
			string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), StartupPreferences.filename);
			if (!File.Exists(path))
			{
				Game1.debugOutput = "File does not exist (-_-)";
				return;
			}
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
				{
					this.readSettings(fileStream);
				}
			}
			catch (Exception arg_4E_0)
			{
				Game1.debugOutput = Game1.parseText(arg_4E_0.Message);
			}
		}

		private void readSettings(Stream stream)
		{
			StartupPreferences startupPreferences = (StartupPreferences)StartupPreferences.serializer.Deserialize(stream);
			this.startMuted = startupPreferences.startMuted;
			this.timesPlayed = startupPreferences.timesPlayed + 1;
			this.levelTenCombat = startupPreferences.levelTenCombat;
			this.levelTenFishing = startupPreferences.levelTenFishing;
			this.levelTenForaging = startupPreferences.levelTenForaging;
			this.levelTenMining = startupPreferences.levelTenMining;
			this.skipWindowPreparation = startupPreferences.skipWindowPreparation;
			this.windowMode = startupPreferences.windowMode;
			LocalizedContentManager.CurrentLanguageCode = startupPreferences.languageCode;
		}
	}
}
