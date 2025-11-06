using UnityEngine;

public class ControlDePuerta : MonoBehaviour
{
    [Header("Configuración de detección")]
    public Vector3 tamañoGizmo = new Vector3(5, 5, 5);
    public string tagCompuerta = "compuerta";
    public string tagJugador = "Player";

    private Animator[] compuertasCercanas = new Animator[2];
    private bool jugadorCerca = false;
    private GameObject jugador;

    void Update()
    {
        if (compuertasCercanas[0] == null || compuertasCercanas[1] == null)
        {
            BuscarCompuertasEnGizmo();
        }

        RevisarCompuertasFueraDeRango();

        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (compuertasCercanas[0] != null && compuertasCercanas[1] != null)
                CambiarEstadoPuertas();
            else
                Debug.LogWarning("[ControlDePuerta] no hay 2 compuertas dentro del rango");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorCerca = true;
            jugador = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorCerca = false;
            jugador = null;
        }
    }

    private void BuscarCompuertasEnGizmo()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, tamañoGizmo * 0.5f);
        GameObject[] compuertas = System.Array.FindAll(System.Array.ConvertAll(colliders, c => c.gameObject),
            go => go.CompareTag(tagCompuerta));

        if (compuertas.Length < 2)
        {
            Debug.LogWarning($"[ControlDePuerta] Se encontraron {compuertas.Length} compuerta(s) dentro del rango");
            compuertasCercanas[0] = compuertas.Length > 0 ? compuertas[0].GetComponent<Animator>() : null;
            compuertasCercanas[1] = null;

            if (compuertasCercanas[0] != null)
                compuertasCercanas[0].SetBool("abrir", false);
            return;
        }

        Animator[] masCercanas = new Animator[2];
        float[] distancias = { float.MaxValue, float.MaxValue };

        foreach (var obj in compuertas)
        {
            float distancia = Vector3.Distance(transform.position, obj.transform.position);
            if (distancia < distancias[0])
            {
                distancias[1] = distancias[0];
                masCercanas[1] = masCercanas[0];
                distancias[0] = distancia;
                masCercanas[0] = obj.GetComponent<Animator>();
            }
            else if (distancia < distancias[1])
            {
                distancias[1] = distancia;
                masCercanas[1] = obj.GetComponent<Animator>();
            }
        }

        compuertasCercanas = masCercanas;

        Debug.Log($"[ControlDePuerta] se asignaron dos compuertas dentro del gizmo: " +
                  $"{compuertasCercanas[0]?.name}, {compuertasCercanas[1]?.name}");

        foreach (Animator anim in compuertasCercanas)
        {
            if (anim != null)
            {
                anim.SetBool("abrir", false);
                anim.SetBool("cerrado", true);
            }
        }
    }

    private void CambiarEstadoPuertas()
    {
        foreach (Animator anim in compuertasCercanas)
        {
            if (anim == null) continue;

            bool abierta = anim.GetBool("abrir");
            anim.SetBool("abrir", !abierta);
            anim.SetBool("cerrado", abierta);

            Debug.Log($"[ControlDePuerta] Cambiando estado de {anim.name}: abrir = {!abierta}");
        }
    }

    private void RevisarCompuertasFueraDeRango()
    {
        for (int i = 0; i < compuertasCercanas.Length; i++)
        {
            Animator anim = compuertasCercanas[i];
            if (anim == null) continue;

            Vector3 localPos = anim.transform.position - transform.position;
            Vector3 half = tamañoGizmo * 0.5f;

            if (Mathf.Abs(localPos.x) > half.x || Mathf.Abs(localPos.y) > half.y || Mathf.Abs(localPos.z) > half.z)
            {
                Debug.LogWarning($"[ControlDePuerta] {anim.name} salió del rango");
                anim.SetBool("abrir", false);
                anim.SetBool("cerrado", true);
                compuertasCercanas[i] = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, tamañoGizmo);

        if (compuertasCercanas[0] != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, compuertasCercanas[0].transform.position);
        }
        if (compuertasCercanas[1] != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, compuertasCercanas[1].transform.position);
        }
    }
}
