using System;
using UnityEngine;

public abstract class InteractiveObjectClass: MonoBehaviour {
    public virtual bool Interact() {
        throw new NotImplementedException();
    }
    
    public virtual bool IsInteractionAvailable() {
        throw new NotImplementedException();
    }
}