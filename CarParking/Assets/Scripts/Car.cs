using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public bool move;
    public bool stopPointSituation = false;
    private float raiseValue;
    private bool raisePlatformTime;
    
    public GameObject[] Ruts;
    public Transform parent;
    public GameManager gameManager;
    public GameObject particlePoint;
    
    
    void Update()
    {
        if(!stopPointSituation)
            transform.Translate(7f * Time.deltaTime * transform.forward);
        if(move)
            transform.Translate(15f * Time.deltaTime * transform.forward);
        if (raisePlatformTime)
        {
            if (raiseValue > gameManager.platform1.transform.position.y)
            {
                gameManager.platform1.transform.position = Vector3.Lerp(gameManager.platform1.transform.position,new Vector3(gameManager.platform1.transform.position.x,
                    gameManager.platform1.transform.position.y + 1.3f,
                    gameManager.platform1.transform.position.z),.010f);
            }
            else
            {
                raisePlatformTime = false;
            }
        }
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Parking"))
        {
            CrashCar();
            transform.SetParent(parent);
            if (gameManager.raisePlatform)
            {
                raiseValue = gameManager.platform1.transform.position.y + 1.3f;
                raisePlatformTime = true;
            }
            gameManager.BringNewCar();
        }
        
        else if (collision.gameObject.CompareTag("Car"))
        {
            gameManager.CrashEffect.transform.position = particlePoint.transform.position;
            gameManager.CrashEffect.Play();
            CrashCar();
            gameManager.Lose();
            
        }
        
    }

    void CrashCar()
    {
        move = false;
        Ruts[0].SetActive(false);
        Ruts[1].SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StopPoint"))
        {
            stopPointSituation = true;
            
        }
        else if (other.CompareTag("Diamond"))
        {
            other.gameObject.SetActive(false);
            gameManager.diamondCount++;
            gameManager.Voices[0].Play();
        }
        else if (other.CompareTag("MiddleBelly"))
        {
            gameManager.CrashEffect.transform.position = particlePoint.transform.position;
            gameManager.CrashEffect.Play();
            CrashCar();
            gameManager.Lose();
        }
        else if (other.CompareTag("FrontParking"))
        {
            other.gameObject.GetComponent<FrontParking>().parking.SetActive(true);
        }
    }
}
