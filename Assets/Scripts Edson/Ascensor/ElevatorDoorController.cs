using UnityEngine;
using System.Collections;

public class ElevatorDoorController : MonoBehaviour
{
    [Header("Puertas del ascensor")]
    public Transform leftDoor;   // Puerta izquierda
    public Transform rightDoor;  // Puerta derecha

    [Header("Posiciones locales")]
    public Vector3 leftClosedPos;
    public Vector3 leftOpenPos;
    public Vector3 rightClosedPos;
    public Vector3 rightOpenPos;

    [Header("Configuración")]
    public float moveDuration = 1.5f; // Tiempo que tarda en abrir/cerrar
    public AnimationCurve movimientoSuave = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isOpen = false; // Estado actual de las puertas
    private Coroutine movimientoActual;

    // Este método será llamado por el ElevatorTrigger
    public void ActivarPuertas()
    {
        if (movimientoActual != null) StopCoroutine(movimientoActual);

        if (isOpen)
        {
            // Si están abiertas, cerrarlas
            movimientoActual = StartCoroutine(MoverPuertas(leftClosedPos, rightClosedPos));
        }
        else
        {
            // Si están cerradas, abrirlas
            movimientoActual = StartCoroutine(MoverPuertas(leftOpenPos, rightOpenPos));
        }

        isOpen = !isOpen; // Cambia el estado
    }

    IEnumerator MoverPuertas(Vector3 leftTarget, Vector3 rightTarget)
    {
        Vector3 startLeft = leftDoor.localPosition;
        Vector3 startRight = rightDoor.localPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float curva = movimientoSuave.Evaluate(t);

            leftDoor.localPosition = Vector3.Lerp(startLeft, leftTarget, curva);
            rightDoor.localPosition = Vector3.Lerp(startRight, rightTarget, curva);

            yield return null;
        }

        leftDoor.localPosition = leftTarget;
        rightDoor.localPosition = rightTarget;
        movimientoActual = null;
    }

    // 🔧 Atajos para configurar desde el editor
    [ContextMenu("Guardar posición actual como Abierta")]
    void GuardarAbierta()
    {
        if (leftDoor) leftOpenPos = leftDoor.localPosition;
        if (rightDoor) rightOpenPos = rightDoor.localPosition;
    }

    [ContextMenu("Guardar posición actual como Cerrada")]
    void GuardarCerrada()
    {
        if (leftDoor) leftClosedPos = leftDoor.localPosition;
        if (rightDoor) rightClosedPos = rightDoor.localPosition;
    }
}
