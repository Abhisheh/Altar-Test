using UnityEngine;
using UnityEngine.UI;
using Assets.UnityElementsAccessClasses;
using Assets.GlobalInfos;
using UnityEditor;
using Assets.Database.PersistenceProviders;
using System;
using System.Linq;

public class RegisterLogic : MonoBehaviour {

    //Input fields we want to access
    private InputField username;
    private InputField password;
    private InputField passwordRe;

    //Alert-ish messages
    private Text usernameAlert;
    private Text passwordAlert;
    private Text passwordReAlert;

    //Buttons
    private Button ok; //to register
    private Button cancel; //to cancel register

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

        username = ElementLocator.getElementForFieldINPUT(unityInpFields, "Register_UsernameInputField");
        password = ElementLocator.getElementForFieldINPUT(unityInpFields, "Register_PasswordInputField");
        //password.inputType = InputField.InputType.Password;
        passwordRe = ElementLocator.getElementForFieldINPUT(unityInpFields, "Register_PasswordReenterInputField");
        //passwordRe.inputType = InputField.InputType.Password;

        usernameAlert = ElementLocator.getElementForTEXT(unityTextAlerts, "Register_UsernameAlert");
        usernameAlert.text = "";
        passwordAlert = ElementLocator.getElementForTEXT(unityTextAlerts, "Register_PasswordAlert");
        passwordAlert.text = "";
        passwordReAlert = ElementLocator.getElementForTEXT(unityTextAlerts, "Register_PasswordReenterAlert");
        passwordReAlert.text = "";

        ok = ElementLocator.getElementForBUTTON(unityButtons, "Register_RegisterButton");
        ok.enabled = false;
        cancel = ElementLocator.getElementForBUTTON(unityButtons, "Register_BackButton");
    }

    private void assignListeners()
    {
        assignListenersInputFields();
        assignListenersButtons();
    }

    private void assignListenersInputFields()
    {
        username.onValueChanged.AddListener((string value) =>
        {
            usernameAlert.text = (username.text.Contains(" ")) ? GlobalStrings.registerUserNameBlankMessage
                : GlobalStrings.registerCriteriaFulfilled;
            setAlertsColor(usernameAlert);
            setOKButtonStatus();
        });


        username.onEndEdit.AddListener((string value) =>
        {
            usernameAlert.text =
            !PlayerPersistenceProvider.playerDoesNotExist(value) ? GlobalStrings.registerUserNameExistsMessage
            : GlobalStrings.registerCriteriaFulfilled;

            setAlertsColor(usernameAlert);
            setOKButtonStatus();
        });

        password.onValueChanged.AddListener((string value) =>
        {
            int numb;
            passwordAlert.text =
            (
                value.Length > 8
                && !int.TryParse(value, out numb)
                && value.Any(c => char.IsDigit(c))
            ) ? GlobalStrings.registerCriteriaFulfilled : GlobalStrings.registerPasswordCriteriaMessage;
            setAlertsColor(passwordAlert);
            passwordReAlert.text = (password.text == passwordRe.text) && passwordAlert.text == GlobalStrings.registerCriteriaFulfilled
                                   ? GlobalStrings.registerCriteriaFulfilled : GlobalStrings.registerPasswordsMustMatchMessage;
            setAlertsColor(passwordReAlert);
            setOKButtonStatus();
        });

        passwordRe.onValueChanged.AddListener((string value) =>
        {
            int numb;
            passwordReAlert.text =
            (
                value.Length > 8
                && !int.TryParse(value, out numb)
                && value.Any(c => char.IsDigit(c))
                && (password.text == passwordRe.text)
            ) ? GlobalStrings.registerCriteriaFulfilled : GlobalStrings.registerPasswordsMustMatchMessage;
            setAlertsColor(passwordReAlert);
            setOKButtonStatus();
        });
    }

    /// <summary>
    /// Sets the color of an alert based on if the register criteria were fulfilled or not
    /// </summary>
    /// <param name="alert"></param>
    private void setAlertsColor(Text alert)
    {
        alert.color = alert.text == GlobalStrings.registerCriteriaFulfilled ? Color.green : Color.red;
    }

    private void setOKButtonStatus()
    {
        ok.enabled =
                (usernameAlert.text == GlobalStrings.registerCriteriaFulfilled)
                && (passwordAlert.text == GlobalStrings.registerCriteriaFulfilled)
                && (passwordReAlert.text == GlobalStrings.registerCriteriaFulfilled);

    }

    private void assignListenersButtons()
    {
        ok.onClick.AddListener(() => {
            PlayerPersistenceProvider.addNewPlayer(username.text, password.text);
            EditorUtility.DisplayDialog("Registered!", "You were successfully registered", "Okay");
            GUIManager.switchFrames(GlobalStrings.loginCanvas);
        });

        cancel.onClick.AddListener(() =>
        {
            GUIManager.switchFrames(GlobalStrings.loginCanvas);
        });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
