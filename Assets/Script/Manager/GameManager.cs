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
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    public PlayerController controller;
    public PlayerStat stat;
    public UIStat uiStat;
    public InteractionHandler interaction;
    public UIManager uiManager;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _instance = new GameObject("GameManager").AddComponent<GameManager>();
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

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        stat = FindObjectOfType<PlayerStat>().GetComponent<PlayerStat> ();
        controller = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        uiStat = FindObjectOfType<UIStat>().GetComponent<UIStat>(); ;

        controller.stat = stat;
        uiStat.stat = stat;

        stat.Init();
        uiStat.Init();

        uiManager = UIManager.Instance;
    }

}
