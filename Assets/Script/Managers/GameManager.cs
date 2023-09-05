using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameCondition {Paused, Playing, Win, Lose }
    public GameCondition condition;

    public static GameManager Instance { get; private set; }

    public float timeLimit
    {
        get
        {
            if (_timeLimit >= 0)
                return _timeLimit;
            else
                return 0;
        }
        set
        {
            if (_timeLimit >= 0)
                _timeLimit = value > 0 ? value : 0;
            else
                _timeLimit = 0;
        }
    }

    public float _timeLimit = 120f;

    public GameObject winPanel;
    public GameObject losePanel;
    private void Awake()
    {
        #region SingletonInstance

        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        #endregion

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(condition == GameCondition.Playing)
            timeLimit -= Time.deltaTime;

        if (timeLimit <= 0)
        {
            TriggerLoseCondition();
        }

        if (!CustomerOrderManager.IsAnyOrderFound())
        {
            TriggerWinCondition();
        }
    }

    public void TriggerWinCondition()
    {
        condition = GameCondition.Win;
        winPanel.gameObject.SetActive(true);
    }

    public void TriggerLoseCondition()
    {
        condition = GameCondition.Lose;
        losePanel.gameObject.SetActive(true);
    }
}
