using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Scripts.ReadWrite.Objects;
using System.Linq;
using System.IO;

public class SettingsProfileBanner : MonoBehaviour, IBanner
{
    [Header("Settings Panel Components")]
    [SerializeField]
    private TextMeshProUGUI usernameText;
    [SerializeField]
    private TextMeshProUGUI userIdText;
    [SerializeField]
    private TextMeshProUGUI emailText;
    [SerializeField]
    private TMP_InputField currencyInputField;

    [Header("Edit Password Components")]
    [SerializeField]
    private TMP_InputField currentPassword;
    [SerializeField]
    private TMP_InputField newPassword;
    [SerializeField]
    private TMP_InputField confirmNewPassword;
    [SerializeField]
    private Transform editPasswordPanel;

    public void Setup(CashFlowTypes? cFlowType)
    {
        if (cFlowType != null)
            throw new System.Exception("CashFlow type should be null in that particular case");
    }

    private void Start()
    {
        // set up variables
        usernameText.text = Engine.UserName;
        userIdText.text = Engine.UserId;
        emailText.text = Engine.MailAddress;

        currencyInputField.text = PlayerPrefs.GetString("currencySymbol", "$");
    }

    public void OnCurrencyValidateEditsClick()
    {
        PlayerPrefs.SetString("currencySymbol", currencyInputField.text);
    }

    public void OnPasswordChangeConfirmation()
    {
        if (newPassword.text != confirmNewPassword.text)
            MessageBar.Instance.DisplayMessage("The confirmation password do not match the new password.");

        // Load infos from user file
        List<Users> users = Engine.ReadList<Users>(SerializationTypes.Users);
        foreach (Users user in users)
        {
            if (user.userId == Engine.UserId)
            {
                if (user.password != currentPassword.text)
                {
                    MessageBar.Instance.DisplayMessage("The current password seems to be wrong.");
                    return;
                }
                else
                {
                    user.password = newPassword.text;
                    Engine.Save(users, SerializationTypes.Users);



                    MessageBar.Instance.DisplayMessage("Your password has been changed successfully");
                }
                return;
            }
        }

        throw new System.Exception("Method should not reach that point");
    }

    public void OnCancelPasswordChangeClick()
    {
        // clear text
        currentPassword.text = "";
        newPassword.text = "";
        confirmNewPassword.text = "";

        editPasswordPanel.gameObject.SetActive(false);
    }

    public void OnExportDataConfirmation()
    {
        // Save files
        Engine.Save(Engine.CashFlows.Values, SerializationTypes.CashFlows);
        Engine.Save(Engine.CashFlowSubCategories, SerializationTypes.CashFlowSubCategories);

        // create sendEmail object
        SendEmail sendEmail = new SendEmail();

        // get attachment -> find a way to get path from readWrite or Engine class
        string strAttachment1 = Engine.GetPath(SerializationTypes.CashFlows);
        string strAttachment2 = Engine.GetPath(SerializationTypes.CashFlowSubCategories);

        sendEmail.SendEmails("Your exported data", "Please find attached your exported data.", new string[2] { strAttachment1, strAttachment2});

        MessageBar.Instance.DisplayMessage("Data successfully exported");
    }
}
