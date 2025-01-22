using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
        void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Gi? Player qua các scene
        }
        else
        {
            Destroy(gameObject); // Tránh t?o thêm Player
        }
    }
}
