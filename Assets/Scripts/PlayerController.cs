using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb2D;
    private CapsuleCollider2D cc2d;
    public float movementSpeed;
    public float jumpForce;
    private bool isFalling;
    public bool dashing;
    public float dashSpeed;
    public float dashDuration;
    public float velocityHorizontal;
    public float velocityVertical;
    public bool grounded;
    public bool hasJumpedOnce;
    public int sceneVal;
    public Animator animator;
    private Jump jump;
    private Run run;
    private Dash dash;
    private static PlayerController uniqueInstance;
    private AnimationBehavior animationBehavior;
    private SaveManager saveManager;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask trapLayer;
    [SerializeField] private LayerMask levelLayer;
    [SerializeField] private Transform respawn;


    
    // Start is called before the first frame update
    void Start() {
        sceneVal = 1;
        hasJumpedOnce = false;
        movementSpeed = 1f;
        jumpForce = 30f;
        isFalling = false;
        dashing = false;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        cc2d = gameObject.GetComponent<CapsuleCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        grounded = true;
        run = new Run(this);
        jump = new Jump(this);
        dash = new Dash(this);
        animationBehavior = new Idle(this);
        saveManager = new SaveManager("Level"+(sceneVal + 1), "Save_1", this);
    }

    // Update is called once per frame
    void Update() {
        grounded = IsGrounded();
        if(dashing){
            if(!dash.execute()){
                return;
            } else {
                Debug.Log("here");
                dashing = false;
            }
        }

        velocityHorizontal = Input.GetAxisRaw("Horizontal");                                // gets keyboard input via Input (1.0, 0, or -1.0 depending on direction)
        velocityVertical = Input.GetAxisRaw("Vertical");                                    // gets keyboard input via Input (1.0 or 0 depending on direction)

        if (Input.GetKeyDown(KeyCode.RightShift) && dashing == false) {
            dash.execute();
            dashing = true;
        }

        if (grounded == false && rb2D.velocity.y < 0) {                                     // checks if the player is falling; used for animation purposes
            isFalling = true;
        } else {
            isFalling = false;
        }


        determineAnimation();
    }

    void FixedUpdate() {
        run.execute();
        jump.execute();

        if(velocityVertical < 0f){
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

    public bool IsGrounded() {
        float buffer = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(cc2d.bounds.center, Vector2.down, cc2d.bounds.extents.y + buffer, groundLayer);
        return (raycastHit.collider != null);
    }

    private bool Kill() {
        float buffer = .01f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc2d.bounds.center, cc2d.bounds.size, 0f, Vector2.down, buffer, trapLayer);
        saveManager.load();
        return (raycastHit.collider != null);
    }

    private bool loadLevelCheck() {
        float buffer = .01f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc2d.bounds.center, cc2d.bounds.size, 0f, Vector2.down, buffer, levelLayer);
        createSave();
        return raycastHit.collider != null;
    }

    private void loadNextLevel() {
        SceneManager.LoadScene(sceneVal);
    }

    private void createSave(){
        saveManager.updateStage(sceneVal + 1);
        saveManager.save();
    }

    public void loadFromSave(string level){
        SceneManager.LoadScene(level);
    }

    // eager instantiation for singleton (only ever one player in scene and thus one playerController object in scene)
    public static PlayerController getInstance() {
        if (uniqueInstance == null) {
            uniqueInstance = new GameObject().AddComponent<PlayerController>();
        }
        return uniqueInstance;
    }

    private void pauseGame() {

    }

    private void determineAnimation() {
        string previousAnimationBehavior = animationBehavior.GetType().Name;        // store animation behavior to detect change

        if (velocityHorizontal == 0 && velocityVertical == 0) {                     // if player is not moving, animation is Idle
            animationBehavior = new Idle(this);
        }

        if (velocityHorizontal != 0 && grounded == true) {                          // if player is on the ground and has non-zero horizontal velocity, animation is Running
            animationBehavior = new Running(this);
        }

        if (grounded == false && rb2D.velocity.y > 0) {                             // if player is not grounded and they have not yet reached jump apex, animation is Jumping
            animationBehavior = new Jumping(this);
        }

        if (grounded == false && isFalling == true) {                               // if player is not grouned and they have negative vertical velocity (post-apex), animation is Falling
            animationBehavior = new Falling(this);
        }

        if (previousAnimationBehavior != animationBehavior.GetType().Name) {        // checks if an animation change has occured. this prevents an animation from replaying every frame or stuttering out
            // Debug.Log("Animation Change");
            animationBehavior.executeAnimation();
        }
    }
}
