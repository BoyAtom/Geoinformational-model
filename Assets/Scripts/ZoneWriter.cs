using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TerrainUtils;
using UnityEngine.UI;

public class ZoneWriter : MonoBehaviour
{

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

    IEnumerator Waiter() {
        yield return new WaitForSeconds(0.5f);
    }

    public class Enterprise {
        public GameObject go_enterprise;
        public int Key;
        public string Name;
        public string Description;

        public Enterprise (int key, string name, string description) {
            this.Key = key;
            this.Name = name;
            this.Description = description;
        }

        public Enterprise (int key, string name) {
            this.Key = key;
            this.Name = name;
        }

        public Enterprise () {}

        public void SetColor(Color color) {
            this.go_enterprise.GetComponent<LineRenderer>().startColor = color;
            this.go_enterprise.GetComponent<LineRenderer>().endColor = color;
        }

        public void CreateEnterprise (GameObject enterprise, Vector2 pos, GameObject parent) {
            this.go_enterprise = Instantiate(enterprise, pos, Quaternion.identity);
            this.go_enterprise.transform.parent = parent.transform;
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
            this.go_dot.GetComponent<SpriteRenderer>().color = color;
        }

        public void CreateDot(GameObject dot, Vector2 pos, GameObject parent) {
            SetCoords(pos);
            this.go_dot = Instantiate(dot, pos, Quaternion.identity);
            this.go_dot.transform.parent = parent.transform;
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
            return new Color(r, g, b);
        }
    }
    List<Dot> _dots = new List<Dot>();
    List<Dot> temp_dots = new List<Dot>();
    List<ColorDB> _colors = new List<ColorDB>();
    int current_count = 0;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        int onDestroy = PlayerPrefs.GetInt("OnDestroy");
        if (onDestroy != -42) {
            //DestroyEnterprise(onDestroy);
        }

        DataBases.DataBase.InitDatabasePath();
        DataTable entData = DataBases.DataBase.GetTable("SELECT * FROM Enterprises");
        current_count = entData.Rows.Count;

        InitEnterprises();
    }

    [System.Obsolete]
    void InitEnterprises()
    {
        PlayerPrefs.SetInt("OnDestroy", -42);

        DataBases.DataBase.InitDatabasePath();
        DataTable dots = DataBases.DataBase.GetTable("SELECT * FROM Tags");
        DataTable enterprises = DataBases.DataBase.GetTable("SELECT * FROM Enterprises");
        DataTable colors = DataBases.DataBase.GetTable("SELECT * FROM Colors");
        
        foreach (DataRow row in enterprises.Rows) {
            int Key = int.Parse(row["Key"].ToString());
            string Name = row["Name"].ToString();
            _enterprises.Add(new Enterprise(Key, Name));
        }

        StartCoroutine(Waiter());

        foreach (DataRow row in colors.Rows) {
            int enterprise_id = int.Parse(row["Enterprise"].ToString());
            float r = float.Parse(row["Red"].ToString());
            float g = float.Parse(row["Green"].ToString());
            float b = float.Parse(row["Blue"].ToString());
            _colors.Add(new ColorDB(enterprise_id, r, g, b));
        }

        StartCoroutine(Waiter());

        foreach (DataRow row in dots.Rows) {
            int enterprise_id = int.Parse(row["Enterprise"].ToString());
            float x = float.Parse(row["X"].ToString());
            float y = float.Parse(row["Y"].ToString());
            _dots.Add(new Dot(enterprise_id, x, y));
        }

        StartCoroutine(Waiter());

        foreach (Enterprise enterprise in _enterprises) {
            enterprise.CreateEnterprise(Enterprise_orgn, new Vector2(0, 0), Enterprise_parent);
            foreach (Dot dot in _dots) {
                if (dot.Enterprise == enterprise.Key) {
                    dot.CreateDot(Dot_orgn, new Vector2(dot.x, dot.y), enterprise.go_enterprise);
                }
            }
            enterprise.go_enterprise.transform.GetComponent<ConnectChilds>().enterprise_key = enterprise.Key;
        }

        StartCoroutine(Waiter());

        foreach (Dot dot in _dots) {
            foreach (ColorDB colorDB in _colors) {
                if (dot.Enterprise == colorDB.Enterprise) {
                    dot.SetColor(colorDB.returnColor());
                }
            }
        }

        StartCoroutine(Waiter());

        foreach (Enterprise enterprise in _enterprises) {
            foreach (ColorDB colorDB in _colors) {
                    if (enterprise.Key == colorDB.Enterprise) {
                        enterprise.SetColor(colorDB.returnColor());
                    }
                }
            enterprise.go_enterprise.transform.GetComponent<ConnectChilds>().UpdateData(); 
        }

        StartCoroutine(Waiter());
    }

    [System.Obsolete]
    int CreateNewEnterprise(){
        DataBases.DataBase.InitDatabasePath();

        string newName = "NewEnterprise";
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Enterprises(Key, Name) VALUES ('{0}', '{1}')", current_count+1, newName));
        DataTable keys = DataBases.DataBase.GetTable(string.Format("SELECT Key FROM Enterprises WHERE Name = ('{0}')", newName));

        int newDBKey = 0;
        foreach (DataRow row in keys.Rows) newDBKey = int.Parse(row["Key"].ToString());
        print("New key = " + newDBKey);

        foreach (Dot dot in temp_dots) DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Tags(Enterprise, X, Y) VALUES ('{0}', '{1}', '{2}')", newDBKey, dot.x, dot.y));

        return newDBKey;
    }

    void DestroyEnterprise(int destroyKey) {
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("DELETE FROM Enterprises WHERE Key = ('{0}')", destroyKey));
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("DELETE FROM Tags WHERE Enterprise = ('{0}')", destroyKey));
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("DELETE FROM Industries WHERE Enterprise = ('{0}')", destroyKey));
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("DELETE FROM Company WHERE Enterprise = ('{0}')", destroyKey));
        DataBases.DataBase.ExecuteQueryWithoutAnswer(string.Format("DELETE FROM Colors WHERE Enterprise = ('{0}')", destroyKey));
    }

    Enterprise tmp_enterprise;
    float buttonPressedTime = 0f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            buttonPressedTime = 0f;
        }

        if (Input.GetMouseButton(0)) {
            buttonPressedTime += Time.deltaTime;

            if (buttonPressedTime >= 3f) {

                buttonPressedTime = 0f;

                if (tmp_enterprise == null) {

                    tmp_enterprise = new Enterprise(-1, "Temp", "Temp");

                    ButtonRecord.interactable = true;
                    ButtonCancel.interactable = true;

                    tmp_enterprise.CreateEnterprise(Enterprise_orgn, new Vector2(0, 0), Enterprise_parent);
                }

                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                print(pos);

                Dot tmp_dot = new Dot();
                tmp_dot.CreateDot(Dot_orgn, new Vector2(pos.x, pos.y), tmp_enterprise.go_enterprise);
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

        SceneManager.LoadScene("MoreInfo");
    }
}