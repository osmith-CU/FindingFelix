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
    private bool canDash;
    public bool dashing;
    public float dashSpeed;
    public float dashDuration;
    public float velocityHorizontal;
    public float velocityVertical;
    public bool grounded;
    public bool hasJumpedOnce;
    public int sceneVal;
    private bool isAlive;
    public Animator animator;
    private Jump jump;
    private Run run;
    private Dash dash;
    private static PlayerController uniqueInstance;
    private AnimationBehavior animationBehavior;
    private SaveManager saveManager;
    private PauseMenu pauseMenu;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask trapLayer;
    [SerializeField] private LayerMask levelLayer;
    [SerializeField] private LayerMask finishLayer;
    [SerializeField] private Transform respawn;


    
    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1;
        isAlive = true;
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
        canDash = true;
        //saveManager = new SaveManager("Save_0", this);
        pauseMenu = new PauseMenu(saveManager, this);
    }

    // Update is called once per frame
    void Update() {
        grounded = IsGrounded();
        if(dashing){
            if(!dash.execute()){                                                            //If currently dashing and still executing, then just immediately returns
                return;
            } else {
                dashing = false;                                                            //else if dash has finished, set dashing to false
            }
        }
        if(grounded){
            canDash = true;
        }

        velocityHorizontal = Input.GetAxisRaw("Horizontal");                                // gets keyboard input via Input (1.0, 0, or -1.0 depending on direction)
        velocityVertical = Input.GetAxisRaw("Vertical");                                    // gets keyboard input via Input (1.0 or 0 depending on direction)

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = 0; 
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);                    // https://forum.unity.com/threads/add-a-scene-into-another-scene-kind-of-overlay.504545/
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashing == false && canDash) {
            dash.execute();
            dashing = true;
            canDash = false;
        }

        if (grounded == false && rb2D.velocity.y < 0) {                                     // checks if the player is falling; used for animation purposes
            isFalling = true;
        } else {
            isFalling = false;
        }

        if(Kill()){
            // Debug.Log("trapped");
            isAlive = false;
            StartCoroutine(Respawn());                                                      // https://stackoverflow.com/questions/30056471/how-to-make-the-script-wait-sleep-in-a-simple-way-in-unity
        }

        if(FinishGame()){
            SceneManager.LoadScene("EndGameMenu");
        }

        determineAnimation();
    }

    void FixedUpdate() {
        if (isAlive == true) {                                                              //if isAlive, we run and jump
            run.execute();
            jump.execute();
        }

        if(velocityVertical < 0f){                                                 
            isFalling = true;
        }
        if(loadLevelCheck()){
            // Debug.Log("nextLevel");                                                      //If we are touching the next level object, go to the next level
            loadNextLevel();
        }
    }

    public bool IsGrounded() {
        //ray detection taken from https://www.youtube.com/watch?v=c3iEl5AwUF8
        float buffer = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(cc2d.bounds.center, Vector2.down, cc2d.bounds.extents.y + buffer, groundLayer);         //raycast downwards to see if grounded
        return (raycastHit.collider != null);
    }

    private bool Kill() {
        //ray detection taken from https://www.youtube.com/watch?v=c3iEl5AwUF8
        float buffer = .01f;    
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc2d.bounds.center, cc2d.bounds.size, 0f, Vector2.down, buffer, trapLayer);             //boxcast (detect on all sides, not jsut down) to see if touching a trap
        //saveManager.load();   
        return (raycastHit.collider != null);
    }

    private bool FinishGame() {
        //ray detection taken from https://www.youtube.com/watch?v=c3iEl5AwUF8
        float buffer = .01f;    
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc2d.bounds.center, cc2d.bounds.size, 0f, Vector2.down, buffer, finishLayer);             //boxcast (detect on all sides, not jsut down) to see if touching EndGame
        return (raycastHit.collider != null);
    }

    IEnumerator Respawn() {
        yield return new WaitForSeconds(1);
        isAlive = true;
        rb2D.position = respawn.position;
        rb2D.velocity = Vector3.zero;
        grounded = true;
    }

    private bool loadLevelCheck() {
        //ray detection taken from https://www.youtube.com/watch?v=c3iEl5AwUF8
        float buffer = .01f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc2d.bounds.center, cc2d.bounds.size, 0f, Vector2.down, buffer, levelLayer);            //boxcast (detect on all sides, not jsut down) to see if touching nextLevel
        return raycastHit.collider != null;
    }

    private void loadNextLevel() {
        Scene currentScene = SceneManager.GetActiveScene();
        int levelNum = currentScene.name[currentScene.name.Length - 1] - '0' ;
        Debug.Log(levelNum);
        SceneManager.LoadScene("Level" + (levelNum + 1));
    }

    // SINGLETON IMPLEMENTATION
    // eager instantiation for singleton (only ever one player in scene and thus one playerController object in scene)
    public static PlayerController getInstance() {
        if (uniqueInstance == null) {
            uniqueInstance = new GameObject().AddComponent<PlayerController>();
        }
        return uniqueInstance;
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

        if (grounded == false && isFalling == true) {                               // if player is not grounded and they have negative vertical velocity (post-apex), animation is Falling
            animationBehavior = new Falling(this);
        }

        if (isAlive == false) {
            animationBehavior = new Death(this);
        }

    
        if (previousAnimationBehavior != animationBehavior.GetType().Name) {        // checks if an animation change has occured. this prevents an animation from replaying every frame or stuttering out
            // Debug.Log("Animation Change");
            animationBehavior.executeAnimation();
        }
    }
}
