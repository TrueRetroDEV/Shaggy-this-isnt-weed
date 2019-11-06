using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : BHealth {

    // PUBLIC REFERENCES;
    public ZombieData zombieData;

    // PUBLIC VARIABLES:
    public NavMeshAgent navMeshAgent;

    // PRIVATE VARIABLES:
    Vector3 target;

    float attackTimer = 0.0f;

    public void Setup(ZombieData zombie) {
        if ((navMeshAgent = GetComponent<NavMeshAgent>()) == null) {
            Destroy(gameObject);
        }

        zombieData = zombie;

        health = zombie.health;
        maxHealth = zombie.health;
        navMeshAgent.speed = zombie.speed;
        navMeshAgent.stoppingDistance = zombie.attackDistance;
    }

    public virtual void SetTarget(Vector3 newTarget, float randomNoise = 0.0f) {
        Vector2 noiseVector = Random.insideUnitCircle * randomNoise;

        newTarget.x += noiseVector.x;
        newTarget.z += noiseVector.y;

        target = newTarget;

        UpdateTarget();
    }
    void UpdateTarget() {
        navMeshAgent.destination = target;
    }

    void Update() {
        AIMove();
        AIAttack();
    }

    public virtual void AIAttack() {
        if ((Player.Instance.transform.position - transform.position).sqrMagnitude <= zombieData.attackDistance && attackTimer <= 0.0f) {
            if (zombieData.projectile) {

            }
            else {
                Player.Instance.TakeDamage(zombieData.damage);
            }
            attackTimer = 60 / zombieData.attackRate;
        }
        attackTimer -= Time.deltaTime;
    }

    void AIMove() {
        SetTarget(Player.Instance.transform.position);
    }

}
