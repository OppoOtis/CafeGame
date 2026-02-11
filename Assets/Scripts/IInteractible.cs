using UnityEngine;

public interface IInteractible
{
    public Transform Visual { get; set; }
    public void HighLight();
    public void DeHighLight();
}
