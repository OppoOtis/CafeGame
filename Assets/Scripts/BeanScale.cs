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

    /*public override void interact(interactor _interactor)
    {
        //maybe set the camera to a determined place?


        //if(currentobjectonscale == null && _interactor.currentlyholding is iweighable) 
        //{
        //    //place the weighable on the scale
        //    _interactor.currentlyholding.visual.transform.position = onscalelocation.position;
        //    _interactor.currentlyholding.visual.transform.rotation = quaternion.identity;
        //    _interactor.dropcurrentlyholding();
        //}

        ////change interaction based on what the player is holding
        //if(_interactor.currentlyholding is coffeebag)
        //{

        //}
    }*/

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
