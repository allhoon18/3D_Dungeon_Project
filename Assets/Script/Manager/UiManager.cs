using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    const string UIDefalutPath = "UI/UIDefault";
    const string UIOverlayPath = "UI/UIOverlay";

    [SerializeField] Canvas UICanvas;

    public void Initialize( PlayerStat stat)
    {
        SetCanvasUI();
        SetInteractable();
        SetUIDefault(stat);
    }

    //캔버스를 생성하고 초기화함
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
        // 모든 아이템에 대해 이벤트 구독
        IInteractable[] interactableObjs = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().ToArray();
        foreach (var interactable in interactableObjs)
        {
            interactable.OnItemInteracted += ShowOverlayUI;
            interactable.OnItemInteractionEnded += CloseOverlayUI;
        }
    }

    public UIStat uiStat;

    //체력바, 조준점과 같은 초기 UI를 세팅
    void SetUIDefault(PlayerStat stat)
    {
        GameObject uiDefalutprefab = Resources.Load<GameObject>(UIDefalutPath);
        GameObject uiDefault = Instantiate(uiDefalutprefab, UICanvas.transform);
        uiStat = uiDefault.GetComponentInChildren<UIStat>();

        uiStat.stat = stat;
        uiStat.Init();
    }

    GameObject overlayPanel;
    //아이템 정보를 전시하는 overlayPanel을 불러옴
    public void ShowOverlayUI(GameObject interactableObj)
    {
        IInteractable interactable = interactableObj.GetComponent<IInteractable>();

        GameObject UIprefab = Resources.Load<GameObject>(UIOverlayPath);
        UIOverlay uiOverlay = ShowOverlayPrefab(UIprefab);
        uiOverlay.NameText.text = interactable.Name;
        uiOverlay.DescriptionText.text = interactable.Description;

        Item item;

        if(interactableObj.TryGetComponent<Item>(out item))
        {
            uiOverlay.UseButtonUI.gameObject.SetActive(item != null);
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
