using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MapInit : MonoBehaviour
{
    public Camera cam;
    private SpriteRenderer spriteRenderer;
    private Rect mapSize;
    private float cameraHeight;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        mapSize = spriteRenderer.sprite.rect;
        cameraHeight = cam.orthographicSize*2f;
        InitMap();
    }

    private void InitMap() {
        float mapHeight = mapSize.height / spriteRenderer.sprite.pixelsPerUnit;
        transform.localScale = new Vector3(cameraHeight / mapHeight, cameraHeight / mapHeight);
    }
}
