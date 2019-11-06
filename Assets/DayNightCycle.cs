using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

    public static DayNightCycle Instance;

    public float speed;
    public float progress;

    public Light sun, moon;
    public Gradient sunLight;

    void Awake() {
        Instance = this;
    }

    void Update() {
        progress = Mathf.Repeat(progress + (Time.deltaTime * speed), 1.0f);

        sun.color = sunLight.Evaluate(Mathf.Clamp(progress * 2, 0, 1f));
        sun.transform.localEulerAngles = new Vector3(30, 360 * progress, 0);

        if (progress > 0.5f) {
            sun.enabled = false;
            moon.enabled = true;
            
            if (sun.intensity < 0.05) {
                sun.intensity = 0;
                moon.intensity = Mathf.Lerp(moon.intensity, 1, Time.deltaTime * 1f);
            }
            else {
                sun.intensity = Mathf.Lerp(sun.intensity, 0, Time.deltaTime * 2.0f);
            }
        }
        else {
            sun.enabled = true;
            moon.enabled = false;

            if (moon.intensity > 0.05) {
                moon.intensity = 0;
                sun.intensity = Mathf.Lerp(sun.intensity, 1, Time.deltaTime * 1f);
            }
            else {
                moon.intensity = Mathf.Lerp(moon.intensity, 0, Time.deltaTime * 2.0f);
            }
        }
    }
}
