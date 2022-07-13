using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;
 

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
        foreach (var body in _rigidbodies)
        {
            body.gameObject.AddComponent<EnemyCollisionDetect>();
        }


    }

    public void OnCollisionDetected(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Sword")) return;
        foreach (var body in _rigidbodies)
        {
            body.velocity = Vector3.zero;
            
        }
        _animator.enabled = false;
        
        StartCoroutine(nameof(RemovePositionDelay));
    }

    private void RemoveToNewPosition()
    { if(_animator.enabled) return;
        SetKinematicState(false);
        _animator.enabled = true;
        var randomX = Random.Range(-10, 10);
        var randomZ = Random.Range(-10, 10);
        transform.position = new Vector3(randomX, 1, randomZ);
        


    }

    private void SetKinematicState(bool state)
    {
        
        

        foreach (var collider1 in _colliders)
        {
            collider1.isTrigger = state;
        }
    }

    private IEnumerator RemovePositionDelay()
    {
        yield return new WaitForSeconds(5);
        SetKinematicState(true);
        float time = 0;
        while (time < 1)
        {
           transform.position += Vector3.down * Time.deltaTime;
           time += Time.deltaTime;
           yield return null;
        }

        yield return new WaitForSeconds(time + 0.5f);
        RemoveToNewPosition();
    }
    
    
}
