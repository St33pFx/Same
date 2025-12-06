using UnityEngine;

public class SpawnerTestSimple : MonoBehaviour
{
    public GameObject prefab;

    [ContextMenu("PROBAR SPAWN")]
    void ProbarSpawn()
    {
        Debug.Log("✅ BOTÓN FUNCIONA");

        if (prefab == null)
        {
            Debug.LogError("❌ Prefab NO asignado");
            return;
        }

        Vector3 pos = transform.position + Vector3.up * 2;
        Instantiate(prefab, pos, Quaternion.identity);

        Debug.Log("✅ PrefAB INSTANCIADO");
    }
}
