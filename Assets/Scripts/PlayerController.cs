using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb2D;
    private CapsuleCollider2D cc2d;
    private float moveSpeed;
    private float jumpForce;
    private bool isFalling;
    private float moveHorizontal;
    private float moveVertical;
    private bool grounded;
    private bool hasJumpedOnce;
    public int loadInt;
    public string loadString;
    public bool useInt;
    public Animator animator;
    Jump jump;
    Run run;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask trapLayer;
    [SerializeField] private LayerMask levelLayer;
    [SerializeField] private Transform respawn;

    
    // Start is called before the first frame update
    void Start() {
        useInt = false;
        loadInt = 1;
        hasJumpedOnce = false;
        moveSpeed = 1f;
        jumpForce = 25f;
        isFalling = false;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        cc2d = gameObject.GetComponent<CapsuleCollider2D>();
        grounded = true;
    }

    // Update is called once per frame
    void Update() {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        if (moveHorizontal != 0f) {
            animator.SetBool("IsRunning", true);
        } else {
            animator.SetBool("IsRunning", false);
        }
    }

    void FixedUpdate() {
        if(IsGrounded()){
            hasJumpedOnce = false;            
        }
        if (moveHorizontal != 0f) {
            if(moveHorizontal > 0f){
                transform.localScale = new Vector3(1, 1, 1);
            } else if(moveHorizontal < 0f){
                transform.localScale = new Vector3(-1, 1, 1);
            }
            rb2D.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
        }

        if (moveVertical > 0f && (IsGrounded() || !hasJumpedOnce)) {
            Debug.Log(hasJumpedOnce);
            Debug.Log(!hasJumpedOnce);
            hasJumpedOnce = !hasJumpedOnce;
            rb2D.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
            
            Debug.Log(hasJumpedOnce);
        }
        // if(hasJumpedOnce == true){
        //     rb2D.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
        //     hasJumpedOnce = false;
        // }
        if(moveVertical < 0f){
            isFalling = true;
        }
        if(Kill()){
            Debug.Log("trapped");
            rb2D.position = respawn.position;
        }
        if(loadLevelCheck()){
            Debug.Log("nextLevel");
            loadNextLevel();
        }
    }

    private bool IsGrounded(){
        float buffer = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(cc2d.bounds.center, Vector2.down, cc2d.bounds.extents.y + buffer, groundLayer);
        return (raycastHit.collider != null);
    }

    private bool Kill(){
        float buffer = .01f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc2d.bounds.center, cc2d.bounds.size, 0f, Vector2.down, buffer, trapLayer);
        return (raycastHit.collider != null);
    }

    private bool loadLevelCheck(){
        float buffer = .01f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc2d.bounds.center, cc2d.bounds.size, 0f, Vector2.down, buffer, levelLayer);
        return raycastHit.collider != null;
    }

    private void loadNextLevel(){
        SceneManager.LoadScene(loadInt);
    }
}
