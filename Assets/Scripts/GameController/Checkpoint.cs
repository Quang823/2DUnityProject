using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameController gameController;

    private SpriteRenderer spriteRenderer;
    private bool isActivated = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ActivateCheckpoint()
    {
        if (!isActivated)
        {
            isActivated = true;
            spriteRenderer.color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateCheckpoint();
            GameController.instance.SetCheckpoint(transform.position); 
        }
    }

}
