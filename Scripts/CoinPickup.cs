using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoinPickup = 100;

    bool wasCollected = false; // bir tane coin toplandı mı?

    // eğer oyuncu coin e çarparsa coin yok edilir
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            // altın toplandığında puan kazanılacak ve ekrana yazdırılacak
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
            wasCollected = true; // coin'in 1 kez toplanmasını sağladım

            // kameranın konumunda oynatılacak çünkü kamerada audio listener vardır
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);

            gameObject.SetActive(false); // coin'in 1 kez toplanmasını sağladım

            Destroy(gameObject);
        }
    }
}