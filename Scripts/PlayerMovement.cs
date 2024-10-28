using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbSpeed = 5f; // tırmanma hızı, y ekseni
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f); // oyuncu öldüğünde
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun; // merminin oluşturulacağı konum

    Vector2 moveInput;
    Rigidbody2D myRigidbody2d;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart; // başlangıçtaki yer çekimi değeri

    bool isAlive = true; // oyuncu yaşıyor mu? (eğer düşmanla çarpışırsa ölür)

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        // öldüyse hiçbir şey yapamayacak
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    // input system > input actions
    void OnFire(InputValue value)
    {
        // öldüyse hiçbir şey yapamayacak
        if (!isAlive) { return; }

        // Silahı ateşlemek için sol fare tuşu kullanılacak
        // Silah(Gun) nesnesinin konumunda mermi(bullet) oluşturulacak
        // mermi oyuncunun rotasyonuna sahip olsun
        Instantiate(bullet, gun.position, transform.rotation);
    }

    // input system > input actions
    void OnMove(InputValue value)
    {
        // öldüyse hiçbir şey yapamayacak
        if (!isAlive) { return; }

        moveInput = value.Get<Vector2>();
        // klavye yön tuşları
        // moveInput: (0, 1) ise up
        // moveInput: (0, -1) ise down
        // moveInput: (-1, 0) ise left
        // moveInput: (1, 0) ise right
        // Debug.Log(moveInput);
    }

    // input system > input actions
    void OnJump(InputValue value)
    {
        // öldüyse hiçbir şey yapamayacak
        if (!isAlive) { return; }

        // eğer zemine dokunmuyorsa zıplamasın
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        // eğer space düğmesine basıldıysa (ve zemine temas ediyorsa zıplar)
        if (value.isPressed)
        {
            myRigidbody2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        // sağa veya sola doğru ilerlemesi için
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody2d.velocity.y);
        myRigidbody2d.velocity = playerVelocity;

        // eğer hareket ediyorsa yani hızı sıfırdan büyükse
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2d.velocity.x) > Mathf.Epsilon;
        // eğer hareket ediyorsa Running durumuna geçilsin
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        // eğer hareket ediyorsa yani hızı sıfırdan büyükse
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2d.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) // bir yere doğru giderken durursa gittiği yere doğru dönük kalması için 
        {
            // Sign eğer sıfırsa veya sıfırdan büyükse 1 değerini döndürür bu da sağa doğru gittiğini gösterir.
            // Bu yüzden yüzünün sağa doğru dönük olması gerekir. eğer scale x 1 ise yüzü sağa dönük olacaktır.
            // Sign eğer sıfırdan küçükse -1 değerini döndürür bu da sola doğru gittiğini gösterir.
            // Bu yüzden yüzünün sola doğru dönük olması gerekir. eğer scale x -1 ise yüzü sola dönük olacaktır.
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2d.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        // eğer tırmanma katmanına (Climbing) dokunmuyorsa yukarıya veya aşağıya doğru gitmesin
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            // eğer merdivende değilse yer çekimi başlangıçtaki değerine eşitlenecek
            myRigidbody2d.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        // merdiven kullanarak aşağı veya yukarı doğru ilerlemesi için
        Vector2 climbVelocity = new Vector2(myRigidbody2d.velocity.x, moveInput.y * climbSpeed);
        myRigidbody2d.velocity = climbVelocity;
        // oyuncunun merdivende aşağı kaymasını engellemek için yer çekimini sıfıra eşitleyeceğim.
        myRigidbody2d.gravityScale = 0f;

        // eğer hareket ediyorsa yani hızı sıfırdan büyükse
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody2d.velocity.y) > Mathf.Epsilon;
        // eğer hareket ediyorsa Climbing durumuna geçilsin
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        // oyuncu düşmana dokundu mu?
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false; // oyuncu ölür
            myAnimator.SetTrigger("Dying");
            // oyuncuyu havaya fırlatacak 
            myRigidbody2d.velocity = deathKick;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
