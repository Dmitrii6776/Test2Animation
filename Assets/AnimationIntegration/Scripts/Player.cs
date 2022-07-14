using System;
using System.Collections;
using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Parameters")]
    public bool canFinish;
    [SerializeField] private int movementSpeed;
    [Header("Weapon Components")]
    [SerializeField] private GameObject weaponSpawnPoint;
    [SerializeField] private GameObject defaultWeapon;
    [SerializeField] private GameObject swordWeapon;
    [SerializeField] private GameObject currentWeapon;
    [Header("Rotation elemets")]
    [SerializeField] private Transform targetBone;
    [SerializeField] private float spineRotateLimit = 90;
    [SerializeField] private Transform playerModel;
    

    private Animator _animator;
    private static readonly int IsRunningRifle = Animator.StringToHash("isRunningRifle");
    private static readonly int IsRunningLeft = Animator.StringToHash("isRunningLeft");
    private static readonly int IsRunningRight = Animator.StringToHash("isRunningRight");
    private static readonly int IsRunningBack = Animator.StringToHash("isRunningBack");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");

    private bool _isPlayerCanMove = true;
    private float _angleX;
    private Vector3 _currentPlayerDirection;




    

    public void Move(Vector3 direction)
    {
        if(!_isPlayerCanMove) return;
        var previousPos = transform.position;
        transform.position += direction * movementSpeed * Time.deltaTime;
        var currentPos = transform.position;
        _currentPlayerDirection = (currentPos - previousPos).normalized;
        SetDirectionAnimation(_currentPlayerDirection);

    }

    public void Rotate(Vector3 rot)
    {

        if(!_isPlayerCanMove) return;
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

    public void ResetPlayerModelRotation()
    {
        playerModel.rotation = transform.rotation;
    }

    public void Attack()
    {
        ChangeWeapon(swordWeapon);
        _animator.SetBool(IsAttack, true);
        _isPlayerCanMove = false;
        StartCoroutine(nameof(ChangeWeaponDelay), 2f);
        StartCoroutine(nameof(ResetAttackFlag), 1);

    }
    public void SetAttackTarget(Transform target)
    {
        var tempTransform = playerModel;
        tempTransform.LookAt(target);

        var angle = tempTransform.eulerAngles;
        angle.x = 0;
        angle.z = 0;
        playerModel.rotation = Quaternion.Euler(angle);
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
            _animator.SetBool(IsRunningRifle, false);
            _animator.SetBool(IsRunningLeft, false);
            _animator.SetBool(IsRunningRight, false);
            _animator.SetBool(IsRunningBack, false);
        }


    }

    private IEnumerator ChangeWeaponDelay(int time)
    {
        yield return new WaitForSeconds(time);
        ChangeWeapon(defaultWeapon);
        _isPlayerCanMove = true;
    }
    
    private IEnumerator ResetAttackFlag(int time)
    {
        yield return new WaitForSeconds(time);
        _animator.SetBool(IsAttack, false);
        ;
    }
    
    
}
