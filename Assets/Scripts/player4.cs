using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class player4 : MonoBehaviour
{

    //Start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
   

    //FSM
    private enum State { idle, running, jumping, falling, hurt}
    private State state = State.idle;

    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private  float jumpForce = 10f;
    [SerializeField] private float hurtforce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource footstep;
   
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
       permanentUI.perm.healthAmount.text = permanentUI.perm.health.ToString();
    }
    private void Update()
    {
       if(state !=State.hurt)
        {
          Movement();
        }
        
        AnimationState();
        anim.SetInteger("state", (int)state); //sets animation based on Enumerator state
    }



     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            permanentUI.perm.cherries += 1;
            permanentUI.perm.cherryText.text = permanentUI.perm.cherries.ToString();
        }      
         if(collision.tag == "PowerUps")
        {
            Destroy(collision.gameObject);
            jumpForce = 55F;
            GetComponent<SpriteRenderer>().color = Color.magenta;
            StartCoroutine(ResetPower());
        }
    }   


     private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
       
        if (other.gameObject.tag == "Enemy")
        {
            if(state == State.falling)
            {
                enemy.JumpedOn();
                jump();
            }
            else
            {
                
                state = State.hurt;
                HandleHealth(); // Deals with health , updating UI, and will reset level if health is <= 0

               if (other.gameObject.transform.position.x > transform.position.x)
                {
                    // Enemy is to my right therefore I should be damaged and moved left
                    rb.velocity = new Vector2(-hurtforce, rb.velocity.y);
                }
                else
                {
                    // Enemy is to my left therefore I should be damaged and moved right
                    rb.velocity = new Vector2(hurtforce, rb.velocity.y);
                }
            }
        }
    }

    private void HandleHealth()
    {
        permanentUI.perm.health -= 1;
        permanentUI.perm.healthAmount.text = permanentUI.perm.health.ToString();
        if (permanentUI.perm.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        //Moving left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //Moving right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        //Jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            jump();
        }
    }
        

     private void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState()

    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }



        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Moving
            state = State.running;
        }
        else
        {
            state = State.idle;

            
        }
    }


    private void Footstep()
    {
        footstep.Play();
    }


    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 23;
        GetComponent<SpriteRenderer>().color = Color.white;

    }
}


  






