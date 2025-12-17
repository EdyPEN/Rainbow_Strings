using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthUI : MonoBehaviour
{
    public AuthClient authClient;

    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;

    public TMP_Text statusText;


    public void OnClickRegister()
    {
        statusText.text = "Registering...";

        string name = nameInput.text.Trim();
        string pass = passwordInput.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
        {
            statusText.text = "Fill name and password!";
            return;
        }

        authClient.Register(
            nameInput.text,
            passwordInput.text,
            OnAuthResult
        );
    }

    public void OnClickLogin()
    {
        statusText.text = "Logging in...";

        string name = nameInput.text.Trim();
        string pass = passwordInput.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
        {
            statusText.text = "Fill name and password!";
            return;
        }

        authClient.Login(
            nameInput.text,
            passwordInput.text,
            OnAuthResult
        );
    }

    public void OnClickReturn()
    {
        SceneManager.LoadScene(0);
    }

    private void OnAuthResult(bool ok, string msg)
    {
        statusText.text = msg;

        if (ok)
        {
            statusText.text = "Login Successful";
            // TODO: Load next scene or enable main menu
            // SceneManager.LoadScene("MainMenu");
        }
    }
}

