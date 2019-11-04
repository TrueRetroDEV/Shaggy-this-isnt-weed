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
    [Range(0, 90)] public float bounceAngle;
    [Range(0, 1)] public float bounceSpeedMultiplier;

    [Header("Destruction Conditions")]
    public float destroySpeedThreshold;
    public float lifeTime;

    [Header("Sub-Objects")]
    public GameObject trail;
    public float trailLifeTime;
    public GameObject debris;
    public float debrisLifeTime;
}
