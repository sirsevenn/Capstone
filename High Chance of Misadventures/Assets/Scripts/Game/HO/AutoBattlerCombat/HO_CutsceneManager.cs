using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class HO_CutsceneManager : MonoBehaviour
{
    [Header("Cutscene Properties")]
    [SerializeField] private PlayableDirector cutsceneDirector;
    [SerializeField] private CinemachineVirtualCamera dollyCamera;

    [Space(20)] [Header("Monologue Properties")]
    [SerializeField] private int currentSpeechIndex;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerExitDuration;

    [Space(20)] [Header("Other References")]
    [SerializeField] private HO_AutoBattleUI UI;
    [SerializeField] private HO_PlayerAI playerScript;
    [SerializeField] private HO_LevelSO currentLevel;


    private void Awake()
    {
        cutsceneDirector.enabled = false;
        dollyCamera.enabled = false;
        currentSpeechIndex = 0;
        currentLevel = null;
    }

    public void EnableCutscene()
    {
        cutsceneDirector.enabled = true;
        dollyCamera.enabled = true;
        currentLevel = HO_GameManager.Instance.GetCurrentLevel();

        UI.UpdateLevelIntroduction();    
    }

    public void OnBeginMonologue()
    {
        UI.SetSpeechBubblePosition(playerScript.transform.position + new Vector3(0, 2, 0));
        StartCoroutine(PlayMonologueCoroutine());
    }

    private IEnumerator PlayMonologueCoroutine()
    {
        currentSpeechIndex = 0;
        yield return HandleOneSpeechLine();

        Vector3 rotation = new Vector3(0, 180, 0);
        playerTransform.DORotate(rotation, 2f);
        yield return HandleOneSpeechLine();

        playerTransform.DOKill();
        playerTransform.eulerAngles = rotation;
        yield return HandleOneSpeechLine();

        UI.DisableSpeechBubble();
        playerScript.PlayerMove();
        playerTransform.DOMove(Vector3.zero, playerExitDuration).SetEase(Ease.Linear).OnComplete(OnCutsceneEnd);
    }

    private IEnumerator HandleOneSpeechLine()
    {
        yield return UI.DisplaySpeechBubble(currentLevel.SpeechList[currentSpeechIndex]);
        currentSpeechIndex++;

        yield return GameUtilities.WaitForPlayerInput();
    }

    public void OnCutsceneEnd()
    {
        HO_GameManager.Instance.TransitionToCraftingScene();
    }
}
