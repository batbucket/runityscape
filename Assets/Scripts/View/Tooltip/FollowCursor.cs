using UnityEngine;
using System.Collections;

/// <summary>
/// Code from http://answers.unity3d.com/questions/903716/object-follow-cursor-how-to-access-mouse-cursor-x.html
/// </summary>
public class FollowCursor : MonoBehaviour {

    // Update is called once per frame
    private void Update() {
        Vector3 temp = Input.mousePosition;
        temp.z = 2499; // Set this to be the distance you want the object to be placed in front of the camera.
        this.transform.position = Camera.main.ScreenToWorldPoint(temp);
    }
}
