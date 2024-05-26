using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectChilds : MonoBehaviour
{

    LineRenderer lr;
    List<Vector3> dots = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        foreach (Transform D in transform.GetComponentInChildren<Transform>()){
            dots.Add(D.position);
        }
        ConnectDots();
    }

    private void ConnectDots() {
        lr.positionCount = dots.Count;
        lr.SetPositions(dots.ToArray());
    }
}
