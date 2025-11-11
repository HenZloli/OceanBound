using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LayerCollisionSetup : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Setup 2D Collision Matrix")]
    public static void SetupMatrix()
    {
        // Lấy index của các layer
        int player = LayerMask.NameToLayer("Player");
        int ground = LayerMask.NameToLayer("Ground");
        int obstacle = LayerMask.NameToLayer("Obstacle");

        // Reset: cho phép tất cả va chạm
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                Physics2D.IgnoreLayerCollision(i, j, false);
            }
        }

        // Ground -> không va chạm với ai
        for (int i = 0; i < 32; i++)
        {
            Physics2D.IgnoreLayerCollision(ground, i, true);
            Physics2D.IgnoreLayerCollision(i, ground, true);
        }

        // Player -> không va chạm với Ground
        Physics2D.IgnoreLayerCollision(player, ground, true);
        Physics2D.IgnoreLayerCollision(ground, player, true);

        // Player -> có va chạm với Obstacle
        Physics2D.IgnoreLayerCollision(player, obstacle, false);
        Physics2D.IgnoreLayerCollision(obstacle, player, false);

        Debug.Log("✅ Collision Matrix đã setup xong: Player chỉ va chạm với Obstacle!");
    }
#endif
}
