using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Speed/Forces")]
    public float moveSpeed;
    public float jumpForce;
    public float doublejumpForce;
    public float diveForce;
    
    public float gravityScale = 5f;
    public float bounceForce = 8f;

    private Vector3 moveDirection;

    public CharacterController charController;

    private Camera theCam;
    [Header("Rotation")]
    public GameObject playerModel;
    public float rotateSpeed;
    [Header("Animation")]
    public Animator anim;
    [Header("Knockback")]
    public bool isKnocking;
    public float knockBackLength = .5f;
    private float knockbackCounter;
    public Vector2 knockbackPower;
    [Header("Pieces")]
    public GameObject[] playerPieces;
    [Header("Abbititys")]    
    public bool stopMove;
    public bool canDouble;
    public bool canDive;
    public GameObject spinTrigger;
 
    bool isSpinning;
    bool doubleJump;
    bool isdive;
    bool dive;
    Vector3 diveDir;
  
    
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        theCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isKnocking && !stopMove)
        {
            float yStore = moveDirection.y;
            //moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection.Normalize();
            moveDirection = moveDirection * moveSpeed;
            
            moveDirection.y = yStore;

            if (charController.isGrounded)
            {
              
                anim.SetBool("IsDive",false);
                anim.SetBool("IsDouble",false);    
                doubleJump = true;
                isdive = false;
                
                moveDirection.y = 0f;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                }
                if(Input.GetKeyDown(KeyCode.LeftShift)){
                    anim.SetBool("Crouch", true);
                    moveSpeed = 3;    
                }else if(Input.GetKeyUp(KeyCode.LeftShift))
                {
                    anim.SetBool("Crouch", false);
                    moveSpeed = 7.5f;
                }

                if(Input.GetKeyDown(KeyCode.Q) && !isSpinning){
                    StartCoroutine(Spin());
                    isSpinning = true;
                }
            }else{
                 if (Input.GetButtonDown("Jump") && doubleJump && canDouble){
                    anim.SetBool("IsDouble",true);  
                     moveDirection.y = jumpForce + doublejumpForce;
                     doubleJump = false;
                     dive = true;
                    }
                else if (Input.GetButtonDown("Jump") && dive && canDive  && (moveDirection.x != 0 ||moveDirection.z != 0))
                 {   
                    //Stops you from moving and set the animator varibles
                    anim.SetBool("IsDouble",false);  
                    anim.SetBool("IsDive",true); 
                    stopMove = true;
                 }
                 else if(Input.GetKeyDown(KeyCode.Q) && !isSpinning){
                    StartCoroutine(Spin());
                    isSpinning = true;
                }
            }
            //Gravity                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            
            //Moves character with the moveDirection varible
            charController.Move(moveDirection * Time.deltaTime);

            if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !isdive)
            {
                transform.rotation = Quaternion.Euler(0f, theCam.transform.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
               

                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }
        }


        if(isKnocking)
        {
            //Knockback
            knockbackCounter -= Time.deltaTime;

            float yStore = moveDirection.y;
            moveDirection = playerModel.transform.forward * -knockbackPower.x;
            moveDirection.y = yStore;

            if (charController.isGrounded)
            {
                moveDirection.y = 0f;
            }

            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            charController.Move(moveDirection * Time.deltaTime);

            if (knockbackCounter <= 0)
            {
                isKnocking = false;
            }
        }

        if(stopMove)
        {
        if(charController.isGrounded){
            // Stops moving if grounded
            moveDirection = Vector3.zero;
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
            charController.Move(moveDirection);
        }else if(!charController.isGrounded && dive){
            
             Dive();
        }
        
            
        }
        //Animations
        anim.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        anim.SetBool("Grounded", charController.isGrounded);
    }

    private IEnumerator Spin()
    {
        
        if(charController.isGrounded == true){
            anim.SetTrigger("Spin");
            spinTrigger.SetActive(true);
            yield return new WaitForSeconds(1f);
            spinTrigger.SetActive(false);
            isSpinning = false;
        }else{
            anim.SetTrigger("Spin");
            spinTrigger.SetActive(true);
            moveDirection.y =  jumpForce+7.5f;
            yield return new WaitForSeconds(1f);
            spinTrigger.SetActive(false);
            isSpinning = false;
        }
       
    }

    void Dive(){
        isdive = true;
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection.Normalize();
            moveDirection = moveDirection * moveSpeed;
             diveDir = moveDirection;
             
             diveDir *= diveForce;
             diveDir.y = jumpForce + diveForce; 
             charController.Move(diveDir * Time.deltaTime);
             playerModel.transform.localRotation = Quaternion.Euler(90f,0f,0f); 
             stopMove = false;
             dive = false;
             
    }

    public void Knockback()
    {
        isKnocking = true;
        knockbackCounter = knockBackLength;
        //Debug.Log("Knocked Back");
        moveDirection.y = knockbackPower.y;
        charController.Move(moveDirection * Time.deltaTime);
    }

    public void Bounce()
    {
        moveDirection.y = bounceForce;
        charController.Move(moveDirection * Time.deltaTime);
    }
   
}
