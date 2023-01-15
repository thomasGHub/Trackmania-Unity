using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameModeMenuView : View
{
    private Regex numberRegex = new Regex(@"[1-9]");

    [Header("Select Buttons")]
    public GameObject selectMode;
    public Button createTimeAttack;
    public Button selectRounds;

    [Header("Create Rounds")]
    public GameObject selectModeRound;
    public InputField nbRounds;
    public Button createRounds;
    public GameObject errorMessage;
    private bool isErrorOn = false;

    public GameMode finalGameMode;



    public override void Initialize()
    {
        selectRounds.onClick.AddListener(SelectRounds);
        createRounds.onClick.AddListener(CreateRounds);
        createTimeAttack.onClick.AddListener(CreateTimeAttack);

        selectModeRound.SetActive(false);
        selectMode.SetActive(true);

    }

    public void SelectRounds()
    {
        //afficher button + inputfield

        selectModeRound.SetActive(true);
        selectMode.SetActive(false );

    }

    public void CreateRounds()
    {
        if (numberRegex.IsMatch(nbRounds.text))
        {
            int nbRoundsInt = Int32.Parse(nbRounds.text);
            finalGameMode = GameModeFactory.Create(GameModeType.Rounds,nbRoundsInt);

            ShowCreateView();
        }
        else
        {
            if (!isErrorOn)
            {
                StartCoroutine(errorPopup());
            }
            //popup

        }
    }

    public void CreateTimeAttack()
    {
        finalGameMode = GameModeFactory.Create(GameModeType.TimeAttack);
        ShowCreateView();

    }

    private IEnumerator errorPopup()
    {
        isErrorOn = true;
        //activate popup
        errorMessage.SetActive(true);

        yield return new WaitForSeconds(5f);

        // desactivate popup
        errorMessage.SetActive(false);

        isErrorOn = false;
    }

    private void ShowCreateView()
    {
        ViewManager.Show<CreateMenuView>();
        ViewManager.GetView<CreateMenuView>().ActiveButton();

    }



}
