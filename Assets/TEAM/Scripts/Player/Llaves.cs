using Unity.VisualScripting;
using UnityEngine;

public class Llaves : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.AgregrarLlave();
            Destroy(this.gameObject);
        }
    }

}
