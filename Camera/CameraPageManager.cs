using UnityEngine;
using System.Collections;

public class CameraPageManager : MonoBehaviour
{
    public Transform currentPage;
    public float transitionSpeed = 2f;

    private bool isMoving = false;

    // Other scripts can check if camera is moving
    public bool IsMoving
    {
        get { return isMoving; }
    }

    IEnumerator MoveCamera()
    {
        isMoving = true;

        Vector3 targetPosition = new Vector3(
            currentPage.position.x,
            currentPage.position.y,
            transform.position.z
        );

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                transitionSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPosition;

        GetComponent<CameraEffects>().UpdatePagePosition();

        isMoving = false;
    }

    public void MoveToPage(Transform newPage)
    {
        if (currentPage == newPage)
            return;

        currentPage = newPage;

        StartCoroutine(MoveCamera());
    }
}