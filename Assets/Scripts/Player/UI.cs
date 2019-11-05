using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public static UI Instance;

    public Image playerHealth;
    public Text weaponText, clipText, ammoText;

    public void Awake() {
        Instance = this;
    }

    // LateUpdate updates at the end of the frame.
    void LateUpdate() {
        UIHealth();
        UIWeapon();
        UIAmmo();
    }

    void UIHealth() {
        playerHealth.fillAmount = Player.Instance.health / Player.Instance.maxHealth;
    }

    void UIWeapon() {
        if (Player.Instance.isDualWielding) {
            if (Player.Instance.weapons[0].weaponData.name == Player.Instance.weapons[1].weaponData.name) {
                weaponText.text = "Dual " + Player.Instance.weapons[Player.Instance.selectedWeapon].weaponData.name + "s";
            }
            else {
                weaponText.text = Player.Instance.weapons[0].weaponData.name + " + " + Player.Instance.weapons[1].weaponData.name;
            }
        }
        else {
            weaponText.text = Player.Instance.weapons[Player.Instance.selectedWeapon].weaponData.name;
        }
    }

    void UIAmmo() {
        if (Player.Instance.isDualWielding) {
            Player.PlayerWeapon[] currWeapons = new Player.PlayerWeapon[2];

            currWeapons[0] = Player.Instance.weapons[0];
            currWeapons[1] = Player.Instance.weapons[1];

            if (currWeapons[0].reloadingWeapon && currWeapons[1].reloadingWeapon || (currWeapons[0].ammo <= 0 && currWeapons[1].ammo <= 0)) {
                clipText.text = string.Empty;

                int noDigits = Mathf.FloorToInt(Mathf.Log10(currWeapons[0].weaponData.clipSize * currWeapons[1].weaponData.clipSize) + 1);
                for (int i = 0; i < noDigits; i++) {
                    clipText.text += '-';
                }

                ammoText.text = (currWeapons[0].ammo - currWeapons[0].weaponData.clipSize).ToString();
            }
            else {
                int magazineSize = currWeapons[0].weaponData.clipSize + currWeapons[1].weaponData.clipSize;
                int clips = ((currWeapons[0].ammo + currWeapons[1].ammo - 1) % magazineSize) + 1;

                clipText.text = clips.ToString();
            }
        }
        else {
            Player.PlayerWeapon currWeapon = Player.Instance.weapons[Player.Instance.selectedWeapon];

            if (currWeapon.reloadingWeapon) {
                clipText.text = string.Empty;

                int noDigits = Mathf.FloorToInt(Mathf.Log10(currWeapon.weaponData.clipSize) + 1);
                for (int i = 0; i < noDigits; i++) {
                    clipText.text += '-';
                }

                ammoText.text = (currWeapon.ammo - currWeapon.weaponData.clipSize).ToString();
            }
            else {
                int magazineSize = currWeapon.weaponData.clipSize;
                clipText.text = (((currWeapon.ammo - 1) % magazineSize) + 1).ToString();
            }
        }
    }
}
