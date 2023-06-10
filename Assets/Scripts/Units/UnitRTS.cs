using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRTS : MonoBehaviour
{
    private GameObject selectedGameObject;
    private IMovePosition movePosition;
    private GameObject hud;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource selectedAudio;

    private void Awake() {
        selectedGameObject = transform.Find("Selected").gameObject;
        hud = transform.Find("PeasantHUD").gameObject;
        movePosition = GetComponent<IMovePosition>();
        SetSelectedVisible(false);
        hud.SetActive(false);
        audioSource.gameObject.SetActive(true);
    }

    public void SetSelectedVisible(bool visible){
        selectedGameObject.SetActive(visible);
        hud.SetActive(visible);
        if(visible)
            selectedAudio.Play();
    }

    public void MoveTo(Vector3 targetPosition){
        movePosition.SetMovePosition(targetPosition);
    }

    public void SelectBuild()
    {
        audioSource.Play();
    }
}
