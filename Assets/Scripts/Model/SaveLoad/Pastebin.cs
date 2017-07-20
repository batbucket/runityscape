using PasteBin;
using Scripts.Game.Serialization;
using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Game.Serialization {
    public static class Pastebin {
        private const string _rawDataOutputUrl = "https://pastebin.com/raw/";
        private const string API_KEY_KEY = "pastebinApiKey";
        private const string USERNAME_KEY = "pastebinUsername";
        private const string PASSWORD_KEY = "pastebinPassword";

        private static readonly string API_KEY;
        private static readonly string USERNAME;
        private static readonly string PASSWORD;

        static Pastebin() {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            var json = JSON.Parse(SaveLoad.ConfigJSON);
            API_KEY = json[API_KEY_KEY];
            USERNAME = json[USERNAME_KEY];
            PASSWORD = json[PASSWORD_KEY];
        }

        public static IEnumerator GetFromKey(string key, Action<string> handleOutput) {
            WWW www = new WWW(_rawDataOutputUrl + key);
            yield return www;
            handleOutput(www.text);
        }

        public static IEnumerator Paste(string message, Action<string> pasteResultHandler) {
            string apiKey = API_KEY;
            var client = new PasteBinClient(apiKey);

            // Optional; will publish as a guest if not logged in
            yield return client.Login(USERNAME, PASSWORD);

            var entry = new PasteBinEntry {
                Title = "Untitled",
                Text = message,
                Expiration = PasteBinExpiration.TenMinutes,
                Private = false,
                Format = "text"
            };

            yield return client.Paste(entry, pasteResultHandler);
        }

        public static IEnumerator PasteTest() {
            string apiKey = API_KEY;
            var client = new PasteBinClient(apiKey);

            // Optional; will publish as a guest if not logged in
            yield return client.Login(USERNAME, PASSWORD);

            var entry = new PasteBinEntry {
                Title = "PasteBin client test",
                Text = "Console.WriteLine(\"Hello PasteBin\");",
                Expiration = PasteBinExpiration.OneDay,
                Private = true,
                Format = "csharp"
            };

            yield return client.Paste(entry, s => Util.Log("Paste response:\n" + s));
        }
    }
}