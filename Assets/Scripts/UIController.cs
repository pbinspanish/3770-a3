using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("PartyDamage1")) {
            // Initialize Player Preferences
            PlayerPrefs.SetFloat("PartyDamage1", 0.0f);
            PlayerPrefs.SetFloat("BossDamage1", 0.0f);
            PlayerPrefs.SetFloat("PartyDamage2", 0.0f);
            PlayerPrefs.SetFloat("BossDamage2", 0.0f);
            PlayerPrefs.SetFloat("PartyDamage3", 0.0f);
            PlayerPrefs.SetFloat("BossDamage3", 0.0f);
        }
    }

    public void ReturnToMenuButton_OnPress()
    {
        SceneManager.LoadScene(0);
    }

    public void ScoresButton_OnPress()
    {
        SceneManager.LoadScene(1);
    }

    public void Level1Button_OnPress()
    {
        SceneManager.LoadScene(2);
    }

    public void Level2Button_OnPress()
    {
        SceneManager.LoadScene(3);
    }

    public void Level3Button_OnPress()
    {
        SceneManager.LoadScene(4);
    }
}
