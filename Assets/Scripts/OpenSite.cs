using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSite : MonoBehaviour
{
    public void Open(string url) {
        Application.OpenURL(url);
    }
}
