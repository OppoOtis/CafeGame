using UnityEngine;

public class Portafilter : Grabbable
{
    public override void GrabInteraction(Interactor _interactor, bool _left)
    {
        Debug.Log(_interactor.closestInteractible is EspressoMachineGroup);
        if(_interactor.closestInteractible is EspressoMachineGroup)
        {
            _interactor.closestInteractible.Interact(_interactor, _left);
        }
    }
}
