using System.Data;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class InfoScene : MonoBehaviour
{

    string name;
    string description;
    string enterprise;
    string company;
    int key;

    [SerializeField]
    GameObject color;
    [SerializeField]
    TMP_InputField name_input;
    [SerializeField]
    TMP_InputField description_input;
    [SerializeField]
    TMP_InputField enterprise_input;
    [SerializeField]
    TMP_InputField company_input;

    void Start()
    {
        name = PlayerPrefs.GetString("name");
        description = PlayerPrefs.GetString("name");
        enterprise = PlayerPrefs.GetString("name");
        company = PlayerPrefs.GetString("name");

        key = PlayerPrefs.GetInt("EnterpriseKey");
        GetEnterpriseFromDB();

        print(name);

        InitText();
    }

    void InitText() {
        name_input.text = name;
        description_input.text = description;
        //enterprise_input.text = enterprise;
        //company_input.text = company;
    }

    int entKey;

    public void Save() {
        SendDataToDB();
        PlayerPrefs.SetInt("EnterpriseKey", entKey);
        SceneManager.LoadSceneAsync("MapScene");
    }

    void GetEnterpriseFromDB() {
        DataTable dt = DataBases.DataBase.GetTable(String.Format("SELECT * FROM Enterprises WHERE Key = '{0}'", key));

        /*
        print(dt.Rows[0][0].ToString());
        print(dt.Rows[0][1].ToString());
        print(dt.Rows[0][2].ToString());
        */
    }
    void SendDataToDB(){
        DataBases.DataBase.InitDatabasePath();
        
        DataBases.DataBase.ExecuteQueryWithoutAnswer(String.Format("INSERT INTO Enterprises(Name, Description) VALUES ('{0}', '{1}')", name, description));
        DataTable dt = DataBases.DataBase.GetTable(String.Format("SELECT Key FROM Enterprises WHERE name = '{0}'", name));
        
        int entKey = int.Parse(dt.Rows[0][0].ToString());
        
        int red = (int) color.GetComponent<GetCurrentColorMoreInfo>().red.value;
        int green = (int) color.GetComponent<GetCurrentColorMoreInfo>().green.value;
        int blue = (int) color.GetComponent<GetCurrentColorMoreInfo>().blue.value;

        DataBases.DataBase.ExecuteQueryWithoutAnswer(String.Format("INSERT INTO Color(Enterprise, Red, Green, Blue) VALUES ({0}, {1}, {2}, {3})", entKey, red, green, blue));
    }

    public void Cancel () {
        PlayerPrefs.SetInt("EnterpriseKey", -1);
        SceneManager.LoadSceneAsync("MapScene");
    }
}
