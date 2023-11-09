using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    
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

    public float noMom;
    
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
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mTxt = momentum.ToString("######.#") + "      " + holdJump.ToString();
        MomTxt.text = mTxt;

        state = PlayerStates.Idle;

        onGround = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        
        float z = Input.GetAxisRaw("Vertical") * momentum;
        float x = Input.GetAxisRaw("Horizontal");

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude >= 0.1f)
        {
            state = PlayerStates.Run;

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
        
        if (!onGround && move.z <= 0)
        {
            move = transform.forward * momentum;
            PC.Move(move * (Speed * Time.deltaTime));
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpInputSTR = true;
            jumpInputTimer = 0.2f;
            holdJump = 0;

        }

        if (Input.GetButton("Jump") && !onGround)
        {
            holdJump += Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            holdJump = 0;
        }

        if (onGround && holdJump >= 0.5 && momentum>1)
        {
            jumpH += (momentum * 10);
            momentum = momentum / 2;
            upVelo.y = Mathf.Sqrt(jumpH * -2f * gravity);
            jumpH = prevJumpH;
            jumpH = jumpH / 2;
            holdJump = 0;

        }



        if (jumpInputSTR)
        {
            jumpInputTimer -= Time.deltaTime;
        }

        if (jumpInputTimer <= 0)
        {
            jumpInputSTR = false;
        }

        if (jumpInputSTR && onGround && Input.GetAxisRaw("Vertical") >= 0.1f)
        {
            upVelo.y = Mathf.Sqrt(jumpH * -2f * gravity);
            jumpInputSTR = false;
            jumpInputTimer = 0.2f;
            momGearUp = true;

        }
        
        else if (jumpInputSTR && onGround && Input.GetAxisRaw("Vertical") <= 0 && State == PlayerStates.Idle)
        {
            momentum = 0f;
            upVelo.y = Mathf.Sqrt(jumpH * -2f * gravity);
            jumpInputSTR = false;
            jumpInputTimer = 0.2f;
        }

        if (momGearUp)
        {
            momentum += 0.2f;
            jumpH += 0.2f;
            momGearUp = false;
        }

        if (momentum > 1f && momTimerBool && State == PlayerStates.Run)
        {
            momTimer -= Time.deltaTime;
        }

        else
        {
            momTimer = 0.2f;
            momTimerBool = false;
        }

        if (momentum > 1f && momTimerBool && momTimer <= 0 && State == PlayerStates.Run)
        {
            momentum = prevMomentum/2;
            jumpH = prevJumpH / 2;
            /*momTimerBool = false;*/
            momTimer = 0.5f;
        }

        if (momentum < 1f && State == PlayerStates.Run)
        {
            momentum = 1f;
            jumpH = 3f;
        }

        /*else if (momTimer <= 0 && !onGround)
        {
            momTimer = 0.1f;
        }
        */
        
        /*else if (momTimer <= 0 && onGround && momentum > 1.5f && !jumpInputSTR)
        {
            momentum = prevMomentum;
            momTimer = 0.1f;
        }*/

        if (Input.GetAxisRaw("Vertical") < 0 && State == PlayerStates.Jump)
        {
            momentum = prevMomentum/2;
            jumpH = prevJumpH / 2;
        }
        if (Input.GetAxisRaw("Vertical") == 0 && State == PlayerStates.Jump)
        {
            
            momentum = prevMomentum;
            jumpH = prevJumpH;

        }

        if (onGround && upVelo.y < 0)
        {
            upVelo.y = -2f;
        }


        upVelo.y += gravity * Time.deltaTime;
        PC.Move(upVelo * Time.deltaTime);


        if (!onGround)
        {
            state = PlayerStates.Jump;
        }

        if (Input.GetMouseButton(1) && !onGround)
        {
            Instantiate(Platform, PltSpwnLoc.transform.position, PltSpwnLoc.rotation);
        }

        SetState(state);
        
        prevMomentum = momentum;
        prevJumpH = jumpH;
    }

    public void SetState(PlayerStates s)
    {
        if (State == s)
        {
            return;
        }
        
        State = s;
        
        
        
        if (State == PlayerStates.Idle)
        {
            
            momentum = 1f;
            jumpH = 3f;
        }
        
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
