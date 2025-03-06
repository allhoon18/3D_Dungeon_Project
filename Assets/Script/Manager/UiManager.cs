using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    static private UiManager _instance;
    static public UiManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 기존의 GameObject에서 UiManager를 찾습니다.
                _instance = FindObjectOfType<UiManager>();

                // 만약 찾지 못했다면 새로 생성합니다.
                if (_instance == null)
                {
                    GameObject go = new GameObject("UiManager");
                    _instance = go.AddComponent<UiManager>();
                }
            }
            return _instance;
        }
    }

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
        // 모든 아이템에 대해 이벤트 구독
        IInteractable[] interactableObjs = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().ToArray();
        foreach (var interactable in interactableObjs)
        {
            interactable.OnItemInteracted += ShowOverlayUI;
        }
    }

    public void ShowOverlayUI(GameObject interactableObj)
    {
        Debug.Log("It is "+interactableObj.name);

        Item item;

        if(interactableObj.TryGetComponent(out item))
        {
            Debug.Log("It is item");
        }
    }

}
