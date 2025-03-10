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
    public UIManager uiManager;
    public InteractionHandler interaction;
    public Camera mainCamera;

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

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        stat = FindObjectOfType<PlayerStat>().GetComponent<PlayerStat> ();
        controller = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        interaction = FindObjectOfType<InteractionHandler>().GetComponent<InteractionHandler>();

        mainCamera = Camera.main;

        interaction.camera = mainCamera;
        //controller.camera = mainCamera;

        controller.stat = stat;

        stat.Init();

        GameObject uiManagerObj = new GameObject("UIManager");
        uiManager = uiManagerObj.AddComponent<UIManager>();
        uiManager.Initialize(stat);
    }

}
