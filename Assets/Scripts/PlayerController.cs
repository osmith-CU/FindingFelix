using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb2D;
    private CapsuleCollider2D cc2d;
    public float moveSpeed;
    public float jumpForce;
    private bool isFalling;
    public float moveHorizontal;
    public float moveVertical;
    public bool grounded;
    public bool hasJumpedOnce;
    public int loadInt;
    public string loadString;
    public bool useInt;
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
        jumpForce = 50f;
        isFalling = false;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        cc2d = gameObject.GetComponent<CapsuleCollider2D>();
        grounded = true;
        run = new Run(this);
        jump = new Jump(this);
    }

    // Update is called once per frame
    void Update() {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        
        run.execute();
        jump.execute();

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

    public bool IsGrounded(){
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
