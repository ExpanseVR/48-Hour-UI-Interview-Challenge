using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MiniGameManager : MonoSingleton<MiniGameManager>
{
    public static event Action<bool> OnIsRoundCommenced;
    public static event Action<bool> OnReset;

    public enum SpawnPoint {
        Top,
        Bottom,
        Left,
        Right
    }

    [SerializeField]
    private TextMeshProUGUI _scoreText, _hiScoreText, _placeTowersText;

    private bool _roundStarted = false;
    private int _score = 0;
    private int _hiScore = 0;


    private void OnEnable()
    {
        Enemy.OnDestroyedEnemy += UpdateScore;
        _placeTowersText.gameObject.SetActive(true);
        LoadHiScore();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(StartRound());
    }

    IEnumerator StartRound ()
    {
        if (!_roundStarted)
        {
            _roundStarted = true;
            yield return null;
            _placeTowersText.gameObject.SetActive(false);
            OnIsRoundCommenced?.Invoke(true);
        }
    }

    public void UpdateScore (int addPoints, GameObject enemy, bool withPoints)
    {
        if (!withPoints)
            return;
        //add score
        _score += addPoints;
        //check if current score is bigger than hi score
        if (_score > _hiScore)
        {
            _hiScore = _score;
            _hiScoreText.text = _hiScore.ToString();
            SaveHiScore();
        }

        //update score and hiscore texts
        _scoreText.text = _score.ToString();
    }

    private void LoadHiScore ()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            HiScoreData data = (HiScoreData)bf.Deserialize(file);
            _hiScore = data.hiScore;
            _hiScoreText.text = _hiScore.ToString();
        }
    }

    private void SaveHiScore ()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            HiScoreData data = new HiScoreData();
            data.hiScore = _hiScore;

            bf.Serialize(file, data);
            file.Close();
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

            HiScoreData data = new HiScoreData();
            data.hiScore = _hiScore;

            bf.Serialize(file, data);
            file.Close();
        }
    }

    private void ResetGame()
    {
        _score = 0;
        _scoreText.text = "0";
        OnReset?.Invoke(false);
        OnIsRoundCommenced?.Invoke(false);
        _roundStarted = false;
    }

    private void OnDisable()
    {
        ResetGame();
        Enemy.OnDestroyedEnemy -= UpdateScore;
    }
}

[Serializable]
class HiScoreData
{
    public int hiScore;
}
