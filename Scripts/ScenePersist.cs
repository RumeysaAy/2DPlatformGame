using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        // kaç tane ScenePersist nesnesi var?
        // bu dizide kaç tane eleman var?
        int numScenePersists = FindObjectsOfType<ScenePersist>().Length;

        // ScenePersist sayısı birden fazlaysa
        if (numScenePersists > 1)
        {
            // bu dosyanın bulunduğu nesne (ScenePersist nesnesi) yok edilir
            Destroy(gameObject);
        }
        else
        {
            // bir sahne her yüklendiğinde bu nesneyi yok etme, orada kalmasını sağla
            DontDestroyOnLoad(gameObject);
        }
    }

    // GameSession.cs'de çağırdım
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
