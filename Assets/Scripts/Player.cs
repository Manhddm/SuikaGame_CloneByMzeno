using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform maxX;
    public Transform minX;

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float targetX =Mathf.Clamp(mousePosition.x, minX.position.x, maxX.position.x);
        Vector3 newPos = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime*moveSpeed);
    }
}
