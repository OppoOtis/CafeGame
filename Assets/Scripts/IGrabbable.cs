using UnityEngine;

public interface IGrabbable : IInteractible
{
    public bool TwoHanded { get; set; }
    public Rigidbody rb { get; set; }
    public void GrabInteraction(Interactor _interactor, bool _left);
    public void GrabInteractionHold(Interactor _interactor, bool _left);
}
