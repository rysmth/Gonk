using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ryan Joshua Smith Determine Player amount in Level Script for Gonk Game

public class PlayerState : MonoBehaviour
{
    [SerializeField] GameObject winState;
    [SerializeField] GameObject[] playerObjects;
    private int activePlayerCount;
    private GameObject activatedPlayer;
    [SerializeField] AudioSource winSFX;

    private Countdown Countdown;

    private void Start()
    {
        if (activatedPlayer != null)
        {
            activatedPlayer.GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        int activePlayerCount = 0;

        foreach (GameObject player in playerObjects)
        {
            if (player.activeSelf)
            {
                activePlayerCount++;
                activatedPlayer = player;
            }
        }

        if (activePlayerCount == 1)
        {
            Reset();
            Countdown.isTimerOn = false;
            winSFX.Play();
            winState.SetActive(true);
            StartCoroutine(ChangeDelay());
        }

        if (activePlayerCount == 0)
        {
            Reset();
            StartCoroutine(ChangeDelay());
            winSFX.Stop();
            winState.SetActive(false);
        }
    }

    private IEnumerator ChangeDelay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    private void Reset()
    {
        CharacterSelect.isPlayer3Active = false;
        CharacterSelect.isPlayer4Active = false;

        for (int i = 0; i < CharacterSelect.activeCharacter.Length; i++)
        {
            CharacterSelect.activeCharacter[i] = 0;
        }
    }
}