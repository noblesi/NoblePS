using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerFSM player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSM>();
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
        player.AttackAnimation();

        player.PerformAttack();
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
