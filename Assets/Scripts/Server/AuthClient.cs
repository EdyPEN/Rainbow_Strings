using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AuthClient : MonoBehaviour
{
    private string baseUrl = "https://rainbowserver-1.onrender.com";

    private class RegisterRequest
    {
        public string name;
        public string password;
    }

    private class LoginRequest
    {
        public string name;
        public string password;
    }

    private class AuthResponse
    {
        public bool ok;
        public int userId;
        public string name;
        public string message;
        public string error;
    }

    public void Register(string name, 
                            string password, 
                                System.Action<bool, string> callback)
    {
        StartCoroutine(RegisterRoutine(name, password, callback));
    }

    public void Login(string name, 
                        string password, 
                            System.Action<bool, string> callback)
    {
        StartCoroutine(LoginRoutine(name, password, callback));
    }

    private IEnumerator RegisterRoutine(string name, string password, System.Action<bool, string> callback)
    {
        RegisterRequest payload = new RegisterRequest();
        payload.name = name;
        payload.password = password;

        string json = JsonUtility.ToJson(payload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest req = new UnityWebRequest(baseUrl + "/register", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            string responseText = "";
            if (req.downloadHandler != null)
            {
                responseText = req.downloadHandler.text;
            }

            if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                callback(false, "Network error: " + req.error);
                yield break;
            }
            else if (req.result == UnityWebRequest.Result.ProtocolError)
            {
                // This is where 400/401/404/500 will be
                callback(false, "HTTP " + req.responseCode + " -> " + responseText);
                yield break;
            }
            else if (req.result != UnityWebRequest.Result.Success)
            {
                callback(false, "Request REGISTER failed: " + req.error + " -> " + responseText);
                yield break;
            }
        }
    }

    private IEnumerator LoginRoutine(string name, string password, System.Action<bool, string> callback)
    {
        LoginRequest payload = new LoginRequest();
        payload.name = name;
        payload.password = password;

        string json = JsonUtility.ToJson(payload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest req = new UnityWebRequest(baseUrl + "/login", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            string responseText = "";
            if (req.downloadHandler != null)
            {
                responseText = req.downloadHandler.text;
            }

            if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                callback(false, "Network error: " + req.error);
                yield break;
            }
            else if (req.result == UnityWebRequest.Result.ProtocolError)
            {
                // This is where 400/401/404/500 will be
                callback(false, "HTTP " + req.responseCode + " -> " + responseText);
                yield break;
            }
            else if (req.result != UnityWebRequest.Result.Success)
            {
                callback(false, "Request LOGIN failed: " + req.error + " -> " + responseText);
                yield break;
            }
        }
    }
}
