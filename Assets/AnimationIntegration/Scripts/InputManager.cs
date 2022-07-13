using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private int sensativity = 10;
    private Vector3 _turn;


    private void KeyboardInput()
    {
        if (Input.anyKey)
        {


            if (Input.GetKey(KeyCode.W))
            {
                player.Move(Vector3.forward);
            }

            if (Input.GetKey(KeyCode.A))
            {
                player.Move(Vector3.left);
            }

            if (Input.GetKey(KeyCode.D))
            {
                player.Move(Vector3.right);
            }

            if (Input.GetKey(KeyCode.S))
            {
                player.Move(-Vector3.forward);
            }

            if (!player.canFinish) return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.Attack();
            }
        }
        else
        {
            player.Move(Vector3.zero);
        }
    }

    private void MouseInput()
    {

        // _turn.x += Input.GetAxis("Mouse X");
        // _turn.y += Input.GetAxis("Mouse Y");
        // _turn = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _turn = Input.mousePosition;
        player.Rotate(_turn);
        
        
    }

    private void Update()
    {
        KeyboardInput();
        
        
    }

    private void LateUpdate()
    {
        MouseInput();
    }
}
