using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager Instance;

    private Camera mainCamera;
    public Camera MainCamera {
        get {
            if (mainCamera == null) {
                mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

                if (mainCamera == null) {
                    mainCamera = new GameObject("Camera").AddComponent<Camera>();
                    mainCamera.transform.tag = "MainCamera";
                }
            }
            
            return mainCamera;
        }
    }

    void Awake() {
        Instance = this;
    }
}
