using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationBehavior 
{
    protected Animator animator = PlayerController.getInstance().animator;
    public abstract void executeAnimation();
}

class Idle : AnimationBehavior
{
    public override void executeAnimation() {
        animator.Play("deez");
    }
}

class Running : AnimationBehavior
{
    public override void executeAnimation() {

    }
}

class Jumping : AnimationBehavior
{
    public override void executeAnimation() {

    }
}

class Falling : AnimationBehavior
{
    public override void executeAnimation() {

    }
}

class Death : AnimationBehavior
{
    public override void executeAnimation() {
        return;
    }
}