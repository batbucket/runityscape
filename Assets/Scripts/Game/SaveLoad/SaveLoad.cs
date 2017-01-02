using System;
using System.IO;
using UnityEngine;

public static class SaveLoad {
    public const int MAX_SAVE_FILES = 10;
    public const string LOAD_SEARCH_PATTERN = "*.JSON";

    public static string SAVE_DIRECTORY {
        get {
            return string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), SAVE_FOLDER_NAME);
        }
    }

    private const string SAVE_FOLDER_NAME = "Saves";

    public static void Save(Camp camp, string filePath, bool isOverwrite) {
        if (!Directory.Exists(SAVE_DIRECTORY)) {
            Directory.CreateDirectory(SAVE_DIRECTORY);
        }

        GameSave gameSave = new GameSave(camp);
        string json = JsonUtility.ToJson(gameSave);

        if (isOverwrite) {
            File.WriteAllText(filePath, json);
            File.Move(filePath, Util.GetUniqueFilePath(filePath));
        } else {
            File.WriteAllText(Util.GetUniqueFilePath(filePath), json);
        }

        Util.Log("Save was successful. File: " + filePath);
    }

    public static Camp Load(string filePath) {
        string json = File.ReadAllText(filePath);

        GameSave gameSave = JsonUtility.FromJson<GameSave>(json);
        Camp camp = gameSave.Restore();

        Util.Log("Load was successful. File: " + filePath);
        return camp;
    }
}