using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 2f; // Tốc độ di chuyển vào map
    public Transform targetPosition; // Vị trí boss sẽ đến khi vào map
    private bool isAttacking = false;

    private void Start()
    {
        StartCoroutine(MoveIntoMap());
    }

    private IEnumerator MoveIntoMap()
    {
        // Nếu targetPosition chưa được gán, boss sẽ vào giữa màn hình theo mặc định
        Vector3 finalPosition = targetPosition != null ? targetPosition.position : new Vector3(0, transform.position.y, 0);

        while (Vector3.Distance(transform.position, finalPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Khi boss đã vào đúng vị trí, bắt đầu tấn công
        StartAttack();
    }

    public void StartAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Debug.Log("Boss bắt đầu tấn công!");
            // Gọi hàm tấn công hoặc AI của boss tại đây
        }
    }
}
