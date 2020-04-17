using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonManager : MonoBehaviour {

    //Decides what button to go to

    public List<UIButton> buttons = new List<UIButton>();
    public float moveDelay = 0.5f;
    Coroutine movingButton = null;

    private UIButton currentButton = null;

    private void OnEnable()
    {
        if (buttons.Count > 0)
            SelectButton(buttons[0]);
    }

    //Handle Input On A Delayed TimeStep
    private void Update()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movingButton == null)
        {
            if (dir.sqrMagnitude > 0)
            {
                movingButton = StartCoroutine(ChangeButton(dir));
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        movingButton = null;
    }

    public void SelectButton(UIButton btn)
    {
        btn.GetComponent<Button>().Select();
        Tooltip tt = btn.GetComponent<Tooltip>();
        if (tt)
            tt.DisplayText();
        currentButton = btn;
    }

    IEnumerator ChangeButton(Vector2 dir)
    {
        //Debug.Log("Move " + dir);
        int i = 0;
        foreach (UIButton btn in buttons)
        {
            Tooltip tt2 = btn.GetComponent<Tooltip>();
            if (tt2)
                tt2.HideText();
            i++;
        }

        //Use the player's current directional input
        float right = dir.x;
        float up = dir.y;
        UIButton selectedButton = null;

        //decide on an overall direction
        if (Mathf.Abs(right) > Mathf.Abs(up))
        {
            if (right > 0 && currentButton && currentButton.rightBtn && currentButton.rightBtn.gameObject.activeSelf)
            {
                //Right
                selectedButton = currentButton.rightBtn;
            }
            if (right < 0 && currentButton && currentButton.leftBtn && currentButton.leftBtn.gameObject.activeSelf)
            {
                //Left
                selectedButton = currentButton.leftBtn;
            }
        }
        else
        {
            if (up > 0 && currentButton && currentButton.upBtn && currentButton.upBtn.gameObject.activeSelf)
            {
                //Up
                selectedButton = currentButton.upBtn;
            }
            if (up < 0 && currentButton && currentButton.downBtn && currentButton.downBtn.gameObject.activeSelf)
            {
                //Down
                selectedButton = currentButton.downBtn;
            }
        }

        //if (!selectedButton.gameObject.activeSelf)
        //    selectedButton = null;

        if (selectedButton != null)
        {
            SelectButton(selectedButton);
        }

        yield return new WaitForSecondsRealtime(moveDelay);
        movingButton = null;
    }
}
