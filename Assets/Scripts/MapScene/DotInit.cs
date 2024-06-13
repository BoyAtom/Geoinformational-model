using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Color color = transform.GetComponentInParent<ConnectChilds>().buttonColor;
        transform.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b);
    }
}
