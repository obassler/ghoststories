using UnityEngine;
using UnityEngine.XR;
public class XRFootstepsIndividual : MonoBehaviour
{
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;

    [Header("Settings")]
    public float stepInterval = 0.5f;
    public float movementThreshold = 0.1f;
    public float volumeVariation = 0.2f;
    public float pitchVariation = 0.1f;

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
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        float speed = distanceMoved / Time.deltaTime;

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
            stepTimer = 0f;
        }

        lastPosition = transform.position;
    }

    void PlayFootstep()
    {
        if (footstepAudioSource == null || footstepSounds.Length == 0)
            return;

        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];

        footstepAudioSource.volume = Random.Range(0.8f - volumeVariation, 0.8f + volumeVariation);
        footstepAudioSource.pitch = Random.Range(1.0f - pitchVariation, 1.0f + pitchVariation);

        footstepAudioSource.PlayOneShot(clip);
    }
}
