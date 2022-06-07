using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameWatch.Helpers
{
    internal static class IOHelper
    {
        #region Loading

        public static List<T> LoadJsonObjects<T>(string basePath)
        {
            var jsonObjects = new List<T>();
            foreach (var fileName in Directory.GetFiles(basePath))
            {
                var item = LoadJsonObject<T>(fileName);
                if (item != null)
                    jsonObjects.Add(item);
            }
            return jsonObjects;
        }

        public static T? LoadJsonObject<T>(string basePath, string fileName)
        {
            return LoadJsonObject<T>($"{basePath}\\{fileName}");
        }

        public static T? LoadJsonObject<T>(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var obj = JsonSerializer.Deserialize<T>(reader.ReadToEnd());
                return obj;
            }
        }

        #endregion

        #region Saving

        public static void SaveJsonObject<T>(string path, T item)
        {
            var serialized = JsonSerializer.Serialize(item);
            File.WriteAllText(path, serialized);
        }

        #endregion

        #region Links

        public static void GenerateShortcut(string folder, string filename, string linkPath)
        {
            string dir = $"{folder}\\{filename}.lnk";
            if (File.Exists(dir))
                File.Delete(dir);

            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(dir);

            shortcut.Description = "Startup shortcut for GameWatch";
            shortcut.TargetPath = linkPath;
            shortcut.Save();
        }

        public static void RemoveShortcut(string folder, string filename, string linkPath)
        {
            string dir = $"{folder}\\{filename}.lnk";
            if (File.Exists(dir))
                File.Delete(dir);
        }

        #endregion
    }
}
