using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager _instance;
    static public GameManager Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    public PlayerController controller;
    public PlayerStat stat;
    public UIStat uiStat;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
