using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsersSettings : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown users_dropdown;
    [SerializeField]
    TMP_InputField username_field;
    [SerializeField]
    TMP_InputField password_field;
    [SerializeField]
    Button delete_button;
    List<string> id = new List<string>();
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
        id.Clear();
        names.Clear();
        passwords.Clear();
        roles.Clear();
        DataBases.DataBase.InitDatabasePath(DBName);
        DataTable users = DataBases.DataBase.GetTable("SELECT * FROM Users");
        foreach (DataRow row in users.Rows) {
            id.Add(row["ID"].ToString());
            names.Add(row["Username"].ToString());
            passwords.Add(row["Password"].ToString());
            roles.Add(row["Role"].ToString());
        }

        users_dropdown.ClearOptions();
        users_dropdown.AddOptions(names);
    }

    public void OnUserChange() {
        int current_user = users_dropdown.value;
        print(current_user);
        username_field.text = names[current_user];
        password_field.text = passwords[current_user];
        if (roles[current_user].Equals("0")) delete_button.interactable = false;
        else delete_button.interactable = true;
    }

    public void CreateNewUser() {
        DataBases.DataBase.InitDatabasePath(DBName);
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Users(Username, Password, Role) VALUES ('{0}', '{1}', '{2}')", ""+Random.Range(100000, 1000000), "", "1"));
        InitUsersData();
        users_dropdown.value = id.Count-1;
    }

    public void DeleteUser() {
        DataBases.DataBase.InitDatabasePath(DBName);
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("DELETE FROM Users WHERE ID = ('{0}')", id[users_dropdown.value]));
        InitUsersData();
    }

    public void ChangeUserData() {
        DataBases.DataBase.InitDatabasePath(DBName);
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("REPLACE INTO Users(ID, Username, Password, Role) VALUES ('{0}', '{1}', '{2}', '{3}')", id[users_dropdown.value], username_field.text, password_field.text, roles[users_dropdown.value]));
        InitUsersData();
        users_dropdown.value = id.Count-1;
    }

    void CheckDB(){
        if (PlayerPrefs.HasKey("DataBaseDIR")) {
            DBName = PlayerPrefs.GetString("DataBaseDIR");
        }
    }


}
