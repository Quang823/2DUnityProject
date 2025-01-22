using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Animator enemyAnimator;  // Đảm bảo rằng Animator của quái đã được gắn
    public Transform player;        // Nhân vật chính

    public float attackDistance = 5f;
    public float runDistance = 10f;
    public float flyHeight = 2f;

    private bool isAttacking = false;

    void Start()
    {
        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponent<Animator>();  // Nếu chưa gắn Animator, gắn từ component
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < attackDistance && !isAttacking)
        {
            // Quái bay đến gần nhân vật và tấn công
            StartCoroutine(FlyToPlayerAndAttack());
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer < runDistance)
        {
            // Quái chạy ra xa nhân vật
            RunAwayFromPlayer();
        }
        else
        {
            // Quái di chuyển qua lại hoặc đứng yên
            enemyAnimator.SetBool("isWalking", true);
            enemyAnimator.SetBool("isFlying", false);
            enemyAnimator.SetBool("isAttacking", false);
            enemyAnimator.SetBool("isIdle", false);
        }
    }

    private IEnumerator FlyToPlayerAndAttack()
    {
        // Bắt đầu animation bay
        enemyAnimator.SetBool("isFlying", true);
        enemyAnimator.SetBool("isWalking", false);

        // Bay đến vị trí gần nhân vật
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y + flyHeight, player.position.z);
        float flyTime = 2f;  // Thời gian bay
        float elapsedTime = 0f;

        while (elapsedTime < flyTime)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / flyTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hạ cánh
        transform.position = targetPosition;

        // Tấn công
        enemyAnimator.SetTrigger("attackTrigger");  // Kích hoạt animation tấn công
        yield return new WaitForSeconds(1f);  // Giả sử tấn công mất 1 giây

        // Sau khi tấn công, chạy ra xa
        StartCoroutine(RunAwayFromPlayerCoroutine());
    }

    private IEnumerator RunAwayFromPlayerCoroutine()
    {
        // Chờ một lúc trước khi chạy ra xa
        yield return new WaitForSeconds(5f);  // Quái sẽ ở lại trong 5 giây

        RunAwayFromPlayer();
    }

    private void RunAwayFromPlayer()
    {
        // Chạy xa khỏi nhân vật
        enemyAnimator.SetBool("isWalking", false);
        enemyAnimator.SetBool("isFlying", false);
        enemyAnimator.SetBool("isAttacking", false);
        enemyAnimator.SetBool("isIdle", false);

        enemyAnimator.SetBool("isRunning", true);

        // Tính hướng chạy ra xa
        Vector3 direction = (transform.position - player.position).normalized;
        transform.position += direction * Time.deltaTime * 5f;  // Tốc độ chạy

        // Sau khi chạy đủ xa, dừng và chuyển về trạng thái Idle
        if (Vector3.Distance(transform.position, player.position) > runDistance)
        {
            enemyAnimator.SetBool("isRunning", false);
            enemyAnimator.SetBool("isIdle", true);  // Quái sẽ đứng yên một lúc
        }
    }
}
