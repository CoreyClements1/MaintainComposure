using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedTextMover : MonoBehaviour
{

    // SavedTextMover moves saved text


    #region AWAKE


    // Awake
    //----------------------------------------//
    private void Awake()
    //----------------------------------------//
    {
        StartCoroutine(MoveCoroutine());

    } // END Awake


    // Moves the text
    //----------------------------------------//
    private IEnumerator MoveCoroutine()
    //----------------------------------------//
    {
        yield return null; 

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;

        Debug.Log(transform.position);

        gameObject.LeanMove(transform.position + Vector3.left * 100f, .3f).setEaseOutQuad();

        yield return new WaitForSeconds(2f);

        canvasGroup.LeanAlpha(0f, 1f);

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);

    } // END MoveCoroutine


    #endregion


}
