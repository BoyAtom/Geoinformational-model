using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWriter : MonoBehaviour
{
    [SerializeField]
    private GameObject dotOrg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    SpriteRenderer parentSprite;
    void InitZones() 
    {
        
    }

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
                GameObject dot = Instantiate(dotOrg, Input.mousePosition, dotOrg.transform.rotation);
                Instantiate(dot);
                buttonPressedTime = 0f;
            }
        }
    }
}
