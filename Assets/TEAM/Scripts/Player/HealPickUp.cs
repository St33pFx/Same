using UnityEngine;

public class HealPickUp : MonoBehaviour
{
    public int vidaCuracion = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        var PlayerS = other.GetComponent<PlayerStats>();

        if (PlayerS == null)
        {
            return;
        }

        PlayerS.Curarse(vidaCuracion);
        Destroy(gameObject);


    }

}
