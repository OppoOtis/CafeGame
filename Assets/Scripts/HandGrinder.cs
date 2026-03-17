using System.Collections.Generic;
using UnityEngine;

public class HandGrinder : Grabbable, IFillable
{
    public float rotationSpeed;
    public float grindSpeed;
    bool canGrind;
    bool grinding;
    float amountGrinded;

    public float CapacityML { get { return capacityML; } set { capacityML = value; } }
    public float capacityML = 20;
    public float CurrentVolumeML { get; set; }
    public List<IFilling> currentFillings { get; set; }
    public HandGrinderUI myUI;


    private void Start()
    {
        currentFillings = new List<IFilling>();
    }

    public override void OnPickUp(Interactor _interactor, bool _left)
    {
        myUI.ActivateUI(_left);
        SetUI(_left);
    }
    public override void OnDrop(Interactor _interactor, bool _left)
    {
        EndGrind();
        myUI.DisableUI(_left);
    }
    public override void GrabInteraction(Interactor _interactor, bool _left)
    {
        if ((_left && _interactor.currentlyHoldingRight != null) || (!_left && _interactor.currentlyHoldingLeft != null))
        {
            Blackboard.playerManager.TextPrompt("I NEED BOTH HANDS FOR THAT");
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
        if (grindSpeed * Time.deltaTime > CurrentVolumeML)
            amountGrinded += CurrentVolumeML;
        else
            amountGrinded += grindSpeed * Time.deltaTime;

        if (CapacityML - amountGrinded < 0.001f)
            amountGrinded = CapacityML;

        Empty(grindSpeed * Time.deltaTime);
        SetUI(_left);
    }

    public override void GrabInteractionRelease(Interactor _interactor, bool _left)
    {
        EndGrind();
        if (amountGrinded == CapacityML)
        {
            //spawn a cup in the other hand filled with coffee grounds
            //temporary solution for now
            Cup newCup = Instantiate(Resources.Load("Items/Cup") as GameObject).GetComponent<Cup>();
            _interactor.PickUpGrabbable(newCup, !_left);
            newCup.Fill(new CoffeeGrounds(), amountGrinded);
            amountGrinded = 0;
            UpdateVisuals(_left);
        }
    }

    void EndGrind()
    {
        grinding = false;
        twoHanded = false;
    }

    void SetUI(bool _left)
    {
        myUI.SetBeanBar(_left, 1 / CapacityML * CurrentVolumeML);
        myUI.SetGroundsBar(_left, 1 / CapacityML * amountGrinded);
    }


    public float Fill(IFilling _filling, float _ml)
    {
        if (CurrentVolumeML >= CapacityML)
        {
            return _ml;
        }

        if (!currentFillings.Contains(_filling))
        {
            currentFillings.Add(_filling);
        }

        if (_ml > CapacityML - CurrentVolumeML)
        {
            CurrentVolumeML = CapacityML;
            return _ml - (CapacityML - CurrentVolumeML);
        }
        CurrentVolumeML += _ml;
        return 0;
    }

    public IFilling Empty(float _ml)
    {
        if (CurrentVolumeML <= 0)
            return null;

        if (_ml > CurrentVolumeML)
            _ml = CurrentVolumeML;

        CurrentVolumeML -= _ml;
        return currentFillings[0];
    }

    public void UpdateVisuals(bool _left)
    {
        SetUI(_left);
    }
}
