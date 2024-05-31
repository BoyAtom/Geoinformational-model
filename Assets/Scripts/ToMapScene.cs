using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMapScene : MonoBehaviour
{
    public void GoToScene(string sceneName){
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void OpenSite() {
        Application.OpenURL("https://geomaps.pangaia.ru/");
    }
}
