using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streetlight : MonoBehaviour {

    // Reference to the light.
    new Light light;

    void Start() {
        light = GetComponent<Light>();
    }

    public IEnumerator Enable(bool enable) {
        yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        
        enabled = enable;
    }

    void LateUpdate() {
        light.enabled = ((((Player.Instance.transform.position - transform.position).sqrMagnitude) * (CameraManager.Instance.MainCamera.orthographicSize / 10))) < ((light.range * 2) * (light.range * 2));
    }
}
