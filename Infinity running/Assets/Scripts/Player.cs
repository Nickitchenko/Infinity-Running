using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    public float speed;
    public float sideSpeed;
    public float jumpForce;
    public float fallForce;

    public float currentX; //-1, 0, 1

    public int moons = 0;//UI
    public int score = 0; //UI
    private int score_future; //for increase speed
    private int speedMax=20;

    private Vector3 targetPosition;
    private Vector3 lastposition;
    private Vector3 lastpositionFloat;

    public int inOnePlace=0;
    public int inOnePlaceMax=5;

    private Rigidbody rb;
    private CapsuleCollider cc;
    private Animator animator;
    private Collider capsulecollider;

    public TMP_Text moonsText;      //UI
    public TMP_Text moonsDeathText; //UI
    public TMP_Text scroreText;     //UI
    public TMP_Text scroreDeathText;//UI

    public bool isGrounded;//jump

    public GameObject ragdollPrefab; //death
    private bool ragdollSpawned = false; //death
    public float ragdollImpulse; //death

    public GameObject deathPanel; //UI

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        capsulecollider = GetComponent<Collider>();
        targetPosition = transform.position;
        score_future = score + 100;
    }

    private void FixedUpdate()
    {
        //================ moving
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);

        targetPosition = new Vector3(currentX, transform.position.y, transform.position.z);
        targetPosition.x = Mathf.Clamp(targetPosition.x, -1f, 1f);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, sideSpeed);
    }

    private void Update()
    {
        //================ JUMP
        if(Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            MoveUp();
        }

        //================ ROLL
        if(Input.GetKeyDown(KeyCode.S))
        {
            MoveDown();
        }

        //================ MOVE LEFT RIGHT
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }

        //================ LastFrame
        Vector3 intPosition = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        
        //================ Death
        if (intPosition == lastposition)
        {
            inOnePlace++;
            if(inOnePlace>=inOnePlaceMax)
            {
                if (ragdollSpawned == false)
                {
                    GameObject newRagdoll =  Instantiate(ragdollPrefab, transform.position, Quaternion.identity, null);
                    newRagdoll.transform.GetChild(1).GetComponent<Rigidbody>().AddForce(-transform.forward *
                        Random.Range(ragdollImpulse - 20f, ragdollImpulse + 20f), ForceMode.Impulse);
                    transform.GetChild(0).gameObject.SetActive(false);
                    capsulecollider.enabled = false;
                    speed = 0; sideSpeed = 0;

                    ragdollSpawned = true;
                    StartCoroutine("DeathPanel");
                }
            }
        }else
        {
            inOnePlace = 0;
        }
        
        //================ not falling player
        if ((transform.position.y<0.7f || transform.position.y>0.9f) && transform.position.y<lastpositionFloat.y)
        {
            rb.AddForce(-transform.up * fallForce, ForceMode.Impulse);
        }
        
        //================ Place sroce on screen
        if ((int)transform.position.z>score)
        {
            score = (int)transform.position.z;
        }
        moonsText.text = moons.ToString();
        scroreText.text = score.ToString();

        //================ increase speed
        if (speed != speedMax)
        {
            if (score == score_future)
            {
                speed++;
                score_future += 100;
            }
        }
    }

    private void LateUpdate()
    {
        lastposition = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        lastpositionFloat = transform.position;
    }
    
    //================ can jump
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag=="Chunk")
        {
            isGrounded = true;
        }
    }
    
    //================ not can jump
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag=="Chunk")
        {
            isGrounded = false;
        }
    }

    public void MoveLeft()
    {
        currentX--;
    }

    public void MoveRight()
    {
        currentX++;
    }

    public void MoveUp()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        animator.Play("jump");
    }

    public void MoveDown()
    {
        cc.height = 1.3f;
        StartCoroutine("RollTimer");
        rb.AddForce(-transform.up * fallForce, ForceMode.Impulse);
        animator.Play("Roll");
    }
    
    //================ activate death panel
    IEnumerator DeathPanel()
    {
        yield return new WaitForSeconds(1f);
        deathPanel.SetActive(true);
        scroreDeathText.text = score.ToString();
        moonsDeathText.text = moons.ToString();
        Time.timeScale = 0f;
    }
    
    //================ can rolling
    IEnumerator RollTimer()
    {
        yield return new WaitForSeconds(1f);
        cc.height = 1.931356f; //height of collider
    }
    
    //================ button on death panel
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
