using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform posA;
    [SerializeField] private Transform posB;
    private Vector3 target;
    private float speed = 2f;
    void Start()
    {
        target = posA.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,target,speed*Time.deltaTime);
        if (Vector3.Distance(transform.position, target) <= 0.1)
        {
            target = target == posA.position ? posB.position : posA.position;
        }
    }
}
