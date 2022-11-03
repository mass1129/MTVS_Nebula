using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class K_CameraMgr : MonoBehaviourPun
{
    public Transform cameraLookAt;
    public Transform camModeRoot;

    public GameObject buildingSystem;
    public Transform buildCamTarget;
    public Transform buildTargetStartPos;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    public Camera playerCamera;
    public CinemachineVirtualCamera firstPersonCamera;
    
    public CinemachineVirtualCamera buildCamera;
    [HideInInspector]
    public CinemachineCameraOffset  buildCamOffset;
    K_Player player;
    public bool firstPersonView= false;

    public float zoomChangeAmount = 100;

    K_PlayerItemSystem itemSystem;
    private void Awake()
    {
        if (!photonView.IsMine)
            this.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        itemSystem = GetComponent<K_PlayerItemSystem>();
        player = GetComponent<K_Player>();
        buildingSystem.SetActive(false);
        buildCamOffset = buildCamera.gameObject.GetComponent<CinemachineCameraOffset>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (itemSystem.isVisible) return;
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

       
            HandleCamMode();

      
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
