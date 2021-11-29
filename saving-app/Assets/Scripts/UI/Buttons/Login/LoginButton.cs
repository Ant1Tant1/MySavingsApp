using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginButton : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;

    private Button loginButton;

    // Start is called before the first frame update
    void Start()
    {
        // setup
        loginButton = gameObject.GetComponent<Button>();
        loginButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // get username and passwords
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        // ensure they are not empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBar.Instance.DisplayMessage("Please fill the username and password fields before trying to log in.", Color.red);
            return;
        }

        // check the couple 
        bool res = Login.CheckPassword(username, password);

        // fail
        if (!res)
        {
            MessageBar.Instance.DisplayMessage("Wrong combination of username and password.", Color.red);
            return;
        }

        // success -> log new scene
        MessageBar.Instance.DisplayMessage("User Id " + Engine.UserId + " is now logged in.");

        // Load next scene
        StartCoroutine(LoadSceneCoroutine());

    }

    private IEnumerator LoadSceneCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManagement.LoadSceneStatic(Scenes.Dashboard);
    }


}
