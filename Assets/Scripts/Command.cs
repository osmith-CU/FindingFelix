using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Command : MonoBehaviour {
    public PlayerController pc; 
    public void execute() {}
}

class Jump : Command {
    new public void execute() {

    }
}

class Run : Command {
    new public void execute() {

    }
}