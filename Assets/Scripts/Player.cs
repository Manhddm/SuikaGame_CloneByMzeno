using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform maxX;
    public Transform minX;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite sadSprite;

    private void Start()
    {
        GameManager.Instance.OnMergeEvent += HandleMergeEffect;
        GameManager.Instance.GameOverEvent += HandleGameOverFace;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMergeEvent -= HandleMergeEffect;
            GameManager.Instance.GameOverEvent -= HandleGameOverFace;
        }
    }

    private void HandleMergeEffect()
    {
        StopAllCoroutines();
        StartCoroutine(ShowHappyFace());

    }

    private void HandleGameOverFace()
    {
        StopAllCoroutines();
        if (spriteRenderer) spriteRenderer.sprite = sadSprite;
    }

    IEnumerator ShowHappyFace()
    {
        if (spriteRenderer != null) spriteRenderer.sprite = happySprite;
        yield return new WaitForSeconds(0.5f);
        
        if (spriteRenderer != null) spriteRenderer.sprite = normalSprite;
    }

    private void Update()
    {
        if (GameManager.Instance.gameOver) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float targetX =Mathf.Clamp(mousePosition.x, minX.position.x, maxX.position.x);
        Vector3 newPos = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime*moveSpeed);
    }
}
