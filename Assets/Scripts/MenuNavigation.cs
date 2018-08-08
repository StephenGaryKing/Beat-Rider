using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour {

    [SerializeField] GameObject[] canvasToDisable;
    [SerializeField] GameObject canvasToEnable;
    [SerializeField] Animation camAnim;

    public void ChangeCanvas()
    {
        for (int i = 0; i < canvasToDisable.Length; i ++)
            canvasToDisable[i].SetActive(false);
        canvasToEnable.SetActive(true);
    }
    public void Customisation()
    {
        for (int i = 0; i < canvasToDisable.Length; i++)
            canvasToDisable[i].SetActive(false);
        canvasToEnable.SetActive(true);
        camAnim.Play("Camera_Customisation Animation");
    }
    public void ExitCustomisation()
    {
        for (int i = 0; i < canvasToDisable.Length; i++)
            canvasToDisable[i].SetActive(false);
        canvasToEnable.SetActive(true);
        camAnim.Play("Return Animation");
    }
    public void PlaySong()
    {
        //canvasToDisable.SetActive(false);
        canvasToEnable.SetActive(true);
        camAnim.Play("MenuShrink Animation");
    }
    public void ExitSong()
    {
        for (int i = 0; i < canvasToDisable.Length; i++)
            canvasToDisable[i].SetActive(false);
        //canvasToEnable.SetActive(true);
        camAnim.Play("MenuGrow Animation");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
