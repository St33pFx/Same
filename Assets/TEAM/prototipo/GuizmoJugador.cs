using UnityEngine;

public class GuizmoJugador : MonoBehaviour
{
    [Header("Configuración del Gizmo")]
    [Tooltip("Radio del gizmo que define el área de influencia del jugador")]
    public float radioDeDeteccion = 10f;

    [Tooltip("Color del gizmo en el editor")]
    public Color gizmoColor = Color.green;

    public bool EstaEnRango(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        return distance <= radioDeDeteccion;
    }

    public static GuizmoJugador EncontrarJugador()
    {
        var go = GameObject.FindWithTag("Player");
        if (go == null) return null;
        return go.GetComponent<GuizmoJugador>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radioDeDeteccion);
    }
}
