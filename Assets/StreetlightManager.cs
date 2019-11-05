using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetlightManager : MonoBehaviour {

    public Streetlight[] streetlights;

    void Start() {
        streetlights = FindObjectsOfType<Streetlight>();
    }

    public void ToggleLights(bool enable) {
        for (int i = 0; i < streetlights.Length; i++) {
            streetlights[i].StartCoroutine(streetlights[i].Enable(true));
        }
    }
}
