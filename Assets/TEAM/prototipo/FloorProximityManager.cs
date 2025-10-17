using System.Collections.Generic;
using UnityEngine;

public class FloorProximityManager : MonoBehaviour
{
    [Tooltip("Retraso antes de destruir el prefab cuando el jugador se aleja completamente")]
    public float destroyDelay = 1.5f;

    private List<DetectorGenerador> detectors = new List<DetectorGenerador>();
    private bool playerNearby = false;
    private float lastSeenTime = 0f;

    // Se llama automáticamente desde el DetectorGenerador al instanciar el prefab
    public void RegisterDetectorsInChildren(GameObject root)
    {
        detectors.Clear();
        detectors.AddRange(root.GetComponentsInChildren<DetectorGenerador>());
    }

    private void Update()
    {
        CheckPlayerProximity();
    }

    void CheckPlayerProximity()
    {
        playerNearby = false;
        foreach (var detector in detectors)
        {
            if (detector == null) continue;

            // Revisa si el jugador está cerca usando OverlapBox
            Collider[] hits = Physics.OverlapBox(
                detector.transform.position,
                Vector3.one * 0.5f,
                Quaternion.identity
            );

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    playerNearby = true;
                    lastSeenTime = Time.time;
                    break;
                }
            }

            if (playerNearby) break;
        }

        if (!playerNearby && Time.time - lastSeenTime > destroyDelay)
        {
            Destroy(gameObject);
        }
    }
}
