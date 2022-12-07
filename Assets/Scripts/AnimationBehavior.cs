using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----STRATEGY IMPLEMENTATION-----*/

public abstract class AnimationBehavior                             // abstract class to implement strategy pattern for animations
{
    public PlayerController playerController;
    public abstract void executeAnimation();
}

class Idle : AnimationBehavior                                      // Idle animation (plays if no other animation is playing)                                
{
    public Idle(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Idle");     // plays animations via an animator component attached to Gary in the scene
    }
}

class Running : AnimationBehavior                                   // Running animation (plays when grounded and x-velocity is nonzero)
{
    public Running(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Run");
    }
}

class Jumping : AnimationBehavior                                   // Jumping animation (plays when not grounded and y-velocity is positive)
{
    public Jumping(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Jump");
    }
}

class Falling : AnimationBehavior                                   // Falling animation (plays when not grounded and y-velocity is negative)
{
    public Falling(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Fall");
    }
}

class Death : AnimationBehavior                                     // plays upon death (Gary's collider detects a trap object)
{
    public Death(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Death");
    }
}