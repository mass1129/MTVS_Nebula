using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Photon.Pun;

namespace K_01_OwnedStates
{
    public class Idle : K_PlayerState<K_Player>
    {
        public override void Enter(K_Player entity)
        {   
            entity.input.x = 0;
            entity.input.y = 0;
            entity.SetTrigger("ThirdMove");
        }

        public override void Execute(K_Player entity)
        {
            //entity.velocity.y += entity.gravity * Time.deltaTime;
            //entity.velocity.y = Mathf.Clamp(entity.velocity.y, -6f, 100);
            if (!entity.canMove) return;

            if (Input.GetButton("Horizontal") || (Input.GetButton("Vertical")))
            {   
               
                    if (!entity.camMgr.firstPersonView)
                        entity.ChangeState(PlayerStates.ThirdMove);
                    else
                        entity.ChangeState(PlayerStates.FirstMove);
                
                
            }
            else
            {
               
                    if (!entity.camMgr.firstPersonView)
                        entity.camMgr.firstPersonCamera.gameObject.SetActive(false);
                    else
                    {
                        entity.camMgr.firstPersonCamera.gameObject.SetActive(true);
                        entity.input.x = Input.GetAxis("Horizontal");
                        entity.input.y = Input.GetAxis("Vertical");

                        float yawCamera = entity.camMgr.playerCamera.transform.rotation.eulerAngles.y;

                        entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, Quaternion.Euler(0, yawCamera, 0),
                            entity.turnSpeed * Time.deltaTime);
                    }
                
            }



            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                entity.ChangeState(PlayerStates.Sitting);
            }

            if (Input.GetButtonDown("Jump"))
            {

                entity.ResetTrigger("ThirdMove");
                entity.ChangeState(PlayerStates.Jump);
                entity.yVelocity = entity.jumpHeight;
            }
            if (!entity.Grounded)
            {
                entity.ChangeState(PlayerStates.Falling);
            }
            //if (Input.GetButtonDown("Jump"))
            //{
            //    entity.ChangeState(PlayerStates.Jump);
            //    entity.velocity.y = entity.jumpHeight;
            //}
           


        }

        public override void Exit(K_Player entity)
        {
            entity.ResetTrigger("ThirdMove");
        }
    }

 
    public class ThirdMove : K_PlayerState<K_Player>
    {
        float turnSmoothVelocity;
        public override void Enter(K_Player entity)
        {
            entity.input.x = 0;
            entity.input.y = 0;
            entity.SetTrigger("ThirdMove");
        }

        public override void Execute(K_Player entity)
        {


            entity.input.x = Input.GetAxis("Horizontal");
            entity.input.y = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(entity.input.x, 0f, entity.input.y).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + entity.cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(entity.transform.eulerAngles.y,
                    targetAngle, ref turnSmoothVelocity, entity.turnSmoothTime);

                entity.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }

            // 이동 방향에 따라 ThirdMove BlendTree 값 설정
            entity.SetFloat("InputX", entity.input.x);
            entity.SetFloat("InputY", entity.input.y);

            Vector3 stepForwardAmount = entity.rootMotion * entity.groundSpeed;
            Vector3 stepDownAmount = Vector3.down * entity.stepDown;

            entity.cc.Move(stepForwardAmount + stepDownAmount);
            entity.rootMotion = Vector3.zero;

            // 이동 중에 떨어지면 Falling으로 상태 전환


            if (entity.input.magnitude <= 0f && !(Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
            {
                entity.ChangeState(PlayerStates.Idle);
            }

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                entity.ChangeState(PlayerStates.ThirdSprinting);
            }
            //if (Input.GetButton("Horizontal") || (Input.GetButton("Vertical")))
            //{
            //    if (!entity.camMgr.spaceView&& entity.camMgr.firstPersonView)
            //    {   
            //        entity.ChangeState(PlayerStates.FirstMove);
            //    }

            //}
            if (Input.GetButtonDown("Jump"))
            {

                entity.ResetTrigger("ThirdMove");
                entity.ChangeState(PlayerStates.Jump);
                entity.yVelocity = entity.jumpHeight;
            }

            if (!entity.Grounded)
            {
                entity.ChangeState(PlayerStates.Falling);
            }

        }

        public override void Exit(K_Player entity)
        {
            entity.ResetTrigger("ThirdMove");
        }
    }

    public class ThirdSprinting : K_PlayerState<K_Player>
    {
        float turnSmoothVelocity;
        public override void Enter(K_Player entity)
        {
            entity.input.x = 0.5f;
            entity.input.y = 0.5f;
            
            entity.SetTrigger("Sprinting");
        }

        public override void Execute(K_Player entity)
        {


            entity.input.x = Input.GetAxis("Horizontal");
            entity.input.y = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(entity.input.x, 0f, entity.input.y).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + entity.cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(entity.transform.eulerAngles.y,
                    targetAngle, ref turnSmoothVelocity, entity.turnSmoothTime);

                entity.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }

            // 이동 방향에 따라 ThirdMove BlendTree 값 설정
            entity.SetFloat("InputX", entity.input.x);
            entity.SetFloat("InputY", entity.input.y);

            Vector3 stepForwardAmount = entity.rootMotion * entity.groundSpeed;
            Vector3 stepDownAmount = Vector3.down * entity.stepDown;

            entity.cc.Move(stepForwardAmount + stepDownAmount);
            entity.rootMotion = Vector3.zero;

            // 이동 중에 떨어지면 Falling으로 상태 전환


            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                entity.ChangeState(PlayerStates.ThirdMove);
            }

            if (Input.GetButtonDown("Jump"))
            {

                entity.ResetTrigger("Sprinting");
                entity.ChangeState(PlayerStates.Jump);
                entity.yVelocity = entity.jumpHeight;
                entity.airControl += entity.jumpDamp;

            }

          

            if (!entity.Grounded)
            {
                entity.ChangeState(PlayerStates.Falling);
            }

        }

        public override void Exit(K_Player entity)
        {
            entity.ResetTrigger("Sprinting");
        }
    }
    public class FirstMove : K_PlayerState<K_Player>
    {
        Vector2 _currentVelocity;
        public override void Enter(K_Player entity)
        {
            entity.input.x = 0;
            entity.input.y = 0;
            entity.camMgr.firstPersonCamera.gameObject.SetActive(true);
            entity.SetTrigger("FirstMove");
        }

        public override void Execute(K_Player entity)
        {


            entity.input.x = Input.GetAxis("Horizontal");
            entity.input.y = Input.GetAxis("Vertical");

            float yawCamera = entity.camMgr.playerCamera.transform.rotation.eulerAngles.y;

            entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, Quaternion.Euler(0, yawCamera, 0),
                entity.turnSpeed * Time.deltaTime);

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, entity.input.x, entity.AnimBlendSpeed * Time.deltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, entity.input.y, entity.AnimBlendSpeed * Time.deltaTime);
            // 이동 방향에 따라 ThirdMove BlendTree 값 설정
            entity.SetFloat("InputX", _currentVelocity.x);
            entity.SetFloat("InputY", _currentVelocity.y);

            Vector3 stepForwardAmount = entity.rootMotion * entity.groundSpeed;
            Vector3 stepDownAmount = Vector3.down * entity.stepDown;

            entity.cc.Move(stepForwardAmount + stepDownAmount);
            entity.rootMotion = Vector3.zero;


            if (_currentVelocity.magnitude <= 0.05f && !(Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
            {
                entity.ChangeState(PlayerStates.Idle);
            }
           
         
            //if (Input.GetButtonDown("Jump"))
            //{

            //    entity.ChangeState(PlayerStates.Jump);
            //    entity.velocity.y = entity.jumpHeight;
            //}

            //if (!entity.Grounded)
            //{
            //    entity.ChangeState(PlayerStates.Falling);
            //}

        }

        public override void Exit(K_Player entity)
        {
            _currentVelocity.x = 0.0f;
            _currentVelocity.y = 0.0f;
           
            entity.ResetTrigger("FirstMove");
        }
    }
    

    public class BuildingMode : K_PlayerState<K_Player>
    {

        float zoomAmount;

        float turnSmoothVelocity;


        public override void Enter(K_Player entity)
        {
            entity.input.x = 0;
            entity.input.y = 0;
            entity.SetTrigger("ThirdMove");
            entity.gridBuildingSystem.TestLoad(entity.ownIslandID);
            entity.camMgr.InActiveBuildingSystem(true);
            entity.camMgr.buildCamera.gameObject.SetActive(true);
            
            entity.camMgr.buildCamOffset.m_Offset.z = 0f;
            entity.camMgr.buildCamOffset.m_Offset.y = 0f;
            zoomAmount = entity.camMgr.zoomChangeAmount;
        }
    

        public override void Execute(K_Player entity)
        {

            entity.camMgr.buildCamOffset.m_Offset.z = Mathf.Clamp(entity.camMgr.buildCamOffset.m_Offset.z, 0.0f, 10.0f);
            entity.camMgr.buildCamOffset.m_Offset.y = Mathf.Clamp(entity.camMgr.buildCamOffset.m_Offset.y, -2.50f, 0.0f);

            if (Input.mouseScrollDelta.y > 0 && entity.camMgr.buildCamOffset.m_Offset.z < 10f && entity.camMgr.buildCamOffset.m_Offset.y>-2.5f)
            {
                entity.camMgr.buildCamOffset.m_Offset.z += zoomAmount * Time.deltaTime;
                entity.camMgr.buildCamOffset.m_Offset.y -= 0.25f*zoomAmount * Time.deltaTime;
            }
            if (Input.mouseScrollDelta.y < 0 && entity.camMgr.buildCamOffset.m_Offset.z > 0.0f && entity.camMgr.buildCamOffset.m_Offset.y < -0.0f)
            {
                entity.camMgr.buildCamOffset.m_Offset.z -= zoomAmount * Time.deltaTime;
                entity.camMgr.buildCamOffset.m_Offset.y += 0.25f * zoomAmount * Time.deltaTime;
            }

            entity.input.x = Input.GetAxis("Horizontal");
            entity.input.y = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(entity.input.x, 0f, entity.input.y).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + entity.cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(entity.transform.eulerAngles.y,
                    targetAngle, ref turnSmoothVelocity, entity.turnSmoothTime);

                entity.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }

            // 이동 방향에 따라 ThirdMove BlendTree 값 설정
            entity.SetFloat("InputX", entity.input.x);
            entity.SetFloat("InputY", entity.input.y);

            Vector3 stepForwardAmount = entity.rootMotion * entity.groundSpeed;
            Vector3 stepDownAmount = Vector3.down * entity.stepDown;

            entity.cc.Move(stepForwardAmount + stepDownAmount);
            entity.rootMotion = Vector3.zero;



        }

    

    public override void Exit(K_Player entity)
        {
            entity.gridBuildingSystem.TestSave(entity.ownIslandID);
            entity.camMgr.InActiveBuildingSystem(false);
            entity.camMgr.buildCamera.gameObject.SetActive(false);
            entity.ResetTrigger("ThirdMove");
        }
    }

    public class Sitting : K_PlayerState<K_Player>
    {
        
        public override void Enter(K_Player entity)
        {
            entity.SetTrigger("Sitting");
        }

        public override void Execute(K_Player entity)
        {


            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                entity.ChangeState(PlayerStates.Idle);
            }



        }

        public override void Exit(K_Player entity)
        {
            entity.ResetTrigger("Sitting");
        }
    }

    //public class Falling : K_PlayerState<K_Player>
    //{
    //    bool isLanding = false;

    //    public override void Enter(K_Player entity)
    //    {
    //        entity.SetTrigger("Falling");
    //    }

    //    public override void Execute(K_Player entity)
    //    {
    //        entity.moveSpeed = 2;

    //        if (!isLanding)
    //        {
    //            entity.velocity.y += entity.gravity * Time.deltaTime;
    //            entity.velocity.y = Mathf.Clamp(entity.velocity.y, -6f, 100);

    //            if (Input.GetButton("Horizontal") || (Input.GetButton("Vertical")))
    //            {
    //                entity.input.x = Input.GetAxis("Horizontal");
    //                entity.input.y = Input.GetAxis("Vertical");
    //            }

    //            entity.dir = entity.transform.right * entity.input.x + entity.transform.forward * entity.input.y;
    //            entity.dir.Normalize();
    //            entity.dir.y = entity.velocity.y;
    //            entity.GetComponent<CharacterController>().ThirdMove(entity.dir * entity.moveSpeed * Time.deltaTime);

    //            // 땅에 닿으면 Landing 애니메이션 재생
    //            if (entity.GetComponent<CharacterController>().isGrounded)
    //            {
    //                entity.SetTrigger("Landing");
    //                isLanding = true;
    //            }
    //        }

    //        if (entity.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
    //            entity.SetTrigger("Landing");

    //        // Landing 애니메이션 재생 시간 끝난 이후의 입력에 따라 다음 상태로 전이 
    //        if (entity.anim.GetCurrentAnimatorStateInfo(0).IsName("Landing") && entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
    //        {
    //            if (Input.GetButton("Horizontal") || (Input.GetButton("Vertical")))
    //            {
    //                entity.ChangeState(PlayerStates.ThirdMove);
    //            }
    //            else if (Input.GetButton("Jump"))
    //            {
    //                entity.ChangeState(PlayerStates.Jump);
    //                entity.velocity.y = entity.jumpHeight;
    //            }
    //            else
    //                entity.ChangeState(PlayerStates.Idle);
    //        }
    //    }

    //    public override void Exit(K_Player entity)
    //    {
    //        entity.input.x = 0;
    //        entity.input.y = 0;
    //        entity.ResetTrigger("Falling");
    //        entity.ResetTrigger("Landing");
    //        isLanding = false;
    //    }
    //}
    public class Jump : K_PlayerState<K_Player>
    {
        
        public override void Enter(K_Player entity)
        {
           
            entity.SetTrigger("Jump");
        }

        public override void Execute(K_Player entity)
        {
            entity.yVelocity += entity.gravity * Time.deltaTime;
            entity.yVelocity = Mathf.Clamp(entity.yVelocity, -6f, 100);

            if (Input.GetButton("Vertical"))
            {
                entity.v = Input.GetAxis("Vertical") >= 0 ? Input.GetAxis("Vertical") : 0;

            }
            entity.dir = entity.transform.forward * entity.v;
            entity.dir.Normalize();
            entity.dir.y = entity.yVelocity;

            entity.GetComponent<CharacterController>().Move(entity.dir * entity.airControl * Time.deltaTime);
           entity.rootMotion = Vector3.zero;

            if (entity.yVelocity <= 0.1f)
            {
               
               
                    entity.ResetTrigger("Jump");
                    entity.ChangeState(PlayerStates.Falling);
               
            }
        }

        public override void Exit(K_Player entity)
        {
            entity.v = 0f;
            entity.dir = Vector3.zero;
            entity.ResetTrigger("Jump");
            entity.ResetTrigger("Landing");
        } 
    }

    public class Falling : K_PlayerState<K_Player>
    {
        bool isLanding = false;

        public override void Enter(K_Player entity)
        {
            entity.SetTrigger("Falling");
        }

        public override void Execute(K_Player entity)
        {

            if (!entity.canMove) return;
            if (!isLanding)
            {
                entity.yVelocity  += entity.gravity * Time.deltaTime;
                entity.yVelocity  = Mathf.Clamp(entity.yVelocity, -6f, 100);

                if (Input.GetButton("Vertical"))
                {
                   
                    entity.v = Input.GetAxis("Vertical") >=0 ? Input.GetAxis("Vertical") : 0;
                }

                entity.dir = entity.transform.forward * entity.v;
                entity.dir.Normalize();
                entity.dir.y = entity.yVelocity;
                if (Input.GetKey(KeyCode.U))
                {
                    entity.GetComponent<CharacterController>().Move(Vector3.up * entity.airControl * Time.deltaTime);
                }
                else
                entity.GetComponent<CharacterController>().Move(entity.dir * entity.airControl * Time.deltaTime);
                entity.rootMotion = Vector3.zero;

                // 땅에 닿으면 Landing 애니메이션 재생
                if (entity.Grounded)
                {
                    entity.SetTrigger("Landing");
                    isLanding = true;
                }
            }
            if (entity.Grounded)
            {
                entity.SetTrigger("Landing");
                isLanding = true;
            }
                if (entity.anim.GetCurrentAnimatorStateInfo(0).IsName("ThirdMove"))
                entity.ChangeState(PlayerStates.ThirdMove);

            // Landing 애니메이션 재생 시간 끝난 이후의 입력에 따라 다음 상태로 전이 
            if (entity.anim.GetCurrentAnimatorStateInfo(0).IsName("Landing") && entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                if (Input.GetButton("Vertical")&& Input.GetKey(KeyCode.LeftShift))
                { 
                  
                        entity.ChangeState(PlayerStates.ThirdSprinting);
                    
                   
                }
                else
                    entity.ChangeState(PlayerStates.ThirdMove);


            }
           
        }

        public override void Exit(K_Player entity)
        {
            entity.input.x = 0;
            entity.input.y = 0;
            entity.ResetTrigger("Falling");
            entity.ResetTrigger("Landing");
            entity.airControl = 5;
            isLanding = false;
        }
    }


    public class Death : K_PlayerState<K_Player>
    {
        public override void Enter(K_Player entity)
        {

        }

        public override void Execute(K_Player entity)
        {

        }

        public override void Exit(K_Player entity)
        {

        }
    }

    // 기본 상태와 별개로 항상 업데이트 되고 있는 전역 상태
    public class Global : K_PlayerState<K_Player>
    {

        public override void Enter(K_Player entity)
        {

        }

        public override void Execute(K_Player entity)
        {
            
           
        }

        public override void Exit(K_Player entity)
        {

        }
    }

    //// 아래부터는 upperBodyStates
    //public class None : K_PlayerState<K_Player>
    //{
    //    public override void Enter(K_Player entity)
    //    {
    //        entity.SetTrigger("None");
    //    }

    //    public override void Execute(K_Player entity)
    //    {
    //        if ((Input.GetButton("Horizontal") || (Input.GetButton("Vertical"))) && entity.CurrentState == PlayerStates.ThirdMove)
    //            entity.ChangeState_UpperBody(PlayerUpperBodyStates.Move_Upper);
    //    }

    //    public override void Exit(K_Player entity)
    //    {
    //        entity.ResetTrigger("None");
    //    }
    //}

    //public class Move_Upper : K_PlayerState<K_Player>
    //{
    //    public override void Enter(K_Player entity)
    //    {
    //        entity.SetTrigger("Move_Upper");
    //    }

    //    public override void Execute(K_Player entity)
    //    {
    //        if (Input.GetButton("Horizontal") || (Input.GetButton("Vertical") && entity.CurrentState == PlayerStates.ThirdMove))
    //        { }
    //        else
    //        {
    //            entity.ResetTrigger("Move_Upper");
    //            entity.ChangeState_UpperBody(PlayerUpperBodyStates.None);
    //        }
    //    }

    //    public override void Exit(K_Player entity)
    //    {
    //        entity.ResetTrigger("Move_Upper");
    //    }
    //}

    

    //// 기본 상체 상태와 별개로 항상 업데이트 되고 있는 전역 상체 상태
    //public class Global_Upper : K_PlayerState<K_Player>
    //{
    //    public override void Enter(K_Player entity)
    //    { }

    //    public override void Execute(K_Player entity)
    //    {
            
    //    }

    //    public override void Exit(K_Player entity)
    //    { }
    //}
}