using Scripts.Model.World.Pages;
using Scripts.Model.World.Serialization.SaveObject;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Scripts.Model.World.Serialization {

    /// <summary>
    /// Utility class that allows saving and loading of data saved as JSONs.
    /// </summary>
    public static class SaveLoad {
        public const string JSON_FILE_EXTENSION = ".json";
        public const int MAX_SAVE_FILES = 10;
        private const string SAVE_FOLDER_NAME = "Saves";

        public static string SAVE_DIRECTORY {
            get {
                return string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), SAVE_FOLDER_NAME);
            }
        }

        public static void DeleteAllSaves() {
            FileUtil.DeleteFileOrDirectory(SAVE_DIRECTORY);
            Directory.CreateDirectory(SAVE_DIRECTORY);
        }

        public static string[] GetSavePaths() {
            DirectoryInfo info = new DirectoryInfo(SAVE_DIRECTORY);
            FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();

            string[] paths = new string[files.Length];
            int index = 0;
            foreach (FileInfo file in files) {
                paths[index++] = file.FullName;
            }
            return paths;
        }

        public static Camp Load(string filePath) {
            string json = File.ReadAllText(filePath);

            GameSave gameSave = JsonUtility.FromJson<GameSave>(json);
            Camp camp = gameSave.Restore();

            return camp;
        }

        public static void Save(Camp camp, string filePath, bool isOverwrite) {
            if (!Directory.Exists(SAVE_DIRECTORY)) {
                Directory.CreateDirectory(SAVE_DIRECTORY);
            }

            GameSave gameSave = new GameSave(camp);
            string json = JsonUtility.ToJson(gameSave, true);

            if (isOverwrite) {
                File.Delete(filePath);
                File.WriteAllText(Util.GetUniqueFilePath(string.Format("{0}\\{1}{2}", SAVE_DIRECTORY, camp.Party.Leader.Name, JSON_FILE_EXTENSION)), json);
            } else {
                File.WriteAllText(Util.GetUniqueFilePath(filePath + JSON_FILE_EXTENSION), json);
            }
        }

        public static string SaveFileDisplay(string filePath, int level) {
            string fileName = Path.GetFileNameWithoutExtension(Path.GetFileName(filePath));
            return string.Format("{0}\nLevel {1}", fileName, level);
        }
    }
}