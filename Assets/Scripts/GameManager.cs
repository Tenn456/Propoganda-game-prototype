using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public int playerCoins = 0;  // Player's initial coin count.
    public int coinsPerClick = 1; // Normal coins per click.

    public GameObject knuckles;
    public bool hasKnuckles; // Whether the player has the Knuckles upgrade.
    public int knucklesCost; // Cost to buy Knuckles.
    public float knucklesCollectInterval; // Time interval for auto collection in seconds.
    public GameObject knucklesDesc;
    public int knucklesLevelUpCost;
    public int knucklesLevelCap = 10; // Knuckles Max Level.
    public int knucklesLevel = 1; // Knuckles Current Level.
    public TextMeshProUGUI knucklesButton;
    public TextMeshProUGUI knucklesCostText;
    public TextMeshProUGUI knucklesDescText;
    public Animator knucklesAnim;

    public GameObject tails;
    public bool hasTails; // Whether the player has the Tails upgrade.
    public int tailsCost; // Cost to buy Tails.
    public float tailsMultiplier = 2f; // Tail's multiplier value
    private bool isTailsActive = false; // Whether the multiplier is active.
    public float tailsIntervalMin; // Minimum time interval for multiplier activation.
    public float tailsIntervalMax; // Maximum time interval for multiplier activation.
    public float tailsDuration; // Duration for which the multiplier stays active
    public GameObject tailsDesc;
    public int tailsLevelUpCost;
    public int tailsLevelCap = 10; // Tails Max Level.
    public int tailsLevel = 1; // Tails Current Level.
    public TextMeshProUGUI tailsButton;
    public TextMeshProUGUI tailsCostText;
    public TextMeshProUGUI tailsDescText;
    public Animator tailsAnim;


    public GameObject eggman;
    public bool hasEggman; // Whether the player has the Eggman upgrade.
    public int eggmanCost; // Cost to buy Eggman.
    public float eggmanCollectInterval; // Time interval for auto collection in seconds.
    public float eggmanCollectAmount; //How much Eggman collects.
    public GameObject eggmanDesc;
    public int eggmanLevelUpCost;
    public int eggmanLevelCap = 10; // Eggman Max Level.
    public int eggmanLevel = 1; // Eggman Current Level.
    public TextMeshProUGUI eggmanButton;
    public TextMeshProUGUI eggmanCostText;
    public TextMeshProUGUI eggmanDescText;

    public Button mario;
    public Sprite mario_idle;
    public Sprite mario_hurt;

    public AudioSource sound;
    public AudioClip buy;
    public AudioClip poor;

    public AudioSource tailsSound;
    public AudioClip tada;

    public AudioSource metalSound;
    public AudioClip woosh;

    public bool metalMoving = false;

    public AudioSource marioHit;
    public AudioClip hit;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinDisplay();

        // Set Cost Text.
        knucklesCostText.text = "Knuckles - " + knucklesCost + " coins";
        tailsCostText.text = "Tails - " + tailsCost + " coins";
        eggmanCostText.text = "Eggman - " + eggmanCost + " coins";
    }

    private void Update()
    {
        knucklesDescText.text = "Knuckles punches Mario once every " + knucklesCollectInterval + " seconds.";
        tailsDescText.text = "Tails deploys a gizmo that gives a x" + tailsMultiplier + " multiplier to all hits.";
        eggmanDescText.text = "Eggman sends Metal Sonic to hit Mario for " + eggmanCollectAmount + " coins every 15 seconds.";
    }

    public void PlayHit()
    {
        marioHit.pitch = UnityEngine.Random.Range(0.8f, 1.2f); // Random pitch between 0.8 and 1.2.
        marioHit.PlayOneShot(hit, 0.5f);
    }

    public void PlayBuy()
    {
        sound.Stop(); // Stops the current audio clip.
        sound.PlayOneShot(buy, 0.5f);
    }

    public void PlayPoor()
    {
        sound.Stop(); // Stops the current audio clip.
        sound.PlayOneShot(poor, 0.5f);
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
            knucklesAnim.SetBool("punching", true);
            marioHit.PlayOneShot(hit, 0.5f);
            AddCoin();  // Increase the player's coin count by 1
            mario.image.sprite = mario_hurt;

            yield return new WaitForSeconds(0.2f);  // Wait for the specified time interval.
            knucklesAnim.SetBool("punching", false);
            mario.image.sprite = mario_idle;
        }
    }

    // Function to enable the Knuckles upgrade and Level Up.
    public void UnlockKnuckles()
    {
        //Level Up Knuckles
        if (hasKnuckles && playerCoins >= knucklesLevelUpCost && knucklesLevel <= knucklesLevelCap)
        {
            playerCoins -= knucklesLevelUpCost;
            PlayBuy();
            UpdateCoinDisplay();
            LevelUpKnuckles();
        }
        else if (hasKnuckles && playerCoins < knucklesLevelUpCost && knucklesLevel < knucklesLevelCap)
        {
            PlayPoor();
            Debug.Log("Not enough coins!");
        }
        else if (knucklesLevel == knucklesLevelCap)
        {
            PlayPoor();
            Debug.Log("Knuckles is at Max Level!");
        }

        if (!hasKnuckles && playerCoins >= knucklesCost) // If the player doesn't have it already.
        {
            playerCoins -= knucklesCost;
            knuckles.SetActive(true);
            UpdateCoinDisplay();
            hasKnuckles = true;  // Enable the upgrade.
            StartCoroutine(Knuckles());  // Start auto-collecting coins.
            PlayBuy();
            Debug.Log("Knuckles Unlocked!");
            knucklesCostText.text = "Knuckles - " + knucklesLevelUpCost + " coins"; // Updates Knuckle's Price Display.
            knucklesButton.text = "Upgrade"; // Changes Call text on Button to Upgrade.
        }
        else if (!hasKnuckles && playerCoins < knucklesCost)
        {
            PlayPoor();
            Debug.Log("Not enough coins!");
        }
    }

    public void LevelUpKnuckles()
    {
        knucklesLevel += 1; // Adds one to Knuckle's Level.
        knucklesCollectInterval -= 1; // Knuckles collects one second faster.
        Debug.Log("Upgraded Knuckles to " + knucklesLevel);

        if (knucklesLevel == knucklesLevelCap)
        {
            knucklesButton.text = "Max Level"; // Changes Upgrade text on Button to Max Level.
        }
        else
        {
            knucklesLevelUpCost *= 2; // Doubles Knuckle's Level Up Cost.
            knucklesCostText.text = "Knuckles - " + knucklesLevelUpCost + " coins";
        }
    }

    // Function to activate multiplier.
    private IEnumerator Tails()
    {
        while (hasTails)
        {
            // Choose a random number within a range
            float randomInterval = UnityEngine.Random.Range(tailsIntervalMin, tailsIntervalMax);
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
        tailsAnim.SetBool("isTailsActive", true);
        tailsSound.PlayOneShot(tada, 0.5f);
        Debug.Log("Multiplier Activated!");
    }

    // Function to deactivate the multiplier.
    private void DeactivateTails()
    {
        isTailsActive = false;
        tailsAnim.SetBool("isTailsActive", false);
        Debug.Log("Multiplier Deactivated.");
    }

    // Function to unlock the multiplier feature via the menu button.
    public void UnlockTails()
    {
        //Level Up Tails
        if (hasTails && playerCoins >= tailsLevelUpCost &&tailsLevel <= tailsLevelCap)
        {
            playerCoins -= tailsLevelUpCost;
            PlayBuy();
            UpdateCoinDisplay();
            LevelUpTails();
        }
        else if (hasTails && playerCoins < tailsLevelUpCost && tailsLevel < tailsLevelCap)
        {
            PlayPoor();
            Debug.Log("Not enough coins!");
        }
        else if (tailsLevel == tailsLevelCap)
        {
            PlayPoor();
            Debug.Log("Tails is at Max Level!");
        }

        if (!hasTails && playerCoins >= tailsCost) // If the player doesn't have it already.
        {
            playerCoins -= tailsCost;
            tails.SetActive(true);
            UpdateCoinDisplay();
            hasTails = true;  // Enable the upgrade.
            StartCoroutine(Tails());  // Start auto-collecting coins.
            PlayBuy();
            Debug.Log("Tails Unlocked!");
            tailsCostText.text = "Tails - " + tailsLevelUpCost + " coins"; // Updates Tail's Price Display/
            tailsButton.text = "Upgrade"; // Changes Call text on Button to Upgrade.
        }
        else if (!hasTails && playerCoins <= tailsCost)
        {
            PlayPoor();
            Debug.Log("Not enough coins!");
        }
    }

    public void LevelUpTails()
    {
        tailsLevel += 1; // Adds one Tail's Level.
        tailsMultiplier += 1; // Increases Multiplier by 1.
        Debug.Log("Upgraded Tails to " + tailsLevel);

        if (tailsLevel == tailsLevelCap)
        {
            tailsButton.text = "Max Level"; // Changes Upgrade text on Button to Max Level.
        }
        else
        {
            tailsLevelUpCost *= 2; // Doubles Eggman's Level Up Cost.
            tailsCostText.text = "Tails - " + tailsLevelUpCost + " coins";
        }
    }

    // Function to auto collect coins.
    private IEnumerator Eggman()
    {
        while (hasEggman)
        {
            yield return new WaitForSeconds(eggmanCollectInterval);  // Wait for the specified time interval.
            metalMoving = true;
            metalSound.PlayOneShot(woosh, 0.5f);

            yield return new WaitForSeconds(1);  // Wait for the specified time interval.
            for (int i = 0; i < eggmanCollectAmount; i++)
            {
                AddCoin();  // Increase the player's coin count by 1.
            }
            mario.image.sprite = mario_hurt;
            marioHit.PlayOneShot(hit, 0.5f);

            yield return new WaitForSeconds(0.2f);  // Wait for the specified time interval.
            mario.image.sprite = mario_idle;


        }
    }

    // Function to enable the Eggman upgrade.
    public void UnlockEggman()
    {
        //Level Up Eggman
        if (hasEggman && playerCoins >= eggmanLevelUpCost && eggmanLevel <= eggmanLevelCap)
        {
            playerCoins -= eggmanLevelUpCost;
            PlayBuy();
            UpdateCoinDisplay();
            LevelUpEggman();
        }
        else if (hasEggman && playerCoins < eggmanLevelUpCost && eggmanLevel < eggmanLevelCap)
        {
            PlayPoor();
            Debug.Log("Not enough coins!");
        }
        else if (eggmanLevel == eggmanLevelCap)
        {
            PlayPoor();
            Debug.Log("Eggman is at Max Level!");
        }

        if (!hasEggman && playerCoins >= eggmanCost) // If the player doesn't have it already.
        {
            playerCoins -= eggmanCost;
            eggman.SetActive(true);
            UpdateCoinDisplay();
            hasEggman = true;  // Enable the upgrade.
            StartCoroutine(Eggman());  // Start auto-collecting coins.
            PlayBuy();
            Debug.Log("Eggman Unlocked!");
            eggmanCostText.text = "Eggman - " + eggmanLevelUpCost + " coins"; // Updates Eggman's Price Display/
            eggmanButton.text = "Upgrade"; // Changes Call text on Button to Upgrade.
        }
        else if (!hasEggman && playerCoins <= eggmanCost)
        {
            PlayPoor();
            Debug.Log("Not enough coins!");
        }
    }

    public void LevelUpEggman()
    {
        eggmanLevel += 1; // Adds one Eggman's Level.
        eggmanCollectAmount += 10;

        Debug.Log("Upgraded Eggman to " + eggmanLevel);

        if (eggmanLevel == eggmanLevelCap)
        {
            eggmanButton.text = "Max Level"; // Changes Upgrade text on Button to Max Level.
        }
        else
        {
            eggmanLevelUpCost *= 2; // Doubles Eggman's Level Up Cost.
            eggmanCostText.text = "Eggman - " + eggmanLevelUpCost + " coins";
        }
    }
}
