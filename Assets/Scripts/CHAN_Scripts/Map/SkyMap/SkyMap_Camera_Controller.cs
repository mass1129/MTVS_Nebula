using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SphericalCoordinates
{
    private float radius, azimuth, elevation;

    public float Azimuth
    {
        get { return azimuth; }
        private set
        {
            azimuth = Mathf.Repeat(value, maxAzimuth_Rad - minAzimuth_Rad);
        }
    }

    public float Elevation
    {
        get { return elevation; }
        private set
        {
            elevation = Mathf.Clamp(value, minElevation_Rad, maxElevation_Rad);
        }
    }

    //Azimuth range
    public float minAzimuth_Deg = 0f;
    private float minAzimuth_Rad;

    public float maxAzimuth_Deg = 360f;
    private float maxAzimuth_Rad;

    //Elevation rages
    public float minElevation_Deg = -90f;
    private float minElevation_Rad;

    public float maxElevation_Deg = 90f;
    private float maxElevation_Rad;

    public SphericalCoordinates(Vector3 _camCoordinate, float _radius)
    {
        //방위각 라디안 값(최대, 최소)을 구한다.
        minAzimuth_Rad = Mathf.Deg2Rad * minAzimuth_Deg;
        maxAzimuth_Rad = Mathf.Deg2Rad * maxAzimuth_Deg;
        //앙각 라디안 값(최대, 최소)을 구한다.
        minElevation_Rad = Mathf.Deg2Rad * minElevation_Deg;
        maxElevation_Rad = Mathf.Deg2Rad * maxElevation_Deg;

        radius = _radius;
        //역함수로 방위각과 앙각을 구한다.
        Azimuth = Mathf.Atan2(_camCoordinate.z, _camCoordinate.x);
        Elevation = Mathf.Asin(_camCoordinate.y / radius);
    }

    public Vector3 toCartesian
    {
        get
        {
            //camera position = (r cosΦ cosθ, r sinΦ, r cosΦ sinθ)
            float t = radius * Mathf.Cos(Elevation);
            return new Vector3(t * Mathf.Cos(Azimuth),
                radius * Mathf.Sin(Elevation), t * Mathf.Sin(Azimuth));
        }
    }

    public SphericalCoordinates Rotate(float newAzimuth, float newElevation)
    {
        Azimuth += newAzimuth;
        Elevation += newElevation;
        
        return this;
    }
}

public class SkyMap_Camera_Controller : MonoBehaviour
{
    private Vector3 lookPosition;
    public float InitialcamPos;
    private Vector3 targetCamPos ;
    bool onClicked;
    public float multiplier = 3;
    Vector3 originPos;
    public SphericalCoordinates sphericalCoordinates;
    float camRadius;
    public Camera cam;

    public float width;
    public float height;
    public float ratio;
    void Start()
    {
        targetCamPos = new Vector3(0, 1.5f, -InitialcamPos);
        originPos = Vector3.zero;
        //카메라 위치 계산을 위해 x, y, z좌표와 반지름 r값을 넘겨준다.
        camRadius = targetCamPos.z;
        sphericalCoordinates = new SphericalCoordinates(targetCamPos, Mathf.Abs(camRadius));
        transform.position = sphericalCoordinates.toCartesian + originPos;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onClicked = true;
            Debug.Log("마우스 누름");
        }
        if (Input.GetMouseButtonUp(0))
        {
            onClicked = false;
            Debug.Log("마우스 땜");
        }
        float horizontal = Input.GetAxis("Mouse X") * -1;
        float vertical = Input.GetAxis("Mouse Y") * -1;
        // 마우스 휠 값은 일단 보류
        float mouseWheel = Input.mouseScrollDelta.y;
        lookPosition = originPos;
        //플레이어 중심으로 구한 구면좌표를 카메라 위치에 적용
        if (mouseWheel > 0)
        { camRadius = InitialcamPos + mouseWheel*10; }
        else if (mouseWheel < 0)
        { camRadius = InitialcamPos - mouseWheel * 10; }
        sphericalCoordinates = new SphericalCoordinates(transform.position, Mathf.Abs(camRadius));
        if (onClicked)
        {
            transform.position = sphericalCoordinates.Rotate
                    (horizontal * multiplier * Time.deltaTime, vertical * multiplier * Time.deltaTime).toCartesian + lookPosition;
        }
        //목표지점으로 카메라를 보게함
        transform.LookAt(lookPosition);
        ShootRay();

    }
    /// <summary>
    /// 스카이뷰 카메라기준으로 Ray를 쏘는 함수
    /// </summary>
    void ShootRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        Debug.Log("Origin Pos" + mousePosition);
        
        mousePosition.x = ((mousePosition.x - (width * (1 - ratio))/2) * (2 - ratio));
        mousePosition.y= ((mousePosition.y - (height * (1 - ratio))/2) * (2 - ratio));
        Debug.Log("Rendor Pos"+mousePosition);

        var WorldPos = cam.ScreenPointToRay(mousePosition);

        RaycastHit hit;
        if(Physics.Raycast(WorldPos, out hit))
        {
            var info = hit.transform.GetComponent<Island_Profile>().user_name;
            Debug.Log(info); 
        }
        
    }
}
