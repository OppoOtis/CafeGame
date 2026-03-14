using UnityEngine;

//interface for anything that can interact with interactibles
public interface Interactor
{
    public IGrabbable currentlyHoldingLeft { get; set; }
    public IGrabbable currentlyHoldingRight { get; set; }

    public IInteractible closestInteractible { get; set; }

    public void PickUpGrabbable(IGrabbable _toPickUp, bool _left);
    public void DropCurrentlyHolding(bool _left);
}
