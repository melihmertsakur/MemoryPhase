using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SesKontrol : MonoBehaviour
{
    public AudioSource[] sesler;
    private static GameObject instance;

    private void Awake()
    {
        // Singleton pattern ile sadece bir örnek olmasını sağlar
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject); // Sahne değişikliklerinde objenin yok olmamasını sağlar
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Sahne yüklendiğinde sesi kontrol etmek için olayı dinle
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Eğer AnaMenu sahnesine geçiliyorsa sesler[0]'ı çal
        if (scene.name == "AnaMenu" && !sesler[0].isPlaying)
        {
            sesler[0].Play();
        }
        // Eğer AnaMenu sahnesinden çıkılıyorsa sesler[0]'ı durdur
        else if (scene.name != "AnaMenu" && sesler[0].isPlaying)
        {
            sesler[0].Stop();
        }
    }

    private void OnDestroy()
    {
        // Olayı temizle
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}




