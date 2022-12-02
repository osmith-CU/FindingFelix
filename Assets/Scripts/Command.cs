using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

abstract class Command {
    public PlayerController pc; 
    public bool execute() {return true;}
}

class Run : Command {
    public Run(PlayerController pc){
        this.pc = pc;
    }
    public new bool execute() {
        if (pc.velocityHorizontal != 0f) {
            if(pc.velocityHorizontal > 0f){
                pc.rb2D.transform.localScale = new Vector3(1, 1, 1);
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

        if (pc.velocityVertical > 0f && (pc.IsGrounded())) {
            doubleJump = true;
            pc.rb2D.velocity = (new Vector2(pc.rb2D.velocity.x, pc.jumpForce));
            pc.hasJumpedOnce = false;
        }
        if (pc.velocityVertical > 0f && pc.hasJumpedOnce) {
            doubleJump = false;
            pc.hasJumpedOnce = false;
            pc.rb2D.velocity = (new Vector2(pc.rb2D.velocity.x, pc.jumpForce));
        }
        if(pc.velocityVertical == 0f && doubleJump){
            pc.hasJumpedOnce = true;
        }
        return true;
    }
}

class Dash: Command {

    private double dashDuration;
    private double dashTime = .1;
    private float dashSpeed;
    private float gravity;

    public Dash(PlayerController pc){
        this.pc = pc;
        this.dashDuration = dashTime;
        this.dashSpeed = 50;
    }

    public new bool execute(){
        if(dashDuration == dashTime){
            this.gravity = pc.rb2D.gravityScale;
            pc.rb2D.gravityScale = 0;
            pc.rb2D.velocity = new Vector2(dashSpeed * pc.rb2D.transform.localScale[0], 0);
        }
        dashDuration -= Time.deltaTime;
        Debug.Log(dashDuration);
        if(dashDuration <= 0){
            pc.rb2D.velocity = new Vector2(0, 0);
            pc.rb2D.gravityScale = gravity;
            dashDuration = dashTime;
            return true;
        }
        return false;
    }

}

