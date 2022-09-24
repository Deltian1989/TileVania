using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 100;
    [SerializeField] AudioClip coinPickupSFX;

    void Start()
    {
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();

        if (player)
        {
            var gameSession = FindObjectOfType<GameSession>();

            gameSession.AddCoins(value);

            var mainCameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;

            AudioSource.PlayClipAtPoint(coinPickupSFX, mainCameraPosition.position);
            Destroy(gameObject);
        }
    }
}
