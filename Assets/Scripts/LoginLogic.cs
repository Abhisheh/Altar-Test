using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.UnityElementsAccessClasses;
using Assets.GlobalInfos;
using Assets.Database.PersistenceProviders;
using Assets.Database;

public class LoginLogic : MonoBehaviour {

    //Input fields we want to access
    private InputField username;
    private InputField password;

    //alert-ish
    private Text credentialsInvalidAlert; 

    //Buttons
    private Button ok; //to login
    private Button register; //to register

    // Use this for initialization
    void Start () {
        assignUnityElementsToFields();
        assignListeners();
    }

    private void assignUnityElementsToFields()
    {
        var unityInpFields = ElementLocator.getCollectionFromCanvasINPUT(gameObject);
        var unityTextAlerts = ElementLocator.getCollectionFromCanvasTEXT(gameObject);
        var unityButtons = ElementLocator.getCollectionFromCanvasBUTTON(gameObject);

        username = ElementLocator.getElementForFieldINPUT(unityInpFields, "Login_UsernameInputField");
        password = ElementLocator.getElementForFieldINPUT(unityInpFields, "Login_PasswordInputField");
        password.inputType = InputField.InputType.Password;

        credentialsInvalidAlert = ElementLocator.getElementForTEXT(unityTextAlerts, "Login_CredentialsInvalidAlert");
        credentialsInvalidAlert.text = "";

        ok = ElementLocator.getElementForBUTTON(unityButtons, "Login_LoginButton");
        register = ElementLocator.getElementForBUTTON(unityButtons, "Login_RegisterButton");
    }

    private void assignListeners()
    {
        assignListenersButtons();
    }
    

    private void assignListenersButtons()
    {
        ok.onClick.AddListener(() => {
            //Login the user
            if (string.IsNullOrEmpty(username.text) || string.IsNullOrEmpty(password.text))
            {
                credentialsInvalidAlert.text = GlobalStrings.loginErrorMessage;
                credentialsInvalidAlert.color = Color.red;
            }
            else {
                if (PlayerPersistenceProvider.getSpecificPlayer(username.text, password.text) != null)
                {
                    LoggedInPlayer.Player = PlayerPersistenceProvider.getSpecificPlayer(username.text, password.text);

                    //GUIManager.setFrame(PlayersPersistenceProvider.doesPlayerHaveAPlayername(LoggedInPlayer.Player) ? GUIManager.frames[])
                    if (LoggedInPlayer.Player != null)
                    {
                        GUIManager.switchFrames((PlayerPersistenceProvider.doesPlayerHaveAPlayername(LoggedInPlayer.Player))
                            ? GlobalStrings.lobbyCanvas : GlobalStrings.playerLoginCanvas);
                    }
                    else
                    {
                        credentialsInvalidAlert.text = GlobalStrings.loginErrorMessage;
                        credentialsInvalidAlert.color = Color.red;
                    }


                }
                else
                {
                    credentialsInvalidAlert.text = GlobalStrings.loginErrorMessage;
                    credentialsInvalidAlert.color = Color.red;
                }
            }
            
        });

        register.onClick.AddListener(() =>
        {
            //Off to register
            GUIManager.switchFrames(GlobalStrings.registerCanvas);
        });
    }

    // Update is called once per frame
    void Update () {
	
	}
}
