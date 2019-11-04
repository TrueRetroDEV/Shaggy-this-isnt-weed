using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BProjectile : MonoBehaviour {

    // REFERENCES
    ProjectileData projectileStats;
    WeaponData weaponStats;

    // INSTANCE VARIABLES
    float currSpeed;
    float lifeTime;

    public void Initialise(ProjectileData projectile, WeaponData weapon) {
        projectileStats = projectile;
        weaponStats = weapon;

        StartProjectile();
    }

    public void StartProjectile() {
        currSpeed = Mathf.Clamp(projectileStats.startSpeed, projectileStats.minSpeed, projectileStats.maxSpeed);

        enabled = true;
    }

    void Update() {
        CheckCollision();

        transform.position += transform.forward * currSpeed * Time.deltaTime;

        currSpeed = Mathf.Clamp(currSpeed + (projectileStats.accelleration * Time.deltaTime), projectileStats.minSpeed, projectileStats.maxSpeed);
        if (currSpeed <= projectileStats.destroySpeedThreshold) {
            DestroyProjectile();
        }

        lifeTime += Time.deltaTime;
        if (lifeTime >= projectileStats.lifeTime) {
            DestroyProjectile();
        }
    }

    void CheckCollision() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, currSpeed* Time.deltaTime, projectileStats.collisionLayers, QueryTriggerInteraction.Ignore)) {

            transform.position = hit.point;

            // FUTURE OPTIMISATION: Replace GetComponent<BHealth>() with a search through dictionary of registered objects.
            BHealth objectHealth = hit.transform.GetComponentInChildren<BHealth>();
            if (objectHealth != null) {
                Hit(objectHealth);
            }

            if (Vector3.Angle(ray.direction, hit.normal) > projectileStats.bounceAngle) {
                DestroyProjectile();
            }
            else {
                transform.forward = Vector3.Reflect(ray.direction, hit.normal);
            }
        }
    }

    void Hit(BHealth objHealth = null) {
        if (objHealth) {
            objHealth.TakeDamage(weaponStats.damage);
        }
    }

    public virtual void DestroyProjectile() {
        Destroy(gameObject);
    }
}
