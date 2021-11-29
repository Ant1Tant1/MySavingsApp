using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.ReadWrite.Objects;

public class AddCashFlowDetails : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private Image titleBackground;
    [SerializeField]
    private Dropdown subcategoryDropdown;
    [SerializeField]
    private Dropdown yearDropdown;
    [SerializeField]
    private Dropdown monthDropdown;
    [SerializeField]
    private Dropdown dayDropdown;
    [SerializeField]
    private TMP_InputField amountInputfield;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Button addButton;
    [SerializeField]
    private Button deleteButton;

    private CashFlowTypes cashFlowType;
    private CashFlow cashflow = null;
    public void AddNewExpense() => AddNewCashFlow(CashFlowTypes.Expenses);
    public void AddNewSaving() => AddNewCashFlow(CashFlowTypes.Savings);
    public void AddNewRevenue() => AddNewCashFlow(CashFlowTypes.Earnings);

    // Start is called before the first frame update
    private void Start()
    {
        // add listeners on button click 
        cancelButton.onClick.AddListener(SetInactive);
        addButton.onClick.AddListener(OnAddButtonClick);
        deleteButton.onClick.AddListener(OnDeleteButtonClick);
    }

    private void AddNewCashFlow(CashFlowTypes cashFlowType)
    {
        // set cashFlowtypes
        this.cashFlowType = cashFlowType;

        // Set title & dropdowns
        SetTitle(false);
        SetSubcategoryDropdown();
        SetDateDropdown();
        deleteButton.gameObject.SetActive(false);

        // set gameobject active
        gameObject.SetActive(true);
    }

    public void EditCashFlow(CashFlow cashflow)
    {
        // set cashflow
        this.cashflow = cashflow;

        // set cashflow type
        cashFlowType = (CashFlowTypes)cashflow.typeId;

        // change button text
        addButton.GetComponentInChildren<Text>().text = "Edit";

        // Set title & dropdowns
        SetTitle(true);
        SetSubcategoryDropdown();
        SetDateDropdown();
        deleteButton.gameObject.SetActive(true);

        // set values
        subcategoryDropdown.value = cashflow.subtypeId;
        amountInputfield.text = cashflow.value.ToString();
        int yearVal = 4 + cashflow.date.Year - System.DateTime.Today.Year;
        if (yearVal < 0 || yearVal > 4)
            throw new System.Exception("Badly implemented design");
        else
            yearDropdown.value = yearVal;
        monthDropdown.value = cashflow.date.Month - 1;
        dayDropdown.value = cashflow.date.Day - 1;

        // set gameobject active
        gameObject.SetActive(true);

    }

    private void OnAddButtonClick()
    {
        // get value
        float.TryParse(amountInputfield.text, out float value);
        if (value <= 0)
        {
            MessageBar.Instance.DisplayMessage("Please enter a positive amount value");
            return;
        }

        // get subtype
        int subtypeId = subcategoryDropdown.value;

        // get date
        System.DateTime date = System.DateTime.ParseExact(yearDropdown.captionText.text + "/" + monthDropdown.captionText.text + "/" + dayDropdown.captionText.text, "yyyy/MM/dd", CultureInfo.InvariantCulture);

        // get cashflow to string
        string cashflowString = cashFlowType.ToString();

        if (cashflow == null)
        {
            // set key
            int key = 0;
            if (Engine.CashFlows.Count != 0)
                key = Engine.CashFlows.Last().Key + 1;
            Engine.CashFlows.Add(key, new CashFlow(key, Engine.UserId, (int)cashFlowType, subtypeId, value, date));

            // Display Success Message
            MessageBar.Instance.DisplayMessage(cashflowString.Remove(cashflowString.Length - 1) + " was successfully added");
        }
        else
        {
            Engine.CashFlows[cashflow.cashflowId].subtypeId = subtypeId;
            Engine.CashFlows[cashflow.cashflowId].value = value;
            Engine.CashFlows[cashflow.cashflowId].date = date;

            // Display Success Message
            MessageBar.Instance.DisplayMessage(cashflowString.Remove(cashflowString.Length - 1) + " was successfully updated");
        }

        // update
        Engine.UpdateCashFlows();
        DatesContainer.DateDisplayed = date;

        // Reset Panel
        Reset();

    }

    private void OnDeleteButtonClick()
    {
        // remove from dictionnary
        Engine.CashFlows.Remove(cashflow.cashflowId);
        DatesContainer.DateDisplayed = System.DateTime.Today;

        // update
        Engine.UpdateCashFlows();
        // Reset
        Reset();
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        title.text = "";
        amountInputfield.text = "";
        subcategoryDropdown.ClearOptions();
        cashflow = null;
        deleteButton.gameObject.SetActive(false);
    }

    private void SetInactive()
    {
        gameObject.SetActive(false);
    }


    private void SetDateDropdown()
    {
        System.DateTime today = System.DateTime.Today;

        // year
        yearDropdown.ClearOptions();
        List<string> options = Enumerable.Range(today.Year - 4, 5).ToList().ConvertAll(x => x.ToString());
        yearDropdown.AddOptions(options);
        yearDropdown.value = 4;

        // month is set up in Unity
        monthDropdown.value = today.Month - 1;

        // days
        UpdateDateDropdown();
        dayDropdown.value = today.Day - 1;
    }

    public void UpdateDateDropdown()
    {
        dayDropdown.ClearOptions();

        // get year month and nb of days in month
        int year = int.Parse(yearDropdown.captionText.text);
        int month = int.Parse(monthDropdown.captionText.text);
        int nbDays = System.DateTime.DaysInMonth(year, month);

        // reset options
        List<string> options = Enumerable.Range(1, nbDays).ToList().ConvertAll(x => x.ToString("00"));
        dayDropdown.AddOptions(options);
    }

    private void SetSubcategoryDropdown()
    {
        subcategoryDropdown.ClearOptions();
        subcategoryDropdown.AddOptions(Engine.CashFlowSubCategories.GetRelevantDict(cashFlowType).Values.ToList());
    }

    private void SetTitle(bool edit = false)
    {
        Image image = addButton.GetComponentsInChildren<Image>()[1];
        image.color = UIElements.Instance.GetBrightColor(cashFlowType);
        titleBackground.color = UIElements.Instance.GetBrightColor(cashFlowType);

        if (!edit)
            title.text = "Add New\n" + cashFlowType.ToString();
        else
            title.text = "Edit\n" + cashFlowType.ToString();
    }
}
