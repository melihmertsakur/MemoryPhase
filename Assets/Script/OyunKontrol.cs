using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

 


public class OyunKontrol : MonoBehaviour
{
    // GENEL AYARLAR
    public int hedefbasari;
    int ilkSecimDegeri;
    int anlikbasari;
    //-----------------------
    GameObject secilenButon;
    GameObject butonunKendisi;
    //-----------------------
    public Sprite defaultSprite;
    public AudioSource[] sesler;
    public GameObject[] Butonlar;
    public TextMeshProUGUI Sayac;
    public GameObject[] OyunSonuPaneller;
    public Slider ZamanSlider;
    // SAYAÇ
    public float ToplamZaman;
    float dakika;
    float saniye;
    bool zamanlayici;
    float gecenzaman;
    //-----------------------  

    public GameObject Grid;
    public GameObject Havuz;
    bool olusturmadurumu;
    int OlusturmaSayisi;
    int ToplamElemanSayisi;

    // Track the current level
    private static int currentLevel = 1;

    void Start()
    {
        
        ilkSecimDegeri = 0;
        zamanlayici = true;
        gecenzaman = 0;
        olusturmadurumu = true;
        OlusturmaSayisi = 0;
        ToplamElemanSayisi = Havuz.transform.childCount;
        ZamanSlider.value = gecenzaman;
        ZamanSlider.maxValue = ToplamZaman;

        StartCoroutine(Olustur());
    }

    private void Update()
    {
        if (zamanlayici && gecenzaman != ToplamZaman)
        {
            gecenzaman += Time.deltaTime;
            ZamanSlider.value = gecenzaman;

            if (ZamanSlider.maxValue == ZamanSlider.value)
            {
                zamanlayici = false;
                GameOver();
            }
        }
    }

    IEnumerator Olustur()
    {
        yield return new WaitForSeconds(.1f);

        while (olusturmadurumu)
        {
            int rasgtelesayi = Random.Range(0, Havuz.transform.childCount - 1);

            if (Havuz.transform.GetChild(rasgtelesayi).gameObject != null)
            {
                Havuz.transform.GetChild(rasgtelesayi).transform.SetParent(Grid.transform);
                OlusturmaSayisi++;

                if (OlusturmaSayisi == ToplamElemanSayisi)
                {
                    olusturmadurumu = false;
                    Destroy(Havuz.gameObject);
                }
            }
        }
    }

    public void Oyunudurdur()
    {
        OyunSonuPaneller[2].SetActive(true);
        Time.timeScale = 0;
    }

    public void OyunaDevamEt()
    {
        OyunSonuPaneller[2].SetActive(false);
        Time.timeScale = 1;
    }

    void GameOver()
    {
        OyunSonuPaneller[0].SetActive(true);
    }

    void Win()
    {
        OyunSonuPaneller[1].SetActive(true);
        Invoke("LoadNextLevel", 5f);
    }

    void LoadNextLevel()
    {
        currentLevel++;
        if (currentLevel <= 4)
        {
            SceneManager.LoadScene("Level" + currentLevel);
        }
        else
        {
            // If there are no more levels, go back to the main menu or show a victory screen
            SceneManager.LoadScene("AnaMenu");
        }
    }

    public void AnaMenu()
    {
        SceneManager.LoadScene("AnaMenu");
    }

    public void TekrarOyna()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ObjeVer(GameObject objem)
    {
        butonunKendisi = objem;
        butonunKendisi.GetComponent<Image>().sprite = butonunKendisi.GetComponentInChildren<SpriteRenderer>().sprite;
        butonunKendisi.GetComponent<Image>().raycastTarget = false;
        sesler[2].Play();
    }

    void Butonlarindurumu(bool durum)
    {
        foreach (var item in Butonlar)
        {
            if (item != null)
            {
                item.GetComponent<Image>().raycastTarget = durum;
            }
        }
    }

    public void ButonTikladi(int deger)
    {
        Kontrol(deger);
    }

    void Kontrol(int gelendeger)
    {
        if (ilkSecimDegeri == 0)
        {
            ilkSecimDegeri = gelendeger;
            secilenButon = butonunKendisi;
        }
        else
        {
            StartCoroutine(kontroletbakalim(gelendeger));
        }
    }

    IEnumerator kontroletbakalim(int gelendeger)
    {
        Butonlarindurumu(false);
        yield return new WaitForSeconds(1);

        if (ilkSecimDegeri == gelendeger)
        {
            anlikbasari++;
            sesler[3].Play();
            secilenButon.GetComponent<Image>().enabled = false;
            butonunKendisi.GetComponent<Image>().enabled = false;
            ilkSecimDegeri = 0;
            secilenButon = null;
            Butonlarindurumu(true);

            if (hedefbasari == anlikbasari)
            {
                Win();
            }
        }
        else
        {
            sesler[1].Play();
            secilenButon.GetComponent<Image>().sprite = defaultSprite;
            butonunKendisi.GetComponent<Image>().sprite = defaultSprite;
            ilkSecimDegeri = 0;
            secilenButon = null;
            Butonlarindurumu(true);
        }
    }
}