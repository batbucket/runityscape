using UnityEngine;
using System.Collections;

/// <summary>
/// Scales a recttransform to fit the camera dimensions.
/// Allows this game to be played in any dimension.
/// </summary>
public class FitToCamera : MonoBehaviour {

    [SerializeField]
    private RectTransform target;

    /// <summary>
    /// http://answers.unity3d.com/questions/640325/get-screen-height-in-real-world-coordinates.html
    /// </summary>
    private void Update() {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;

        target.sizeDelta = new Vector2(width, height);
    }

}