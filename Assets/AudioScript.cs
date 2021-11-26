using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameObject instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // objenin sahne arasýnda yok olmamsýný saðlýyoruz.
        //if bloðunda koruduðumuz game objeler her oyun baþladýðýnda çoðalýyor bunu engellemek için destroy ediyorum.
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }
}
