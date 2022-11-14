using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Command : MonoBehaviour {
    public PlayerController pc; 
    public void execute() {}
}

class Run : Command {
    public Run(PlayerController pc){
        this.pc = pc;
    }
    new public void execute() {
        if (pc.moveHorizontal != 0f) {
            if(pc.moveHorizontal > 0f){
                pc.rb2D.transform.localScale = new Vector3(1, 1, 1);
            } else if(pc.moveHorizontal < 0f){
                pc.rb2D.transform.localScale = new Vector3(-1, 1, 1);
            }
            pc.rb2D.AddForce(new Vector2(pc.moveHorizontal * pc.moveSpeed, 0f), ForceMode2D.Impulse);
        }
    }
}

class Jump : Command {

    bool doubleJump;

    public Jump(PlayerController pc){
        doubleJump = false;
        this.pc = pc;
    }

    new public void execute() {

        if (pc.moveVertical > 0f && (pc.IsGrounded())) {
            doubleJump = true;
            pc.rb2D.AddForce(new Vector2(0f, pc.moveVertical * pc.jumpForce), ForceMode2D.Impulse);
            pc.hasJumpedOnce = false;
        }
        if (pc.moveVertical > 0f && pc.hasJumpedOnce) {
            doubleJump = false;
            pc.hasJumpedOnce = false;
            pc.rb2D.AddForce(new Vector2(0f, pc.moveVertical * pc.jumpForce), ForceMode2D.Impulse);
        }
        if(pc.moveVertical == 0f && doubleJump){
            pc.hasJumpedOnce = true;
        }
    }

    // private bool IsGrounded(){
    //     // float buffer = .01f;
    //     // RaycastHit2D raycastHit = Physics2D.Raycast(cc2d.bounds.center, Vector2.down, cc2d.bounds.extents.y + buffer, groundLayer);
    //     // return (raycastHit.collider != null);
    // }
}