using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //REF
    //UIMANAGER SENN� NON VA LA UI :)
    [SerializeField]
    Animator animator;

    //Energy
    //[SerializeField]
    public int maxEnergy = 10;
    private bool isDead;

    //[SerializeField]
    public float currentEnergy;


    public bool isFull { get; private set; }


    //FRIENDLY ROBOTS
    public int robotsSaved = 0;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentEnergy = maxEnergy;
        isFull = true;
        //UI

    }

    public void ConsumeEnergy(float energyConsumed)
    {
        float energyDiff = currentEnergy -= energyConsumed;
        isFull = false;
        //play sound

        if (energyDiff > 0)
        {
            currentEnergy = energyDiff;
        }
        else if(!isDead)
        {
            isDead = true;
            currentEnergy = 0;
            Debug.Log("Muerto");
            //ANIMATION morte
            animator.SetTrigger("Die");
            StartCoroutine(CallEndLevel());
        }
        //UI
    }

    private IEnumerator CallEndLevel()
    {
        GameManager.singleton.gameState = GameState.gameOver;
        yield return new WaitForSeconds(1.5f);
        GameManager.singleton.OnLevelFinish(false);
    }

    public void RefillEnergy(int energyRefill)
    {
        float energySum = currentEnergy += energyRefill;
        
        //play sound
        if (currentEnergy == maxEnergy)
        {
            currentEnergy = maxEnergy;
            Debug.Log("Full Energy");
            isFull = true;
            return;
        }
        if (energySum >= maxEnergy)
        {
            currentEnergy = maxEnergy;
            Debug.Log("Full Energy");
            isFull = true;
            return;
        }
        else 
        {
            currentEnergy = energySum;
            isFull = false;
            //UI
        }
    }


    float NormalizeEnergy(int maxEnergy, float currentEnergy) //mi serve per dare un valore sensato allo slider DA ZERO A 1
    {
        float max = maxEnergy;
        float current = (float)currentEnergy;
        float normalizedEnergy = Mathf.InverseLerp(0, max, current);
        return normalizedEnergy;
    }


    //void AddSavedRobot()
    //{
    //    robotsSaved++;
    //    //UI
    //    GameManager.singleton.CurrentRescuedRobots = robotsSaved;
    //}



    private void Update()
    {
        UIManager.singleton.UpdateEnergySlider(NormalizeEnergy(maxEnergy, currentEnergy));

        //ROBINE DI PROVA
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    ConsumeEnergy(1);
        //}

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    RefillEnergy(1);
        //}
    }
}
