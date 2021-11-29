using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashboardCashFlowSummaryPrefab : MonoBehaviour
{
    [SerializeField]
    private Image leftbar;
    [SerializeField]
    private Image valueImage;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI value;
    [SerializeField]
    private Button infoButton;

    private CashFlowTypes cashFlowType;

    private void Start()
    {
        // Add listeners to info button
        infoButton.onClick.RemoveAllListeners();
        AddListeners();
    }

    public void Setup(CashFlowTypes cashFlowType, float value)
    {
        // set cashflowtype
        this.cashFlowType = cashFlowType;

        // set left bar color
        Color bright = UIElements.Instance.GetBrightColor(this.cashFlowType);
        leftbar.color = bright;
        valueImage.color = bright;
        infoButton.image.color = bright;

        // set title
        title.text = cashFlowType.ToString();

        // set value
        this.value.text = value.ToString();
    }

    private void AddListeners()
    {
        switch (cashFlowType)
        {
            case CashFlowTypes.Earnings:
                infoButton.interactable = false;
                break;

            case CashFlowTypes.Savings:
                infoButton.onClick.AddListener(LoadSavingsScene);
                break;

            case CashFlowTypes.Expenses:
                infoButton.onClick.AddListener(LoadExpensesScene);
                break;

            default:
                throw new System.Exception("No default case here.");
        }
    }

    private void LoadSavingsScene() => SceneManagement.LoadSceneStatic(Scenes.Savings);
    private void LoadExpensesScene() => SceneManagement.LoadSceneStatic(Scenes.Expenses);
}
