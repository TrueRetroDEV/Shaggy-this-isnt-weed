using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    public static Cursor Instance;

    void Start() {
        Instance = this;
    }

    void Update() {
        transform.position = CalculatePosition();
    }

    Vector3 CalculatePosition() {
        Vector3 screenPosition = Input.mousePosition + (Vector3.up * CameraManager.Instance.MainCamera.transform.position.y);
        Vector3 worldPosition = CameraManager.Instance.MainCamera.ScreenToWorldPoint(screenPosition, Camera.MonoOrStereoscopicEye.Mono);

        worldPosition.y = 0.1f;

        return worldPosition;
    }

    public static Quaternion LookToCursorRotation(Vector3 currPosition, Quaternion currRotation) {
        Vector3 localCursorPosition = Cursor.Instance.transform.position - currPosition;
        Vector3 rot = currRotation.eulerAngles;

        rot.x = 0;
        rot.y = Mathf.Atan2(localCursorPosition.x, localCursorPosition.z) * Mathf.Rad2Deg;
        rot.z = 0;

        return Quaternion.Euler(rot);
    }
}
