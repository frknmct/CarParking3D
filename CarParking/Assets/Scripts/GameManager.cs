using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Car Settings")]
    public GameObject[] Cars;
    
    public int carCount;
    private int remainedCarCount;
    private int currentCarIndex = 0;

    [Header("Canvas Settings")]
    public Sprite CarCamePic;
    public TextMeshProUGUI remainedCarCountText;
    public GameObject[] CarCanvasPics;
    public GameObject[] Panels;
    public GameObject[] TapToButtons;
    public TextMeshProUGUI[] Texts;
    
    [Header("Platform Settings")]
    public GameObject platform1;
    public GameObject platform2;
    public float[] TurnSpeeds;
    bool turnExists;

    [Header("Level Settings")] 
    public int diamondCount;
    public ParticleSystem CrashEffect;
    public AudioSource[] Voices;
    public bool raisePlatform;
    bool touchLock;
    void Start()
    {
        touchLock = true;
        turnExists = true;
        CheckDefaultValues();
        remainedCarCount = carCount;
        //remainedCarCountText.text = remainedCarCount.ToString();
        for (int i = 0; i < carCount; i++)
        {
           CarCanvasPics[i].SetActive(true);
        }
        
    }
    public void BringNewCar()
    {
        
        remainedCarCount--;
        if (currentCarIndex < carCount)
        {
            Cars[currentCarIndex].SetActive(true);
        }
        else
        {
            Win();
        }
        
        CarCanvasPics[currentCarIndex - 1].GetComponent<Image>().sprite = CarCamePic;
        //remainedCarCountText.text = remainedCarCount.ToString();
    }
    void Update()
    {

        /*if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (touchLock)
                {
                    Panels[0].SetActive(false);
                    Panels[3].SetActive(true);
                    touchLock = false;
                }
                else
                {
                    Cars[currentCarIndex].GetComponent<Car>().move = true;
                    currentCarIndex++;
                }
            }
        }*/
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            Cars[currentCarIndex].GetComponent<Car>().move = true;
            currentCarIndex++;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Panels[0].SetActive(false);
            Panels[3].SetActive(true);
        }

        if (turnExists)
        {
            platform1.transform.Rotate(new Vector3(0,0, TurnSpeeds[0]),Space.Self);
            if (platform2 != null)
            {
                platform2.transform.Rotate(new Vector3(0,0, -TurnSpeeds[1]),Space.Self);
            }
        }

        
    }
    public void Lose()
    {
        turnExists = false;
        Texts[6].text = PlayerPrefs.GetInt("Diamond").ToString();
        Texts[7].text = SceneManager.GetActiveScene().name;
        Texts[8].text = (carCount - remainedCarCount).ToString();
        Texts[9].text = diamondCount.ToString();
        Voices[1].Play();
        Voices[3].Play();
        Panels[1].SetActive(true);
        Panels[3].SetActive(false);
        Invoke("LoseShowButton",2f);
    }
    void Win()
    {
        PlayerPrefs.SetInt("Diamond",PlayerPrefs.GetInt("Diamond")+diamondCount);
        
        Texts[2].text = PlayerPrefs.GetInt("Diamond").ToString();
        Texts[3].text = SceneManager.GetActiveScene().name;
        Texts[4].text = (carCount - remainedCarCount).ToString();
        Texts[5].text = diamondCount.ToString();
        Voices[2].Play();
        Panels[2].SetActive(true);
        Panels[3].SetActive(false);
        Invoke("WinShowButton",2f);
    }
    void LoseShowButton()
    {
        TapToButtons[0].SetActive(true);
    }
    void WinShowButton()
    {
        TapToButtons[1].SetActive(true);
    }
    
    // Disk Management
    void CheckDefaultValues()
    {
        Texts[0].text = PlayerPrefs.GetInt("Diamond").ToString();
        Texts[1].text = SceneManager.GetActiveScene().name;
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        PlayerPrefs.SetInt("Level",SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartGame()
    {
        Panels[0].SetActive(false);
        Time.timeScale = 1;
    }
}
