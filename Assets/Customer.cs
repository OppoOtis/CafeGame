using UnityEngine;

public class Customer : Interactible, Interactor
{
    public IGrabbable currentlyHoldingLeft { get; set; }
    public IGrabbable currentlyHoldingRight { get; set; }

    public void DropCurrentlyHolding(bool _left)
    {
        throw new System.NotImplementedException();
    }

    public void PickUpGrabbable(IGrabbable _toPickUp, bool _left)
    {
        throw new System.NotImplementedException();
    }

    //generate an order for the customer
    void GenerateOrder() { }

    //receive an item and check if part of the order is fulfilled
    void GetOrder() { }
}
