using UnityEngine;

public class CoffeeBag : Grabbable
{
    public override void GrabInteraction(Interactor _interactor, bool _left)
    {
        IGrabbable oppositeHand;
        if (_left)
            oppositeHand = _interactor.currentlyHoldingRight;
        else
            oppositeHand = _interactor.currentlyHoldingLeft;

        Debug.Log(oppositeHand is IFillable);
        if(oppositeHand is IFillable)
        {
            //FILL THAT SHIT WITH BEAAAANNS
            IFillable holdingFillable = oppositeHand as IFillable;
            holdingFillable.Fill(new CoffeeBean(), holdingFillable.CapacityML);
            holdingFillable.UpdateVisuals(!_left);
        }

    }
}
