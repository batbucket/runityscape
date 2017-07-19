using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using SimpleJSON;
using System;
using UnityEngine;

namespace Scripts.Game.Serialization {

    /// <summary>
    /// Utility class that allows saving and loading of data saved as JSONs.
    /// In order to ensure multiplatform support, we encrypt these JSONs
    /// and store them in PlayerPrefs.
    /// </summary>
    public static class SaveLoad {
        public const bool IS_ENCRYPT_VALUES = true;

        public const int MAX_SAVE_FILES = 10;
        private const string SAVE_PREFIX = "save_";

        private const string CONFIG_FOLDER_NAME = "Config";
        private const string CONFIG_FILE_NAME = "config";

        private const string ENCRYPTION_KEY_KEY = "encryptionKey";
        private static readonly string ENCRYPTION_KEY;

        static SaveLoad() {
            JSONNode node = JSON.Parse(ConfigJSON);
            ENCRYPTION_KEY = node[ENCRYPTION_KEY_KEY];
        }

        public static string ConfigJSON {
            get {
                TextAsset ta = Resources.Load<TextAsset>(string.Format("{0}/{1}", CONFIG_FOLDER_NAME, CONFIG_FILE_NAME));
                return ta.text;
            }
        }

        public static void PrintSaves() {
            for (int i = 0; i < MAX_SAVE_FILES; i++) {
                PostMessage("Save index {0}\n{1}", GetSaveKey(i), GetSaveValue(i));
            }
        }

        public static bool IsSaveUsed(int index) {
            return !string.IsNullOrEmpty(PlayerPrefs.GetString(GetSaveKey(index)));
        }

        public static void DeleteAllSaves() {
            for (int i = 0; i < MAX_SAVE_FILES; i++) {
                PlayerPrefs.DeleteKey(GetSaveKey(i));
            }
            Util.Log("Saves deleted.");
        }

        public static WorldSave Load(int saveIndex, string successNoun) {
            return Load(GetSaveValue(saveIndex), successNoun);
        }

        public static WorldSave Load(string save, string successNoun) {
            string json = null;
            try {
                string saveValue = save;
                if (IS_ENCRYPT_VALUES) {
                    json = DecryptString(saveValue);
                } else {
                    json = saveValue;
                }
            } catch (Exception e) {
                PostMessage(Util.ColorString(e.ToString(), Color.red));
                return null;
            }
            WorldSave load = JsonUtility.FromJson<WorldSave>(json);
            PostMessage("Load from {0} was successful.", successNoun);
            return load;
        }

        public static void Save(WorldSave saveObject, int saveIndex) {
            string json = JsonUtility.ToJson(saveObject);
            try {
                string modified = string.Empty;
                if (IS_ENCRYPT_VALUES) {
                    modified = EncryptString(json);
                } else {
                    modified = json;
                }
                SetSave(saveIndex, modified);
            } catch (Exception e) {
                PostMessage(Util.ColorString(e.ToString(), Color.red));
                return;
            }
            PostMessage("Saved successfully to slot {0}.", saveIndex);
        }

        public static void DeleteSave(int index) {
            try {
                PlayerPrefs.DeleteKey(GetSaveKey(index));
            } catch (Exception e) {
                PostMessage(Util.ColorString(e.ToString(), Color.red));
                return;
            }
            PostMessage(string.Format("Save file {0} was deleted.", index));
        }

        public static string SaveFileDisplay(string name, int level) {
            return string.Format("{0}\nLevel {1}", name, level);
        }

        public static string GetSaveValue(int index) {
            return PlayerPrefs.GetString(GetSaveKey(index));
        }

        private static void SetSave(int index, string value) {
            PlayerPrefs.SetString(GetSaveKey(index), value);
        }

        private static string GetSaveKey(int index) {
            return string.Format("{0}{1}", SAVE_PREFIX, index);
        }

        private static string EncryptString(string jsonToEncrypt) {
            return Encrypter.Encrypt(jsonToEncrypt, ENCRYPTION_KEY);
        }

        private static string DecryptString(string encryptedJson) {
            return Encrypter.Decrypt(encryptedJson, ENCRYPTION_KEY);
        }

        private static void PostMessage(string message, params object[] args) {
            Presenter.Main.Instance.TextBoxes.AddTextBox(new TextBox(string.Format(message, args)));
        }
    }
}