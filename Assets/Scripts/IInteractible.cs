using UnityEngine;

public interface IInteractible
{
    public bool CanInteract { get; set; }
    public Transform Visual { get; set; }
    public void Interact();
    public void HighLight();
    public void DeHighLight();
}
