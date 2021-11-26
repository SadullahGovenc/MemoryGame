using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameObject instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // objenin sahne aras�nda yok olmams�n� sa�l�yoruz.
        //if blo�unda korudu�umuz game objeler her oyun ba�lad���nda �o�al�yor bunu engellemek i�in destroy ediyorum.
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }
}
