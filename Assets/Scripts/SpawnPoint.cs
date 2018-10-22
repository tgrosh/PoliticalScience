using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetClosestEnemyDistance()
    {
        EnemyController[] enemyControllers = FindObjectsOfType<EnemyController>();
        float closestEnemyDistance = -1f;

        //find each enemy
        foreach (EnemyController enemyController in enemyControllers)
        {
            //get distance to enemy
            float distance = Vector2.Distance(transform.position, enemyController.transform.position);

            //if not set, or this distance is smaller
            if (closestEnemyDistance == -1f || distance < closestEnemyDistance)
            {
                closestEnemyDistance = distance;
            }
        }

        return closestEnemyDistance;
    }
}
