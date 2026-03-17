using UnityEngine;

public class Tamp : Grabbable
{
    public override void GrabInteraction(Interactor _interactor, bool _left)
    {
        IGrabbable otherGrabbable;

        if (_left)
            otherGrabbable = _interactor.currentlyHoldingRight;
        else
            otherGrabbable = _interactor.currentlyHoldingLeft;

        if (otherGrabbable == null)
            return;

        if(otherGrabbable is Portafilter)
        {
            Portafilter p = otherGrabbable as Portafilter;
            if(p.myState == PortaFilterState.grounds)
                p.ChangePortaFilterState(PortaFilterState.tampedgrounds);
        }
    }
}
