using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private GameObject finishButton;
    [SerializeField] private int distanceToFinish = 2;

    [SerializeField] private Player player;
    private EnemyBehaviour _enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        _enemy = FindObjectOfType<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        ActivateFinishButton();
    }

    private void ActivateFinishButton()
    {
        if (Vector3.Distance(player.transform.position, _enemy.transform.position) < distanceToFinish)
        {
            finishButton.SetActive(true);
            player.SetAttackTarget(_enemy.transform);
            player.canFinish = true;
            
        }
        else
        {
            finishButton.SetActive(false);
            player.canFinish = false;
            player.ResetPlayerModelRotation();
        }
    }
}
