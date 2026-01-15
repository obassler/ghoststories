using UnityEngine;

public class PriestManager : MonoBehaviour
{
    public Transform[] priestSpots;
    public int requiredBanished = 3;
    public float respawnDelay = 1.5f;

    private int banishCount = 0;
    private PriestBanish priestBanish;

    void Start()
    {
        priestBanish = GetComponent<PriestBanish>();

        if (priestSpots == null || priestSpots.Length == 0)
        {
            Debug.LogError("PriestManager: No priest spots assigned!");
            return;
        }

        MoveToSpot(0);
    }

    public void PriestBanished()
    {
        banishCount++;

        if (banishCount >= requiredBanished)
            EndGame();
        else if (banishCount < priestSpots.Length)
            Invoke(nameof(SpawnNextPriest), respawnDelay);
    }

    void SpawnNextPriest()
    {
        MoveToSpot(banishCount);
    }

    void MoveToSpot(int index)
    {
        transform.position = priestSpots[index].position;
        transform.rotation = priestSpots[index].rotation;

        if (priestBanish != null)
            priestBanish.ResetForNewSpawn();

        gameObject.SetActive(true);
    }

    void EndGame()
    {
        gameObject.SetActive(false);
    }
}
