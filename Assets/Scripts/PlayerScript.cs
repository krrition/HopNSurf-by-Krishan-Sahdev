using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    public Camera cam;
    
    public CharacterController PC;

    public float Speed = 20f;

    public float airSpeed = 20f;

    private Vector3 upVelo;

    public float gravity = -9.81f;

    public float jumpH = 3f;
    
    public float prevJumpH = 3f;

    public float holdJump;

    public float drag = 15f;

    public float momentum = 1f;

    public float prevMomentum = 1f;

    public bool momTimerBool;

    public float momTimer = 0.2f;

    public bool momGearUp;

    public bool jumpInputSTR;

    public float jumpInputTimer = 0.2f;

    private bool onGround;

    public Transform groundCheck;

    public float groundDist = 0.4f;

    public LayerMask groundMask;

    public PlayerStates State = PlayerStates.None;
    public PlayerStates state = PlayerStates.None;

    public TextMeshProUGUI MomTxt;

    public string mTxt;

    public Transform PltSpwnLoc;

    public GameObject Platform;

    public float z;
    
    public float x;

    public float tempGrav;

    public bool tempGravOn;

    public float PltCount = 1f;

    public Slider PltSlider;
    
    public float SloCount = 2f;

    public Slider SloSlider;

    public float SloFactor = 0.5f;

    public bool sloToggle;

    public Slider ChargeSlider;
    public Slider ChargeSlider2;

    public string[] colliderTags;

    public Transform firePoint;

    public GameObject proj;

    public float projVelo = 50;

    public Vector3 destination;

    public string CurScene;

    public bool SloLock;

    public bool ShootLock;


    // Start is called before the first frame update
    void Start()
    {
        CurScene = SceneManager.GetActiveScene().name;

        if (CurScene == "Level 1" || CurScene == "Level 2")
        {
            SloLock = true;
            ShootLock = true;
        }

        if (CurScene == "Level 3")
        {
            SloLock = false;
            ShootLock = true;
        }

        if (SceneManager.GetActiveScene().buildIndex >= 4)
        {
            SloLock = false;
            ShootLock = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Setting Idle State
        state = PlayerStates.Idle;


        //Ground Check 2.0
        Collider[] hitcollider;
        hitcollider = Physics.OverlapSphere(groundCheck.position, groundDist, groundMask);

        colliderTags = new string[hitcollider.Length];

        for (int i = 0; i < hitcollider.Length; i++)
        {
            colliderTags[i] = hitcollider[i].gameObject.tag;
        }

        if (hitcollider.Length > 0)
        {
            onGround = true;
        }

        else
        {
            onGround = false;
        }




        /*onGround = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);*/


        //Getting Movement Inputs
        z = Input.GetAxisRaw("Vertical") * momentum;
        x = Input.GetAxisRaw("Horizontal");

        


        //Proper Air Strafing
        if (Input.GetAxisRaw("Horizontal") < 0 && !onGround)
        {
            z = momentum;
            x = -momentum / 3;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0 && !onGround)
        {
            z = momentum;
            x = momentum / 3;
        }

        //Diving When Holding Down
        if (!onGround && Input.GetAxisRaw("Vertical") < 0)
        {
            z = momentum;
            tempGravOn = true;
            tempGrav -= momentum;

        }

        if (tempGravOn && onGround)
        {
            tempGravOn = false;
            tempGrav = 0f;
        }
        
        //Speed UI
        mTxt = momentum.ToString("######.#");
        

        //Multiplying and Plugging in Movement Inputs
        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude >= 0.1f)
        {
            state = PlayerStates.Run;

            //Friction on Ground and No Friction in Air
            if (onGround)
            {
                Speed = drag;
            }

            else if (!onGround)
            {
                Speed = airSpeed;
            }

            PC.Move(move * (Speed * Time.deltaTime));
        }

        else
        {
            mTxt = "0";
        }
        
        MomTxt.text = mTxt;
        

        //Adding Momentum
        if (!onGround && move.z <= 0)
        {
            move = transform.forward * momentum;
            PC.Move(move * (Speed * Time.deltaTime));
        }


        //Jump Input Storing
        if (Input.GetButtonDown("Jump"))
        {
            jumpInputSTR = true;
            jumpInputTimer = 0.2f;
            holdJump = 0;

        }

        //Hold Jump Input Storing
        if (Input.GetButton("Jump") && !onGround)
        {
            holdJump += Time.deltaTime;
        }

        //Resetting Hold Jump Input
        if (Input.GetButtonUp("Jump"))
        {
            holdJump = 0;
        }

        //Hold to Jump Higher
        if (onGround && holdJump >= 0.5 && momentum > 1 && Input.GetAxisRaw("Vertical") > 0)
        {
            jumpH += momentum * (holdJump * 10);
            momentum = momentum / (holdJump * 4);
            upVelo.y = Mathf.Sqrt(jumpH * -2f * gravity);
            jumpH = prevJumpH;
            jumpH = jumpH / (holdJump * 4);
            holdJump = 0;

        }



        //Resetting Jump Input Storing after Timer
        if (jumpInputSTR)
        {
            jumpInputTimer -= Time.deltaTime;
        }

        if (jumpInputTimer <= 0)
        {
            jumpInputSTR = false;
        }

        //Main Jump and Building Momentum
        if (jumpInputSTR && onGround && Input.GetAxisRaw("Vertical") >= 0.1f)
        {
            upVelo.y = Mathf.Sqrt(jumpH * -2f * gravity);
            jumpInputSTR = false;
            jumpInputTimer = 0.2f;
            momGearUp = true;

        }


        //Straight Idle Jump
        else if (jumpInputSTR && onGround && Input.GetAxisRaw("Vertical") <= 0 && State == PlayerStates.Idle)
        {
            momentum = 0f;
            upVelo.y = Mathf.Sqrt(jumpH * -2f * gravity);
            jumpInputSTR = false;
            jumpInputTimer = 0.2f;
        }


        //Setting Momentum and Jump Height Higher
        if (momGearUp)
        {
            momentum += 0.2f;
            jumpH += 0.2f;
            momGearUp = false;
        }


        //Resetting Momentum if Just Running- Failed Attempt?
        if (momentum > 1f && momTimerBool && State == PlayerStates.Run)
        {
            momTimer -= Time.deltaTime;
        }

        else
        {
            momTimerBool = false;
            momTimer = 0.2f;

        }

        //Depleting Momentum if Just Running
        if (momentum > 1f && momTimerBool && momTimer <= 0 && State == PlayerStates.Run)
        {
            momentum = prevMomentum / 2;
            jumpH = prevJumpH / 2;
            /*momTimerBool = false;*/
            momTimer = 0.5f;
        }


        //Setting Momentum and Jump Height to Minimum Values for Bugs
        if (momentum < 1f && State == PlayerStates.Run)
        {
            momentum = 1f;
            jumpH = 3f;
        }


        //Completing the Jump Arc Even if Inputs are Null
        if (Input.GetAxisRaw("Vertical") == 0 && State == PlayerStates.Jump)
        {

            momentum = prevMomentum;
            jumpH = prevJumpH;

        }

        //Constant and Minimum Gravity Force
        if (onGround && upVelo.y < 0)
        {
            upVelo.y = -2f;
        }


        //Plugging in Gravity
        upVelo.y += (gravity + tempGrav) * Time.deltaTime;
        PC.Move(upVelo * Time.deltaTime);


        //Setting Jump State
        if (!onGround)
        {
            state = PlayerStates.Jump;
        }
        
        //Creating Projectiles
        if (ShootLock == false)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.55f, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    destination = hit.point;
                }

                else
                {
                    destination = ray.GetPoint(100);
                }

                Vector3 momPoint = new Vector3(firePoint.position.x, firePoint.position.y, firePoint.position.z);
                var projObj = Instantiate(proj, momPoint, firePoint.rotation);
                projObj.GetComponent<Rigidbody>().velocity =
                    (destination - firePoint.position).normalized * (projVelo * momentum);
                Destroy(projObj, 5f);
            }
        }


        //Creating Platform
        if (Input.GetButton("Fire2") && !onGround && PltCount > 0)
        {
            Instantiate(Platform, PltSpwnLoc.transform.position, PltSpwnLoc.rotation);
            PltCount -= Time.deltaTime;

        }

        //Slow Mo Toggle
        if (SloLock == false)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !sloToggle && SloCount > 0)
            {
                sloToggle = true;
            }

            else if (Input.GetKeyDown(KeyCode.LeftShift) && sloToggle || SloCount <= 0)
            {
                sloToggle = false;
            }

            if (sloToggle && SloCount > 0)
            {
                Time.timeScale = SloFactor;
                Time.fixedDeltaTime = Time.timeScale * .02f;
            }


            //Implementing Slo Mo Based on Toggle
            if (sloToggle)
            {
                SloCount -= Time.unscaledDeltaTime;
            }

            if (!sloToggle)
            {
                Time.timeScale += (1f / 2) * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            }
        }

        //Check Collision and Meters Properly Reset
        foreach (string tag in colliderTags)
        {

            if (onGround && PltCount != 1 && tag == "GenFloor")
            {
                PltCount = 1;
            }

            if (onGround && SloCount != 2 && tag == "GenFloor")
            {
                SloCount = 2;
            }

            if (onGround && tag == "Reset")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
        
        //Implementing UI Sliders with Locks
        PltSlider.value = PltCount;
        if (SloLock == false)
        {
            SloSlider.gameObject.SetActive(true);
            SloSlider.value = SloCount * (0.5f);
        }

        else
        {
            SloSlider.gameObject.SetActive(false);
        }


        if (holdJump >= 0.15 && momentum > 1)
        {
            ChargeSlider.gameObject.SetActive(true);
            ChargeSlider2.gameObject.SetActive(true);
            ChargeSlider.value = holdJump * (2);
            ChargeSlider2.value = holdJump * (2);
        }
        else
        {
            ChargeSlider.gameObject.SetActive(false);
            ChargeSlider2.gameObject.SetActive(false);
        }


        //Plugging in State
        SetState(state);
        
        
        //Saving Momentum and Jump Height
        prevMomentum = momentum;
        prevJumpH = jumpH;
    }

    //Setting State when States changes
    public void SetState(PlayerStates s)
    {
        if (State == s)
        {
            return;
        }
        
        State = s;
        
        
        //Setting Minimum Values
        if (State == PlayerStates.Idle)
        {
            
            momentum = 1f;
            jumpH = 3f;
        }
        
        
        //Setting Run Momentum Depletion: Line 210
        if (State == PlayerStates.Run)
        {

            momTimerBool = true;
        }
        
        

    }
    


    public enum PlayerStates
    {
        None,
        Idle,
        Run,
        Jump,
    }


    
}
