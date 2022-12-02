using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LoginView : View
{

    public TMP_InputField userName;
    public TMP_InputField password;
    public Button loginAccount;
    public Button back;
    public TextMeshProUGUI errorText;


    private void OnEnable()
    {
        UserAccountManager.OnSignInFailed.AddListener(OnSignInFailed);
        UserAccountManager.OnSignInSuccess.AddListener(OnSignInSuccess);
    }

    private void OnDisable()
    {
        UserAccountManager.OnSignInFailed.RemoveListener(OnSignInFailed);
        UserAccountManager.OnSignInSuccess.RemoveListener(OnSignInSuccess);
    }

    public void OnSignInFailed(string error)
    {
        errorText.text = error;
        errorText.gameObject.SetActive(true);

    }
    public void OnSignInSuccess()
    {
        errorText.text = "Account Login";
    }

    public override void Initialize()
    {
        errorText.gameObject.SetActive(false);
        back.onClick.AddListener(() => ViewManager.ShowLast());
        loginAccount.onClick.AddListener(() => UserAccountManager.instance.SignIn(userName.text, password.text));


    }

    

}
