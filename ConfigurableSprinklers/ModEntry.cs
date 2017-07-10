using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Collections.Specialized;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;
using Microsoft.Xna.Framework.Input;
using System.IO;
using ConfigurableSprinklers.JsonSerialization;
using ConfigurableSprinklers.Configuration;

namespace ConfigurableSprinklers
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {

        //TODO: when current sprinkler is set, update lastPlacementTime and the DefaultSpotsToWater

        /// <summary>
        /// <see cref="CurrentSprinkler"/>
        /// </summary>
        private SprinklerObject currentSprinkler;

        /// <summary>
        /// The current sprinkler being configured by the player
        /// </summary>
        public SprinklerObject CurrentSprinkler
        {
            get => currentSprinkler;
            set
            {
                if (value != null)
                {
                    LastPlacementTime = Game1.currentGameTime.TotalGameTime.Ticks;
                }

                if (currentSprinkler != null)
                {
                    Configuration.Config.UpdateDefaultSpotsToWater(currentSprinkler);
                }
                currentSprinkler = value;
            }
        }

        /// <summary>
        /// The time in GameTime ticks that the last sprinkler was placed 
        /// </summary>
        public long LastPlacementTime { get; set; } = 0;

        /// <summary>Registers the events and populates Utils for convenience</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            SaveEvents.BeforeSave += SaveEvents_BeforeSave;
            SaveEvents.AfterSave += SaveEvents_AfterSave;
            SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            
            LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
            GraphicsEvents.OnPostRenderHudEvent += GraphicsEvents_OnPostRenderHudEvent;
            ControlEvents.MouseChanged += ControlEvents_MouseChanged; //TODO: also register for controller events

            Utils.Mod = this;
            Utils.Manifest = ModManifest;
            Utils.Helper = Helper;
            Utils.Monitor = Monitor;
        }
        
        /// <summary>
        /// Handles the mouse clicks needed for configuring sprinklers in-game
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void ControlEvents_MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
            if (Game1.currentLocation != null && e.NewState.LeftButton == ButtonState.Released && e.PriorState.LeftButton == ButtonState.Pressed)
                // onLeftClick
            {
                Vector2 mouseTilePos = Utils.GetMouseTile();
                Game1.currentLocation.objects.TryGetValue(mouseTilePos, out StardewValley.Object obj);

                if (CurrentSprinkler != null && CurrentSprinkler.tileLocation.Equals(mouseTilePos))
                    // If the sprinkler being configured is clicked
                {
                    if (new TimeSpan(Game1.currentGameTime.TotalGameTime.Ticks - LastPlacementTime).Seconds > 1)
                        // Prevent configuration mode being exited as soon as the sprinkler is placed
                    {
                        CurrentSprinkler = null;
                    }
                }
                else if (obj is SprinklerObject sprinkler && (CurrentSprinkler == null || !CurrentSprinkler.WaterTiles.Contains(mouseTilePos)))
                    // if a different sprinkler than the one being configured is clicked
                    // ignoring clicks where the user might be trying to unselect the tile this sprinkler is on
                {
                    CurrentSprinkler = sprinkler;
                }
                else
                {
                    if (CurrentSprinkler != null)
                    {
                        CurrentSprinkler.OnClickDuringConfig();
                    }
                }
            }
        }

        /// <summary>
        /// This handler registers/unregisters object collection changed handlers.
        /// Also unsets the currently configured sprinkler.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">The args containing the prior and new location</param>
        private void LocationEvents_CurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {

            if (e.PriorLocation != null)
            {
                e.PriorLocation.objects.CollectionChanged -= Object_CollectionChanged;
            }
            e.NewLocation.objects.CollectionChanged += Object_CollectionChanged;

            // Can't configure a sprinkler in a different location
            CurrentSprinkler = null;
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Object control.
        /// If the new object is a sprinkler, convert it to a <see cref="SprinklerObject"/> amd set <c>currentSprinkler</c> to that.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Object_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GameLocation loc = Game1.currentLocation;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Vector2 pos in e.NewItems)
                {
                    loc.objects[pos] = SprinklerObject.GetSprinklerObjectAs<SprinklerObject>(loc.objects[pos]);
                    CurrentSprinkler = loc.objects[pos] as SprinklerObject;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // Can't configure a sprinkler that no longer exists
                CurrentSprinkler = null;
            }
        }

        /// <summary>
        /// This handler renders the configuration squares of the sprinkler currently being modified
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void GraphicsEvents_OnPostRenderHudEvent(object sender, EventArgs e)
        {
            if (CurrentSprinkler != null)
            {
                CurrentSprinkler.DrawConfigurationSquares();
            }
        }

        /// <summary>
        /// This handler converts all sprinklers to SprinklerObjects.
        /// Also loads each sprinklers data from the sprinkler data file.
        /// </summary>
        /// <remarks>
        /// SprinklerObject overrides DayUpdate so that we can modify the watering and animation logic.
        /// </remarks>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            Configuration.Config.Load();
            DataManager.LoadSprinklers();
        }

        /// <summary>
        /// Converts all sprinklers back to StardewValley.Object for serialization
        /// </summary>
        /// <remarks>
        /// This allows Stardew Valley to save (serialize) the sprinklers.
        /// </remarks>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            DataManager.TentativelySaveSprinklers();
        }

        /// <summary>
        /// This event handler converts all sprinkler to <see cref="SprinklerObject"/>, finalizes sprinkler saving, then reloads the sprinklers 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void SaveEvents_AfterSave(object sender, EventArgs e)
        {
            DataManager.ConfirmSaveSprinklers();
            DataManager.LoadSprinklers();
        }
        
    }
}