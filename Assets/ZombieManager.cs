using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance;

    public ZombieData[] zombieTypes;
    public List<Zombie> zombies = new List<Zombie>();

    float spawnTimer = 0.0f;

    void Awake() {
        Instance = this;
    }

    public void LateUpdate() {
        if (spawnTimer <= 0.0f) {
            SpawnZombie(zombieTypes[Random.Range(0, zombieTypes.Length - 1)], Player.Instance.transform.position + RandomDirection() * 60.0f);

            spawnTimer = 10.0f;
        }

        spawnTimer -= Time.deltaTime;
    }

    Vector3 RandomDirection() {
        Vector2 dir = Random.insideUnitCircle.normalized;

        Vector3 output = new Vector3();
        output.x = dir.x;
        output.z = dir.y;

        return output;
    }

    void SpawnZombie(ZombieData zombieData, Vector3 position) {
        GameObject zombieObject = Instantiate(ZombieParameters.Instance.zombiePrefab, position, Quaternion.Euler(0, Random.Range(0, 360), 0));

        zombieObject.GetComponent<Zombie>().Setup(zombieData);
    }
}
