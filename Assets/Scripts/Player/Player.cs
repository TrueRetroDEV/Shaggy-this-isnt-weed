using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BHealth {

    // PUBLIC VARAIBLES
    public float playerSpeed;
    public float cameraSize;

    public WeaponData[] weapons = new WeaponData[2];
    public int[] ammo = new int[2];

    // PRIVATE VARIABLES
    CharacterController characterController;
    Vector3 motionVector;

    bool[] weaponFireReady = new bool[2];
    float[] weaponTimers = new float[2];

    float weaponSwitchTimer = 0;
    int selectedWeapon = 0;

    void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    void PInput() {
        // WEAPON FIRING INPUT:
        int primaryFire = -1;

        if (weapons[0].dualWield && weapons[1].dualWield) {
            primaryFire = 0;
        }
        else {
            primaryFire = selectedWeapon;
        }

        if (Input.GetAxis("Fire1") > 0.25f && ammo[primaryFire] > 0) {
            if (weapons[primaryFire].singleShot) {
                if (weaponFireReady[primaryFire]) {
                    weaponFireReady[primaryFire] = false;
                    StartCoroutine(PShoot(primaryFire));
                }
            }
            else {
                StartCoroutine(PShoot(primaryFire));
            }
        }
        else {
            weaponFireReady[0] = true;
        }
        if (weapons[0].dualWield && weapons[1].dualWield && Input.GetAxis("Fire2") > 0.25f && weapons[1] && ammo[1] > 0) {
            if (weapons[1].singleShot) {
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

        // PLAYER MOVEMENT:
        motionVector.x = Input.GetAxis("Horizontal");
        motionVector.z = Input.GetAxis("Vertical");
        motionVector.Normalize();

        // WEAPON SWITCHING INPUT:
        SwitchWeapon((int)Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel"), -1, 1));
    }

    void Update() {
        PInput();
        PLook();
        PCamera();
        PMove();

        UpdateTimers();
    }

    void PMove() {
        characterController.Move(motionVector * playerSpeed * Time.deltaTime);
    }

    void PCamera() {
        Vector3 cameraPosition = CameraManager.Instance.MainCamera.transform.position;
        Vector3 newPosition = transform.position + Vector3.ClampMagnitude(Vector3.Lerp(Cursor.Instance.transform.position - transform.position, Vector3.zero, 0.5f), 4.5f);

        newPosition.y = transform.position.y + 10.0f;

        CameraManager.Instance.MainCamera.transform.position = Vector3.Lerp(cameraPosition, newPosition, Time.deltaTime * 10.0f);
        CameraManager.Instance.MainCamera.orthographicSize = Mathf.Lerp(CameraManager.Instance.MainCamera.orthographicSize, 10 + Mathf.Lerp(0, motionVector.magnitude * 100, Time.deltaTime * 3.0f), Time.deltaTime * 0.5f);
    }

    void PLook() {
        Vector3 localCursorPosition = Cursor.Instance.transform.position - transform.position;
        Vector3 rot = transform.eulerAngles;

        rot.y = Mathf.Atan2(localCursorPosition.x, localCursorPosition.z) * Mathf.Rad2Deg;

        transform.eulerAngles = rot;
    }

    void UpdateTimers() {
        for (int i = 0; i < weaponTimers.Length; i++) {
            weaponTimers[i] -= Time.deltaTime;
        }

        weaponSwitchTimer -= Time.deltaTime;
    }

    IEnumerator PShoot(int weapon) {
        if (ammo[weapon] == 0) {
            if (selectedWeapon == 1) {
                selectedWeapon = 0;
            }
        }
        else if (weaponTimers[weapon] <= 0) {
            WeaponData weaponData = weapons[weapon];

            if (weaponData) {

                for (int i = 0; i < weaponData.roundsPerShot; i++) {
                    BProjectile projectile = Instantiate(weapons[weapon].projectile, transform.position, transform.rotation).AddComponent<BProjectile>();

                    if (weapon == 0) {
                        projectile.transform.position += transform.rotation * new Vector3(0.45f, 0, 0.6f);
                    }
                    else {
                        projectile.transform.position += transform.rotation * new Vector3(-0.45f, 0, 0.6f);
                    }

                    projectile.transform.forward = Cursor.Instance.transform.position - projectile.transform.position;
                    projectile.Initialise(weaponData.projectileData, weaponData);

                    if (weaponData.roundsPerShot > 1 && weaponData.burstDelay != 0) {
                        yield return new WaitForSeconds(weaponData.burstDelay);
                    }

                }

                ammo[weapon]--;
                weaponTimers[weapon] = 60 / weaponData.rateOfFire;
            }
            else {
                yield return null;
            }
        }
    }

    void SwitchWeapon(int value) {
        if (value != 0) {
            if (weapons[1] && weapons[1].dualWield) {
                selectedWeapon = 0;
            }
            else if (weaponSwitchTimer <= 0) {
                selectedWeapon = (int)Mathf.Repeat(selectedWeapon + value, weapons.Length);

                weaponSwitchTimer = 0.33f;
            }
        }
    }
}
