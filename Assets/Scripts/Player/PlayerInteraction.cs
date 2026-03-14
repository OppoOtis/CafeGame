using UnityEditor;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour, Interactor
{
    public Transform holdingLocationLeft, holdingLocationRight, holdingLocationMiddle;
    public IGrabbable currentlyHoldingLeft { get; set; }
    public IGrabbable currentlyHoldingRight { get; set; }

    public IInteractible closestInteractible { get; set; }

    BoxCollider boxCollider;
    public CharacterController playerCharacterController;
    public Camera playerCamera;

    public float interactionRaycastRange;
    public float throwPower;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

    }

    private void Start()
    {
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.DropLeft, DropGrabbableLeft);
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.DropRight, DropGrabbableRight);
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.InteractLeft, InteractWithLeft);
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.InteractRight, InteractWithRight);
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.LeftUse, CurrentGrabbableInteractionLeft);
        Blackboard.inputManager.AddActionToInputCancelled(Blackboard.inputSystemActions.Player.LeftUse, CurrentGrabbableReleaseInteractionLeft);
        Blackboard.inputManager.AddActionToInput(Blackboard.inputSystemActions.Player.RightUse, CurrentGrabbableInteractionRight);
        Blackboard.inputManager.AddActionToInputCancelled(Blackboard.inputSystemActions.Player.RightUse, CurrentGrabbableReleaseInteractionRight);
    }

    private void Update()
    {
        CheckForInteractibles();


        if (currentlyHoldingLeft != null)
        {
            if (currentlyHoldingLeft.TwoHanded)
            {
                currentlyHoldingLeft.Visual.transform.position = holdingLocationMiddle.position;
            }
            else
            {
                currentlyHoldingLeft.Visual.transform.position = holdingLocationLeft.position;
            }
        }
        if (currentlyHoldingRight != null)
        {
            if (currentlyHoldingRight.TwoHanded)
            {
                currentlyHoldingRight.Visual.transform.position = holdingLocationMiddle.position;
            }
            else
            {
                currentlyHoldingRight.Visual.transform.position = holdingLocationRight.position;
            }
        }

        if (currentlyHoldingLeft != null)
            Debug.Log("Left:" + currentlyHoldingLeft.GetType());
        if (currentlyHoldingRight != null)
            Debug.Log("Right" + currentlyHoldingRight.GetType());

        CurrentGrabbableHoldInteraction();
    }

    void CheckForInteractibles()
    {
        if (closestInteractible != null) 
            closestInteractible.DeHighLight();
        
        closestInteractible = null;

        //check if we hit something with a ray first
        RaycastHit rayHit;
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out rayHit, interactionRaycastRange);
        if (rayHit.collider != null && rayHit.collider.GetComponent<Interactible>() != null && rayHit.collider.GetComponent<Interactible>().isActiveAndEnabled && rayHit.collider.GetComponent<Interactible>().canInteract)
        {
            closestInteractible = rayHit.collider.GetComponent<IInteractible>();
        }

        else
        {
            Collider[] hitColliders = Physics.OverlapBox(boxCollider.transform.position, boxCollider.size, transform.rotation);

            foreach (Collider hit in hitColliders)
            {
                if (hit.GetComponent<Interactible>() && hit.GetComponent<Interactible>().isActiveAndEnabled && hit.GetComponent<Interactible>().CanInteract &&
                    (closestInteractible == null ||
                    Vector3.Distance(hit.transform.position, boxCollider.transform.position) < Vector3.Distance(closestInteractible.Visual.transform.position, boxCollider.transform.position)))
                    closestInteractible = hit.GetComponent<IInteractible>();
            }
        }

        if (closestInteractible != null)
            closestInteractible.HighLight();
    }

    void InteractWithLeft(InputAction.CallbackContext context)
    {
        InteractWith(context, true);
    }
    void InteractWithRight(InputAction.CallbackContext context)
    {
        InteractWith(context, false);
    }

    void InteractWith(InputAction.CallbackContext context, bool _left)
    {
        if (closestInteractible == null || (_left && currentlyHoldingLeft != null) || (!_left && currentlyHoldingRight != null))
            return;
        if (closestInteractible.Visual.GetComponent<IGrabbable>() != null)
        {
            GrabInteractible(context, _left);
            return;
        }

        closestInteractible.Interact(this, _left);
    }

    void GrabInteractible(InputAction.CallbackContext context, bool _left)
    {
        if (closestInteractible == null || closestInteractible.Visual.GetComponent<IGrabbable>() == null)
            return;
        if ((_left && currentlyHoldingLeft != null) || (!_left && currentlyHoldingRight != null))
            return;
        if (closestInteractible.Visual.GetComponent<IGrabbable>().TwoHanded)
        {
            if (!(closestInteractible.Visual.GetComponent<IGrabbable>().TwoHanded && currentlyHoldingLeft == null && currentlyHoldingRight == null))
                return;
        }

        Debug.Log("Pick up");
        PickUpGrabbable(closestInteractible.Visual.GetComponent<IGrabbable>(), _left);
        closestInteractible = null;
    }

    public void PickUpGrabbable(IGrabbable _toPickUp, bool _left)
    {
        _toPickUp.CanInteract = false;
        _toPickUp.rb.isKinematic = true;
        _toPickUp.rb.linearVelocity = Vector3.zero;
        _toPickUp.Visual.gameObject.layer = 8;
        _toPickUp.Visual.GetComponent<Collider>().enabled = false; 
        _toPickUp.Visual.transform.forward = transform.forward;

        if (_toPickUp.TwoHanded)
        {
            currentlyHoldingLeft = _toPickUp;
            currentlyHoldingRight = _toPickUp;
            _toPickUp.Visual.transform.parent = holdingLocationMiddle;
        }
        else if (_left)
        {
            currentlyHoldingLeft = _toPickUp;
            _toPickUp.Visual.transform.parent = holdingLocationLeft;
        }
        else
        {
            currentlyHoldingRight = _toPickUp;
            _toPickUp.Visual.transform.parent = holdingLocationRight;
        }

        _toPickUp.Visual.transform.localPosition = Vector3.zero;
    }

    void DropGrabbableLeft(InputAction.CallbackContext context)
    {
        DropCurrentlyHolding(true);
    }

    void DropGrabbableRight(InputAction.CallbackContext context)
    {
        DropCurrentlyHolding(false);
    }

    public void DropCurrentlyHolding(bool _left)
    {
        if (currentlyHoldingLeft != null && currentlyHoldingLeft.TwoHanded)
        {
            ActivateGrabbable(currentlyHoldingLeft);
            currentlyHoldingLeft = null;
            currentlyHoldingRight = null;
            return;
        }

        if (_left)
        {
            if (currentlyHoldingLeft == null)
                return;
            ActivateGrabbable(currentlyHoldingLeft);
            currentlyHoldingLeft = null;
        }
        else
        {
            if (currentlyHoldingRight == null)
                return;
            ActivateGrabbable(currentlyHoldingRight);
            currentlyHoldingRight = null;
        }
    }

    void ActivateGrabbable(IGrabbable _target)
    {
        _target.rb.isKinematic = false;
        _target.CanInteract = true;
        _target.rb.linearVelocity = playerCharacterController.velocity;
        _target.Visual.gameObject.layer = 0;
        _target.Visual.GetComponent<Collider>().enabled = true;
        _target.Visual.transform.parent = null;
    }

    void ThrowGrabbableLeft(InputAction.CallbackContext context)
    {
        if (currentlyHoldingLeft == null)
            return;
        currentlyHoldingLeft.rb.isKinematic = false;
        currentlyHoldingLeft.CanInteract = true;
        currentlyHoldingLeft.rb.linearVelocity = playerCamera.transform.forward * throwPower;
        currentlyHoldingLeft.Visual.gameObject.layer = 0;
        currentlyHoldingLeft = null;
    }
    void ThrowGrabbableRight(InputAction.CallbackContext context)
    {
        if (currentlyHoldingRight == null)
            return;
        currentlyHoldingRight.rb.isKinematic = false;
        currentlyHoldingRight.CanInteract = true;
        currentlyHoldingRight.rb.linearVelocity = playerCamera.transform.forward * throwPower;
        currentlyHoldingRight.Visual.gameObject.layer = 0;
        currentlyHoldingRight = null;
    }

    void CurrentGrabbableInteractionLeft(InputAction.CallbackContext context)
    {
        if(currentlyHoldingLeft == null)
            return;
        currentlyHoldingLeft.GrabInteraction(this, true);
    }
    void CurrentGrabbableInteractionRight(InputAction.CallbackContext context)
    {
        if(currentlyHoldingRight == null)
            return;
        currentlyHoldingRight.GrabInteraction(this, false);
    }
    void CurrentGrabbableHoldInteraction()
    {
        if (currentlyHoldingLeft != null && Blackboard.inputSystemActions.Player.LeftUseHold.ReadValue<float>() > 0)
            currentlyHoldingLeft.GrabInteractionHold(this, true);
        
        else if (currentlyHoldingRight != null && Blackboard.inputSystemActions.Player.RightUseHold.ReadValue<float>() > 0)
            currentlyHoldingRight.GrabInteractionHold(this, false);
    }

    void CurrentGrabbableReleaseInteractionLeft(InputAction.CallbackContext context)
    {
        CurrentGrabbableReleaseInteraction(this, true);
    }
    void CurrentGrabbableReleaseInteractionRight(InputAction.CallbackContext context)
    {
        CurrentGrabbableReleaseInteraction(this, false);
    }

    void CurrentGrabbableReleaseInteraction(Interactor _interactor, bool _left)
    {
        currentlyHoldingRight.GrabInteractionRelease(this, _left);
    }
}
