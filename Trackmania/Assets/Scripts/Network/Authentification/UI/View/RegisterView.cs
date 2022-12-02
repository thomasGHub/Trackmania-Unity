using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RegisterView : View
{
    public TMP_InputField userName;
    public TMP_InputField email;
    public TMP_InputField password;
    public Button createAccount;
    public Button back;
    public TextMeshProUGUI errorText;

    private void OnEnable()
    {
        UserAccountManager.OnRegisterFailed.AddListener(OnRegisterFailed);
        UserAccountManager.OnSignInSuccess.AddListener(OnRegisterSuccess);
    }

    private void OnDisable()
    {
        UserAccountManager.OnSignInFailed.RemoveListener(OnRegisterFailed);
        UserAccountManager.OnSignInSuccess.RemoveListener(OnRegisterSuccess);
    }

    private void OnRegisterFailed(string error)
    {
        errorText.text = error;
        errorText.gameObject.SetActive(true);
    }
    private void OnRegisterSuccess()
    {
        errorText.text = "Account Register";
    }

    public override void Initialize()
    {
        errorText.gameObject.SetActive(false);
        createAccount.onClick.AddListener(() => UserAccountManager.instance.CreateAccount(userName.text, email.text, password.text));
        back.onClick.AddListener(() => ViewManager.ShowLast());

    }

    
}
