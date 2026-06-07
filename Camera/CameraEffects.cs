using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour
{
    [Header("Jump Effect")]
    public Transform player;
    public float jumpOffsetY = 0.3f;
    public float jumpSmoothness = 3f;

    [Header("Zoom")]
    public Camera cam;
    //public float defaultZoom = 5f;
    private float defaultZoom;
    private Vector3 originalPosition;
    private float currentYOffset;

    void Start()
    {
        originalPosition = transform.position;

        if (cam == null)
            cam = Camera.main;

        //cam.orthographicSize = defaultZoom;
        defaultZoom = cam.orthographicSize;
    }

    void LateUpdate()
    {
        HandleJumpEffect();
    }

    void HandleJumpEffect()
    {
        if (player == null) return;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb == null) return;

        // Slight upward camera movement while jumping
        if (rb.linearVelocity.y > 0.1f)
        {
            currentYOffset = Mathf.Lerp(
                currentYOffset,
                jumpOffsetY,
                jumpSmoothness * Time.deltaTime
            );
        }
        else
        {
            currentYOffset = Mathf.Lerp(
                currentYOffset,
                0f,
                jumpSmoothness * Time.deltaTime
            );
        }

        // Keep page position intact
        transform.position = new Vector3(
            transform.position.x,
            originalPosition.y + currentYOffset,
            transform.position.z
        );
    }

    // =========================
    // CAMERA SHAKE
    // =========================
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 startPos = transform.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(
                startPos.x + x,
                startPos.y + y,
                startPos.z
            );

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = startPos;
    }

    // =========================
    // ZOOM IN
    // =========================
    public IEnumerator ZoomIn(float targetZoom, float duration)
    {
        float startZoom = cam.orthographicSize;

        float timer = 0f;

        while (timer < duration)
        {
            cam.orthographicSize = Mathf.Lerp(
                startZoom,
                targetZoom,
                timer / duration
            );

            timer += Time.deltaTime;

            yield return null;
        }

        cam.orthographicSize = targetZoom;
    }

    // =========================
    // RESET ZOOM
    // =========================
    public IEnumerator ResetZoom(float duration)
    {
        float startZoom = cam.orthographicSize;

        float timer = 0f;

        while (timer < duration)
        {
            cam.orthographicSize = Mathf.Lerp(
                startZoom,
                defaultZoom,
                timer / duration
            );

            timer += Time.deltaTime;

            yield return null;
        }

        cam.orthographicSize = defaultZoom;
    }

    // =========================
    // UPDATE PAGE POSITION
    // =========================
    public void UpdatePagePosition()
    {
        originalPosition = transform.position;
    }
}