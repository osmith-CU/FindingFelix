using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationBehavior 
{
    public PlayerController playerController;
    public abstract void executeAnimation();
}

class Idle : AnimationBehavior
{
    public Idle(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Idle");
    }
}

class Running : AnimationBehavior
{
    public Running(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Run");
    }
}

class Jumping : AnimationBehavior
{
    public Jumping(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Jump");
    }
}

class Falling : AnimationBehavior
{
    public Falling(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Fall");
    }
}

class Death : AnimationBehavior
{
    public Death(PlayerController pc) {
        this.playerController = pc;
    }
    public override void executeAnimation() {
        playerController.animator.Play("Base Layer.Gary_Death");
    }
}