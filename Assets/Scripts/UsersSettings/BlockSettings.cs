using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSettings : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("AreLogIn").Equals(0) || PlayerPrefs.GetInt("AreLogIn").Equals(1)) transform.GetComponent<Button>().interactable = false;
    }
}
