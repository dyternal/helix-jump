using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;
    private Vector3 newPos;

    void Update()
    {
        newPos = Vector3.Lerp(transform.position, offset + GameData.PlayerObject.transform.position, speed);
        transform.position = newPos;
    }
}
