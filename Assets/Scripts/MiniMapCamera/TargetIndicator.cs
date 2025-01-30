using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    public Transform player;   // Nhân vật
    public Transform target;   // Mục tiêu (kẻ địch, điểm nhiệm vụ)
    public RectTransform indicator; // UI Image của mũi tên
    public RectTransform miniMapRect; // Kích thước Mini-Map

    void Update()
    {
        if (player == null || target == null || indicator == null) return;

        Vector2 playerPos = new Vector2(player.position.x, player.position.y);
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);
        Vector2 direction = (targetPos - playerPos).normalized;

        // Tính góc xoay của mũi tên
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        indicator.rotation = Quaternion.Euler(0, 0, angle);

        // Lấy kích thước Mini-Map
        float halfWidth = miniMapRect.rect.width / 2;
        float halfHeight = miniMapRect.rect.height / 2;

        // Đặt vị trí của mũi tên ở rìa Mini-Map
        float borderOffset = 10f; // Khoảng cách cách rìa một chút (tùy chỉnh nếu cần)
        Vector2 newPos = direction * Mathf.Min(halfWidth, halfHeight) * 0.9f; // Đặt sát rìa
        newPos = newPos.normalized * (Mathf.Min(halfWidth, halfHeight) - borderOffset);

        indicator.anchoredPosition = newPos;
    }
}
