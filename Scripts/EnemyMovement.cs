using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // sağa doğru ilerler
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // düşman duvara çarptığında yönünün değişmesi için
        moveSpeed = -moveSpeed;
        FlipEnemyFacing(); // düşmanın gittiği yöne doğru bakması için
    }

    void FlipEnemyFacing()
    {
        // Sign eğer sıfırsa veya sıfırdan büyükse 1 değerini döndürür.
        // Sign eğer sıfırdan küçükse -1 değerini döndürür.
        // her çarptığında yönü değişecek
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }
}
