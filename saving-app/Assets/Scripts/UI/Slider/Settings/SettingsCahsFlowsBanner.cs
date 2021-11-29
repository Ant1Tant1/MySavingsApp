using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.ReadWrite.Objects;
using TMPro;

public class SettingsCahsFlowsBanner : MonoBehaviour, IBanner
{
    [Header("Categories")]
    [SerializeField]
    private TextMeshProUGUI titleCategory;
    [SerializeField]
    private Transform categoryParent;
    [SerializeField]
    private GameObject categoryPrefab;
    public Button confirmButton;

    private Image image;
    // cashflowtype
    private CashFlowTypes cashFlowType;
    // dict for subcategories
    private Dictionary<int, string> subcategoryDict;
    // nb max of subcategories
    private int nbMaxSubCategory;

    // list of instantiated prefabs
    private List<GameObject> prefabsList = new List<GameObject>();

    public void Setup(CashFlowTypes? cFlowType)
    {
        if (cFlowType == null)
            throw new System.Exception("Wrong Banner instantiated.");

        cashFlowType = (CashFlowTypes) cFlowType;

        if(image == null)
            image = gameObject.GetComponent<Image>();

        titleCategory.text = cashFlowType.ToString() + " categories";

        image.color = UIElements.Instance.GetBrightColor(cashFlowType);
        subcategoryDict = Engine.CashFlowSubCategories.GetRelevantDict(cashFlowType);
        nbMaxSubCategory = CashFlowSubCategories.NB_MAX_OF_CATEGORIES;

        SetupCategoryPrefabs();
    }

    public void OnAddCategoryClick()
    {
        if (prefabsList.Count < nbMaxSubCategory)
        {
            GameObject gameObject = Instantiate(categoryPrefab, categoryParent);
            gameObject.GetComponent<CategoryPrefab>().Setup(cashFlowType, (prefabsList.Count + 1).ToString(), "");
            prefabsList.Add(gameObject);
        }
        else
            MessageBar.Instance.DisplayMessage("You cannot have more than " + nbMaxSubCategory.ToString() + " categories in " + cashFlowType.ToString());
    }

    public void OnDeleteCategoryClick()
    {
        if (prefabsList.Count > 1)
        {
            GameObject gO = prefabsList[prefabsList.Count - 1];
            prefabsList.Remove(gO);
            Destroy(gO);
        }
        else
            MessageBar.Instance.DisplayMessage("You cannot have less than 1 category in " + cashFlowType.ToString());
    }

    public void OnConfirmModificationClick()
    {
        // reset
        int index = 0;
        subcategoryDict.Clear();

        // get values
        foreach(GameObject gameObject in prefabsList)
        {
            subcategoryDict[index] = gameObject.GetComponent<CategoryPrefab>().GetCategoryText();
            gameObject.GetComponent<CategoryPrefab>().OnToggleEdit(true);
            index += 1;
        }

        // change engine cashflow subcategories
        Engine.CashFlowSubCategories.SetRelevantDict(cashFlowType, subcategoryDict);

        // save 
        Engine.Save(Engine.CashFlowSubCategories, SerializationTypes.CashFlowSubCategories);
    }

    public void OnResetModificationsClick()
    {
        Setup((CashFlowTypes?)cashFlowType);
    }

    private void SetupCategoryPrefabs()
    {
        DestroyPrefabs();

        foreach (KeyValuePair<int, string> keyValuePair in subcategoryDict)
        {
            GameObject gameObject = Instantiate(categoryPrefab, categoryParent);
            gameObject.GetComponent<CategoryPrefab>().Setup(cashFlowType, (keyValuePair.Key + 1).ToString(), keyValuePair.Value);
            prefabsList.Add(gameObject);
        }

        // will not be called when modifications are done so it is better to clear
        subcategoryDict.Clear();
    }

    private void DestroyPrefabs()
    {
        foreach (GameObject gameObject in prefabsList)
        {
            Destroy(gameObject);
        }
        prefabsList.Clear();
    }
}
