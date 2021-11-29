using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CategoryPrefab : MonoBehaviour
{
    [SerializeField]
    private Text categoryName;
    [SerializeField]
    private TMP_InputField categoryText;
    [SerializeField]
    private Toggle toggle;
    [Header("Colors")]
    [SerializeField]
    private Color earningsColor;
    [SerializeField]
    private Color expensesColor;
    [SerializeField]
    private Color savingsColor;
    [Header("Sprites for Toggle")]
    [SerializeField]
    private Sprite editSprite;
    [SerializeField]
    private Sprite confirmSprite;

    // Start is called before the first frame update
    void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleEdit);
    }

    public void Setup(CashFlowTypes cashFlowType, string categoryName, string categoryText)
    {
        // set toggle color
        switch(cashFlowType)
        {
            case CashFlowTypes.Earnings:
                toggle.image.color = earningsColor;
                break;
            case CashFlowTypes.Expenses:
                toggle.image.color = expensesColor;
                break;
            case CashFlowTypes.Savings:
                toggle.image.color = savingsColor;
                break;
            default:
                throw new System.Exception("Non implemented cashflow type");
        }

        // set text
        this.categoryName.text = categoryName;
        this.categoryText.text = categoryText;
    }

    public string GetCategoryText()
    {
        return categoryText.text;
    }

    public void OnToggleEdit(bool b)
    {
        // isOn true -> saved -> reset to edit icon
        if (b)
        {
            categoryText.interactable = false;
            toggle.image.sprite = editSprite;
        }
        else
        {
            categoryText.interactable = true;
            toggle.image.sprite = confirmSprite;
        }
    }

}
