using UnityEngine;

namespace MeroStoreStudios.Parallax
{
    public class ParallaxEffect : MonoBehaviour
    {
        private Material mat;

        [Range(0f, 0.5f)]
        public float speed = 0.1f;

        public Transform player;

        private float lastPlayerX;

        void Start()
        {
            mat = GetComponent<Renderer>().material;

            if (player != null)
            {
                lastPlayerX = player.position.x;
            }
        }

        void Update()
        {
            if (player == null) return;

            // Calculate player movement
            float playerDelta = player.position.x - lastPlayerX;

            // Move texture only if player moves
            if (Mathf.Abs(playerDelta) > 0.001f)
            {
                Vector2 offset = mat.mainTextureOffset;

                offset.x += playerDelta * speed * Time.deltaTime;

                mat.mainTextureOffset = offset;
            }

            lastPlayerX = player.position.x;
        }
    }
}