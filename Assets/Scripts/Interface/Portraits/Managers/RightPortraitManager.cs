using UnityEngine;

public class RightPortraitManager : PortraitManager {
    public const int X_OFFSET = -100;

    public override ResourceManager addResource() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("Right_Resource"));
        Util.parent(g, Util.findChild(gameObject, "Resources"));
        return g.GetComponent<ResourceManager>();
    }

    public override void setIconImage(Sprite image) {
        base.setIconImage(image);
        iconImage.transform.localRotation = Quaternion.Euler(0, 180, 0);

        //Hack to fix offset issues involving flipping
        iconImage.transform.position = new Vector3(iconImage.transform.position.x + X_OFFSET, iconImage.transform.position.y);
    }
}
