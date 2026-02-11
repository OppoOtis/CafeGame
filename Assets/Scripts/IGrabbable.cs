using UnityEngine;

public interface IGrabbable : IInteractible
{
    public Rigidbody rb { get; set; }
    public Collider col { get; set; }
}
