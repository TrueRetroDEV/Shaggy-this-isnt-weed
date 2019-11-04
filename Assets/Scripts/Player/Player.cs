using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BHealth {

    // PUBLIC VARAIBLES
    public WeaponData[] weapons = new WeaponData[2];
    public int[] ammo = new int[2];

    // PRIVATE VARIABLES
    float[] weaponTimers = new float[2];
    bool[] weaponFireReady = new bool[2];

    float weaponSwitchTimer = 0;
    int selectedWeapon = 0;
    
    void PInput() {
        // WEAPON FIRING INPUT:
        if (Input.GetAxis("Fire1") > 0.25f && ammo[0] > 0) {
            if (weapons[0].singleShot) {
                if (weaponFireReady[0]) {
                    weaponFireReady[0] = false;
                    StartCoroutine(PShoot(0));
                }
            }
            else {
                StartCoroutine(PShoot(0));
            }
        }
        else {
            weaponFireReady[0] = true;
        }
        if (Input.GetAxis("Fire2") > 0.25f && weapons[1] && ammo[1] > 0) {
            if (weapons[1].dualWield && weapons[1].singleShot) {
                if (weaponFireReady[1]) {
                    weaponFireReady[1] = false;
                    StartCoroutine(PShoot(1));
                }
            }
            else {
                StartCoroutine(PShoot(1));
            }
        }
        else {
            weaponFireReady[1] = true;
        }
        
        // WEAPON SWITCHING INPUT:
        SwitchWeapon((int)Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel"), -1, 1));
    }

    void Update() {
        PInput();
        PLook();
        PCamera();

        UpdateTimers();
    }

    IEnumerator PShoot(int weapon) {
        if (ammo[weapon] == 0) {
            if (selectedWeapon == 1) {
                selectedWeapon = 0;
            }
        }
        else if (weaponTimers[weapon] <= 0){
            WeaponData weaponData = weapons[selectedWeapon];
            if (weaponData)
            {
                for (int i = 0; i < weaponData.roundsPerShot; i++)
                {
                    BProjectile projectile = Instantiate(weaponData.projectile, transform.position, transform.rotation, null).AddComponent<BProjectile>();
                    projectile.Initialise(weaponData.projectileData, weaponData);

                    if (weaponData.roundsPerShot > 1)
                    {
                        yield return new WaitForSeconds(weaponData.burstDelay);
                    }
                    else
                    {
                        yield return null;
                    }

                    ammo[weapon]--;
                }

                weaponTimers[weapon] = 60 / weaponData.rateOfFire;
            }
            else {
                yield return null;
            }
        }
    }

    void PCamera() {
        Vector3 cameraPosition = CameraManager.Instance.MainCamera.transform.position;
        Vector3 newPosition = Vector3.ClampMagnitude(Vector3.Lerp(Cursor.Instance.transform.position, transform.position, 0.5f), 3.0f);

        newPosition.y = transform.position.y + 10.0f;

        CameraManager.Instance.MainCamera.transform.position = Vector3.Lerp(cameraPosition, newPosition, Time.deltaTime * 10.0f);
    }

    void PLook() {
        Vector3 rot = transform.eulerAngles;
        rot.y = Mathf.Atan2(Cursor.Instance.transform.position.x, Cursor.Instance.transform.position.z) * Mathf.Rad2Deg;

        transform.eulerAngles = rot;
    }

    void UpdateTimers() {
        for (int i = 0; i < weaponTimers.Length; i++) {
            weaponTimers[i] -= Time.deltaTime;
        }

        weaponSwitchTimer -= Time.deltaTime;
    }

    void SwitchWeapon(int value) {
        if (value != 0) {
            if (weapons[1] && weapons[1].dualWield) {
                selectedWeapon = 0;
            }
            else if (weaponSwitchTimer <= 0) {
                selectedWeapon = (int)Mathf.Repeat(selectedWeapon + value, weapons.Length);

                weaponSwitchTimer = 0.15f;
            }
        }
    }
}
