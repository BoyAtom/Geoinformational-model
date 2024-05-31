using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    float rotationSpeedX = 0.1f;
    [SerializeField]
    float rotationSpeedY = 0.1f;
    [SerializeField]
    float rotationSpeedZ = 0.1f;

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ), Space.World);
    }
}
