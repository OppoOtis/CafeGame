using System.Collections.Generic;
using UnityEngine;

public class Cup : Grabbable, IFillable
{
    public float CapacityML { get { return capacityML; } set { capacityML = value; } }
    public float capacityML = 200;
    public float CurrentVolumeML { get; set; }
    public List<IFilling> currentFillings { get; set; } = new List<IFilling>();

    public GameObject groundsVisual;

    private void Start()
    {
        ChangeFilling();
    }

    public override void GrabInteraction(Interactor _interactor, bool _left)
    {
        IGrabbable otherGrabbable;

        if (_left)
            otherGrabbable = _interactor.currentlyHoldingRight;
        else
            otherGrabbable = _interactor.currentlyHoldingLeft;

        if(otherGrabbable == null)
        {
            //maybe take a sip idk
            return;
        }

        if(otherGrabbable is Portafilter)
        {
            Portafilter p = otherGrabbable as Portafilter;
            p.ChangePortaFilterState(PortaFilterState.grounds);
            Empty(CapacityML);
        }
    }
    public IFilling Empty(float _ml)
    {
        if (CurrentVolumeML <= 0)
            return null;

        if(_ml > CurrentVolumeML)
            _ml = CurrentVolumeML;

        CurrentVolumeML -= _ml;

        if (CurrentVolumeML <= 0)
        {
            IFilling _f = currentFillings[0];
            currentFillings.Clear();
            ChangeFilling();
            return _f;
        }

        return currentFillings[0];
    }

    public float Fill(IFilling _filling, float _ml)
    {
        if (CurrentVolumeML >= CapacityML)
        {
            Blackboard.playerManager.TextPrompt("The cup is full!");
            return _ml;
        }

        if (!currentFillings.Contains(_filling))
        {
            currentFillings.Add(_filling);
        }

        if(_ml > CapacityML - CurrentVolumeML)
        {
            CurrentVolumeML = CapacityML;
            ChangeFilling();
            return _ml - (CapacityML - CurrentVolumeML);
        }
        CurrentVolumeML += _ml;
        ChangeFilling();
        return 0;
    }

    void ChangeFilling()
    {
        groundsVisual.SetActive(false);
        foreach(IFilling _f in currentFillings)
        {
            if (_f is CoffeeGrounds)
                groundsVisual.gameObject.SetActive(true);
        }
    }

    public void UpdateVisuals(bool _left)
    {

    }
}
