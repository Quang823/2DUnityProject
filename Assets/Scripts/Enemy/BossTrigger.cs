using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour
{
    public GameObject dangerEffect;
    public Transform boss;
    public float dangerDuration = 3f;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(StartBossSequence());

            PlayerMovement player = other.GetComponent<PlayerMovement>();
        }
    }

    private IEnumerator StartBossSequence()
    {
        BossPatrol bossPatrol = boss.GetComponent<BossPatrol>();
        if (bossPatrol != null)
        {
            bossPatrol.PausePatrol();
        }

        if (dangerEffect != null)
        {
            dangerEffect.SetActive(true);
            if (dangerEffect.TryGetComponent<Animator>(out var dangerAnimator))
            {
                dangerAnimator.SetTrigger("Flash");
            }
        }

        yield return new WaitForSeconds(dangerDuration);

        if (dangerEffect != null)
        {
            dangerEffect.SetActive(false);
        }

        Vector3 cameraStartPos = Camera.main.transform.position;

        if (Camera.main.TryGetComponent<FollowPlayer>(out var followPlayerScript))
        {
            followPlayerScript.enabled = false;
        }

        yield return StartCoroutine(MoveCameraToBoss(cameraStartPos));

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(MoveCameraBackToPlayer(cameraStartPos));

        if (followPlayerScript != null)
        {
            followPlayerScript.enabled = true;
        }
    }

    private IEnumerator MoveCameraToBoss(Vector3 cameraStartPos)
    {
        Vector3 cameraTargetPos = new Vector3(boss.position.x, boss.position.y, Camera.main.transform.position.z);
        float timeToMove = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < timeToMove)
        {
            Camera.main.transform.position = Vector3.Lerp(cameraStartPos, cameraTargetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = cameraTargetPos;
    }

    private IEnumerator MoveCameraBackToPlayer(Vector3 cameraStartPos)
    {
        float timeToMove = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < timeToMove)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraStartPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = cameraStartPos;
    }
}