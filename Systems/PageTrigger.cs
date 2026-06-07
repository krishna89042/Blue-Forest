using UnityEngine;

public class PageTrigger : MonoBehaviour
{
    public IntroManager introManager;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            introManager.ReachBlueForest();

            // Disable trigger permanently
            GetComponent<Collider2D>().enabled = false;
        }
    }
}