using Scripts.Model.TextBoxes;
using Scripts.Model.World.Serialization.SaveObject;
using Scripts.Presenter;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.World.Serialization {

    /// <summary>
    /// Utility class that allows saving and loading of data saved as JSONs.
    /// In order to ensure multiplatform support, we encrypt these JSONs
    /// and store them in PlayerPrefs.
    /// </summary>
    public static class SaveLoad {
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

        public static bool IsSaveUsed(int index) {
            return !string.IsNullOrEmpty(PlayerPrefs.GetString(GetSaveKey(index)));
        }

        public static void DeleteAllSaves() {
            for (int i = 0; i < MAX_SAVE_FILES; i++) {
                PlayerPrefs.DeleteKey(GetSaveKey(i));
            }
            Util.Log("Saves deleted.");
        }

        //public static void Load(int saveIndex, string successMessage = null) {
        //    string json = null;
        //    try {
        //        string encryptedSave = GetSaveValue(saveIndex);
        //        json = Encrypter.Decrypt(encryptedSave, EncryptKey);
        //    } catch (Exception e) {
        //        Game.Instance.TextBoxes.AddTextBox(new TextBox(Util.Color(e.ToString(), Color.red)));
        //        //TODO return null;
        //    }
        //    if (!string.IsNullOrEmpty(successMessage)) {
        //        Game.Instance.TextBoxes.AddTextBox(new TextBox(Util.Color(successMessage, Color.cyan)));
        //    }
        //    GameSave gameSave = JsonUtility.FromJson<GameSave>(json);
        //    //Camp camp = gameSave.Restore();

        //    return camp;
        //}

        //public static void Save(Camp camp, int saveIndex, string successMessage = null) {
        //    GameSave gameSave = new GameSave(camp);
        //    string json = JsonUtility.ToJson(gameSave);
        //    try {
        //        string encryptedJson = Encrypter.Encrypt(json, EncryptKey);
        //        SetSave(saveIndex, encryptedJson);
        //    } catch (Exception e) {
        //        Game.Instance.TextBoxes.AddTextBox(new TextBox(Util.Color(e.ToString(), Color.red)));
        //        return;
        //    }
        //    if (!string.IsNullOrEmpty(successMessage)) {
        //        Game.Instance.TextBoxes.AddTextBox(new TextBox(Util.Color(successMessage, Color.cyan)));
        //    }
        //}

        public static void DeleteSave(int index, string successMessage = null) {
            try {
                PlayerPrefs.DeleteKey(GetSaveKey(index));
            } catch (Exception e) {
                Game.Instance.TextBoxes.AddTextBox(new TextBox(Util.Color(e.ToString(), Color.red)));
            }
            if (!string.IsNullOrEmpty(successMessage)) {
                Game.Instance.TextBoxes.AddTextBox(new TextBox(Util.Color(successMessage, Color.cyan)));
            }
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
    }
}