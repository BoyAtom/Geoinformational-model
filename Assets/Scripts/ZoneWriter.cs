using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class ZoneWriter : MonoBehaviour
{

    [SerializeField]
    GameObject Enterprise_orgn;
    [SerializeField]
    GameObject Dot_orgn;
    [SerializeField]
    GameObject Enterprise_parent;

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

        public void CreateEnterprise (GameObject enterprise, GameObject parent) {
            this.go_enterprise = enterprise;
            this.go_enterprise.transform.parent = parent.transform;
            Instantiate(this.go_enterprise);
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

        public void CreateDot(GameObject dot, GameObject parent) {
            this.go_dot = dot;
            this.go_dot.transform.parent = parent.transform;
            Instantiate(this.go_dot);
        }

        public void DestroyDot() {
            Destroy(this.go_dot);
        }
    }
    List<Dot> _dots = new List<Dot>();
    List<Dot> temp_dots = new List<Dot>();

    [SerializeField]
    private GameObject dotOrg;
    // Start is called before the first frame update
    void Start()
    {
        //InitZones();
    }

    void InitZones()
    {
        DataBases.DataBase.InitDatabasePath();
        DataTable dots = DataBases.DataBase.GetTable("SELECT * FROM Tags");
        DataTable enterprises = DataBases.DataBase.GetTable("SELECT * FROM Enterprises");
        
        foreach (DataRow row in enterprises.Rows) {
            int Key = int.Parse(row["Key"].ToString());
            string Name = row["Name"].ToString();
            string Description = row["Description"].ToString();
            _enterprises.Add(new Enterprise(Key, Name, Description));
        }

        foreach (DataRow row in dots.Rows) {
            int enterprise_id = int.Parse(row["Enterprise"].ToString());
            float x = float.Parse(row["X"].ToString());
            float y = float.Parse(row["Y"].ToString());
            _dots.Add(new Dot(enterprise_id, x, y));
        }

        for (int i = 0; i < _enterprises.Count; i++) {
            _enterprises[i].CreateEnterprise(Instantiate(Enterprise_orgn, new Vector3(0, 0, 0), Quaternion.identity), Enterprise_parent);
            for (int j = 0; j < _dots.Count; j++) {
                if (_enterprises[i].Key == _dots[j].Enterprise) {
                    _dots[j].CreateDot(Instantiate(Dot_orgn, new Vector3(_dots[j].x, _dots[j].y, 0), Quaternion.identity), _enterprises[i].go_enterprise);
                }
            }
        }
    }
    Enterprise tmp_enterprise;
    float buttonPressedTime = 0f;
    bool on_screen = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            buttonPressedTime = 0f;

            if (tmp_enterprise == null) {
                tmp_enterprise = new Enterprise(-1, "Temp", "Temp");
                tmp_enterprise.CreateEnterprise(Instantiate(Enterprise_orgn, new Vector3(0, 0, 0), Quaternion.identity), Enterprise_parent);
            }
        }

        if (Input.GetMouseButton(0)) {
            buttonPressedTime += Time.deltaTime;
            if (buttonPressedTime >= 3f) {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Dot tmp_dot = new Dot(pos.x, pos.y);
                tmp_dot.CreateDot(Instantiate(Dot_orgn, pos, Quaternion.identity), tmp_enterprise.go_enterprise);
                temp_dots.Add(new Dot(pos.x, pos.y));

                buttonPressedTime = 0f;
            }
        }
    }
}