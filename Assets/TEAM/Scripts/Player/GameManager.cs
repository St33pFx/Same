using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int keys = 0;

    private void Awake()
    {
        instance = this;
    }

    public void AgregrarLlave(int cantidad = 1)
    {
        keys += Mathf.Max(0, cantidad);

    }

    public bool IntentarUsarLlave(int cantidad = 1)
    {
        if (keys >= cantidad)
        {
            keys -= cantidad;
            return true;
        }
        return false;
    }

}
