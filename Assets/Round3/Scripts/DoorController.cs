using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        anim.SetTrigger("activeDoor"); // Kích hoạt animation của cửa
    }
}
