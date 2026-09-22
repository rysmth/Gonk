using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ryan Joshua Smith Level Timer Script for Gonk Game

public class Countdown : MonoBehaviour
{
    [Header("Countdown")]
    [SerializeField] private TMP_Text countdownText;
    private float timeAmount = 60f;
    private float timer;
    public static bool isTimerOn;

    private void Start()
    {
        timer = timeAmount;
        isTimerOn = true;
    }

    private void Update()
    {
        if (isTimerOn == true)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }

            if (timer < 0f)
            {
                Reset();
                SceneManager.LoadScene(0);
            }

            countdownText.text = ((int)timer).ToString();
        }
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