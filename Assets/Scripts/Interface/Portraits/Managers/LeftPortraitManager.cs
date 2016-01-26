using UnityEngine;
using System.Collections;
using System;

public class LeftPortraitManager : PortraitManager {
    public override ResourceManager AddResource(string resourceName, Color overBarColor, Color underBarColor, int numerator, int denominator) {
        return AddResource(resourceName, overBarColor, underBarColor, numerator, denominator, "Left_Resource");
    }
}
