using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
        cameraHeight = mapSize.height / spriteRenderer.sprite.pixelsPerUnit / 2f;
        GetComponent<CameraMovement>().maxCamSize = cameraHeight;
        //cam.orthographicSize = cameraHeight;
    }
}
