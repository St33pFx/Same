using UnityEngine;

public class ControlDePuerta : MonoBehaviour
{
    [Header("Configuración de detección")]
    public Vector3 tamañoGizmo = new Vector3(5, 5, 5);
    public string tagCompuerta = "compuerta";
    public string tagJugador = "Player";

    private Animator[] compuertasCercanas = new Animator[2];
    private ControlDePuerta otroControlEnRango = null;

    private bool jugadorCerca = false;
    private bool puertasAbiertas = false;

    void Update()
    {
        BuscarCompuertasEnGizmo();
        BuscarOtroControl();

        RevisarCompuertasFueraDeRango();

        if (!HayDeteccionMutua())
        {
            if (puertasAbiertas)
            {
                Debug.Log("[ControlDePuerta] Detección mutua perdida: cerrando puertas.");
            }
            CerrarPuertas();
            puertasAbiertas = false;
        }
        else
        {
            if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
            {
                // Alternar estado
                if (puertasAbiertas)
                {
                    CerrarPuertas();
                    puertasAbiertas = false;
                    Debug.Log("[ControlDePuerta] Jugador presionó E -> Cerrando puertas.");
                }
                else
                {
                    AbrirPuertas();
                    puertasAbiertas = true;
                    Debug.Log("[ControlDePuerta] Jugador presionó E -> Abriendo puertas.");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorCerca = false;
        }
    }
    private void BuscarOtroControl()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, tamañoGizmo * 0.5f);

        ControlDePuerta encontrado = null;

        foreach (var col in colliders)
        {
            if (col.gameObject == this.gameObject) continue;

            ControlDePuerta ctrl = col.GetComponent<ControlDePuerta>();
            if (ctrl != null)
            {
                encontrado = ctrl;
                break;
            }
        }

        if (encontrado != otroControlEnRango)
        {
            otroControlEnRango = encontrado;
            if (otroControlEnRango == null)
            {
                Debug.Log("[ControlDePuerta] No hay otro control en rango.");
            }
            else
            {
                Debug.Log($"[ControlDePuerta] Encontrado otro control: {otroControlEnRango.name}");
            }
        }
    }
    private bool HayDeteccionMutua()
    {
        if (otroControlEnRango == null) return false;
        return otroControlEnRango.otroControlEnRango == this;
    }

    private void BuscarCompuertasEnGizmo()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, tamañoGizmo * 0.5f);
        GameObject[] compuertas = System.Array.FindAll(
            System.Array.ConvertAll(colliders, c => c.gameObject),
            go => go.CompareTag(tagCompuerta));

        if (compuertas.Length < 2)
        {
            compuertasCercanas[0] = compuertas.Length > 0 ? compuertas[0].GetComponent<Animator>() : null;
            compuertasCercanas[1] = null;
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
    }

    private void AbrirPuertas()
    {
        foreach (Animator anim in compuertasCercanas)
        {
            if (anim == null) continue;
            anim.SetBool("abrir", true);
            anim.SetBool("cerrado", false);
        }
    }

    private void CerrarPuertas()
    {
        foreach (Animator anim in compuertasCercanas)
        {
            if (anim == null) continue;
            anim.SetBool("abrir", false);
            anim.SetBool("cerrado", true);
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

        if (otroControlEnRango != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, otroControlEnRango.transform.position);
        }
    }
}
