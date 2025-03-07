using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //static private UIManager _instance;
    //static public UIManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            // ������ GameObject���� UiManager�� ã���ϴ�.
    //            _instance = FindObjectOfType<UIManager>();

    //            // ���� ã�� ���ߴٸ� ���� �����մϴ�.
    //            if (_instance == null)
    //            {
    //                GameObject go = new GameObject("UIManager");
    //                _instance = go.AddComponent<UIManager>();
    //            }
    //        }
    //        return _instance;
    //    }
    //}

    //private void Awake()
    //{
    //    if (_instance == null)
    //    {
    //        _instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        if (_instance != this)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    [SerializeField] Canvas UICanvas;

    public void Initialize()
    {
        SetCanvasUI();
        SetInteractable();
    }

    void SetCanvasUI()
    {
        GameObject canvasObj = new GameObject("UICanvas");
        UICanvas = canvasObj.AddComponent<Canvas>();
        UICanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.referencePixelsPerUnit = 100;
    }

    void SetInteractable()
    {
        // ��� �����ۿ� ���� �̺�Ʈ ����
        IInteractable[] interactableObjs = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().ToArray();
        foreach (var interactable in interactableObjs)
        {
            interactable.OnItemInteracted += ShowOverlayUI;
            interactable.OnItemInteractionEnded += CloseOverlayUI;
        }
    }

    GameObject overlayPanel;

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
        overlayPanel = Instantiate(prefab, UICanvas.transform);
        return overlayPanel.GetComponent<UIOverlay>();
    }

    public void CloseOverlayUI()
    {
        Destroy(overlayPanel);
    }

}
