using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    

    [SerializeField] private LayerMask jumpableGround;
    
    private float dirX = 0f;
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce = 18f;
     
     private enum MovementState {idle, running , jumping, falling }

     [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
         {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
         }
 
         UpdateAnimationState();

    }
    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
         {
            state = MovementState.running;
             anim.SetBool("running", true);
             sprite.flipX = false;
         }
         else if(dirX < 0f)
         {
            state = MovementState.running;
            anim.SetBool("running", true);
            sprite.flipX = true;
         }
         else
         {
            state = MovementState.idle;
         }
         
         if (rb.velocity.y > .1f)
         {
             state = MovementState.jumping;
         }
         else if (rb.velocity.y < -.1f)
         {
            state = MovementState.falling;
         }

         anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
