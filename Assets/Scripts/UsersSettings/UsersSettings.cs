using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class UsersSettings : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown users_dropdown;
    [SerializeField]
    TMP_InputField username_field;
    [SerializeField]
    TMP_InputField password_field;
    List<string> names = new List<string>();
    List<string> passwords = new List<string>();
    List<string> roles = new List<string>();
    string DBName = "GeoInfo.db";

    [System.Obsolete]
    void Start() {
        CheckDB();
        InitUsersData();
        OnUserChange();
    }

    [System.Obsolete]
    void InitUsersData() {
        names.Clear();
        passwords.Clear();
        roles.Clear();
        DataBases.DataBase.InitDatabasePath(DBName);
        DataTable users = DataBases.DataBase.GetTable("SELECT * FROM Users");
        foreach (DataRow row in users.Rows) {
            names.Add(row["Username"].ToString());
            passwords.Add(row["Password"].ToString());
            roles.Add(row["Role"].ToString());
        }

        users_dropdown.ClearOptions();
        users_dropdown.AddOptions(names);
    }

    public void OnUserChange() {
        int current_user = users_dropdown.value;
        username_field.text = names[current_user];
        password_field.text = passwords[current_user];
    }

    public void CreateNewUser() {
        DataBases.DataBase.InitDatabasePath(DBName);
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Users(Username, Password, Role) VALUES ('{0}', '{1}', '{2}')", ""+Random.Range(100000, 1000000), "", "1"));
        InitUsersData();
    }

    public void ChangeUserData() {
        DataBases.DataBase.InitDatabasePath(DBName);
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Users(ID, Username, Password) VALUES ('{0}', '{1}', '{2}')", users_dropdown.value, username_field.text, password_field.text));
        InitUsersData();
    }

    void CheckDB(){
        if (PlayerPrefs.HasKey("DataBaseDIR")) {
            DBName = PlayerPrefs.GetString("DataBaseDIR");
        }
    }


}
