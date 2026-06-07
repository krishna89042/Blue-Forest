using UnityEngine;

public class MonsterIntroFollow : MonoBehaviour
{
    public Transform player;

    [Header("Follow Settings")]
    public float followSpeed = 4f;
    public float followDistance = 3f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // Target position behind player
        Vector3 targetPosition = new Vector3(
            player.position.x - followDistance,
            transform.position.y,
            transform.position.z
        );

        // Smooth follow
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

        // Always face player direction
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        // Play running animation
        animator.SetBool("IsRunning", true);
    }
}