using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public bool onClicked;
    public float multiplier = 3;
    Vector3 originPos;
    public SphericalCoordinates sphericalCoordinates;
    float camRadius;
    public Camera cam;
    public Transform Image_ToolTip;
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
        Image_ToolTip.gameObject.SetActive(false);
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
        Debug.Log("Render Pos"+mousePosition);

        var WorldPos = cam.ScreenPointToRay(mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(WorldPos, out hit))
        {
            // 감지되는 오브젝트의 태그를 분류하자 
            //만약 hit가 섬이면 그섬의 유저 이름을 가져와서 Texture에 위치시킨다. 

                Image_ToolTip.position = Input.mousePosition + new Vector3(150, -50, 0);
            if (hit.collider.CompareTag("UserIsland"))
            {
                var info_name = hit.transform.GetComponent<Island_Profile>().user_name;
                var info_keyword1 = hit.transform.GetComponent<Island_Profile>().user_keyword1;
                var info_keyword2 = hit.transform.GetComponent<Island_Profile>().user_keyword2;
                var info_IslandId = hit.transform.GetComponent<Island_Profile>().user_IslandID;
                Image_ToolTip.gameObject.SetActive(true);
                // 유저 섬ToolTip 생성부
                Image_ToolTip.GetChild(0).GetComponent<Text>().text = info_name + " 의 섬";
                Image_ToolTip.GetChild(1).GetComponent<Text>().text = info_keyword1;
                Image_ToolTip.GetChild(2).GetComponent<Text>().text = info_keyword2;
                Image_ToolTip.GetChild(3).GetComponent<Text>().text = "'F' 를 눌러서 섬에 놀러가기 ";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    PlayerPrefs.SetString("User_Island_ID", info_IslandId);
                    CHAN_GameManager.instance.Go_User_Scene(info_name);
                }
            }
            else if (hit.collider.CompareTag("Player"))
            {
                var nickName = hit.transform.GetComponent<User_Move>().my_Nickname;
                Image_ToolTip.gameObject.SetActive(true);
                Image_ToolTip.GetChild(0).GetComponent<Text>().text = nickName;
                Image_ToolTip.GetChild(1).GetComponent<Text>().text = "";
                Image_ToolTip.GetChild(2).GetComponent<Text>().text = "";
                Image_ToolTip.GetChild(3).GetComponent<Text>().text = "";
            }



        }
        else
        {
            Image_ToolTip.gameObject.SetActive(false);
        }
        
    }
}
