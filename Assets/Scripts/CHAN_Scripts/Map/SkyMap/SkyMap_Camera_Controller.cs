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
        //������ ���� ��(�ִ�, �ּ�)�� ���Ѵ�.
        minAzimuth_Rad = Mathf.Deg2Rad * minAzimuth_Deg;
        maxAzimuth_Rad = Mathf.Deg2Rad * maxAzimuth_Deg;
        //�Ӱ� ���� ��(�ִ�, �ּ�)�� ���Ѵ�.
        minElevation_Rad = Mathf.Deg2Rad * minElevation_Deg;
        maxElevation_Rad = Mathf.Deg2Rad * maxElevation_Deg;

        radius = _radius;
        //���Լ��� �������� �Ӱ��� ���Ѵ�.
        Azimuth = Mathf.Atan2(_camCoordinate.z, _camCoordinate.x);
        Elevation = Mathf.Asin(_camCoordinate.y / radius);
    }

    public Vector3 toCartesian
    {
        get
        {
            //camera position = (r cos�� cos��, r sin��, r cos�� sin��)
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
        //ī�޶� ��ġ ����� ���� x, y, z��ǥ�� ������ r���� �Ѱ��ش�.
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
            Debug.Log("���콺 ����");
        }
        if (Input.GetMouseButtonUp(0))
        {
            onClicked = false;
            Debug.Log("���콺 ��");
        }
        float horizontal = Input.GetAxis("Mouse X") * -1;
        float vertical = Input.GetAxis("Mouse Y") * -1;
        // ���콺 �� ���� �ϴ� ����
        float mouseWheel = Input.mouseScrollDelta.y;
        lookPosition = originPos;
        //�÷��̾� �߽����� ���� ������ǥ�� ī�޶� ��ġ�� ����
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
        //��ǥ�������� ī�޶� ������
        transform.LookAt(lookPosition);
        ShootRay();

    }
    /// <summary>
    /// ��ī�̺� ī�޶�������� Ray�� ��� �Լ�
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
            // �����Ǵ� ������Ʈ�� �±׸� �з����� 
            //���� hit�� ���̸� �׼��� ���� �̸��� �����ͼ� Texture�� ��ġ��Ų��. 

                Image_ToolTip.position = Input.mousePosition + new Vector3(150, -50, 0);
            if (hit.collider.CompareTag("UserIsland"))
            {
                var info_name = hit.transform.GetComponent<Island_Profile>().user_name;
                var info_keyword1 = hit.transform.GetComponent<Island_Profile>().user_keyword1;
                var info_keyword2 = hit.transform.GetComponent<Island_Profile>().user_keyword2;
                var info_IslandId = hit.transform.GetComponent<Island_Profile>().user_IslandID;
                Image_ToolTip.gameObject.SetActive(true);
                // ���� ��ToolTip ������
                Image_ToolTip.GetChild(0).GetComponent<Text>().text = info_name + " �� ��";
                Image_ToolTip.GetChild(1).GetComponent<Text>().text = info_keyword1;
                Image_ToolTip.GetChild(2).GetComponent<Text>().text = info_keyword2;
                Image_ToolTip.GetChild(3).GetComponent<Text>().text = "'F' �� ������ ���� ����� ";

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
