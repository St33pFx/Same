using UnityEngine;
using UnityEngine.AI;

public class VisionEnemigo : MonoBehaviour
{
    [Header("Detección")]
    public float visionDistance = 10f;
    [Header("Dimensiones del FOV")]
    public float visionAncho = 4f;
    public float visionAlto = 3f;
    
    private float anguloVision = 60f;
    
    public string playerTag = "Player";

    [Header("Ataque")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 20f;
    public float tiempoEntreDisparos = 1.5f;

    private Transform jugador;
    private float proximoDisparo = 0f;
    private bool jugadorEnVision = false;

    [Header("Rotación aleatoria")]
    public float velocidadRotacion = 50f;
    private float objetivoRotacionY;
    private float tiempoParaCambiar = 0f;

    private NavMeshAgent agente;

    private void Start()
    {
        objetivoRotacionY = transform.eulerAngles.y;

        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        puntoDisparo = GameObject.FindGameObjectWithTag("Player").transform;
        agente = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        jugadorEnVision = EstaEnAngulo();

        if (jugadorEnVision)
        {
            Atacar();
            agente.SetDestination(jugador.position);
        }
        else
        {
            RotacionAleatoria();
        }
    }

    //disparo
    private void Atacar()
    {
        if (Time.time >= proximoDisparo)
        {
            proximoDisparo = Time.time + tiempoEntreDisparos;

            GameObject bala = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);

            Vector3 direccion = (jugador.position - puntoDisparo.position).normalized;
            bala.GetComponent<Rigidbody>().AddForce(direccion * fuerzaDisparo, ForceMode.Impulse);
        }
    }

    //rotacion en guardia
    private void RotacionAleatoria()
    {
        if (Time.time >= tiempoParaCambiar)
        {
            tiempoParaCambiar = Time.time + Random.Range(1f, 3f);
            objetivoRotacionY = Random.Range(0f, 360f);
        }

        Vector3 rotActual = transform.eulerAngles;
        float nuevaY = Mathf.MoveTowardsAngle(rotActual.y, objetivoRotacionY, velocidadRotacion * Time.deltaTime);
        transform.eulerAngles = new Vector3(rotActual.x, nuevaY, rotActual.z);
    }

    //vision
    private bool EstaEnAngulo()
    {
        Vector3 direccion = jugador.position - transform.position;

        if (direccion.magnitude > visionDistance)
            return false;

        float angulo = Vector3.Angle(transform.forward, direccion);

        return angulo < anguloVision / 2f;
    }

    //gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        float h = visionDistance;        
        float w = visionAncho / 2f;       
        float t = visionAlto / 2f;      

        Vector3 forwardPos = transform.position + transform.forward * h;

        Vector3 topLeft = forwardPos + transform.up * t - transform.right * w;
        Vector3 topRight = forwardPos + transform.up * t + transform.right * w;
        Vector3 bottomLeft = forwardPos - transform.up * t - transform.right * w;
        Vector3 bottomRight = forwardPos - transform.up * t + transform.right * w;

        Gizmos.DrawLine(transform.position, topLeft);
        Gizmos.DrawLine(transform.position, topRight);
        Gizmos.DrawLine(transform.position, bottomLeft);
        Gizmos.DrawLine(transform.position, bottomRight);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
