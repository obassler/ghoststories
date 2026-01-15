using UnityEngine;
using UnityEngine.XR;

public class XRFootstepsIndividual : MonoBehaviour
{
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds; // Drag multiple footstep clips here

    [Header("Settings")]
    public float stepInterval = 0.5f; // Time between steps
    public float movementThreshold = 0.1f; // Minimum speed to play footsteps
    public float volumeVariation = 0.2f; // Randomize volume slightly
    public float pitchVariation = 0.1f; // Randomize pitch slightly

    private Vector3 lastPosition;
    private float stepTimer = 0f;

    void Start()
    {
        lastPosition = transform.position;

        if (footstepAudioSource != null)
        {
            footstepAudioSource.loop = false;
            footstepAudioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // Calculate movement speed
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        float speed = distanceMoved / Time.deltaTime;

        // Check if player is moving
        if (speed > movementThreshold)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; // Reset timer when not moving
        }

        lastPosition = transform.position;
    }

    void PlayFootstep()
    {
        if (footstepAudioSource == null || footstepSounds.Length == 0)
            return;

        // Pick random footstep sound
        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];

        // Randomize volume and pitch for variety
        footstepAudioSource.volume = Random.Range(0.8f - volumeVariation, 0.8f + volumeVariation);
        footstepAudioSource.pitch = Random.Range(1.0f - pitchVariation, 1.0f + pitchVariation);

        // Play the sound
        footstepAudioSource.PlayOneShot(clip);
    }
}