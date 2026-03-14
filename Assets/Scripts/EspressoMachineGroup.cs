using UnityEngine;

public class EspressoMachineGroup : Interactible
{
    public Transform portaFilterLocation;

    Portafilter currentlyLoaded;

    public override void Interact(Interactor _interactor, bool _left)
    {
        if (_left)
        {
            //Pick up the loaded portafilter
            if (currentlyLoaded != null && _interactor.currentlyHoldingLeft == null)
            {
                currentlyLoaded.GetComponent<Grabbable>().enabled = true;
                _interactor.PickUpGrabbable(currentlyLoaded, true);
                currentlyLoaded = null;
            }

            else if (currentlyLoaded == null && _interactor.currentlyHoldingLeft is Portafilter)
            {
                //load the portafilter
                currentlyLoaded = _interactor.currentlyHoldingLeft as Portafilter;
                _interactor.DropCurrentlyHolding(true);
                currentlyLoaded.Visual.transform.position = portaFilterLocation.position;
                currentlyLoaded.Visual.transform.rotation = portaFilterLocation.rotation;
                currentlyLoaded.rb.isKinematic = true;
                currentlyLoaded.transform.parent = transform;
                currentlyLoaded.GetComponent<Grabbable>().enabled = false;
            }
        }
    }
}
