using UnityEngine;

public class RightPortraitManager : PortraitManager {
    public const int X_OFFSET = -100;

    public override ResourceManager AddResource(string resourceName, Color overBarColor, Color underBarColor, int numerator, int denominator) {
        return AddResource(resourceName, overBarColor, underBarColor, numerator, denominator, "Right_Resource");
    }

    public override void SetSprite(Sprite image) {
        base.SetSprite(image);
        iconImage.transform.localRotation = Quaternion.Euler(0, 180, 0);

        //Hack to fix offset issues involving flipping
        iconImage.transform.position = new Vector3(iconImage.transform.position.x + X_OFFSET, iconImage.transform.position.y);
    }
}
