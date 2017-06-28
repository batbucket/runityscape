using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using System;
using UnityEngine;

namespace Scripts.Game.Serialization {

    /// <summary>
    /// Utility class that allows saving and loading of data saved as JSONs.
    /// In order to ensure multiplatform support, we encrypt these JSONs
    /// and store them in PlayerPrefs.
    /// </summary>
    public static class SaveLoad {
#if UNITY_EDITOR
        public const bool IS_ENCRYPT_VALUES = false;
#else
        public const bool IS_ENCRYPT_VALUES = true;
#endif

        public const int MAX_SAVE_FILES = 10;
        private const string SAVE_PREFIX = "save_";

        private const string ENCRYPT_FOLDER_NAME = "Encrypt";
        private const string ENCRYPT_FILE_NAME = "config";

        private static string EncryptKey {
            get {
                TextAsset ta = Resources.Load<TextAsset>(string.Format("{0}/{1}", ENCRYPT_FOLDER_NAME, ENCRYPT_FILE_NAME));
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

        public static GameSave Load(int saveIndex, bool isEncrypted) {
            string json = null;
            try {
                string saveValue = GetSaveValue(saveIndex);
                if (isEncrypted) {
                    json = DecryptString(saveValue);
                } else {
                    json = saveValue;
                }
            } catch (Exception e) {
                Main.Instance.TextBoxes.AddTextBox(new TextBox(Util.ColorString(e.ToString(), Color.red)));
                return null;
            }
            GameSave load = JsonUtility.FromJson<GameSave>(json);
            PostMessage("Load from {0} was successful.", saveIndex);
            return load;
        }

        public static void Save(GameSave saveObject, bool isEncrypted, int saveIndex) {
            string json = JsonUtility.ToJson(saveObject);
            try {
                string modified = string.Empty;
                if (isEncrypted) {
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

        private static void SetSave(int index, string value) {
            PlayerPrefs.SetString(GetSaveKey(index), value);
        }

        private static string GetSaveKey(int index) {
            return string.Format("{0}{1}", SAVE_PREFIX, index);
        }

        private static string EncryptString(string jsonToEncrypt) {
            return Encrypter.Encrypt(jsonToEncrypt, EncryptKey);
        }

        private static string DecryptString(string encryptedJson) {
            return Encrypter.Decrypt(encryptedJson, EncryptKey);
        }

        private static string GetSaveValue(int index) {
            return PlayerPrefs.GetString(GetSaveKey(index));
        }

        private static void PostMessage(string message, params object[] args) {
            Presenter.Main.Instance.TextBoxes.AddTextBox(new TextBox(string.Format(message, args)));
        }
    }
}