using UnityEngine;

public class EnterpriseButtons : MonoBehaviour
{
    public int key = 0;
    public Color color;

    void Start() {
        color = transform.GetComponentInParent<ConnectChilds>().buttonColor;
        transform.GetComponent<SpriteRenderer>().color = color;
    }
}
