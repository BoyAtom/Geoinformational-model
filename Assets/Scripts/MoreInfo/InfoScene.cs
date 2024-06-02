using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class InfoScene : MonoBehaviour
{
    string old_name = "Name";
    string old_description = "Description";
    string old_industry = "Industry";
    string old_company = "Description";
    string new_name = "Name";
    string new_description = "Description";
    string new_industry = "Industry";
    string new_company = "Company";
    string IsNew = "t";
    int key;

    [SerializeField]
    GameObject color;
    [SerializeField]
    TMP_InputField name_input;
    [SerializeField]
    TMP_InputField description_input;
    [SerializeField]
    TMP_InputField industry_input;
    [SerializeField]
    TMP_InputField company_input;

    [Obsolete]
    void Start()
    {
        key = PlayerPrefs.GetInt("EnterpriseKey");
        IsNew = PlayerPrefs.GetString("IsNew");

        if (IsNew.Equals("f")) {
            GetEnterpriseFromDB();
        }
        else if (IsNew.Equals("t")) {
            InitText();
        }
    }

    void InitText() {
        name_input.text = old_name;
        description_input.text = old_description;
        industry_input.text = old_industry;
        company_input.text = old_company;
    }

    void GetText() {
        new_name = name_input.text;
        new_description = description_input.text;
        new_industry = industry_input.text;
        new_company = company_input.text;
    }

    public void GetName() {
        new_name = name_input.text;
    }

    public void GetDescription() {
        new_description = description_input.text;
    }

    public void GetIndustry() {
        new_industry = industry_input.text;
    }

    public void GetCompany() {
        new_company = company_input.text;
    }

    [Obsolete]
    public void Save() {
        if (IsNew.Equals("f")) {
            UpdateEnterprise();
        }
        else if (IsNew.Equals("t")){
            NewEnteprise();
        }
        SceneManager.LoadScene("MapScene");
    }

    public void Cancel () {
        if (IsNew.Equals("f")) PlayerPrefs.SetInt("OnDestroy", -42);
        else PlayerPrefs.SetInt("OnDestroy", key);
        SceneManager.LoadSceneAsync("MapScene");
    }

    public void Delete() {
        PlayerPrefs.SetInt("OnDestroy", key);
        SceneManager.LoadSceneAsync("MapScene");
    }

    [Obsolete]
    void GetEnterpriseFromDB() {
        DataBases.DataBase.InitDatabasePath();
        DataTable DTEnterprise = DataBases.DataBase.GetTable(string.Format("SELECT * FROM Enterprises WHERE Key = '{0}'", key));
        DataTable DTColor = DataBases.DataBase.GetTable(string.Format("SELECT * FROM Colors WHERE Enterprise = '{0}'", key));
        foreach (DataRow row in DTEnterprise.Rows) {
            old_name = row["Name"].ToString();
            old_description = row["Description"].ToString();
            old_company = row["Company"].ToString();
            old_industry = row["Industry"].ToString();
        }
        InitText();

        foreach (DataRow row in DTColor.Rows) {
            float r = int.Parse(row["Red"].ToString()) / 255;
            float g = int.Parse(row["Green"].ToString()) / 255;
            float b = int.Parse(row["Blue"].ToString()) / 255;

            color.GetComponent<GetCurrentColorMoreInfo>().red.value = r;
            color.GetComponent<GetCurrentColorMoreInfo>().green.value = g;
            color.GetComponent<GetCurrentColorMoreInfo>().blue.value = b;
        }
    }

    [Obsolete]
    void NewEnteprise(){
        GetText();
        DataBases.DataBase.InitDatabasePath();
        
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Enterprises(Key, Name, Description, Company, Industry) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",key, new_name, new_description, new_company, new_industry));
        
        int red = (int) (color.GetComponent<GetCurrentColorMoreInfo>().red.value * 255);
        int green = (int) (color.GetComponent<GetCurrentColorMoreInfo>().green.value * 255);
        int blue = (int) (color.GetComponent<GetCurrentColorMoreInfo>().blue.value * 255);

        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Colors(Enterprise, Red, Green, Blue) VALUES ('{0}', '{1}', '{2}', '{3}')", key, red, green, blue));
    }

    [Obsolete]
    void UpdateEnterprise() {
        GetText();
        DataBases.DataBase.InitDatabasePath();

        DataTable ColorDT = DataBases.DataBase.GetTable(string.Format("SELECT ID FROM Colors WHERE Enterprise = '{0}'", key));

        int ColorID = int.Parse(ColorDT.Rows[0][0].ToString());

        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Enterprises(Key, Name, Description, Company, Industry) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", key, new_name, new_description, new_company, new_industry));
        
        int red = (int) (color.GetComponent<GetCurrentColorMoreInfo>().red.value*255);
        int green = (int) (color.GetComponent<GetCurrentColorMoreInfo>().green.value*255);
        int blue = (int) (color.GetComponent<GetCurrentColorMoreInfo>().blue.value*255);

        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Colors(ID, Enterprise, Red, Green, Blue) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", ColorID, key, red, green, blue));

        PlayerPrefs.SetInt("OnDestroy", -42);
    }
}
