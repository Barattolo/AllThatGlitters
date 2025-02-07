using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    //REF
    PlayerStats stats;
    PlayerInputs inputs;
    FieldOfView fieldOfView;
    AudioManager audioM;

    //FieldOfView
    [Header("Settings")]

    float baseRadius;
    [SerializeField] float strongRadius = 9f;
    [SerializeField] Material fovMaterial;
    
    //Torch
    [SerializeField] Light torch; //per ora va draggata
    float baseRange;
    [SerializeField] float strongRange;
    float intensityBase;
    [SerializeField] float intensityStrong;

    //Torch behaviour
    [Header("Torch behaviour")]

    bool isActive;
    [SerializeField] int lightActivationPrice = 1;
    [SerializeField] int lightUsagePrice = 1;
    [SerializeField] int lightUsageDelay = 1;
    [SerializeField] int maxLoopDiConsumo = 5; //da rinominare
    [SerializeField] int torchDuration; // so che sar� n� di loop per secondi di attesa maxLoopDiConsumo*lightUsageDelay. vorrei far editare questo e non maxLoopDiConsumo
    bool enoughEnergy;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        inputs = GetComponent<PlayerInputs>();
        fieldOfView = GetComponent<FieldOfView>();
        audioM = FindObjectOfType<AudioManager>(); //uSO IL FINd <_<
        //assegno i valori attuali
        PopulateBaseValues();
    }

    private void PopulateBaseValues()
    {
        baseRadius = fieldOfView.viewRadius;
        baseRange = torch.range;
        intensityBase = torch.intensity;
    }

    private void Update()
    {     
        LightActivation();
    }


    void LightActivation()
    {
        if (inputs.inputQ)
        {
            EnergySafety();

            if (!enoughEnergy)
            {
                Debug.Log("Not enough energy");
                //AUDIO
                audioM.PlaySound("TorchActivationFailed");
                return;
            }
            if (!isActive && enoughEnergy)
            {
                StrongLight();
            }
            else if(isActive) //se � accesa
            {
                StandardLight();
            }
        }
    }

    private void StrongLight()
    {
        fieldOfView.viewRadius = strongRadius;
        torch.range = strongRange;
        torch.intensity = intensityStrong;
        fovMaterial.SetFloat("_MaxDistance", strongRange);
        isActive = true;
        StartCoroutine("BatteryUsage"); //faccio partire il timer che consuma l'energia
        //AUDIO
        audioM.PlaySound("TorchActivation");
    }

    private void StandardLight()
    {
        fieldOfView.viewRadius = baseRadius;
        torch.range = baseRange;
        torch.intensity = intensityBase;
        fovMaterial.SetFloat("_MaxDistance", baseRange);
        isActive = false;
        StopCoroutine("BatteryUsage");
        //AUDIO
        audioM.PlaySound("TorchActivation");
    }

    IEnumerator BatteryUsage()
    {            
        stats.ConsumeEnergy(lightActivationPrice); //consumo all'attivazione

        for (int i = 0; i < maxLoopDiConsumo; i++)
        {
            stats.ConsumeEnergy(lightUsagePrice); // consumo nel tempo
            yield return new WaitForSeconds(lightUsageDelay); //ogni quanto consumo
        }

        StandardLight();
    }

    void EnergySafety()
    {
        int energyRequirement = lightActivationPrice + (lightUsagePrice * maxLoopDiConsumo) + 1; //il +1 � per non far morire il giocatore

        if (stats.currentEnergy > energyRequirement)//voglio che abbia abbastanza energia per una accensione lunga (loop della torcia)
        {
            enoughEnergy = true;
        }
        else
        {
            enoughEnergy = false;
        }
    }


   

}
