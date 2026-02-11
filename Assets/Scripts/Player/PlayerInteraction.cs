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

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        CheckForInteractibles();

        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.Attack, GrabInteractible);
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.Cancel, DropGrabbable);

        if(currentlyHolding != null)
        {
            currentlyHolding.Visual.transform.position = holdingLocation.position;
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

    void GrabInteractible(InputAction.CallbackContext context)
    {
        if (closestInteractible == null || closestInteractible.Visual.GetComponent<IGrabbable>() == null)
            return;

        currentlyHolding = closestInteractible.Visual.GetComponent<IGrabbable>();
        currentlyHolding.rb.isKinematic = true;
        currentlyHolding.rb.linearVelocity = Vector3.zero;
    }

    void DropGrabbable(InputAction.CallbackContext context)
    {
        if (currentlyHolding == null)
            return;
        currentlyHolding.rb.isKinematic = false;
        Debug.Log(playerCharacterController.velocity);
        currentlyHolding.rb.linearVelocity = playerCharacterController.velocity;
        currentlyHolding = null;
    }
}
