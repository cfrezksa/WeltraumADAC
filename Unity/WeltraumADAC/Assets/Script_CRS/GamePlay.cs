using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {

    public int initialDamages = 3;
    public int totalDamagesForLevel = 10;
    public float damageTime = 20.0f;
    float localTime = 0.0f;
    float winTime = 0.0f;
    int totalDamagePoints = 0;

    public GameObject winningScreen;
    public GameObject losingScreen;
	// Use this for initialization
    void Start()
    {
        if (null != winningScreen)
        {
            winningScreen.SetActive(false);
        }

        if (null != losingScreen)
        {
            losingScreen.SetActive(false);
        }

        DamagePoint[] damages = FindObjectsOfType<DamagePoint>();
        totalDamagePoints = damages.Length;

        foreach (var d in damages)
        {
            d.Deactivate();
        }

        SpawnDamages(initialDamages);
	}

    private void SpawnDamages(int count)
    {
        List<DamagePoint> listDamages = InactiveDamages();

        Debug.Log("inactive damages " + listDamages.Count);

        for (int i = 0; i < count; i++)
        {
            if (listDamages.Count == 0)
            {
                if (localTime > 30.0f)
                {
                    GameOver();
                }
                return;
            }
            int index = Random.Range(0, listDamages.Count);
            DamagePoint dmg = listDamages[index];
            dmg.Activate();
            listDamages.Remove(dmg);
            totalDamagesForLevel--;
        }
    }

    private static List<DamagePoint> InactiveDamages()
    {
        DamagePoint[] allDamages = FindObjectsOfType<DamagePoint>();

        List<DamagePoint> listDamages = new List<DamagePoint>();

        foreach (var d in allDamages)
        {
            if (!d.IsActive()) listDamages.Add(d);
        }
        return listDamages;
    }

    private static List<DamagePoint> ActiveDamages()
    {
        DamagePoint[] allDamages = FindObjectsOfType<DamagePoint>();

        List<DamagePoint> listDamages = new List<DamagePoint>();

        foreach (var d in allDamages)
        {
            if (d.IsActive()) listDamages.Add(d);
        }
        return listDamages;
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        if (null != losingScreen)
        {
            losingScreen.SetActive(true);
        }
    }

    void GameWon()
    {
        Debug.Log("Game Won!");
        if (null != winningScreen)
        {
            winningScreen.SetActive(true);
        }
    }

	// Update is called once per frame
	void Update () {

        CheckForSpawn();
        CheckForWin();
	}

    private void CheckForSpawn()
    {
        localTime += Time.deltaTime;
        if (localTime > damageTime)
        {
            localTime = 0.0f;
            damageTime *= 0.95f;
            SpawnDamages(1);
        }
    }

    private void CheckForWin()
    {
        var damagesToRepair = ActiveDamages().Count;

        if (damagesToRepair == 0)
        {
            winTime += Time.deltaTime;
            if (winTime > 5.0)
            {
                GameWon();
            }
        }
        else
        {
            winTime = 0.0f;
        }
    }
}
