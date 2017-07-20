using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace PasteBin {

    /// <summary>
    /// Modified from
    /// https://stackoverflowcom/questions/1987440/submitting-posts-on-pastie-or-pastebin-in-c-sharp
    /// </summary>
    class PasteBinClient {
        private const string _apiPostUrl = "https://pastebin.com/api/api_post.php";
        private const string _apiLoginUrl = "https://pastebin.com/api/api_login.php";

        private readonly string _apiDevKey;
        private string _userName;
        private string _apiUserKey;

        public PasteBinClient(string apiDevKey) {
            if (string.IsNullOrEmpty(apiDevKey))
                throw new ArgumentNullException("apiDevKey");
            _apiDevKey = apiDevKey;
        }

        public string UserName {
            get { return _userName; }
        }

        public IEnumerator Login(string userName, string password) {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            WWWForm form = GetBaseParameters();
            form.AddField(ApiParameters.UserName, userName);
            form.AddField(ApiParameters.UserPassword, password);

            using (UnityWebRequest www = UnityWebRequest.Post(_apiLoginUrl, form)) {
                yield return www.Send();
                string response = www.downloadHandler.text;
                if (response.StartsWith("Bad API request")) {
                    throw new PasteBinApiException(response);
                } else {
                    _userName = userName;
                    _apiUserKey = response;
                }
            }
        }

        public void Logout() {
            _userName = null;
            _apiUserKey = null;
        }

        public IEnumerator Paste(PasteBinEntry entry, Action<string> responseHandler) {
            if (entry == null)
                throw new ArgumentNullException("entry");
            if (string.IsNullOrEmpty(entry.Text))
                throw new ArgumentException("The paste text must be set", "entry");

            WWWForm form = GetBaseParameters();
            form.AddField(ApiParameters.Option, "paste");
            form.AddField(ApiParameters.PasteCode, entry.Text);
            SetIfNotEmpty(form, ApiParameters.PasteName, entry.Title);
            SetIfNotEmpty(form, ApiParameters.PasteFormat, entry.Format);
            SetIfNotEmpty(form, ApiParameters.PastePrivate, entry.Private ? "1" : "0");
            SetIfNotEmpty(form, ApiParameters.PasteExpireDate, FormatExpireDate(entry.Expiration));
            SetIfNotEmpty(form, ApiParameters.UserKey, _apiUserKey);

            string resp = string.Empty;
            using (UnityWebRequest www = UnityWebRequest.Post(_apiPostUrl, form)) {
                yield return www.Send();
                string response = www.downloadHandler.text;
                if (response.StartsWith("Bad API request")) {
                    throw new PasteBinApiException(resp);
                }
                responseHandler(response);
            }
        }

        private static string FormatExpireDate(PasteBinExpiration expiration) {
            switch (expiration) {
                case PasteBinExpiration.Never:
                    return "N";
                case PasteBinExpiration.TenMinutes:
                    return "10M";
                case PasteBinExpiration.OneHour:
                    return "1H";
                case PasteBinExpiration.OneDay:
                    return "1D";
                case PasteBinExpiration.OneMonth:
                    return "1M";
                default:
                    throw new ArgumentException("Invalid expiration date");
            }
        }

        private static void SetIfNotEmpty(WWWForm parameters, string name, string value) {
            if (!string.IsNullOrEmpty(value))
                parameters.AddField(name, value);
        }

        private WWWForm GetBaseParameters() {
            var parameters = new WWWForm();
            parameters.AddField(ApiParameters.DevKey, _apiDevKey);

            return parameters;
        }

        private static string GetResponseText(byte[] bytes) {
            using (var ms = new MemoryStream(bytes))
            using (var reader = new StreamReader(ms)) {
                return reader.ReadToEnd();
            }
        }

        private static class ApiParameters {
            public const string DevKey = "api_dev_key";
            public const string UserKey = "api_user_key";
            public const string Option = "api_option";
            public const string UserName = "api_user_name";
            public const string UserPassword = "api_user_password";
            public const string PasteCode = "api_paste_code";
            public const string PasteName = "api_paste_name";
            public const string PastePrivate = "api_paste_private";
            public const string PasteFormat = "api_paste_format";
            public const string PasteExpireDate = "api_paste_expire_date";
        }
    }

    public class PasteBinApiException : Exception {
        public PasteBinApiException(string message)
            : base(message) {
        }
    }

    public class PasteBinEntry {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Format { get; set; }
        public bool Private { get; set; }
        public PasteBinExpiration Expiration { get; set; }
    }

    public enum PasteBinExpiration {
        Never,
        TenMinutes,
        OneHour,
        OneDay,
        OneMonth
    }
}