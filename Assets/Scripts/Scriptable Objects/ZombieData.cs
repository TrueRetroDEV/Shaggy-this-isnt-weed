﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zombie", menuName = "New Zombie", order = 1)]
public class ZombieData : ScriptableObject {

    [Header("ID")]
    public new string name;

    [Header("Stats")]
    public float speed;
    public float damage;
    public float attackRate;
}
