
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class OyunKontrol : MonoBehaviour
{
    public int GoalSuccess;
    int InstantSuccess;
    int SelectNumber;
    //*******************
    // ilk valuation deðerini tutmak için
    GameObject SelectedButton;
    GameObject OrjinalButton;
    //*************
    public Sprite defaultSprite;
    public AudioSource[] voices;
    public GameObject[] Buttons;
    public TextMeshProUGUI Sayac;
    public GameObject[] OyunSonuPanelleri;
   // public Slider TimeSlider;

    //************* SAYAC

    public float TotalTime ;
    float Minutes;
    float Seconds;
    bool Timer;
    // float pastTime;   //slider için

    public GameObject Grid;
    public GameObject Havuz;
    bool CreateState;
    int CreateNumber;
    int TotalImage;

    // Start is called before the first frame update
    void Start()
    {
        SelectNumber = 0;
        Timer = true;
        
        CreateState = true;
        CreateNumber = 0;
        TotalImage = Havuz.transform.childCount;                        //havuzdaki toplam obje sayýsý
        //pastTime = 0;// SLÝDER için.
        //TimeSlider.value = pastTime;
        //TimeSlider.maxValue = TotalTime;
        /* Imageleri rastgele daðýtmak için*/

        StartCoroutine(CreatObject());
    }


    private void Update()
    {

        if (Timer && TotalTime > 1)                                         //&& pastTime!= TotalTime) for slider
        {
            TotalTime -= Time.deltaTime;

            Minutes = Mathf.FloorToInt(TotalTime / 60);                     //dakika hesabý 
            Seconds = Mathf.FloorToInt(TotalTime % 60);                     //Saniye hesaplama 

            // Sayac.text =Mathf.FloorToInt(TotalTime).ToString();
            Sayac.text = string.Format("{0:00}:{1:00}", Minutes, Seconds);                          // Stringin nasýl bir format þeklinde olmasý gerektiðini süslü parantezlerein arasýna yazýyoruz.
        }
        else
        {
            Timer = false;
            GameOver();
        }
        

    }
    //Imageleri random tayin etmek için
    IEnumerator CreatObject()
    {
        yield return new WaitForSeconds(.1f);
        while (CreateState)
        {
            int Rastgele = Random.Range(0, Havuz.transform.childCount - 1);                          //havuzdaki resim sayisi kadar

            //indis numarasý boþa düþmesin diye kontrol ediyoruz.
            if (Havuz.transform.GetChild(Rastgele).gameObject != null)
            {
                Havuz.transform.GetChild(Rastgele).transform.SetParent(Grid.transform);
                CreateNumber++;
                if (CreateNumber == TotalImage)                                                      // Grid icine tüm resimler geldi? Kontrolu yapiliyor.
                {
                    CreateState = false;
                    Destroy(Havuz.gameObject);
                }
            }

        }
    }

    public void GameStopped()
    {
        OyunSonuPanelleri[2].SetActive(true);
        Time.timeScale = 0;
    }
    public void GameContuine()
    {
        OyunSonuPanelleri[2].SetActive(false);                   // stop panelini kapatýyoruz.
        Time.timeScale = 1;

    }
    void GameOver()
    {
        OyunSonuPanelleri[0].SetActive(true);
    }

    void Win()
    {
        OyunSonuPanelleri[1].SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);               // Herseyin bulunmasiyla sonraki bolume gecis
        Time.timeScale = 1;
    }

    //************* Buton 
    public void MainLobby() => SceneManager.LoadScene("MainLobby");

    public void Again() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);                         //active olan sahneyi tekrar oynatmak için //buildIndex yerine .name ile de çalýþýr

    public void Next() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);                       // Sonraki bölüm icin


        Time.timeScale = 1; 
    
    }                

    //*********************
    public void GiveObject(GameObject obje)
    {
        // Burada Image'in içinde sprite oluþturduk ve ana resmi gizledik seçilmesi haline ana resim gözükecek.

        OrjinalButton = obje;

        OrjinalButton.GetComponent<Image>().sprite = OrjinalButton.GetComponentInChildren<SpriteRenderer>().sprite;

        OrjinalButton.GetComponent<Image>().raycastTarget = false;                                                  // seçilen Icon u týklamaz yaptýk.

        voices[0].Play();                                                                                           // her buton seçildiðinde gelen sesi ayarlamak için. + Play on awake seçeneðini kapattýk.

    }

    void StatusOfButtons(bool ButtonStatus)                                                                         // butonlari multiClick özelliðini kapatacaðýz + buttonStatus= durum
    {
        foreach (var item in Buttons)
        {

            if (item != null)
            {
                item.GetComponent<Image>().raycastTarget = ButtonStatus;                                            // butonlarýn Imagelerinin tiklanabilirlik özelliðini butonDurumundan gelen boolean  deðer ile ayarlayacaðýz. 
            }

        }

    }


    public void ButtonClick(int valuation)
    {
        Control(valuation);
    }

    void Control(int price)
    {
        if (SelectNumber == 0)
        {
            SelectNumber = price;
            SelectedButton = OrjinalButton;                                                                         // kullanýcýnýn seçtiði icon'um nosunu kaydettik. Hem de objenin image degerini tutuyoruz.
        }
        else
        {
            StartCoroutine(DoControl(price));
        }
    }
    IEnumerator DoControl(int price)
    {
        StatusOfButtons(false);                                                                                     // Kontrol etme aþamasýnda butonlarýn durumlarýný false yapýyoruz

        yield return new WaitForSeconds(1);

        if (SelectNumber == price)
        {

            InstantSuccess++;

            SelectedButton.GetComponent<Image>().enabled = false;
            SelectedButton.GetComponent<Button>().enabled = false;

            OrjinalButton.GetComponent<Image>().enabled = false;
            OrjinalButton.GetComponent<Button>().enabled = false;

            /* Destroy(SelectedButton.gameObject);
             Destroy(OrjinalButton.gameObject);*/

            SelectNumber = 0;
            SelectedButton = null;
            StatusOfButtons(true);

            if (GoalSuccess == InstantSuccess)
            {
                Win();
            }
        }
        else
        {
            voices[1].Play();

            //Eþleþme olmadiði için deðerleri default hale getiriyoruz.

            SelectedButton.GetComponent<Image>().sprite = defaultSprite;                                                         //ilk butonu seçip daha sonra default haline getiriyoruz.
            OrjinalButton.GetComponent<Image>().sprite = defaultSprite;                                                          //Seçilen butonu default olarak tanýmlamýþ oluyoruz.

            
            OrjinalButton.GetComponent<Image>().raycastTarget = true;                                                           // seçilen Iconlar uyuþmaz ise tekrar týklanabilir hale getirmeye yarar. ilk adým 34. satýrda
            SelectedButton.GetComponent<Image>().raycastTarget = true;

            // burada StatusOfButtons(true); iþlemi ile bu satýrlarýn görevini yapýyoruz. Kodlarda update

            SelectNumber = 0;
            SelectedButton = null;
            StatusOfButtons(true);
        }
    }
}
