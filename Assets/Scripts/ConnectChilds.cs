using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConnectChilds : MonoBehaviour
{
    [SerializeField]
    GameObject enterprice_button;
    [SerializeField]
    string enterprise_name = "Visual Basic";
    string tag_name = "Dots";
    LineRenderer lr;
    List<Vector3> dots = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        foreach (Transform D in transform.GetComponentInChildren<Transform>()){
            if (D.tag != tag_name) break;

            dots.Add(D.position);
            D.GetComponent<Button>().onClick.AddListener(ShowName);
        }

        Vector3 middle = GetMiddle();
        GameObject mid = Instantiate(enterprice_button, middle, Quaternion.identity);
        mid.transform.SetParent(transform);
        mid.GetComponent<EnterpriseButtons>().name = enterprise_name;

        

        ConnectDots();
    }

    private Vector3 GetMiddle(){
        float x = 0;
        float y = 0;

        foreach (Vector3 D in dots) {
            x += D.x / dots.Count;
            y += D.y / dots.Count;
        }

        print(x + " / " + y);

        return new Vector3(x, y, 0);
    }

    private void ConnectDots() {
        lr.positionCount = dots.Count;
        lr.SetPositions(dots.ToArray());
    }

    public void ShowName() {
        print(enterprise_name);
    }
}
