using UnityEditor;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Transform holdingLocation;
    IGrabbable currentlyHolding;
    IInteractible closestInteractible;

    BoxCollider boxCollider;
    public CharacterController playerCharacterController;
    public Camera playerCamera;

    public float throwPower;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

    }

    private void Start()
    {
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.Interact, InteractWith);
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.Drop, DropGrabbable);
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.Throw, ThrowGrabbable);
    }

    private void Update()
    {
        CheckForInteractibles();

        if(currentlyHolding != null)
        {
            currentlyHolding.Visual.transform.position = holdingLocation.position;
            currentlyHolding.Visual.transform.forward = transform.forward; 
        }
    }

    void CheckForInteractibles()
    {
        if (closestInteractible != null) 
            closestInteractible.DeHighLight();
        
        closestInteractible = null;
        Collider[] hitColliders = Physics.OverlapBox(boxCollider.transform.position, boxCollider.size, transform.rotation);

        foreach (Collider hit in hitColliders)
        {
            if (hit.GetComponent<Interactible>() && hit.GetComponent<Interactible>().CanInteract && 
                (closestInteractible == null || 
                Vector3.Distance(hit.transform.position, boxCollider.transform.position) < Vector3.Distance(closestInteractible.Visual.transform.position, boxCollider.transform.position)) )
                closestInteractible = hit.GetComponent<IInteractible>();
        }

        if (closestInteractible != null)
            closestInteractible.HighLight();
    }

    void InteractWith(InputAction.CallbackContext context)
    {
        if (closestInteractible == null)
            return;
        if (closestInteractible.Visual.GetComponent<IGrabbable>() != null)
        {
            GrabInteractible(context);
            return;
        }

        closestInteractible.Interact();
    }

    void GrabInteractible(InputAction.CallbackContext context)
    {
        if (closestInteractible == null || closestInteractible.Visual.GetComponent<IGrabbable>() == null || currentlyHolding != null)
            return;
        currentlyHolding = closestInteractible.Visual.GetComponent<IGrabbable>();
        currentlyHolding.CanInteract = false;
        currentlyHolding.rb.isKinematic = true;
        currentlyHolding.rb.linearVelocity = Vector3.zero;
        currentlyHolding.Visual.gameObject.layer = 8;
        closestInteractible = null;
    }

    void DropGrabbable(InputAction.CallbackContext context)
    {
        Debug.Log("Drop");
        if (currentlyHolding == null)
            return;
        currentlyHolding.rb.isKinematic = false;
        currentlyHolding.CanInteract = true;
        currentlyHolding.rb.linearVelocity = playerCharacterController.velocity;
        currentlyHolding.Visual.gameObject.layer = 0;
        currentlyHolding = null;
    }

    void ThrowGrabbable(InputAction.CallbackContext context)
    {
        Debug.Log("Throw");
        if (currentlyHolding == null)
            return;
        currentlyHolding.rb.isKinematic = false;
        currentlyHolding.CanInteract = true;
        currentlyHolding.rb.linearVelocity = playerCamera.transform.forward * throwPower;
        currentlyHolding.Visual.gameObject.layer = 0;
        currentlyHolding = null;
    }
}
