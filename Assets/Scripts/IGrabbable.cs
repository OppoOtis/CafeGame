using UnityEngine;

public interface IGrabbable : IInteractible
{
    public Rigidbody rb { get; set; }
}
