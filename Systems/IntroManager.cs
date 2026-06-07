using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public Transform player;
    public Rigidbody2D playerRB;

    public CameraPageManager cameraPageManager;
    public Transform blueForestPage;

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    [Header("Monster")]
    public GameObject monster;

    [Header("UI")]
    public GameObject dialoguePanel;

    [Header("Blocker")]
    public GameObject returnBlocker;

    private bool introFinished = false;

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

   IEnumerator IntroSequence()
{
    // Disable controls
    //playerMovement.canMove = false;
    playerMovement.allowInput = false;

    // Hide monster initially
    monster.SetActive(false);

    // Small delay
   // yield return new WaitForSeconds(2f);

    // Slow walk
   // playerRB.linearVelocity = new Vector2(walkSpeed, playerRB.linearVelocity.y);
   playerMovement.autoMove = true;
   playerMovement.autoMoveSpeed = 0.4f;

    yield return new WaitForSeconds(3f);

    // Spawn monster
    monster.SetActive(true);

    // Faster running
    //playerRB.linearVelocity = new Vector2(runSpeed, playerRB.linearVelocity.y);
    playerMovement.autoMoveSpeed = 1f;

    yield return new WaitForSeconds(8f);

    // Stop player movement
    //playerRB.linearVelocity = Vector2.zero;
    playerMovement.autoMove = false;
   yield return new WaitForSeconds(2f);
    // Move camera to blue forest
    cameraPageManager.MoveToPage(blueForestPage);

   // yield return new WaitForSeconds(2f);

    // Monster disappears
    monster.SetActive(false);
    //yield return new WaitForSeconds(5f);

    // Enable return blocker
    returnBlocker.SetActive(true);


    // Show dialogue
    dialoguePanel.SetActive(true);
}

    // Called from UI button
    public void StartGameplay()
    {
        dialoguePanel.SetActive(false);

       // playerMovement.canMove = true;
       playerMovement.allowInput = true;


        introFinished = true;
    }
    public void ReachBlueForest()
{
    playerMovement.autoMove = false;

    cameraPageManager.MoveToPage(blueForestPage);

    monster.SetActive(false);

    returnBlocker.SetActive(true);

    dialoguePanel.SetActive(true);
}
}