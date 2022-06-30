﻿using GameWatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.Helpers
{
    public static class PresetSaverHelper
    {
        public static readonly string PresetPath = "presets/";
        private static readonly string _defaultPresetName = "Default";

        public static void SavePreset(WindowContext context)
        {
            if (!Directory.Exists(PresetPath))
                Directory.CreateDirectory(PresetPath);
            IOHelper.SaveJsonObject($"{PresetPath}/{context.Name}.json", context);
        }

        public static void LoadPreset(WindowContext context, string name)
        {
            if (!Directory.Exists(PresetPath))
                Directory.CreateDirectory(PresetPath);
            if (!File.Exists($"{PresetPath}/{name}.json"))
            {
                GenerateDefaultPresetIfNotThere();
                name = _defaultPresetName;
            }
            var newContext = IOHelper.LoadJsonObject<WindowContext>($"{PresetPath}/{name}.json");
            if (newContext != null) {
                context.Name = newContext.Name;
                context.Settings = newContext.Settings;
                context.Watched = newContext.Watched;
                context.Watchers = newContext.Watchers;
            }
            else
                throw new IOException("Could not deserialize the preset!");
        }

        public static void LoadDefaultPreset(WindowContext context)
        {
            LoadPreset(context, _defaultPresetName);
        }

        public static void DeletePreset(string name)
        {
            if (!Directory.Exists(PresetPath))
                Directory.CreateDirectory(PresetPath);
            if (File.Exists($"{PresetPath}/{name}.json"))
                File.Delete($"{PresetPath}/{name}.json");
        }

        public static void LoadNewPreset(WindowContext context, string path)
        {
            if (!Directory.Exists(PresetPath))
                Directory.CreateDirectory(PresetPath);
            if (!File.Exists(path))
                throw new IOException("Could not find the requested preset!");

            CopyNewPresetToPresetFolder(path);

            var fileInfo = new FileInfo(path);
            LoadPreset(context, fileInfo.Name);
        }

        public static void CopyNewPresetToPresetFolder(string path)
        {
            if (!Directory.Exists(PresetPath))
                Directory.CreateDirectory(PresetPath);
            if (!File.Exists(path))
                throw new IOException("Could not find the requested preset!");

            var fileInfo = new FileInfo(path);
            string newFilePath = $"{PresetPath}/{fileInfo.Name}.json";
            if (File.Exists(newFilePath))
                File.Delete(newFilePath);
            File.Copy(path, newFilePath);
        }

        private static void GenerateDefaultPresetIfNotThere()
        {
            if (!File.Exists($"{PresetPath}/{_defaultPresetName}.json"))
            {
                var newContext = new WindowContext(new List<WatchedProcessGroup>(), new List<Services.IWatcherService>(), new SettingsModel(), "Default");
                SavePreset(newContext);
            }
        }

        public static List<string> GetPresetNames()
        {
            var presetNames = new List<string>();
            if (!Directory.Exists(PresetPath))
                Directory.CreateDirectory(PresetPath);

            foreach (var file in Directory.GetFiles(PresetPath))
                if (file.ToUpper().EndsWith(".JSON"))
                    presetNames.Add(file.Substring(file.LastIndexOf('/') + 1).Replace(".json", "").Replace(".JSON", ""));

            return presetNames;
        }
    }
}