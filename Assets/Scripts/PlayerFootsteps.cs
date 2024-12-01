using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] grassFootsteps;
    [SerializeField] private AudioClip[] woodFootsteps;

    // The pitch and volume range for the footstep sounds
    [Range(0.1f, 0.5f)] public float pitchRange = 0.2f;
    [Range(0.1f, 0.5f)] public float volumeRange = 0.2f;

    CharacterController characterController;

    // The interval between each footstep sound
    private float stepInterval = 0.5f;
    // Timer to keep track of the time between each footstep sound
    private float stepTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        stepTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            stepInterval = 0.3f;
        }
        else
        {
            stepInterval = 0.5f;
        }

        if (stepTimer >= stepInterval && IsPlayerMoving())
        {
            PlayFootstep();
            stepTimer = 0;
        }
    }

    // Checks if the player is moving
    private bool IsPlayerMoving()
    {
        return characterController.isGrounded && characterController.velocity.magnitude > 2f;
    }

    // Plays a random footstep sound based on the surface the player is walking on
    private void PlayFootstep()
    {
        string surface = DetectSurface();

        AudioClip[] footsteps = surface == "Wood" ? woodFootsteps :
                                 surface == "Grass" ? grassFootsteps : null;

        if (footsteps != null)
        {
            audioSource.clip = footsteps[Random.Range(0, footsteps.Length)];
            audioSource.pitch = Random.Range(1f - pitchRange, 1f + pitchRange);
            audioSource.volume = Random.Range(1f - volumeRange, 1f + volumeRange);
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    // Detects the surface the player is walking on
    private string DetectSurface()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
        {
            return hit.collider.CompareTag("Wood") ? "Wood" :
                   hit.collider.CompareTag("Grass") ? "Grass" : "Default";
        }
        return "Default";
    }
}
