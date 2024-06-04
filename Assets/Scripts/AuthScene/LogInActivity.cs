using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInActivity : MonoBehaviour
{
    
    [SerializeField]
    TMP_InputField loginField;
    [SerializeField]
    TMP_InputField passwordField;
    [SerializeField]
    string targetScene;


    public void LogIn(){
        string answer = GetDataFromDB(loginField.text, passwordField.text);

        if (answer.Equals("Success")) {
            PlayerPrefs.SetInt("AreLogIn", 1);
            SceneManager.LoadScene(targetScene);
        } 
    }

    public void AsGuest(){
        PlayerPrefs.SetInt("AreLogIn", 0);
        SceneManager.LoadScene(targetScene);
    }

    [System.Obsolete]
    private string GetDataFromDB(string name, string password) {
        string answer = "Hoho, error";
        DataBases.DataBase.InitDatabasePath();
        if (name != "" && password != "") {
            DataTable user_info = DataBases.DataBase.GetTable(string.Format("SELECT * FROM Users WHERE Username = '{0}'", name));
            if (!user_info.Rows.Count.Equals(0)) {
                string passw = "";
                foreach (DataRow user in user_info.Rows) {
                    passw = user["Password"].ToString();
                }

                if (password.Equals(passw)) {
                    answer = "Success";
                }
                else answer = "Wrong password";

            }
            else {
                answer = "No such user";
            }
        }
        else {
            answer = "Empty fields";
        }
        GlobalData._ShowAndroidToastMessage(answer);
        return answer;
    }
}
