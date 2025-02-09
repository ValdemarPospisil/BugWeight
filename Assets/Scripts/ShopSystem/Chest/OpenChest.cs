using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prodigy
{
    public class OpenChest : MonoBehaviour
    {
        private Animator anim;
    //    public GameObject[] chestDrop;
        [SerializeField] private GameObject[] chestCoins;
        private int rand;
        private int randomAmountOfCoins;
        [SerializeField] private int minAmountOfCoins;
        [SerializeField] private int maxAmountOfCoins;
        private int randomItem;
        
        private void Start() {
            anim = GetComponent<Animator>();
        }
    
        private void OnTriggerEnter2D(Collider2D collision) {

            if(collision.transform.tag == "Player")
            {
                anim.SetTrigger("Open");
            }

        }

        private void AnimationOpenTrigger()
        {
            randomAmountOfCoins = Random.Range(minAmountOfCoins, maxAmountOfCoins);
           // randomItem = Random.Range(0, chestDrop.Length);

            for(int i = 0; i < randomAmountOfCoins; i++) {
                rand = Random.Range(0, chestCoins.Length);
                Instantiate(chestCoins[rand], transform.position, Quaternion.identity);
            }

        //    Instantiate(chestDrop[randomItem], transform.position, Quaternion.identity);

        }
    }
}
