using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DashboardAvailableCashContainer : MonoBehaviour
{
    public TextMeshProUGUI currencySymbol;
    public TextMeshProUGUI accountValue;

    // Start is called before the first frame update
    void Start()
    {
        // set currency symbol
        currencySymbol.text = PlayerPrefs.GetString("currencySymbol", "$");

        // Subscribe to event
        Engine.DataUpdated += UpdateValue;

        // Update value
        UpdateValue();
    }

    private void OnDisable()
    {
        Engine.DataUpdated -= UpdateValue;
    }

    private void UpdateValue()
    {
        // cash avaible cash
        float value = Operations.GetAvailableCash();

        if (value > 0)
            accountValue.transform.parent.GetComponent<Image>().color = UIElements.Instance.earningsColorBright;
        else
            accountValue.transform.parent.GetComponent<Image>().color = UIElements.Instance.expensesColorBright;

        // set accountValue
        accountValue.text = value.ToString("### ##0.00");
    }
}
