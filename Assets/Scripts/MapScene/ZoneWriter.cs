using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZoneWriter : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    SpriteRenderer mapRenderer;
    [SerializeField]
    GameObject Enterprise_orgn;
    [SerializeField]
    GameObject Dot_orgn;
    [SerializeField]
    GameObject Enterprise_parent;
    [SerializeField]
    Button ButtonRecord;
    [SerializeField]
    Button ButtonCancel;
    [SerializeField]
    Toggle toggle;
    [SerializeField]
    TMP_Dropdown DropdownIndustries;

    IEnumerator Waiter() {
        yield return new WaitForSeconds(0.5f);
    }

    public class Enterprise {
        public GameObject go_enterprise;
        public int Key;
        public string Name;
        public string Description;
        public int Industry;

        public Enterprise (int key, string name, int industry) {
            this.Key = key;
            this.Name = name;
            this.Industry = industry;
        }

        public Enterprise (int key, string name) {
            this.Key = key;
            this.Name = name;
        }

        public Enterprise () {}

        public void SetColor(Color color) {
            this.go_enterprise.GetComponent<LineRenderer>().startColor = new Color(color.r, color.g, color.b);
            this.go_enterprise.GetComponent<LineRenderer>().endColor = new Color(color.r, color.g, color.b);
            this.go_enterprise.GetComponent<ConnectChilds>().buttonColor = color;
        }

        public void CreateEnterprise(GameObject enterprise, Vector2 pos, GameObject parent, List<ColorDB> colors) {
            this.go_enterprise = Instantiate(enterprise, pos, Quaternion.identity);
            this.go_enterprise.transform.parent = parent.transform;

            Color clr = new Color(1, 0, 0, 0.32f);
            foreach (ColorDB color in colors) {
                if (this.Key.Equals(color.Enterprise)) {
                    clr = color.returnColor();
                    break;
                }
            }

            SetColor(clr);
        }

        public void DestroyEnterprise() {
            Destroy(this.go_enterprise);
        }
    }

    List<Enterprise> _enterprises = new List<Enterprise>();

    public class Dot {
        public GameObject go_dot;
        public int Enterprise;
        public float x;
        public float y;

        public Dot(int Enterprise, float x, float y) {
            this.Enterprise = Enterprise;
            this.x = x;
            this.y = y;
        }

        public Dot(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public Dot(int Enterprise) {
            this.Enterprise = Enterprise;
        }

        public Dot() {}

        public void SetCoords(Vector2 pos){
            this.x = pos.x;
            this.y = pos.y;
        }

        public void SetColor(Color color){
            Color clr = this.go_dot.GetComponentInParent<ConnectChilds>().buttonColor;
            this.go_dot.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b);
        }

        public void CreateDot(GameObject dot, Vector2 pos, GameObject parent, List<ColorDB> colors) {
            SetCoords(pos);
            this.go_dot = Instantiate(dot, new Vector3(0f, 0f), Quaternion.identity);
            this.go_dot.transform.parent = parent.transform;
            this.go_dot.transform.position = new Vector2(this.x, this.y);

            Color clr = new Color(1, 0, 0);
            foreach (ColorDB color in colors) {
                if (color.Enterprise.Equals(this.Enterprise)) {
                    clr = color.returnColor();
                    break;
                }
            }

            SetColor(clr);
        }

        public void DestroyDot() {
            Destroy(this.go_dot);
        }
    }

    public class ColorDB {
        public int Enterprise;
        public float r;
        public float g;
        public float b;

        public ColorDB (int enter, float r, float g, float b) {
            this.Enterprise = enter;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color returnColor () {
            return new Color(r, g, b, 0.32f);
        }
    }
    List<string> industries = new List<string>();
    List<Dot> _dots = new List<Dot>();
    List<Dot> temp_dots = new List<Dot>();
    List<ColorDB> _colors = new List<ColorDB>();
    bool is_guest = false;
    string DBName = "GeoInfo.db";

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        InitMap("RF_okruga_2022.jpg");
        InitIndustries();
        is_guest = CheckGuest();
        CheckDB();

        int onDestroy = PlayerPrefs.GetInt("OnDestroy");
        if (onDestroy != -42 && !is_guest) {
            DestroyEnterprise(onDestroy);
            PlayerPrefs.SetInt("OnDestroy", -42);
        }

        DataBases.DataBase.InitDatabasePath(DBName);
        InitEnterprises();
    }

    [Obsolete]
    void InitMap(string map_name) {
        if (Application.platform == RuntimePlatform.Android) {
            string path = Application.persistentDataPath + "/Images/" + map_name;
            if(!File.Exists(path))
                {
                     WWW load = new WWW("jar:file://" + Application.dataPath + "!/assets/Images/" + map_name);
                     while (!load.isDone) { }
                     File.WriteAllBytes(path, load.bytes);
                }
        }
    }

    bool CheckGuest() {
        if (PlayerPrefs.GetInt("AreLogIn").Equals(1) || PlayerPrefs.GetInt("AreLogIn").Equals(2)) return false;
        else return true;
    }

    void CheckDB(){
        if (PlayerPrefs.HasKey("DataBaseDIR")) {
            DBName = PlayerPrefs.GetString("DataBaseDIR");
        }
    }

    string CheckMap(){
        if (PlayerPrefs.GetString("ImageDIR") != "Empty") {
            return PlayerPrefs.GetString("ImageDIR");
        }
        else return "Empty";
    }

    public void SetSettings() {
        PlayerPrefs.SetInt("CurrentIndustry", DropdownIndustries.value);
        if (!DropdownIndustries.interactable) {
            PlayerPrefs.SetInt("SettingIsOn", 0);
        }
        else PlayerPrefs.SetInt("SettingIsOn", 1);
    }

    public void GetSettings() {
        DropdownIndustries.value = PlayerPrefs.GetInt("CurrentIndustry");
        if (PlayerPrefs.GetInt("SettingIsOn").Equals(1)) toggle.isOn = true;
        else if (PlayerPrefs.GetInt("SettingIsOn").Equals(0)) toggle.isOn = false;
        DropdownIndustries.interactable = toggle.isOn;
    }

    public void InitEnterprises()
    {
        DataBases.DataBase.InitDatabasePath(DBName);

        ClearMap();
        GetSettings();

        DataTable enterprises;
        if (toggle.isOn) {
            enterprises = DataBases.DataBase.GetTable(string.Format("SELECT * FROM Enterprises WHERE Industry = '{0}'", DropdownIndustries.value));
        }
        else enterprises = DataBases.DataBase.GetTable("SELECT * FROM Enterprises");
        DataTable dots = DataBases.DataBase.GetTable("SELECT * FROM Tags");
        DataTable colors = DataBases.DataBase.GetTable("SELECT * FROM Colors");
        
        foreach (DataRow row in enterprises.Rows) {
            int Key = int.Parse(row["Key"].ToString());
            string Name = row["Name"].ToString();
            int indust = int.Parse(row["Industry"].ToString());
            _enterprises.Add(new Enterprise(Key, Name, indust));
        }

        foreach (DataRow row in colors.Rows) {
            int enterprise_id = int.Parse(row["Enterprise"].ToString());
            float r = float.Parse(row["Red"].ToString());
            float g = float.Parse(row["Green"].ToString());
            float b = float.Parse(row["Blue"].ToString());
            _colors.Add(new ColorDB(enterprise_id, r, g, b));
        }

        foreach (DataRow row in dots.Rows) {
            int enterprise_id = int.Parse(row["Enterprise"].ToString());
            float x = float.Parse(row["X"].ToString());
            float y = float.Parse(row["Y"].ToString());
            _dots.Add(new Dot(enterprise_id, x, y));
        }

        //Создание разметки
        foreach (Enterprise enterprise in _enterprises) {
            enterprise.CreateEnterprise(Enterprise_orgn, new Vector2(0, 0), Enterprise_parent, _colors);
            foreach (Dot dot in _dots) {
                if (dot.Enterprise == enterprise.Key) {
                    dot.CreateDot(Dot_orgn, new Vector2(dot.x, dot.y), enterprise.go_enterprise, _colors);
                }
            }
            enterprise.go_enterprise.transform.GetComponent<ConnectChilds>().enterprise_key = enterprise.Key;
        }

        /*
        foreach (ColorDB colorDB in _colors) {
            foreach (Enterprise enterprise in _enterprises) {
                print(colorDB.Enterprise + " = " + enterprise.Key);
                if (enterprise.Key == colorDB.Enterprise) {
                    enterprise.SetColor(colorDB.returnColor());
                    break;
                }
            }
        }

        foreach (Dot dot in _dots) {
            foreach (ColorDB colorDB in _colors) {
                print(dot.Enterprise + " = " + colorDB.Enterprise);
                if (dot.Enterprise == colorDB.Enterprise) {
                    dot.SetColor(colorDB.returnColor());
                }
            }
        }
        */
    }

    [System.Obsolete]
    public void InitIndustries(){
        DataBases.DataBase.InitDatabasePath(DBName);
        DataTable industriesDT = DataBases.DataBase.GetTable("SELECT * FROM Industries");

        foreach (DataRow industry in industriesDT.Rows) {
            industries.Add(industry["Name"].ToString());
        }

        DropdownIndustries.ClearOptions();
        DropdownIndustries.AddOptions(industries);
    }

    [System.Obsolete]
    int CreateNewEnterprise(){
        DataBases.DataBase.InitDatabasePath(DBName);

        string newName = "NewEnterprise";
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Enterprises(Name) VALUES ('{0}')", newName));
        print("New enterprise created");
        DataTable keys = DataBases.DataBase.GetTable(string.Format("SELECT Key FROM Enterprises WHERE Name = ('{0}')", newName));

        int newDBKey = 0;
        foreach (DataRow row in keys.Rows) newDBKey = int.Parse(row["Key"].ToString());
        print("New key = " + newDBKey);

        foreach (Dot dot in temp_dots) DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Tags(Enterprise, X, Y) VALUES ('{0}', '{1}', '{2}')", newDBKey, dot.x, dot.y));

        return newDBKey;
    }

    void ClearMap(){
        foreach (Dot dot in _dots) {
            dot.DestroyDot();       
        }
        _dots.Clear();
        foreach (Enterprise enterprise in _enterprises) {
            enterprise.DestroyEnterprise();
        }
        _enterprises.Clear();
        _colors.Clear();

        foreach (Transform enter in transform.GetComponentInChildren<Transform>()) {
            foreach (Transform obj in enter.GetComponentInChildren<Transform>()) {
                Destroy(obj.gameObject);
            }
            Destroy(enter.gameObject);
        }
    }

    void DestroyEnterprise(int destroyKey) {
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("DELETE FROM Enterprises WHERE Key = ('{0}')", destroyKey));
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("DELETE FROM Tags WHERE Enterprise = ('{0}')", destroyKey));
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("DELETE FROM Colors WHERE Enterprise = ('{0}')", destroyKey));

        PlayerPrefs.SetInt("OnDestroy", -42);
    }

    Enterprise tmp_enterprise;
    float buttonPressedTime = 0f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !is_guest) {
            buttonPressedTime = 0f;
        }
        if (Input.GetMouseButton(0) && !is_guest) {
            buttonPressedTime += Time.deltaTime;

            if (buttonPressedTime >= 3f) {
                buttonPressedTime = 0f;

                if (tmp_enterprise == null) {
                    tmp_enterprise = new Enterprise(-1, "Temp", 0);
                    ButtonRecord.interactable = true;
                    ButtonCancel.interactable = true;

                    tmp_enterprise.CreateEnterprise(Enterprise_orgn, new Vector2(0, 0), Enterprise_parent, _colors);
                }
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                print(pos);

                Dot tmp_dot = new Dot();
                tmp_dot.CreateDot(Dot_orgn, new Vector2(pos.x, pos.y), tmp_enterprise.go_enterprise, _colors);
                temp_dots.Add(tmp_dot);
                tmp_enterprise.go_enterprise.transform.GetComponent<ConnectChilds>().UpdateData();
            }
        }
    }

    public void OnCancelClick() {
        foreach (Dot dot in temp_dots)
        {
            dot.DestroyDot();
        }
        temp_dots.Clear();
        tmp_enterprise.DestroyEnterprise();
        tmp_enterprise = null;

        ButtonRecord.interactable = false;
        ButtonCancel.interactable = false;
    }

    public void OnRecordClick() {
        PlayerPrefs.SetString("IsNew", "t");

        int newKey = CreateNewEnterprise();
        PlayerPrefs.SetInt("EnterpriseKey", newKey);

        SceneManager.LoadScene("MoreInfo");
    }

    public void OnIndustriesClick() {
        SetSettings();
        InitEnterprises();
    }

    public void OnToggleClick() {
        DropdownIndustries.interactable = toggle.isOn;

        SetSettings();
        InitEnterprises();
    }
}