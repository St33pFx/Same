using System.Collections.Generic;
using UnityEngine;

public class DetectorGenerador : MonoBehaviour
{
    [Header("Opciones de generación")]
    public List<GameObject> floorPrefabs;
    public Transform spawnOrigin;
    public bool hasSpawned = false;

    private GameObject spawnedFloor;

    public bool TrySpawnFloor()
    {
        if (hasSpawned) return false;
        if (floorPrefabs == null || floorPrefabs.Count == 0 || spawnOrigin == null) return false;

        SpawnRandomFloor();
        return true;
    }

    void SpawnRandomFloor()
    {
        int index = Random.Range(0, floorPrefabs.Count);
        GameObject prefab = floorPrefabs[index];

        spawnedFloor = Instantiate(prefab, spawnOrigin.position, spawnOrigin.rotation);
        hasSpawned = true;

        // Conecta el prefab con este detector
        FloorProximityManager proximity = spawnedFloor.GetComponent<FloorProximityManager>();
        if (proximity != null)
        {
            proximity.RegisterDetectorsInChildren(spawnedFloor);
        }

        Debug.Log($"[DetectorGenerador] Piso generado desde {name}");
    }

    public void ResetSpawnFlag() => hasSpawned = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = hasSpawned ? Color.red : Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);

        if (spawnOrigin != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, spawnOrigin.position);
            Gizmos.DrawWireSphere(spawnOrigin.position, 0.2f);
        }
    }
}

