using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.UnityElementsAccessClasses;
using Assets.GlobalInfos;
using Assets.Database;
using Assets.Database.PersistenceProviders;

public class PlayerLoginLogic : MonoBehaviour {

    private InputField playername;
    private Text playernameAlert;
    private Button ok;
    private Button cancel;


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

        playername = ElementLocator.getElementForFieldINPUT(unityInpFields, "PlayerLogin_PlayernameInputField");

        playernameAlert = ElementLocator.getElementForTEXT(unityTextAlerts, "PlayerLogin_PlayernameAlert");
        playernameAlert.text = "";
        ok = ElementLocator.getElementForBUTTON(unityButtons, "PlayerLogin_SubmitButton");
        cancel = ElementLocator.getElementForBUTTON(unityButtons, "PlayerLogin_BackButton");
    }


    private void assignListeners()
    {
        assignListenersButtons();
    }

    private void assignListenersButtons()
    {
        ok.onClick.AddListener(() =>
        {
            playernameAlert.text = 
                                string.IsNullOrEmpty(playername.text) ? GlobalStrings.playerLoginEmptyMessage
                                : PlayerPersistenceProvider.doesThePlayernameAlreadyExist(playername.text) ? GlobalStrings.playerLoginAlreadyExistsMessage 
                                : "";
            if(playernameAlert.text == "")
            {
                PlayerPersistenceProvider.setPlayerName(LoggedInPlayer.Player, playername.text);
                GUIManager.switchFrames(GlobalStrings.lobbyCanvas);
            }
        });

        cancel.onClick.AddListener(() => 
        {
            LoggedInPlayer.Player = null;
            GUIManager.switchFrames(GlobalStrings.loginCanvas);
        });
    }
    // Update is called once per frame
    void Update () {
	
	}
}
