using System.Collections.Generic;
using UnityEngine;

public class ConnectChilds : MonoBehaviour
{
    public Color buttonColor;
    [SerializeField]
    GameObject enterprice_button;
    [SerializeField]
    public int enterprise_key = 1;
    string tag_name = "Dots";
    LineRenderer lr;
    List<Vector3> dots = new List<Vector3>();
    GameObject mid;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        GetDots();

        Vector3 middle = GetMiddle();
        mid = Instantiate(enterprice_button, middle, Quaternion.identity);
        mid.transform.SetParent(transform);
        mid.GetComponent<EnterpriseButtons>().key = enterprise_key;
        ConnectDots();
    }

    public void UpdateData() {
        GetDots();
        ConnectDots();
        mid.GetComponent<EnterpriseButtons>().color = buttonColor;
        mid.transform.position = GetMiddle();
    }

    public void GetDots(){
        dots.Clear();
        foreach (Transform D in transform.GetComponentInChildren<Transform>()){
            if (D.tag == tag_name) dots.Add(D.position);
        }
    }

    private Vector2 GetMiddle(){
        float x = 0;
        float y = 0;

        foreach (Vector3 D in dots) {
            x += D.x / dots.Count;
            y += D.y / dots.Count;
        }

        return new Vector2(x, y);
    }

    private void ConnectDots() {
        lr.positionCount = dots.Count;
        lr.SetPositions(dots.ToArray());
    }
}
