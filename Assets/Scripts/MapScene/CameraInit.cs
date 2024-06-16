using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CameraInit : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Rect mapSize;
    private float cameraHeight;

    void Awake() {
        print(spriteRenderer.sprite.name + ".jpg");
        if (PlayerPrefs.GetString("ImageDIR") != "Empty") SetSprite();
    }

    public void initCamera() {
        mapSize = spriteRenderer.sprite.rect;
        cameraHeight = cam.orthographicSize*2;

        cameraHeight = mapSize.height / spriteRenderer.sprite.pixelsPerUnit / 2f;
        GetComponent<CameraMovement>().maxCamSize = cameraHeight;
        cam.orthographicSize = cameraHeight;
    }

    public void SetSprite() {
        if (Application.platform != RuntimePlatform.Android) {
            print("file:///" + Application.dataPath + "/StreamingAssets/Images/" + PlayerPrefs.GetString("ImageDIR"));
            StartCoroutine(LoadFromWeb("file:///" + Application.dataPath + "/StreamingAssets/Images/" + PlayerPrefs.GetString("ImageDIR"), PlayerPrefs.GetString("ImageDIR")));
        }
        else {
            print("file:///" + Application.persistentDataPath + "/Images/" + PlayerPrefs.GetString("ImageDIR"));
            StartCoroutine(LoadFromWeb("file:///" + Application.persistentDataPath + "/Images/" + PlayerPrefs.GetString("ImageDIR"), PlayerPrefs.GetString("ImageDIR")));
        }
    }

    IEnumerator LoadFromWeb(string url, string name)
    {
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if (wr.result == UnityWebRequest.Result.Success) {
            Texture2D t = texDl.texture;
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height),
                new Vector2(0.5f, 0.5f), 100f);
            spriteRenderer.sprite = s;
            spriteRenderer.sprite.name = name;
            spriteRenderer.bounds = s.bounds;
        }
        else {
            print(texDl.error);
            print(wr.error);
        }

        transform.GetComponent<CameraMovement>().GetCamLimits();
        initCamera();
    }
}
