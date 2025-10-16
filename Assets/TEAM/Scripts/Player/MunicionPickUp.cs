using UnityEngine;

public class MunicionPickUp : MonoBehaviour
{
    public int cantidad = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        var playerS = other.GetComponent<PlayerStats>();

        if(playerS == null)
        {
            return;
        }

        playerS.AgregarMunicion(cantidad);
        Destroy(gameObject);
    }

}
