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
    string DBName = "GeoInfo.db";

    void Start() {
        CheckDB();
    }

    void CheckDB(){
        if (PlayerPrefs.HasKey("DataBaseDIR")) {
            DBName = PlayerPrefs.GetString("DataBaseDIR");
        }
    }

    public void LogIn(){
        string answer = GetDataFromDB(loginField.text, passwordField.text);

        if (answer.Equals("Авторизация прошла успешно")) {
            PlayerPrefs.SetInt("AreLogIn", 1);
            SceneManager.LoadScene(targetScene);
        }
        else if (answer.Equals("Вход в качестве админа выполнен")) {
            PlayerPrefs.SetInt("AreLogIn", 2);
            SceneManager.LoadScene(targetScene);
        }
    }

    public void AsGuest(){
        PlayerPrefs.SetInt("AreLogIn", 0);
        if(Application.platform == RuntimePlatform.Android) GlobalData._ShowAndroidToastMessage("Вы вошли в качестве гостя - часть функционала не доступна");
        SceneManager.LoadScene(targetScene);
    }

    [System.Obsolete]
    private string GetDataFromDB(string name, string password) {
        string answer = "Hoho, error";
        print(DBName);
        DataBases.DataBase.InitDatabasePath(DBName);
        if (name != "" && password != "") {
            DataTable user_info = DataBases.DataBase.GetTable(string.Format("SELECT * FROM Users WHERE Username = '{0}'", name));
            if (!user_info.Rows.Count.Equals(0)) {
                string passw = "";
                string role = "0";
                foreach (DataRow user in user_info.Rows) {
                    passw = user["Password"].ToString();
                    role = user["Role"].ToString();
                }

                if (password.Equals(passw)) {
                    if (role == "1") {
                        answer = "Авторизация прошла успешно";
                    }
                    else if (role == "0") {
                        answer = "Вход в качестве админа выполнен";
                    }
                }
                else answer = "Неверный пароль";

            }
            else {
                answer = "Нет такого имени пользователя";
            }
        }
        else {
            answer = "Поля для ввода данных пустые";
        }
        if(Application.platform == RuntimePlatform.Android) GlobalData._ShowAndroidToastMessage(answer);
        return answer;
    }
}
