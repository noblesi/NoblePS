using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleAttack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleMove();
        }
    }

    private void HandleAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                player.GetComponent<PlayerFSM>().AttackEnemy(hit.collider.gameObject);
            }
            else
            {
                player.GetComponent<PlayerFSM>().AttackEnemy(null);
            }
        }
        else
        {
            player.GetComponent<PlayerFSM>().AttackEnemy(null);
        }
    }

    private void HandleMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Map"))
            {
                player.GetComponent<PlayerFSM>().MoveTo(hit.point);
            }
        }
    }

    private void Update()
    {
        CheckClick();
    }
}
