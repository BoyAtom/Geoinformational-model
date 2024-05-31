using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OnEnterpriseClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider.gameObject.tag == "EnterpriceButton" && hit.collider != null)
            {
                int Key = hit.collider.gameObject.GetComponent<EnterpriseButtons>().key;

                PlayerPrefs.SetInt("EnterpriseKey", Key);
                PlayerPrefs.SetString("IsNew", "f");
                
                SceneManager.LoadScene("MoreInfo");
            }
        }
    }
}

