using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;

    Rigidbody2D myRigidbody;
    PlayerMovement player; // oyuncu hangi yöne bakıyor?

    float xSpeed;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // oyuncunun baktığı yöne doğru mermi hareket eder
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    // mermi düşmana çarptığında, düşman ölecek ve mermi yok olacak
    // mermi duvara çarptığında yok olacak
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject); // düşman yok edilir
        }
        Destroy(gameObject); // mermi yok edilir
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject); // mermi yok edilir
    }
}
