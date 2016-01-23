using UnityEngine;
using System.Collections;
using System;

public class LeftPortraitManager : PortraitManager {
    public override ResourceManager addResource(string resourceName, Color overBarColor, Color underBarColor, int numerator, int denominator) {
        return addResource(resourceName, overBarColor, underBarColor, numerator, denominator, "Left_Resource");
    }
}
