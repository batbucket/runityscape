using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatDidIClick : MonoBehaviour {
    [SerializeField]
    private new Camera camera;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.Log(ray);

            if (Physics.Raycast(ray, out hit)) {
                Debug.Log(hit.transform.gameObject.name);
            }
        }
    }
}
