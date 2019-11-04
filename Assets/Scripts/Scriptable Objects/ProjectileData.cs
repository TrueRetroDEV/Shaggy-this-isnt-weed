using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "New Projectile", order = 1)]
public class ProjectileData : ScriptableObject {

    [Header("Projectile Velocity")]
    public float startSpeed;
    public float minSpeed;
    public float maxSpeed;
    public float accelleration;

    [Header("Collision Settings")]
    public LayerMask collisionLayers;

    [Header("Ricochet Parameters")]
    public float bounceAngle;
    public float bounceSpeedMultiplier;

    [Header("Destruction Conditions")]
    public float destroySpeedThreshold;
    public float lifeTime;

}
