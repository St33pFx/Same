using UnityEngine;

public class ManagerVidaPiso : MonoBehaviour
{
    private int activeEdges = 0;
    private bool playerNearby = true;

    public float destroyDelay = 2f;

    public void PlayerEnteredEdge()
    {
        activeEdges++;
        playerNearby = true;
    }

    public void PlayerLeftEdge()
    {
        activeEdges = Mathf.Max(0, activeEdges - 1);
        if (activeEdges == 0)
        {
            playerNearby = false;
            Invoke(nameof(CheckAndDestroy), destroyDelay);
        }
    }

    void CheckAndDestroy()
    {
        if (!playerNearby)
        {
            Destroy(gameObject);
        }
    }
}

