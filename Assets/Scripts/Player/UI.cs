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

            ammoText.text = Mathf.Clamp(currWeapons[0].ammo + currWeapons[1].ammo, 0, Mathf.Infinity).ToString();
        }
        else {
            Player.PlayerWeapon currWeapon = Player.Instance.weapons[Player.Instance.selectedWeapon];

            ammoText.text = currWeapon.ammo.ToString();
        }
    }
}
