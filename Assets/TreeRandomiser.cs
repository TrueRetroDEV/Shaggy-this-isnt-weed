using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRandomiser : MonoBehaviour {

    void Awake() {
        transform.localScale += Random.insideUnitSphere.normalized * 2f;
        transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        Destroy(this);
    }
}
