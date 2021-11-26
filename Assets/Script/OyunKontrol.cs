
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
    // ilk valuation de�erini tutmak i�in
    GameObject SelectedButton;
    GameObject OrjinalButton;
    //*************
    public Sprite defaultSprite;
    public AudioSource[] voices;
    public GameObject[] Buttons;
    public TextMeshProUGUI Sayac;
    public GameObject[] OyunSonuPanelleri;
    public Slider TimeSlider;

    //************* SAYAC

    public float TotalTime ;
    float Minutes;
    float Seconds;
    bool Timer;
    // float pastTime;   //slider i�in

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
        GoalSuccess = 18;
        CreateState = true;
        CreateNumber = 0;
        TotalImage = Havuz.transform.childCount; //havuzdaki toplam obje say�s�
        //pastTime = 0;// SL�DER i�in.
        //TimeSlider.value = pastTime;
        //TimeSlider.maxValue = TotalTime;
        /* Imageleri rastgele da��tmak i�in*/

        StartCoroutine(CreatObject());
    }


    private void Update()
    {

        if (Timer && TotalTime > 1)//&& pastTime!= TotalTime) slider i�in
        {
            TotalTime -= Time.deltaTime;

            Minutes = Mathf.FloorToInt(TotalTime / 60);   //dakika hesab� yap�yorum
            Seconds = Mathf.FloorToInt(TotalTime % 60);   //Saniye hesaplama 

            // Sayac.text =Mathf.FloorToInt(TotalTime).ToString();

            Sayac.text = string.Format("{0:00}:{1:00}", Minutes, Seconds);       // Stringin nas�l bir format �eklinde olmas� gerekti�ini s�sl� parantezlerein aras�na yaz�yoruz.

        }
        else
        {
            Timer = false;
            GameOver();
        }
        //SL�DER YAPMAK ���N

        //if (Timer && pastTime!= TotalTime)
        //{
        //    pastTime += Time.deltaTime;

        //    TimeSlider.value = pastTime;

        //    if (TimeSlider.maxValue == TimeSlider.value)
        //    {
        //        Timer = false;
        //        GameOver();
        //    }

        //}

    }
    //Imageleri random tayin etmek i�in
    IEnumerator CreatObject()
    {
        yield return new WaitForSeconds(.1f);
        while (CreateState)
        {
            int Rastgele = Random.Range(0, Havuz.transform.childCount - 1);

            //indis numaras� bo�a d��mesin diye kontrol ediyoruz.
            if (Havuz.transform.GetChild(Rastgele).gameObject != null)
            {
                Havuz.transform.GetChild(Rastgele).transform.SetParent(Grid.transform);
                CreateNumber++;
                if (CreateNumber == TotalImage)
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
        OyunSonuPanelleri[2].SetActive(false);  // stop panelini kapat�yoruz.
        Time.timeScale = 1;

    }
    void GameOver()
    {
        OyunSonuPanelleri[0].SetActive(true);
    }

    void Win()
    {
        OyunSonuPanelleri[1].SetActive(true);
    }

    //************* Buton 
    public void MainLobby()
    {

        SceneManager.LoadScene("MainLobby");

    }

    public void Again()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //active olan sahneyi tekrar oynatmak i�in //buildIndex yerine .name ile de �al���r
    }
    //*********************
    public void GiveObject(GameObject obje)
    {
        // Burada Image'in i�inde sprite olu�turduk ve ana resmi gizledik se�ilmesi haline ana resim g�z�kecek.

        OrjinalButton = obje;

        OrjinalButton.GetComponent<Image>().sprite = OrjinalButton.GetComponentInChildren<SpriteRenderer>().sprite;

        OrjinalButton.GetComponent<Image>().raycastTarget = false; // se�ilen Icon u t�klamaz yapt�k.

        voices[0].Play(); // her buton se�ildi�inde gelen sesi ayarlamak i�in. + Play on awake se�ene�ini kapatt�k.

    }

    void StatusOfButtons(bool ButtonStatus) // butonlar� mult�Click �zelli�ini kapataca��z + buttonStatus= durum
    {
        foreach (var item in Buttons)
        {

            if (item != null)
            {
                item.GetComponent<Image>().raycastTarget = ButtonStatus; // buttonlar�n Imagelerinin t�klanabilirlik �zelli�ini butonDurumundan gelen boolean  de�er ile ayarlayaca��z. 
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
            SelectedButton = OrjinalButton;// kullan�c�n�n se�ti�i icon'um nosunu kaydettik. Hem de objenin image degerini tutuyoruz.
        }
        else
        {
            StartCoroutine(DoControl(price));
        }
    }
    IEnumerator DoControl(int price)
    {
        StatusOfButtons(false); // Kontrol etme a�amas�nda butonlar�n durumlar�n� false yap�yoruz

        

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
            //E�le�me olmadi�i i�in de�erleri default hale getiriyoruz.
            SelectedButton.GetComponent<Image>().sprite = defaultSprite; //ilk butonu se�ip daha sonra default haline getiriyoruz.
            OrjinalButton.GetComponent<Image>().sprite = defaultSprite; //Se�ilen butonu default olarak tan�mlam�� oluyoruz.

            /*
            OrjinalButton.GetComponent<Image>().raycastTarget = true; // se�ilen Iconlar uyu�maz ise tekrar t�klanabilir hale getirmeye yarar. ilk ad�m 34. sat�rda
            SelectedButton.GetComponent<Image>().raycastTarget = true;
             */ // burada StatusOfButtons(true); i�lemi ile bu sat�rlar�n g�revini yap�yoruz. Kodlarda update


            SelectNumber = 0;
            SelectedButton = null;
            StatusOfButtons(true);
        }
    }
}
