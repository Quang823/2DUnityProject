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
            DontDestroyOnLoad(gameObject); // Gi? Player qua c�c scene
        }
        else
        {
            Destroy(gameObject); // Tr�nh t?o th�m Player
        }
    }
}
