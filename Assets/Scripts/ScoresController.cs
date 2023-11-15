using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoresController : MonoBehaviour
{
    public TextMeshProUGUI partyDamage1;
    public TextMeshProUGUI bossDamage1;
    public TextMeshProUGUI partyDamage2;
    public TextMeshProUGUI bossDamage2;
    public TextMeshProUGUI partyDamage3;
    public TextMeshProUGUI bossDamage3;


    void Start()
    {
        partyDamage1.text = PlayerPrefs.GetFloat("PartyDamage1").ToString();
        bossDamage1.text = PlayerPrefs.GetFloat("BossDamage1").ToString();
        partyDamage2.text = PlayerPrefs.GetFloat("PartyDamage2").ToString();
        bossDamage2.text = PlayerPrefs.GetFloat("BossDamage2").ToString();
        partyDamage3.text = PlayerPrefs.GetFloat("PartyDamage3").ToString();
        bossDamage3.text = PlayerPrefs.GetFloat("BossDamage3").ToString();
    }
}
