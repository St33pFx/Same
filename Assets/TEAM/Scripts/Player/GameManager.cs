using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int keys = 0;

    private void Awake()
    {
        instance = this;
    }

    public void AgregrarLlave()
    {
        keys++;
        Debug.Log("Llaves: " + keys);
    }

}
