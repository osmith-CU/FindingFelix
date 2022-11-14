using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Command : MonoBehaviour {
    public PlayerController pc; 
    public void execute() {}
}

class Jump : Command {
    new public void execute(float moveHorizontal) {
        // if (moveHorizontal != 0f) {
        //     if(moveHorizontal > 0f){
        //         transform.localScale = new Vector3(1, 1, 1);
        //     } else if(moveHorizontal < 0f){
        //         transform.localScale = new Vector3(-1, 1, 1);
        //     }
        //     rb2D.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
        // }
    }
}

class Run : Command {

    new public void execute(float verticalHorizontal) {
        // if (moveVertical > 0f && IsGrounded()) {
        //     rb2D.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
        // }
    }

    // private bool IsGrounded(){
    //     // float buffer = .01f;
    //     // RaycastHit2D raycastHit = Physics2D.Raycast(cc2d.bounds.center, Vector2.down, cc2d.bounds.extents.y + buffer, groundLayer);
    //     // return (raycastHit.collider != null);
    // }
}