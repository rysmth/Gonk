using UnityEngine;
using UnityEngine.SceneManagement;

// Ryan Joshua Smith Title screen to gameplay Script for Gonk Game

public class Opening : MonoBehaviour
{
    [SerializeField] GameObject openingCanvas;
    [SerializeField] GameObject openingModel;

    [SerializeField] GameObject characterSelectCanvas;
    [SerializeField] GameObject characterSelectModel;

    [SerializeField] AudioSource menuSFX;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            ChangeCanvas();
            menuSFX.Play();
        }
    }

    private void ChangeCanvas()
    {
        openingCanvas.SetActive(false);
        openingModel.SetActive(false);

        characterSelectCanvas.SetActive(true);
        characterSelectModel.SetActive(true);
    }
}