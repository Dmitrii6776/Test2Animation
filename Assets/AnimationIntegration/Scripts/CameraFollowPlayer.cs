using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private int yOffSet;
    [SerializeField] private int zOffSet;

    private void CameraFollow()
    {
        var playerT = player.transform.position;
        transform.position = new Vector3(playerT.x, playerT.y + yOffSet, playerT.z - zOffSet);
        transform.LookAt(player.transform);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        CameraFollow();
    }
}
