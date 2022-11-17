using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using CodeMonkey.Utils;


public class K_CameraMgr : MonoBehaviourPun
{
    public Transform cameraLookAt;
    public Transform camModeRoot;

    public List<GameObject> buildingSystemSet;


    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    public Camera playerCamera;
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineVirtualCamera subBuildCamera;
    
    public CinemachineVirtualCamera buildCamera;
    [HideInInspector]
    public CinemachineCameraOffset  buildCamOffset;
    K_Player player;
    public bool firstPersonView= false;

    public float zoomChangeAmount = 100;

    public bool isCursorVisible = false;
   
    private void Awake()
    {
        if (!photonView.IsMine)
            this.enabled = false;
    }
    public void InActiveBuildingSystem(bool b)
    {
        for (int i = 0; i < buildingSystemSet.Count; i++)
            buildingSystemSet[i].SetActive(b);
    }
    // Start is called before the first frame update
    void Start()
    {
        

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        player = GetComponent<K_Player>();
       
        buildCamOffset = buildCamera.gameObject.GetComponent<CinemachineCameraOffset>();
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
           
            if (!isCursorVisible)
            {

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                isCursorVisible = true;

            }
            else if (isCursorVisible)
            {

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                isCursorVisible = false;

            }
            

        }

        if (UtilsClass.IsPointerOverUI()) return;
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

       
            HandleCamMode();

      
    }
    private void OnEnable()
    {
        //buildingSystem.SetActive(false);
    }
    void FixedUpdate()
    {
        
        
    }
    private void LateUpdate()
    {
        
        
            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
            camModeRoot.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        
        
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
   

}
