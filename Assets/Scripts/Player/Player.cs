using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BHealth {

    // PUBLIC REFERENCES
    public static Player Instance;

    // PUBLIC VARAIBLES
    public float playerSpeed;
    public float cameraSize;

    public int selectedWeapon = 0;

    public GameObject flashlight;
    
    public WeaponData[] startWeapons = new WeaponData[2];
    public PlayerWeapon[] weapons = new PlayerWeapon[2];

    public struct PlayerWeapon {
        public WeaponData weaponData;

        public int ammo;
        public int clipSizeOffset;
        public float nextFire;
        public bool fireReady;
        public bool reloadingWeapon;
    }

    // PRIVATE VARIABLES
    CharacterController characterController;
    Vector3 motionVector;

    float weaponSwitchTimer = 0;

    public bool isDualWielding {
        get {
            if (weapons[1].weaponData == null) {
                return false;
            }
            else {
                return weapons[0].weaponData.dualWield && weapons[1].weaponData.dualWield;
            }
        }
    }

    bool hasCommonAmmoType {
        get {
            if (weapons[1].weaponData == null) {
                return false;
            }
            else {
                return weapons[0].weaponData.ammoType == weapons[1].weaponData.ammoType;
            }
        }
    }

    void Awake() {
        Instance = this;
        characterController = GetComponent<CharacterController>();

        PSetup();
    }

    void PSetup() {
        // Set default ammo for weapons.
        for (int i = 0; i < weapons.Length; i++) {
            if (startWeapons[i] != null) {
                weapons[i].weaponData = startWeapons[i];
            }
            weapons[i].ammo = startWeapons[i].defaultAmmo;
            weapons[i].clipSizeOffset = 0;//CalculateClipSizeOffset(weapons[i].ammo, weapons[i].weaponData.clipSize);
        }
    }
    
    void PInput() {
        // WEAPON FIRING INPUT:
        int primaryFire;
        if (isDualWielding) {
            primaryFire = 0;
        }
        else {
            primaryFire = selectedWeapon;
        }

        if (Input.GetAxis("Fire1") > 0.25f && weapons[primaryFire].ammo > 0) {
            if (weapons[primaryFire].weaponData.singleShot) {
                if (weapons[primaryFire].fireReady) {
                    weapons[primaryFire].fireReady = false;
                    StartCoroutine(PShoot(primaryFire));
                }
            }
            else {
                StartCoroutine(PShoot(primaryFire));
            }
        }
        else {
            weapons[0].fireReady = true;
        }
        if (isDualWielding && Input.GetAxis("Fire2") > 0.25f && weapons[1].weaponData && weapons[1].ammo > 0) {
            if (weapons[1].weaponData.singleShot) {
                if (weapons[1].fireReady) {
                    weapons[1].fireReady = false;
                    StartCoroutine(PShoot(1));
                }
            }
            else {
                StartCoroutine(PShoot(1));
            }
        }
        else {
            weapons[1].fireReady = true;
        }

        // RELOAD WEAPONS:
        if (Input.GetKeyDown(KeyCode.R)) {
            if (isDualWielding) {
                ReloadWeapon(0);
                ReloadWeapon(1);
            }
            else {
                ReloadWeapon(selectedWeapon);
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            flashlight.SetActive(!flashlight.activeSelf);
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
        transform.rotation = Cursor.LookToCursorRotation(transform.position, transform.rotation);
    }

    void UpdateTimers() {
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].nextFire -= Time.deltaTime;

            if (i == selectedWeapon || isDualWielding) {
                if (weapons[i].reloadingWeapon == true && weapons[i].nextFire <= 0) {
                    weapons[i].reloadingWeapon = false;
                }
            }
        }

        weaponSwitchTimer -= Time.deltaTime;
    }

    IEnumerator PShoot(int weaponNo) {
        if (weapons[weaponNo].ammo == 0) {
            if (selectedWeapon == 1) {
                selectedWeapon = 0;
            }
        }
        else if (weapons[weaponNo].nextFire <= 0) {
            WeaponData weaponData = weapons[weaponNo].weaponData;

            if (weaponData) {

                for (int i = 0; i < weaponData.roundsPerShot; i++) {
                    Vector3 spawnPosition = new Vector3();

                    if (weaponNo == 0) {
                        spawnPosition = transform.position + (transform.rotation * new Vector3(0.45f, 0, 0.6f));
                    }
                    else {
                        spawnPosition = transform.position + (transform.rotation * new Vector3(-0.45f, 0, 0.6f));
                    }
                    
                    BProjectile projectile = Instantiate(weaponData.projectile).AddComponent<BProjectile>();
                    projectile.Initialise(weaponData.projectileData, weaponData, spawnPosition, transform.rotation);

                    foreach (Zombie zombie in ZombieManager.Instance.zombies) {
                        if ((zombie.transform.position - Cursor.Instance.transform.position).sqrMagnitude <= 9) {
                            projectile.transform.forward = zombie.transform.position - projectile.transform.position;
                        }
                    }

                    Vector3 rot = projectile.transform.eulerAngles;
                    rot.y += Random.Range(-weaponData.weaponSpread, weaponData.weaponSpread) / 2;

                    projectile.transform.eulerAngles = rot;

                    if (weaponData.roundsPerShot > 1 && weaponData.burstDelay != 0) {
                        yield return new WaitForSeconds(weaponData.burstDelay);
                    }

                }

                weapons[weaponNo].ammo--;

                if (weapons[weaponNo].ammo % (weaponData.clipSize + weapons[weaponNo].clipSizeOffset) == 0) {
                    ReloadWeapon(weaponNo);
                }
                else {
                    weapons[weaponNo].nextFire = 60 / weaponData.rateOfFire;
                }
            }
            else {
                yield return null;
            }
        }
    }

    public void PickupWeapon(WeaponData weaponData, int weaponAmmo) {
        if (weapons[1].weaponData == null) {
            weapons[1].weaponData = weaponData;
            weapons[1].ammo = weaponAmmo;
            weapons[1].clipSizeOffset = CalculateClipSizeOffset(weaponAmmo, weaponData.clipSize);
            weapons[1].reloadingWeapon = false;
            weapons[1].nextFire = 0.0f;
        }
        else {
            DropWeapon(weapons[selectedWeapon]);

            weapons[selectedWeapon].weaponData = weaponData;
            weapons[selectedWeapon].ammo = weaponAmmo;
            weapons[selectedWeapon].clipSizeOffset = CalculateClipSizeOffset(weaponAmmo, weaponData.clipSize);
            weapons[selectedWeapon].reloadingWeapon = false;
            weapons[selectedWeapon].nextFire = 0.0f;
        }
    }

    void DropWeapon(PlayerWeapon weapon) {
        WeaponPickup droppedWeapon = Instantiate(ItemParameters.Instance.weaponPickup, transform.position + transform.forward, transform.rotation).GetComponent<WeaponPickup>();

        droppedWeapon.weapon = weapon.weaponData;
        droppedWeapon.weaponAmmo = weapon.ammo;
    }

    void ReloadWeapon(int weaponData) {
        weapons[weaponData].reloadingWeapon = true;
        weapons[weaponData].nextFire = weapons[weaponData].weaponData.reloadTime;
        weapons[weaponData].clipSizeOffset = 0;
    }

    void SwitchWeapon(int value) {
        if (value != 0) {
            if (isDualWielding) {
                selectedWeapon = 0;
            }
            else if (weaponSwitchTimer <= 0) {
                selectedWeapon = (int)Mathf.Repeat(selectedWeapon + value, weapons.Length);

                if (weapons[selectedWeapon].reloadingWeapon == true) {
                    weapons[selectedWeapon].nextFire = weapons[selectedWeapon].weaponData.reloadTime;
                }

                weaponSwitchTimer = 0.33f;
            }
        }
    }

    int CalculateClipSizeOffset(int ammo, int clipSize) {
        return ammo % clipSize;
    }
}
