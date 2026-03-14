using UnityEngine;
using UnityEngine.UIElements;

public class Grabbable : Interactible, IGrabbable
{
    public bool TwoHanded { get { return twoHanded; } set { twoHanded = value; } }
    public bool twoHanded;
    public virtual void GrabInteraction(Interactor _interactor, bool _left)
    {
        //the interaction to perform when you hold an item 
    }
    public virtual void GrabInteractionHold(Interactor _interactor, bool _left)
    {
        //the interaction to perform when you hold an item 
    }
}
