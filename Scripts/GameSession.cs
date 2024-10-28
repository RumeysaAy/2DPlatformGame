using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3; // toplam can sayısı
    [SerializeField] int score = 0; // toplam puan

    [SerializeField] TextMeshProUGUI livesText; // kalan can sayısı gösterilecek
    [SerializeField] TextMeshProUGUI scoreText; // toplam puan gösterilecek

    void Awake()
    {
        // kaç tane GameSession nesnesi var?
        // bu dizide kaç tane eleman var?
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        // GameSession sayısı birden fazlaysa
        if (numGameSessions > 1)
        {
            // bu dosyanın bulunduğu nesne (GameSession nesnesi) yok edilir
            Destroy(gameObject);
        }
        else
        {
            // bir sahne her yüklendiğinde bu nesneyi yok etme, orada kalmasını sağla
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString(); // UI can sayısı
        scoreText.text = score.ToString();
    }

    // PlayerMovement.cs dosyasından çağıracağım
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            // can azaltılır ve bulunduğu sahne en baştan başlar
            TakeLife();
        }
        else
        {
            // oyuncunun canı bittiyse
            ResetGameSession(); // ilk sahneden başlayacak
        }
    }

    // CoinPickup.cs dosyasında çağıracağım
    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    void TakeLife()
    {
        playerLives--; // bir can azaltılır
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex); // sahne en baştan başlatılır
        livesText.text = playerLives.ToString(); // UI can sayısı
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();

        // ilk sahne yüklenir
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}