using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUiController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputFieldSeed;
    private int currentSeed;
    [SerializeField]
    private MapGenerator mapGenerator;
    

    public void GenerateSeed()
    {
        int rndSeed = Random.Range(0, 99999999);
        inputFieldSeed.text = rndSeed.ToString();
    }

    public void GenerateIsland()
    {
        if (!string.IsNullOrEmpty(inputFieldSeed.text))
        {
            currentSeed = int.Parse(inputFieldSeed.text);
        }
        PlayerPrefs.SetInt("Seed", currentSeed);
        currentSeed = PlayerPrefs.GetInt("Seed", 0);
        inputFieldSeed.text = currentSeed.ToString();
        mapGenerator.GenerateMap(currentSeed);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
