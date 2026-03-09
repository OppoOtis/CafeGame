using UnityEngine;

public interface IWeighable : IInteractible, IGrabbable
{
    public float Weight { get; set; }
}
