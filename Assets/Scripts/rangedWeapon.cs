using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public float speed;
    public GameObject attackCollider;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        speed = _spriteRenderer.flipX ? -speed : speed;
    }

    private void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0);
    }

    private void OpenCollider()
    {
        attackCollider.SetActive(true);
    }

    private void CloseCollider()
    {
        attackCollider.SetActive(false);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}