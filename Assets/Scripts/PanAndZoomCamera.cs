using UnityEngine;
using Cinemachine;

public class PanAndZoomCamera : MonoBehaviour
{
    public float panSpeed = 2.0f;
    public float zoomSpeed = 3.0f;
    public float minZoom = 5.0f;
    public float maxZoom = 15.0f;


    private CinemachineVirtualCamera vCam;
    private CinemachineInputProvider input;
    private Transform camTrans;

    void Start()
    {
        vCam = this.GetComponent<CinemachineVirtualCamera>();
        input = this.GetComponent<CinemachineInputProvider>();
        camTrans = vCam.gameObject.transform;
    }

    void Update()
    {
        float x = input.GetAxisValue(0);
        float y = input.GetAxisValue(1);
        float z = input.GetAxisValue(2);

        if(x != 0 || y != 0)
        {
            PanCamera(x, y);
        }

        if(z != 0)
        {
            ZoomCamera(z);
        }

    }

    public void ZoomCamera(float z)
    {
        float newZoom = vCam.m_Lens.OrthographicSize + z * zoomSpeed * Time.deltaTime;
        vCam.m_Lens.OrthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);

    }

    public Vector3 PanDirection(float x, float y)
    {
        Vector3 dir = Vector3.zero;
        if(y >= Screen.height * .95f)
        {
            dir.z += 1f;
        }
        else if(y <= Screen.height * .05f)
        {
            dir.z -= 1f;
        }

        if (x >= Screen.width * .95f)
        {
            dir.x += 1f;
        }
        else if (x <= Screen.width * .05f)
        {
            dir.x -= 1f;
        }

        return dir;


    }

    public void PanCamera(float x, float y)
    {
        Vector3 direction = PanDirection(x, y);
        camTrans.position = Vector3.Lerp(camTrans.position, camTrans.position + direction * panSpeed, Time.deltaTime);
    }
}
