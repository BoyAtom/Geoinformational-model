using UnityEngine;

public class CameraInit : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Rect mapSize;
    private float cameraHeight;

    void Awake() {
        mapSize = spriteRenderer.sprite.rect;
        cameraHeight = cam.orthographicSize*2;

        initCamera();
    }

    void initCamera() {
        if (transform.position.x.Equals(0) && transform.position.x.Equals(0)){
            cameraHeight = mapSize.height / spriteRenderer.sprite.pixelsPerUnit / 2f;
            GetComponent<CameraMovement>().maxCamSize = cameraHeight;
            cam.orthographicSize = cameraHeight;
        }
        else {
            cam.orthographicSize = PlayerPrefs.GetFloat("CamSize");
        }
    }
}
