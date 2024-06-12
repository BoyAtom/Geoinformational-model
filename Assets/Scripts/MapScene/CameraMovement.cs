using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public float minCamSize, maxCamSize;
    private float zoomModifierSpeed = 0.05f;
    public GameObject map;
    public SpriteRenderer mapRenderer;
    private float mapScaleX;
    private float mapScaleY;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;
    private Vector3 dragOrigin;

    private void Start() 
    {
        mapScaleX = map.transform.localScale.x;
        mapScaleY = map.transform.localScale.y;

        float remX = PlayerPrefs.GetFloat("CamX");
        float remY = PlayerPrefs.GetFloat("CamY");
        cam.transform.position = new Vector3(0, 0, -10);
    }

    private void Awake() {
        GetCamLimits();
    }

    public void GetCamLimits() {
        mapMinX = -mapRenderer.bounds.size.x * map.transform.localScale.x / 2f;
        mapMaxX = mapRenderer.bounds.size.x * map.transform.localScale.x / 2f;

        mapMinY = -mapRenderer.bounds.size.y * map.transform.localScale.x / 2f;
        mapMaxY = mapRenderer.bounds.size.y * map.transform.localScale.x / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();
        ZoomCamera();
    }

    //Перемещение камеры
    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            //Код ниже использовался для контроля выходных данных
            //print("origin " + dragOrigin + " newPosition " + cam.ScreenToWorldPoint(Input.mousePosition) + " =difference" + difference);

            cam.transform.position = ClampCamera(cam.transform.position + difference);
            PlayerPrefs.SetFloat("CamX", cam.transform.position.x);
            PlayerPrefs.SetFloat("CamY", cam.transform.position.y);
        }
    }

    //Зум камеры
    private void ZoomCamera() 
    {
        if (Input.touchCount == 2) {
			Touch firstTouch = Input.GetTouch (0);
			Touch secondTouch = Input.GetTouch (1);

			Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
			Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

			float touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
			float touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

			float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

			if (touchesPrevPosDifference > touchesCurPosDifference)
				cam.orthographicSize += zoomModifier;
			if (touchesPrevPosDifference < touchesCurPosDifference)
				cam.orthographicSize -= zoomModifier;
			
		}

		cam.orthographicSize = Mathf.Clamp (cam.orthographicSize, minCamSize, maxCamSize);
        PlayerPrefs.SetFloat("CamSize", cam.orthographicSize);
    }


    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;
        //print("CH = " + camHeight * 2 + "\nCW = " + camWidth * 2);

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        
        var r = mapRenderer;
        if (r == null) return;
        var hounds = r.bounds;

        Gizmos.DrawWireCube(map.transform.position, hounds.extents * 2);
    }
}
