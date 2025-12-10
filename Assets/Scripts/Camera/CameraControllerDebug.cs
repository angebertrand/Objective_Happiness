using UnityEngine;

public class CameraControllerDebug : MonoBehaviour
{
    public float cameraSpeed = 250f;
    public float BorderThickness = 10f;
    public Vector2 CameraLimit = new Vector2(400f, 400f);

    public float scrollSpeed = 20f;
    public float yLowLimit = 20f;
    public float yHighLimit = 70f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= Screen.height - BorderThickness)
        {
            pos.z += cameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y < BorderThickness)
        {
            pos.z -= cameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x < BorderThickness)
        {
            pos.x -= cameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - BorderThickness)
        {
            pos.x += cameraSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, yLowLimit, yHighLimit);

        pos.x = Mathf.Clamp(pos.x, -CameraLimit.x, CameraLimit.x);
        pos.z = Mathf.Clamp(pos.z, -CameraLimit.y, CameraLimit.y);
        transform.position = pos;
    }
}
