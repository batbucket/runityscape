using UnityEngine;
using System.Collections;
using System;

public class LeftPortraitView : PortraitView {
    public override ResourceView AddResource(string resourceName, Color overBarColor, Color underBarColor, int numerator, int denominator) {
        return AddResource(resourceName, overBarColor, underBarColor, numerator, denominator, "Left_Resource");
    }
}
