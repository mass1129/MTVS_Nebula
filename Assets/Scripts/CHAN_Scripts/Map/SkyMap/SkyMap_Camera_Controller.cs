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
    private Vector3 targetCamPos = new Vector3(0, 1.5f, -400);

    bool onClicked;
    public float multiplier = 3;
    Vector3 originPos;
    public SphericalCoordinates sphericalCoordinates;

    void Start()
    {
        originPos = Vector3.zero;
        //ī�޶� ��ġ ����� ���� x, y, z��ǥ�� ������ r���� �Ѱ��ش�.
        sphericalCoordinates = new SphericalCoordinates(targetCamPos, Mathf.Abs(targetCamPos.z));
        transform.position = sphericalCoordinates.toCartesian + originPos;
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
        if (!onClicked) return;
        float horizontal = Input.GetAxis("Mouse X") * -1;
        float vertical = Input.GetAxis("Mouse Y") * -1;

        //�÷��̾� ��ġ���� ���ݴ� �������� �ڸ���� �����.
        //lookPosition = new Vector3(PlayerTr.position.x,
        //PlayerTr.position.y + targetCamPos.y, PlayerTr.position.z);
        lookPosition = originPos;

        //�÷��̾� �߽����� ���� ������ǥ�� ī�޶� ��ġ�� ����
        transform.position = sphericalCoordinates.Rotate
            (horizontal * multiplier*Time.deltaTime, vertical * multiplier*Time.deltaTime).toCartesian + lookPosition;

        //��ǥ�������� ī�޶� ������
        transform.LookAt(lookPosition);
    }
}
