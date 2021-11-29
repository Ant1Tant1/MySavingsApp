using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewAccountButton : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameInputField;
    [SerializeField]
    private TMP_InputField emailInputField;
    [SerializeField]
    private TMP_InputField passwordInputField;
    [SerializeField]
    private TMP_InputField confirmPasswordInputField;
    [SerializeField]
    private SettingsCahsFlowsBanner settingsCahsFlowsBanner;

    private Button newAccountButton;
    private int enumLength;
    // Start is called before the first frame update
    void Start()
    {
        enumLength = Enum.GetNames(typeof(CashFlowTypes)).Length;
        newAccountButton = GetComponent<Button>();
        newAccountButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // get username and passwords
        string username = usernameInputField.text;
        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        // ensure they are not empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(confirmPassword))
        {
            MessageBar.Instance.DisplayMessage("Please fill up all fields to create new account.", Color.red, 6f);
            return;
        }

        // ensure the passwords are matching
        if (password != confirmPassword)
        {
            MessageBar.Instance.DisplayMessage("Passwords do not match.", Color.red, 6f);
            return;
        }

        bool res = Login.CreateNewUser(username, password, email);
        
        // fail
        if (!res)
        {
            MessageBar.Instance.DisplayMessage("Failed. Please try again.", Color.red);
            return;
        }

        // success -> log new scene
        MessageBar.Instance.DisplayMessage("Your account has been created.");

        // setup subcategories
        Engine.CashFlowSubCategories.Earnings[0] = "Work";
        Engine.CashFlowSubCategories.Expenses[0] = "Groceries";
        Engine.CashFlowSubCategories.Savings[0] = "Saving Account";

        SetupCashFlowsBanner((CashFlowTypes)0);

        // close panel
        newAccountButton.transform.parent.gameObject.SetActive(false);

    }

    private void SetupCashFlowsBanner(CashFlowTypes cashFlowType)
    {
        settingsCahsFlowsBanner.Setup(cashFlowType);
        settingsCahsFlowsBanner.gameObject.SetActive(true);
        settingsCahsFlowsBanner.confirmButton.onClick.RemoveAllListeners();

        // call back the same method unless last category has been reached
        if ((int) cashFlowType < enumLength - 1)
        {
            settingsCahsFlowsBanner.confirmButton.onClick.AddListener(() => { SetupCashFlowsBanner((CashFlowTypes)((int)cashFlowType + 1)); });
        }
        else
        {
            settingsCahsFlowsBanner.confirmButton.onClick.AddListener(() => { SceneManagement.LoadSceneStatic(Scenes.Dashboard); });
        }
    }
}
