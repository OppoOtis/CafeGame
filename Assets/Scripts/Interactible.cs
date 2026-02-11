using UnityEngine;
using UnityEngine.UIElements;

public class Interactible : MonoBehaviour, IGrabbable
{
    public Transform Visual { get; set; }
    public Rigidbody rb { get; set; }
    public Collider col { get; set; }

    public bool CanInteract = true;
    public bool IsVisible { get { return myRenderer.isVisible; } }

    Renderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        Visual = transform;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public virtual void Interact()
    {

    }

    public void DeHighLight()
    {
        gameObject.layer = 0;
    }
    public void HighLight()
    {
        gameObject.layer = 7;
    }
}
