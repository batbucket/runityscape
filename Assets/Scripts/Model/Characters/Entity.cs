using UnityEngine;
using UnityEngine.UI;

/**
 * An entity is anything that can show up
 * On the portrait holders.
 * Naturally, the most basic one just has a sprite representation
 */
public abstract class Entity {
    Sprite sprite;

    public void setSprite(Sprite sprite) {
        this.sprite = sprite;
    }
}
