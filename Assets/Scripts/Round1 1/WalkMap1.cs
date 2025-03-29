using UnityEngine;

public class WalkMap1 : MonoBehaviour
{
    [SerializeField] private Transform posA;
    [SerializeField] private Transform posB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Animator animator;

    private bool isMoving = true; 
    private Vector3 target; 
    private bool isMovingRight = true;

    void Start()
    {
        if (posA == null || posB == null)
        {
            Debug.LogError("posA or posB is not assigned.");
            return;
        }

        target = posB.position; 
    }

    void Update()
    {
        if (posA == null || posB == null || !isMoving) return;

        MoveCharacter();
    }

    void MoveCharacter()
    {
        float newX = Mathf.MoveTowards(transform.position.x, target.x, speed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);


        if (Mathf.Abs(transform.position.x - target.x) <= 0.1f)
        {
            target = (target == posA.position) ? posB.position : posA.position;
            FlipDirection();
        }
    }

    void FlipDirection()
    {
        isMovingRight = (target.x > transform.position.x);

        Vector3 newScale = transform.localScale;
        newScale.x = isMovingRight ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;

        //if (animator != null)
        //{
        //    animator.SetBool("isMovingRight", isMovingRight);
        //}
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void StartMoving()
    {
        isMoving = true;
    }

}
