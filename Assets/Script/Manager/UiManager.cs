using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static private UIManager _instance;
    static public UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 기존의 GameObject에서 UiManager를 찾습니다.
                _instance = FindObjectOfType<UIManager>();

                // 만약 찾지 못했다면 새로 생성합니다.
                if (_instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    _instance = go.AddComponent<UIManager>();
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

    Canvas UICanvas;

    private void Start()
    {
        //한 Scene에서 여러 개일 수도 있는데
        UICanvas = FindObjectOfType<Canvas>();

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

        IInteractable interactable = interactableObj.GetComponent<IInteractable>();

        GameObject UIprefab = Resources.Load<GameObject>("UI/Overlay");
        UIOverlay uiOverlay = ShowOverlayPrefab(UIprefab);
        uiOverlay.NameText.text = interactable.Name;
        uiOverlay.DescriptionText.text = interactable.Description;

        Item item;

        if(interactableObj.TryGetComponent(out item))
        {
            Debug.Log("It is item");
        }
    }

    UIOverlay ShowOverlayPrefab(GameObject prefab)
    {
        GameObject overlayPanel = Instantiate(prefab, UICanvas.transform);
        return overlayPanel.GetComponent<UIOverlay>();
    }

}
