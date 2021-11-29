using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CashFlowPrefab : MonoBehaviour
{
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image leftBar;
    [SerializeField]
    private Image cashFlowLogo;
    [SerializeField]
    private TextMeshProUGUI subcategoryText;
    [SerializeField]
    private TextMeshProUGUI dateText;
    [SerializeField]
    private Image valueBackgroundImage;
    [SerializeField]
    private TextMeshProUGUI valueText;
    public Button editButton;

    public void Setup(CashFlowTypes cashFlowType, string subcategoryText, string dateText, float valueText)
    {
        // get type related UI elements
        Color bright = UIElements.Instance.GetBrightColor(cashFlowType);
        Color dark = UIElements.Instance.GetDarkColor(cashFlowType);
        Sprite sprite = UIElements.Instance.GetLogo(cashFlowType);

        // setup ui
        leftBar.color = bright;
        backgroundImage.color = bright;
        valueBackgroundImage.color = dark;
        editButton.image.color = dark;
        cashFlowLogo.sprite = sprite;
        this.subcategoryText.text = subcategoryText;
        this.dateText.text = dateText;
        this.dateText.color = dark;
        this.valueText.text = PlayerPrefs.GetString("currencySymbol", "$") + " " + valueText.ToString("### ##0.0");
    }
}
