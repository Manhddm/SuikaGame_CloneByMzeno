using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform maxX;
    public Transform minX;
    [SerializeField] private Spawner spawner; 
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite sadSprite;

    private void Awake() 
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if  (spawner == null)
        {
            spawner = GetComponent<Spawner>();
        }
    }

    private void OnEnable()
    {
        
        Fruit.OnFruitMerged += HandleMergeEffect;
        GameManager.Instance.GameOverEvent += HandleGameOverFace;
    }
    
    private void OnDisable()
    {
        Fruit.OnFruitMerged -= HandleMergeEffect;
        if (GameManager.Instance != null)
        {
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

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameOver) return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            spawner.DropCurrentFruit();
        }
    }
}
