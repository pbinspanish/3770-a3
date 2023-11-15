using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    #region Variables

    // GLOBAL
    public bool isSimRunning = true;
    public int level = 1;
    public int timeStep = 0;

    // BOSS
    public float bossHealth = 5000.0f;
    public float bossMinDamage = 5.0f;
    public float bossMaxDamage = 20.0f;
    public float bossMinTankDamage = 40.0f;
    public float bossMaxTankDamage = 50.0f;
    public float bossDamageDealt = 0.0f;

    // WARRIOR
    public float warriorHealth = 3000.0f;
    public float warriorMinDamage = 5.0f;
    public float warriorMaxDamage = 10.0f;
    public float warriorDamageDealt = 0.0f;

    // ROGUE
    public float rogueHealth = 1500.0f;
    public float rogueMinDamage = 15.0f;
    public float rogueMaxDamage = 25.0f;
    public float rogueDamageDealt = 0.0f;

    // MAGE
    public float mageHealth = 1000.0f;
    public float mageMinDamage = 5.0f;
    public float mageMaxDamage = 30.0f;
    public float mageDamageDealt = 0.0f;

    // MOONKIN DRUID
    public float moonkinDruidHealth = 1250.0f;
    public float moonkinDruidMinDamage = 5.0f;
    public float moonkinDruidMaxDamage = 15.0f;
    public float moonkinDruidDamageDealt = 0.0f;

    // PRIEST
    public float priestHealth = 900.0f;
    public float priestMana = 1000.0f;
    public float priestManaRegenRate = 3.0f;
    public float priestSmallHeal = 15.0f;
    public float priestSmallHealCost = 5.0f;
    public float priestBigHeal = 25.0f;
    public float priestBigHealCost = 10.0f;

    // UI VARIABLES
    public TextMeshProUGUI bossHealthValue;
    public TextMeshProUGUI warriorHealthValue;
    public TextMeshProUGUI rogueHealthValue;
    public TextMeshProUGUI mageHealthValue;
    public TextMeshProUGUI moonkinDruidHealthValue;
    public TextMeshProUGUI priestHealthValue;
    public TextMeshProUGUI bossDamageDealtValue;
    public TextMeshProUGUI warriorDamageDealtValue;
    public TextMeshProUGUI rogueDamageDealtValue;
    public TextMeshProUGUI mageDamageDealtValue;
    public TextMeshProUGUI moonkinDruidDamageDealtValue;

    public GameObject returnToMenuButton;

    #endregion

    void FixedUpdate()
    {
        // Don't update if the sim isn't running
        if (!isSimRunning) return;

        // Stop simulation if any characters are dead
        if (bossHealth <= 0.0f || warriorHealth <= 0.0f || rogueHealth <= 0.0f || mageHealth <= 0.0f || moonkinDruidHealth <= 0.0f || priestHealth <= 0.0f)
        {
            StopSimulation();
            return;
        }

        // Deal Damage
        float bossTankDamage = Random.Range(bossMinTankDamage, bossMaxTankDamage);
        float bossDamage = Random.Range(bossMinDamage, bossMaxDamage);
        float warriorDamage = Random.Range(warriorMinDamage, warriorMaxDamage);
        float rogueDamage = Random.Range(rogueMinDamage, rogueMaxDamage);
        float mageDamage = Random.Range(mageMinDamage, mageMaxDamage);
        float moonkinDruidDamage = Random.Range(moonkinDruidMinDamage, moonkinDruidMaxDamage);

        warriorHealth -= bossTankDamage;
        rogueHealth -= bossDamage;
        mageHealth -= bossDamage;
        moonkinDruidHealth -= bossDamage;
        priestHealth -= bossDamage;

        bossHealth -= warriorDamage;
        bossHealth -= rogueDamage;
        bossHealth -= mageDamage;
        bossHealth -= moonkinDruidDamage;

        bossDamageDealt += bossTankDamage + (bossDamage * 4);
        warriorDamageDealt += warriorDamage;
        rogueDamageDealt += rogueDamage;
        mageDamageDealt += mageDamage;
        moonkinDruidDamageDealt += moonkinDruidDamage;

        // punish the warrior on level 3
        if (level == 3)
        {
            warriorHealth -= (int)(bossDamageDealt / 100.0f);
        }

        // Healer Actions
        // Small Heal
        if (priestMana >= priestSmallHealCost)
        {
            CastSmallHeal();
            priestMana -= priestSmallHealCost;
        }

        // Big Heal
        if (priestMana >= priestBigHealCost)
        {
            CastBigHeal();
        }

        // cast an extra heal for free in level 2
        // if the warrior is dying
        if (level == 2)
        {
            if (warriorHealth <= 1500.0f)
            {
                int spell = Random.Range(0, 1);
                switch (spell)
                {
                    case 0:
                        CastSmallHeal();
                        break;
                    case 1:
                        CastBigHeal();
                        break;
                }
            }
        }

        // Regen Mana
        priestMana += priestManaRegenRate;

        // Export Current Timestep Info
        WriteToCSV();

        // Increment Timestep
        timeStep++;

        // Update UI
        UpdateUI();
    }

    private void CastBigHeal()
    {
        warriorHealth += priestBigHeal;
    }

    private void CastSmallHeal()
    {
        int target = Random.Range(0, 2); // results in 0, 1, or 2
        if (target == 0)
        {
            // heal a party member
            int partyTarget = Random.Range(0, 3);
            switch (partyTarget)
            {
                case 0:
                    warriorHealth += priestSmallHeal;
                    break;
                case 1:
                    rogueHealth += priestSmallHeal;
                    break;
                case 2:
                    mageHealth += priestSmallHeal;
                    break;
                case 3:
                    moonkinDruidHealth += priestSmallHeal;
                    break;
            }
        }
        else
        {
            // heal itself
            priestHealth += priestSmallHeal;
        }
    }

    private void StopSimulation()
    {
        isSimRunning = false;

        returnToMenuButton.SetActive(true);

        // compare highest damage scores
        if (PlayerPrefs.GetFloat("PartyDamage" + level) < (warriorDamageDealt + rogueDamageDealt + mageDamageDealt + moonkinDruidDamageDealt))
        {
            PlayerPrefs.SetFloat("PartyDamage" + level, warriorDamageDealt + rogueDamageDealt + mageDamageDealt + moonkinDruidDamageDealt);
        }
        if (PlayerPrefs.GetFloat("BossDamage" + level) < bossDamageDealt)
        {
            PlayerPrefs.SetFloat("BossDamage" + level, bossDamageDealt);
        }
    }

    private void UpdateUI()
    {
        bossHealthValue.text = bossHealth.ToString();
        warriorHealthValue.text = warriorHealth.ToString();
        rogueHealthValue.text = rogueHealth.ToString();
        mageHealthValue.text = mageHealth.ToString();
        moonkinDruidHealthValue.text = moonkinDruidHealth.ToString();
        priestHealthValue.text = priestHealth.ToString();
        bossDamageDealtValue.text = bossDamageDealt.ToString();
        warriorDamageDealtValue.text = warriorDamageDealt.ToString();
        rogueDamageDealtValue.text = rogueDamageDealt.ToString();
        mageDamageDealtValue.text = mageDamageDealt.ToString();
        moonkinDruidDamageDealtValue.text = moonkinDruidDamageDealt.ToString();
    }

    private void WriteToCSV()
    {
        string timeStepInfo = timeStep + "," + bossHealth + "," + warriorHealth + "," + rogueHealth + "," + mageHealth + "," + moonkinDruidHealth + "," + priestHealth;

        var folder = Application.streamingAssetsPath;

        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        var filePath = Path.Combine(folder, "Level" + level + ".csv");

        using (var writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(timeStepInfo);
        }

        Debug.Log($"CSV file written to \"{filePath}\"");

        AssetDatabase.Refresh();
    }
}
