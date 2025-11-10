using UnityEngine;

public class IgnorarColisionJugador : MonoBehaviour
{
    void Start()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            // Ignora solo las colisiones físicas, NO los triggers
            Collider colCaja = GetComponent<Collider>();
            Collider colJugador = jugador.GetComponent<Collider>();

            if (colCaja != null && colJugador != null)
            {
                Physics.IgnoreCollision(colCaja, colJugador, true);
            }
        }
    }
}
