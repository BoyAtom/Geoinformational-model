using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMapScene : MonoBehaviour
{
    public void GoToScene(string sceneName){
        SceneManager.LoadSceneAsync(sceneName);
    }
}
