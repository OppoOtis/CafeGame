using UnityEngine;

public enum PortaFilterState
{
    empty,
    grounds,
    tampedgrounds,
    usedgrounds
}
public class Portafilter : Grabbable
{
    public PortaFilterState myState;
    public GameObject groundsVisual, tampedGroundsVisual, usedGroundsVisual;

    private void Start()
    {
        ChangePortaFilterState(PortaFilterState.empty);
    }
    public override void GrabInteraction(Interactor _interactor, bool _left)
    {
        Debug.Log(_interactor.closestInteractible is EspressoMachineGroup);
        if(_interactor.closestInteractible is EspressoMachineGroup)
        {
            _interactor.closestInteractible.Interact(_interactor, _left);
        }
    }

    public void ChangePortaFilterState(PortaFilterState _newState)
    {
        myState = _newState;

        groundsVisual.SetActive(false);
        tampedGroundsVisual.SetActive(false);
        usedGroundsVisual.SetActive(false);

        switch (myState)
        {
            case PortaFilterState.grounds:
                groundsVisual.SetActive(true);
                break;
            case PortaFilterState.tampedgrounds:
                tampedGroundsVisual.SetActive(true);
                break;
            case PortaFilterState.usedgrounds:
                usedGroundsVisual.SetActive(true);
                break;
        }
    }
}
