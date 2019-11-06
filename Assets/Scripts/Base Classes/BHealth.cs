﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHealth : MonoBehaviour
{

    public float health;
    public float maxHealth;

    public virtual void Start() {
        health = maxHealth;
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float amount) {
        health -= amount;

        if (health < 0) {
            health = 0;
        }

        if (health == 0) {
            Die();
        }
    }

    public void AddHealth(float amount) {
        health += amount;

        if (health > maxHealth) {
            health = maxHealth;
        }
    }

}
