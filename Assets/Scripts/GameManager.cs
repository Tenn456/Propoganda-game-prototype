using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public int playerCoins = 0;  // Player's initial coin count.
    public int coinsPerClick = 1; // Normal coins per click.

    public GameObject knuckles;
    private bool hasKnuckles = false; // Whether the player has the Knuckles upgrade.
    public int knucklesCost; // Cost to buy Knuckles.
    public float knucklesCollectInterval; // Time interval for auto collection in seconds.
    public GameObject knucklesDesc;

    public GameObject tails;
    private bool hasTails; // Whether the player has the Tails upgrade.
    public int tailsCost; // Cost to buy Tails.
    private float tailsMultiplier = 2f; // Tail's multiplier value
    private bool isTailsActive = false; // Whether the multiplier is active.
    public float tailsIntervalMin; // Minimum time interval for multiplier activation.
    public float tailsIntervalMax; // Maximum time interval for multiplier activation.
    public float tailsDuration; // Duration for which the multiplier stays active
    public GameObject tailsDesc;

    public GameObject eggman;
    private bool hasEggman;
    public int eggmanCost;
    public float eggmanCollectInterval;
    public float eggmanCollectAmount;
    public GameObject eggmanDesc;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinDisplay();
    }

    public void AddCoin()
    {
        int coinsToAdd = coinsPerClick;  // Base coins per click.

        // If the multiplier is active, apply it.
        if (isTailsActive)
        {
            coinsToAdd *= (int)tailsMultiplier;  // Apply the multiplier.
        }

        playerCoins += coinsToAdd;  // Increase the player's coin count by the calculated amount.
        UpdateCoinDisplay();
    }

    private void UpdateCoinDisplay()
    {
        coinText.text = "Coins: " + playerCoins;  // Update the displayed coin count.
    }

    public void TailsDescriptionOn()
    {
        tailsDesc.SetActive(true);
    }

    public void TailsDescriptionOff()
    {
        tailsDesc.SetActive(false);
    }

    public void KnucklesDescriptionOn()
    {
        knucklesDesc.SetActive(true);
    }

    public void KnucklesDescriptionOff()
    {
        knucklesDesc.SetActive(false);
    }

    public void EggmanDescriptionOn()
    {
        eggmanDesc.SetActive(true);
    }

    public void EggmanDescriptionOff()
    {
        eggmanDesc.SetActive(false);
    }

    // Function to auto collect coins.
    private IEnumerator Knuckles()
    {
        while (hasKnuckles)
        {
            yield return new WaitForSeconds(knucklesCollectInterval);  // Wait for the specified time interval.
            AddCoin();  // Increase the player's coin count by 1.
        }
    }

    // Function to enable the Knuckles upgrade.
    public void UnlockKnuckles()
    {
        if (!hasKnuckles && playerCoins >= knucklesCost) // If the player doesn't have it already.
        {
            playerCoins -= knucklesCost;
            knuckles.SetActive(true);
            UpdateCoinDisplay();
            hasKnuckles = true;  // Enable the upgrade.
            StartCoroutine(Knuckles());  // Start auto-collecting coins.
            Debug.Log("Knuckles Unlocked!");
        }
        else if (!hasKnuckles && playerCoins <= knucklesCost)
        {
            Debug.Log("Not enough coins!");
        }
        else
        {
            Debug.Log("Knuckles is already here!");
        }
    }

    // Function to activate multiplier.
    private IEnumerator Tails()
    {
        while (hasTails)
        {
            // Choose a random number within a range
            float randomInterval = Random.Range(tailsIntervalMin, tailsIntervalMax);
            yield return new WaitForSeconds(randomInterval);  // Wait for the random interval.

            ActivateTails();
            yield return new WaitForSeconds(tailsDuration);  // Keep multiplier active for 5 seconds.
            DeactivateTails();
        }
    }

    // Function to activate the multiplier.
    private void ActivateTails()
    {
        isTailsActive = true;
        Debug.Log("Multiplier Activated!");
    }

    // Function to deactivate the multiplier.
    private void DeactivateTails()
    {
        isTailsActive = false;
        Debug.Log("Multiplier Deactivated.");
    }

    // Function to unlock the multiplier feature via the menu button.
    public void UnlockTails()
    {
        if (!hasTails && playerCoins >= tailsCost) // If the player doesn't have it already.
        {
            playerCoins -= tailsCost;
            tails.SetActive(true);
            UpdateCoinDisplay();
            hasTails = true;  // Enable the upgrade.
            StartCoroutine(Tails());  // Start auto-collecting coins.
            Debug.Log("Tails Unlocked!");
        }
        else if (!hasTails && playerCoins <= tailsCost)
        {
            Debug.Log("Not enough coins!");
        }
        else
        {
            Debug.Log("Tails is already here!");
        }
    }

    // Function to auto collect coins.
    private IEnumerator Eggman()
    {
        while (hasEggman)
        {
            yield return new WaitForSeconds(eggmanCollectInterval);  // Wait for the specified time interval.
            for (int i = 0; i < eggmanCollectAmount; i++)
            {
                AddCoin();  // Increase the player's coin count by 1.
            }
            
        }
    }

    // Function to enable the Eggman upgrade.
    public void UnlockEggman()
    {
        if (!hasEggman && playerCoins >= eggmanCost) // If the player doesn't have it already.
        {
            playerCoins -= eggmanCost;
            eggman.SetActive(true);
            UpdateCoinDisplay();
            hasEggman = true;  // Enable the upgrade.
            StartCoroutine(Eggman());  // Start auto-collecting coins.
            Debug.Log("Eggman Unlocked!");
        }
        else if (!hasEggman && playerCoins <= eggmanCost)
        {
            Debug.Log("Not enough coins!");
        }
        else
        {
            Debug.Log("Eggman is already here!");
        }
    }
}
