using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightPanelManager : MonoBehaviour
{

    // RightPanelManager manages swapping the active panel on the right side


    #region VARIABLES


    [SerializeField] private Button[] buttons = new Button[4];
    [SerializeField] private CanvasGroup[] canvasGroups = new CanvasGroup[4];

    private int activePanel = 0;
    private bool canSwap = true;


    #endregion


    #region SWAPPING


    // Swaps to a panel
    //----------------------------------------//
    private void SwapToPanel(int panelNum)
    //----------------------------------------//
    {
        if (canSwap && activePanel != panelNum)
        {
            StartCoroutine(SwapCoroutine(panelNum));
        }

    } // END SwapToPanel


    // Swaps to a panel
    //----------------------------------------//
    private IEnumerator SwapCoroutine(int panelNum)
    //----------------------------------------//
    {
        canSwap = false;

        float smallSize = 30;
        float bigSize = 80;
        float transitionTime = .15f;

        int oldActive = activePanel;
        activePanel = panelNum;
        
        // Shrink old active
        if (oldActive >= 0 && oldActive <= 3)
        {
            RectTransform oldRect = buttons[oldActive].GetComponent<RectTransform>();
            CanvasGroup oldCanvasGroup = canvasGroups[oldActive];

            LeanTween.value(oldRect.gameObject, bigSize, smallSize, transitionTime).setEaseOutExpo().setOnUpdate((value) => 
            {
                oldRect.sizeDelta = new Vector2(oldRect.sizeDelta.x, value);
            });

            LeanTween.value(oldCanvasGroup.gameObject, 1f, 0f, transitionTime / 2f).setOnUpdate((value) =>
            {
                oldCanvasGroup.alpha = value;
            });
        }

        // Grow new active
        RectTransform newRect = buttons[activePanel].GetComponent<RectTransform>();
        CanvasGroup newCanvasGroup = canvasGroups[activePanel];
        newCanvasGroup.gameObject.SetActive(true);
        newCanvasGroup.alpha = 0f;

        // If switching to active panel, update character name
        if (activePanel == 1)
        {
            RightEditManager rightEditManager = newCanvasGroup.GetComponent<RightEditManager>();
            rightEditManager.UpdateCharName(HeaderAndSaveManager.Instance.charNameText.text);
        }

        LeanTween.value(newRect.gameObject, smallSize, bigSize, transitionTime).setEaseOutExpo().setOnUpdate((value) =>
        {
            newRect.sizeDelta = new Vector2(newRect.sizeDelta.x, value);
        });

        LeanTween.value(newCanvasGroup.gameObject, 0f, 1f, transitionTime / 2f).setOnUpdate((value) =>
        {
            newCanvasGroup.alpha = value;
        });

        // Wait for transition
        yield return new WaitForSeconds(transitionTime);

        // Deactivate old
        if (oldActive >= 0 && oldActive <= 3)
            canvasGroups[oldActive].gameObject.SetActive(false);

        canSwap = true;

    } // END SwapCoroutine


    #endregion


    #region ON BUTTON


    // OnButtonPress, swap to given button
    //----------------------------------------//
    public void OnButtonPress(int buttonNum)
    //----------------------------------------//
    {
        SwapToPanel(buttonNum);

    } // END OnButtonPress


    #endregion


    #region LEARN NEW ART


    public void OnLearnNewArt()
    {

    }


    #endregion


} // END RightPanelManager
