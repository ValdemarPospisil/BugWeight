using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyCounter : MonoBehaviour//, IDataPersistence
{
    public int coins { get; private set; }
    private int coinsMax;

    public TMP_Text coinsValueText;
    [SerializeField] private GameObject CoinsMinus;

    [SerializeField] private TMP_Text coinsMinusValue;

    public static CurrencyCounter instance;

    private void Awake() 
    {
        if (instance != null)
            {
                Debug.LogError("Found more than one Input Manager in the scene.");
            }
        instance = this;
    }

    private void Start() 
    {
        coinsValueText.text = "" + coins.ToString();
    }
/*
    public void LoadData (GameData data) 
    {
        this.coins = data.coins;
    }

    public void SaveData (GameData data) {
        data.coins = this.coins;

    }
*/
    public void UpdateText () 
    {
        coinsValueText.text = "" + coins.ToString();
    }


    //Coins

    public IEnumerator AddCurrency(int currency)
    {
        
        coins = coins + currency;
        coinsValueText.text = "" + coins.ToString();
        yield return new WaitForSeconds(0.5f);
        
       
    }

    public void CutInhalf() 
    {
        CoinsMinus.SetActive(true);
        coinsMax = coins;
        coinsMax /= 2;
        coinsMinusValue.text ="- " + coinsMax.ToString();
        coins -= coinsMax;
        coinsValueText.text = coins.ToString();
    }

}
