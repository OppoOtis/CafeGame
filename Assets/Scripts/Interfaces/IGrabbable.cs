using System;
using UnityEngine;

public interface IGrabbable : IInteractible
{
    public bool TwoHanded { get; set; }
    public Rigidbody rb { get; set; }
    public void OnPickUp(Interactor _interactor, bool _left);
    public void OnDrop(Interactor _interactor, bool _left);
    public void GrabInteraction(Interactor _interactor, bool _left);
    public void GrabInteractionHold(Interactor _interactor, bool _left);
    public void GrabInteractionRelease(Interactor _interactor, bool _left);

}
