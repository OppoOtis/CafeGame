using System.Collections.Generic;
using UnityEngine;

public class HandGrinder : Grabbable, IFillable
{
    public float rotationSpeed;

    bool canGrind;
    bool grinding;
    float amountToGrind;
    float amountGrinded;

    public float CapacityML { get; set; }
    public float CurrentVolumeML { get; set; }
    public List<IFilling> currentFillings { get; set; }

    public override void OnDrop(Interactor _interactor, bool _left)
    {
        EndGrind();
    }
    public override void GrabInteraction(Interactor _interactor, bool _left)
    {
        if ((_left && _interactor.currentlyHoldingRight != null) || (!_left && _interactor.currentlyHoldingLeft != null))
        {
            StartCoroutine(Blackboard.playerManager.ShowTextPrompt("I NEED BOTH HANDS FOR THAT"));
        }
    }

    public override void GrabInteractionHold(Interactor _interactor, bool _left)
    {
        //can't grind with both hands if you're also holding something else
        if((_left && _interactor.currentlyHoldingRight != null) || (!_left && _interactor.currentlyHoldingLeft != null))
        {
            return;
        }

        TwoHanded = true;

        grinding = true;

        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed, Space.Self);
    }

    public override void GrabInteractionRelease(Interactor _interactor, bool _left)
    {
        EndGrind();
    }

    void EndGrind()
    {
        grinding = false;
        twoHanded = false;
    }


    public void Fill(IFilling _filling, float _ml)
    {
        if (CurrentVolumeML >= CapacityML)
            return;

        //check if currentfillings contains what you want to fill it with

        CurrentVolumeML += _ml;
    }

    public IFilling Empty(float _ml)
    {
        if(CurrentVolumeML <= 0) 
            return null;

        CurrentVolumeML -= _ml;
        return null;
    }
}
