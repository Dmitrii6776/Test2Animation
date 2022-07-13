using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionDetect : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour;

    private void Awake()
    {
        _enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _enemyBehaviour.OnCollisionDetected(other);
    }
}
