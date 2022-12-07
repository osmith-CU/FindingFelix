using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

/*-----COMMAND IMPLEMENTATION-----*/

abstract class Command {
    public PlayerController pc; 
    public bool execute() {return true;}
}

//Some base structure inspired by https://www.youtube.com/watch?v=TcranVQUQ5U&t=377s, modified heavily

class Run : Command {
    public Run(PlayerController pc){
        this.pc = pc;
    }
    public new bool execute() {
        //If the player has entered a movement, moves that direction
        if (pc.velocityHorizontal != 0f) {
            if(pc.velocityHorizontal > 0f){
                pc.rb2D.transform.localScale = new Vector3(1, 1, 1);                            //flips character around
            } else if(pc.velocityHorizontal < 0f){
                pc.rb2D.transform.localScale = new Vector3(-1, 1, 1);
            }
            pc.rb2D.AddForce(new Vector2(pc.velocityHorizontal * pc.movementSpeed, 0f), ForceMode2D.Impulse);
        }
        return true;
    }
}

class Jump : Command {

    private bool doubleJump;

    public Jump(PlayerController pc){
        doubleJump = false;
        this.pc = pc;
    }

    public new bool execute() {

        if (pc.velocityVertical > 0f && (pc.IsGrounded())) {                            //If palyer is trying to jump and currentl grounded
            doubleJump = true;                                                          //Jumps and set double jump to true, and has JumpedOnce to false and change velocity
            pc.rb2D.velocity = (new Vector2(pc.rb2D.velocity.x, pc.jumpForce));
            pc.hasJumpedOnce = false;
        }
        if (pc.velocityVertical > 0f && pc.hasJumpedOnce) {                             //If player is trying to jump and hasJumpedOnce to true
            doubleJump = false;                                                         //set doublejump to false and hasJumpedOnce to false and change the velocity
            pc.hasJumpedOnce = false;
            pc.rb2D.velocity = (new Vector2(pc.rb2D.velocity.x, pc.jumpForce));         
        }
        if(pc.velocityVertical == 0f && doubleJump){                                    //Makes sure we have to reset the jump (let go of the up arrow) before we can jump again
            pc.hasJumpedOnce = true;
        }
        return true;
    }
}

class Dash: Command {

    private double dashDuration;                                                                //how long this dash has been going
    private double dashTime = .1;                                                               //how long each dash should be
    private float dashSpeed;            
    private float gravity;

    public Dash(PlayerController pc){
        this.pc = pc;
        this.dashDuration = dashTime;
        this.dashSpeed = 50;
    }

    public new bool execute(){
        if(dashDuration == dashTime){                                                           //If we've just started the dash
            this.gravity = pc.rb2D.gravityScale;                                                //save the old gravity value and set to 0, then set vel to dash speed
            pc.rb2D.gravityScale = 0;
            pc.rb2D.velocity = new Vector2(dashSpeed * pc.rb2D.transform.localScale[0], 0);
        }
        dashDuration -= Time.deltaTime;                                                         //subtract our dash duration by however long its been
        if(dashDuration <= 0){                                                                  //Once we've been dashing for dashtime, set velocity to 15 in teh direction (very slow at teh end), and restore gravity
            pc.rb2D.velocity = new Vector2(15 * pc.rb2D.transform.localScale[0], 0);            
            pc.rb2D.gravityScale = gravity;
            dashDuration = dashTime;
            return true;
        }
        return false;
    }

}

