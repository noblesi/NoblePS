using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float heightOffset = 20f;

    private void LateUpdate()
    {
        Vector3 newPosition = playerTransform.position;
        newPosition.y += heightOffset;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
