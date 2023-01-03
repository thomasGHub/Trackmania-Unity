using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthentificationStartView : View
{
    public Button goToLogin;
    public Button goToRegister;
    public Button goToRegisterID;
    public override void Initialize()
    {
        goToLogin.onClick.AddListener(() => ViewManager.Show<LoginView>());
        goToRegister.onClick.AddListener(() => ViewManager.Show<RegisterView>());
        goToRegisterID.onClick.AddListener(() => UserAccountManager.instance.SignInWithDevice());
    }
}
