using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public TMP_InputField nameInput;
    public TMP_Text bestScoreText;
    public Button startButton;

    public string bestScoreDisplay;

    public string playerName;
    public string bestScorePlayer;
    public int bestScore;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadBestScore();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (nameInput.text.Length == 0)
            {
                startButton.interactable = false;
            }
            else
            {
                startButton.interactable = true;
            }
        }
    }

    public void StartGame()
    {
        playerName = nameInput.text;
        SceneManager.LoadScene(1);

    }

    public void ExitGame()
    {
        SaveBestScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int bestScore;
        public string bestScorePlayer;
        public string bestScoreDisplay;
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestScorePlayer = playerName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        { 
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
            bestScoreDisplay = data.bestScorePlayer + " : " + data.bestScore;
            bestScoreText.text = bestScoreDisplay;
        }
    }
}
