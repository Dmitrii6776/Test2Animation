using System;
using System.Collections;
using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform targetBone;
    public bool canFinish;

    [SerializeField] private int movementSpeed;
    
    [SerializeField] private GameObject weaponSpawnPoint;
    [SerializeField] private GameObject defaultWeapon;
    [SerializeField] private GameObject swordWeapon;
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private float spineRotateLimit = 90;
    

    private Animator _animator;
    private static readonly int IsRunningRifle = Animator.StringToHash("isRunningRifle");
    private static readonly int IsRunningLeft = Animator.StringToHash("isRunningLeft");
    private static readonly int IsRunningRight = Animator.StringToHash("isRunningRight");
    private static readonly int IsRunningBack = Animator.StringToHash("isRunningBack");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");

    private bool isPlayerCanMoving = true;
    private float _angleX;
    






    public void Move(Vector3 direction)
    {
        if(!isPlayerCanMoving) return;
        // SetDirection(direction);
        SetDirectionAnimation(direction);
        transform.position += direction * movementSpeed * Time.deltaTime;
        
    }

    public void Rotate(Vector3 rot)
    {

        if(!isPlayerCanMoving) return;
        rot.z = 10;
        var angle = targetBone.eulerAngles;
        var boneRotationX = targetBone.rotation.x;
        var objectPos = Camera.main.WorldToScreenPoint(transform.position);
        rot.x -= objectPos.x;
        rot.y -= objectPos.y;

        angle.y = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
        angle.y = -angle.y;

        targetBone.rotation = Quaternion.Euler(angle);
        _angleX = UnityEditor.TransformUtils.GetInspectorRotation(targetBone).x;




    }

    public void Attack()
    {
        ChangeWeapon(swordWeapon);
        _animator.SetBool(IsAttack, true);
        isPlayerCanMoving = false;
        StartCoroutine(nameof(ChangeWeaponDelay), 2f);
        StartCoroutine(nameof(ResetAttackFlag), 1);

    }
    public void SetAttackTarget(Transform target)
    {
        transform.LookAt(target);

        var angle = transform.eulerAngles;
        angle.x = 0;
        angle.z = 0;
        transform.rotation = Quaternion.Euler(angle);
    }

    private void LateUpdate()
    {
        if (_angleX >= spineRotateLimit)
        {
            var playerRot = transform.eulerAngles.y - spineRotateLimit;
            transform.rotation = Quaternion.Euler(0, playerRot, 0);
        }

        if (_angleX <= - spineRotateLimit)
        {
            var playerRot = transform.eulerAngles.y + spineRotateLimit;
            transform.rotation = Quaternion.Euler(0, playerRot, 0);
        }
    }


    private void ChangeWeapon(GameObject weapon)
    {
        Destroy(currentWeapon);
        currentWeapon = Instantiate(weapon, weaponSpawnPoint.transform.position, weaponSpawnPoint.transform.rotation, weaponSpawnPoint.transform);
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();

    }

    // private void SetDirection(Vector3 direction)
    // {
    //     if (direction == Vector3.forward)
    //     {
    //         transform.rotation = Quaternion.Euler(0,0,0);
    //     }
    //     if (direction == Vector3.left)
    //     {
    //         transform.rotation = Quaternion.Euler(0,-90,0);
    //     }
    //     if (direction == Vector3.right)
    //     {
    //         transform.rotation = Quaternion.Euler(0,90,0);
    //     }
    //    
    // }

    private void SetDirectionAnimation(Vector3 direction)
    {
        if (direction == transform.forward)
        {
            _animator.SetBool(IsRunningRifle, true);
            _animator.SetBool(IsRunningLeft, false);
            _animator.SetBool(IsRunningRight, false);
            _animator.SetBool(IsRunningBack, false);
        }
        if (direction == -transform.right)
        {
            _animator.SetBool(IsRunningLeft, true);
            _animator.SetBool(IsRunningRifle, false);
            _animator.SetBool(IsRunningRight, false);
            _animator.SetBool(IsRunningBack, false);
        }
        if (direction == transform.right)
        {
            _animator.SetBool(IsRunningRight, true);
            _animator.SetBool(IsRunningRifle, false);
            _animator.SetBool(IsRunningLeft, false);
            _animator.SetBool(IsRunningBack, false);
        }
        if (direction == -transform.forward)
        {
            _animator.SetBool(IsRunningBack, true);
            _animator.SetBool(IsRunningRifle, false);
            _animator.SetBool(IsRunningLeft, false);
            _animator.SetBool(IsRunningRight, false);
            
        }
        if (direction == Vector3.zero)
        {
            _animator.SetBool("isRunningRifle", false);
            _animator.SetBool("isRunningLeft", false);
            _animator.SetBool("isRunningRight", false);
            _animator.SetBool("isRunningBack", false);
        }


    }

    private IEnumerator ChangeWeaponDelay(int time)
    {
        yield return new WaitForSeconds(time);
        ChangeWeapon(defaultWeapon);
        isPlayerCanMoving = true;
    }
    
    private IEnumerator ResetAttackFlag(int time)
    {
        yield return new WaitForSeconds(time);
        _animator.SetBool(IsAttack, false);
    }
    
    
}
