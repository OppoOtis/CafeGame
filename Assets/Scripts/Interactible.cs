using UnityEngine;
using UnityEngine.UIElements;

public class Interactible : MonoBehaviour, IInteractible
{
    public Transform Visual { get; set; }
    public Rigidbody rb { get; set; }

    public bool CanInteract { get { return canInteract; } set { canInteract = value; } }
    public bool canInteract = true;
    public bool IsVisible { get { return myRenderer.isVisible; } }

    Renderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        Visual = transform;
        rb = GetComponent<Rigidbody>();
    }
        
    public virtual void Interact(Interactor _interactor, bool _left)
    {

    }


    public void DeHighLight()
    {
        gameObject.layer = 0;
        for(int i = 0; i < Visual.childCount; i++)
        {
            Visual.GetChild(i).gameObject.layer = 0;
        }
    }
    public void HighLight()
    {
        gameObject.layer = 7;
        for (int i = 0; i < Visual.childCount; i++)
        {
            Visual.GetChild(i).gameObject.layer = 7;
        }
    }
}
