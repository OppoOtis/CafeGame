using System;
using System.Collections.Generic;
using UnityEngine;

//Interface for all things that can be filled (with IFilling classes)
public interface IFillable
{
    //I'm going to work with milliliters, that's the most logical solution (I think)
    public float CapacityML { get; set; }
    public float CurrentVolumeML { get; set; }

    //a variable that indicates what kind of filling is inside the fillable.
    //mixing two fillings creates a mix that can't be separated (yet) (TODO: make a machine that separates Ifillings)
    //I guess whenever you pour a filling to something else, you copy the class if it isn't already inside the IFillable
    List<IFilling> currentFillings { get; set; }

    //returns the overflow of ml
    public float Fill(IFilling _filling, float _ml);
    
    //returns what comes out of the fillable
    public IFilling Empty(float _ml);

    public void UpdateVisuals(bool _left);
}
