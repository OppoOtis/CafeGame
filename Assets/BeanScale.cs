using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class BeanScale : Interactible
{
    public Transform onScaleLocation;
    IWeighable currentObjectOnScale;
    float currentScaleValue;

    public TextMeshPro weightText;

    public override void Interact(Interactor _interactor)
    {
        //Maybe set the camera to a determined place?


        //if(currentObjectOnScale == null && _interactor.currentlyHolding is IWeighable) 
        //{
        //    //place the weighable on the scale
        //    _interactor.currentlyHolding.Visual.transform.position = onScaleLocation.position;
        //    _interactor.currentlyHolding.Visual.transform.rotation = Quaternion.identity;
        //    _interactor.DropCurrentlyHolding();
        //}

        ////change interaction based on what the player is holding
        //if(_interactor.currentlyHolding is CoffeeBag)
        //{

        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IWeighable>() == null)
            return;
        UpdateWeightText(collision.gameObject.GetComponent<IWeighable>().Weight);
        Debug.Log("collision entered");
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<IWeighable>() == null)
            return;
        UpdateWeightText(-collision.gameObject.GetComponent<IWeighable>().Weight);
    }

    void UpdateWeightText(float _num)
    {
        currentScaleValue += _num;
        weightText.text = currentScaleValue.ToString();
    }
}
