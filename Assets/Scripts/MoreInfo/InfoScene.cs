using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class InfoScene : MonoBehaviour
{
    string old_name = "Name";
    string old_description = "Description";
    string new_name = "Name";
    string new_description = "Description";

    /*
    string industry = "Industry";
    string company = "Company";
    */
    string IsNew = "t";
    int key;

    [SerializeField]
    GameObject color;
    [SerializeField]
    TMP_InputField name_input;
    [SerializeField]
    TMP_InputField description_input;
    /*
    [SerializeField]
    TMP_InputField industry_input;
    [SerializeField]
    TMP_InputField company_input;
    */

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
        //industry_input.text = industry;
        //company_input.text = company;
    }

    void SetText() {
        name_input.text = new_name;
        description_input.text = new_description;
        //industry_input.text = industry;
        //company_input.text = company;
    }

    void GetText() {
        new_name = name_input.text;
        new_description = description_input.text;
        //industry = industry_input.text;
        //company = company_input.text;
    }

    public void GetName() {
        new_name = name_input.text;
    }

    public void GetDescription() {
        new_description = description_input.text;
    }

    /*
    public void GetIndustry() {
        industry = industry_input.text;
    }

    public void GetCompany() {
        company = company_input.text;
    }
    */

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

    [Obsolete]
    void GetEnterpriseFromDB() {
        DataBases.DataBase.InitDatabasePath();
        DataTable DTEnterprise = DataBases.DataBase.GetTable(string.Format("SELECT * FROM Enterprises WHERE Key = '{0}'", key));
        //DataTable DTIndustry = DataBases.DataBase.GetTable(string.Format("SELECT * FROM Industries WHERE Enterprise = '{0}'", key));
        //DataTable DTCompany = DataBases.DataBase.GetTable(string.Format("SELECT * FROM Company WHERE Enterprise = '{0}'", key));
        DataTable DTColor = DataBases.DataBase.GetTable(string.Format("SELECT * FROM Colors WHERE Enterprise = '{0}'", key));
        foreach (DataRow row in DTEnterprise.Rows) {
            old_name = row["Name"].ToString();
            old_description = row["Description"].ToString();
        }
        InitText();
        /*
        foreach (DataRow row in DTIndustry.Rows) {
            industry = row["Name"].ToString();
        }
        foreach (DataRow row in DTCompany.Rows) {
            company = row["Name"].ToString();
        }
        */
        foreach (DataRow row in DTColor.Rows) {
            float r = int.Parse(row["Red"].ToString()) / 255;
            float g = int.Parse(row["Green"].ToString()) / 255;
            float b = int.Parse(row["Blue"].ToString()) / 255;

            color.GetComponent<GetCurrentColorMoreInfo>().red.value = r;
            color.GetComponent<GetCurrentColorMoreInfo>().green.value = g;
            color.GetComponent<GetCurrentColorMoreInfo>().blue.value = b;
        }
        name_input.readOnly = true;
    }

    [Obsolete]
    void NewEnteprise(){
        GetText();
        DataBases.DataBase.InitDatabasePath();
        
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Enterprises(Key, Name, Description) VALUES ('{0}', '{1}', '{2}')", key, new_name, new_description));
        //DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Industries(Enterprise, Name) VALUES ('{0}', '{1}')", key, industry));
        //DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Company(Enterprise, Name) VALUES ('{0}', '{1}')", key, company));
        
        int red = (int) (color.GetComponent<GetCurrentColorMoreInfo>().red.value * 255);
        int green = (int) (color.GetComponent<GetCurrentColorMoreInfo>().green.value * 255);
        int blue = (int) (color.GetComponent<GetCurrentColorMoreInfo>().blue.value * 255);

        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Colors(Enterprise, Red, Green, Blue) VALUES ('{0}', '{1}', '{2}', '{3}')", key, red, green, blue));
    }

    [Obsolete]
    void UpdateEnterprise() {
        DataBases.DataBase.InitDatabasePath();

        //DataTable IndusDT = DataBases.DataBase.GetTable(string.Format("SELECT ID FROM Industries WHERE Enterprise = '{0}'", key));
        //DataTable CompaDT = DataBases.DataBase.GetTable(string.Format("SELECT ID FROM Company WHERE Enterprise = '{0}'", key));
        DataTable ColorDT = DataBases.DataBase.GetTable(string.Format("SELECT ID FROM Colors WHERE Enterprise = '{0}'", key));

        //int IndusID = int.Parse(IndusDT.Rows[0][0].ToString());
        //int CompaID = int.Parse(CompaDT.Rows[0][0].ToString());
        int ColorID = int.Parse(ColorDT.Rows[0][0].ToString());

        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Enterprises(Key, Description) VALUES ('{0}', '{1}')", key, new_description));

        //DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Industries(ID, Enterprise, Name) VALUES ('{0}', '{1}', '{2}')", IndusID, key, industry));
        //DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Company(ID, Enterprise, Name) VALUES ('{0}', '{0}', '{2}')", CompaID, key, company));
        
        int red = (int) (color.GetComponent<GetCurrentColorMoreInfo>().red.value*255);
        int green = (int) (color.GetComponent<GetCurrentColorMoreInfo>().green.value*255);
        int blue = (int) (color.GetComponent<GetCurrentColorMoreInfo>().blue.value*255);

        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Colors(ID, Enterprise, Red, Green, Blue) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", ColorID, key, red, green, blue));

        PlayerPrefs.SetInt("OnDestroy", -42);
    }

    public void Cancel () {
        if (IsNew.Equals("f")) PlayerPrefs.SetInt("OnDestroy", -42);
        else PlayerPrefs.SetInt("OnDestroy", key);
        SceneManager.LoadSceneAsync("MapScene");
    }
}
