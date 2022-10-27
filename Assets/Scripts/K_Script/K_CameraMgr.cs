using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class K_CameraMgr : MonoBehaviour
{
    public Transform cameraLookAt;
    public Transform camModeRoot;
    public Transform spaceCamRoot;
    public Transform spaceCamStartPos;

    public GameObject buildingSystem;
    public Transform buildCamTarget;
    public Transform buildTargetStartPos;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    public Camera playerCamera;
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineVirtualCamera spaceCamera;
    public CinemachineVirtualCamera buildCamera;
    [HideInInspector]
    public CinemachineCameraOffset  buildCamOffset;
    K_Player player;
    public bool firstPersonView= false;
    public bool spaceView= false;

    public float zoomChangeAmount = 100;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<K_Player>();
        buildingSystem.SetActive(false);
        buildCamOffset = buildCamera.gameObject.GetComponent<CinemachineCameraOffset>();
        
    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        if (!spaceView)
            HandleCamMode();

        HandleSpaceCamMode();
    }
    void FixedUpdate()
    {
        
        
    }
    private void LateUpdate()
    {   
        if(!spaceView)
        {
            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
            camModeRoot.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        }
        else
        {
            spaceCamRoot.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        }    
    }
    void HandleCamMode()
    {
        if (player.CurrentState != PlayerStates.Idle) return;
        bool tryCamMode = Input.GetKeyDown(KeyCode.V);
        
        if (tryCamMode && !firstPersonView)
        {
            firstPersonView = true;

        }

        else if (firstPersonView)
        {
            
            //float yawCamera = playerCamera.transform.rotation.eulerAngles.y;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), 
            //    player.turnSpeed * Time.deltaTime);
            if (tryCamMode)
            {
               
                
                firstPersonView = false;
             
            }
        }
    }
    void HandleSpaceCamMode()
    {
        
        bool trySpaceCamMode = Input.GetKeyDown(KeyCode.B);
        if (trySpaceCamMode && !spaceView && player.CurrentState == PlayerStates.Idle)
        {
            spaceView = true;

        }

        else if (spaceView)
        {
            
            
            if (trySpaceCamMode)
            {

                
                spaceView = false;

            }
        }
    }

}
