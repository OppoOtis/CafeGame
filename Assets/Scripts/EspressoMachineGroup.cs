using UnityEngine;

public class EspressoMachineGroup : Interactible
{
    public Transform portaFilterLocation;

    Portafilter currentlyLoaded;

    public override void Interact(Interactor _interactor, bool _left)
    {
        IGrabbable holdingReference;
        if (_left)
            holdingReference = _interactor.currentlyHoldingLeft;
        else
            holdingReference = _interactor.currentlyHoldingRight;

        //Pick up the loaded portafilter
        if (currentlyLoaded != null && holdingReference == null)
        {
            currentlyLoaded.GetComponent<Grabbable>().enabled = true;
            _interactor.PickUpGrabbable(currentlyLoaded, _left);
            currentlyLoaded = null;
        }

        else if (currentlyLoaded == null && holdingReference is Portafilter)
        {
            //load the portafilter
            currentlyLoaded = holdingReference as Portafilter;
            _interactor.DropCurrentlyHolding(_left);
            currentlyLoaded.Visual.transform.position = portaFilterLocation.position;
            currentlyLoaded.Visual.transform.rotation = portaFilterLocation.rotation;
            currentlyLoaded.rb.isKinematic = true;
            currentlyLoaded.transform.parent = transform;
            currentlyLoaded.GetComponent<Grabbable>().enabled = false;
        }

    }
}
