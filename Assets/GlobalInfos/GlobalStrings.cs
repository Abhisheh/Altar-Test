using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GlobalInfos
{
    class GlobalStrings
    {
        #region login strings
        public static string loginErrorMessage = "Wrong credentials";
        #endregion

        #region register strings
        public static string registerPasswordCriteriaMessage = "Password must be at least 8 characters long and must contain both digits and text";
        public static string registerUserNameExistsMessage = "This username is already in usage";
        public static string registerUserNameBlankMessage = "No blank spaces allowed!";
        public static string registerUserNameIsNotAnEmail = "The user name is should be an E-Mail!";
        public static string registerPasswordsMustMatchMessage = "Both password fields should be the same";
        public static string registerCriteriaFulfilled = "✓";
        #endregion

        #region playername strings
        public static string playerLoginAlreadyExistsMessage = "This playername already exists. Please choose a different one.";
        public static string playerLoginEmptyMessage = "Player name is not allowed to be empty!";
        #endregion

        #region canvas names
        public static string mainCanvas = "Canvas_Main";
        public static string loginCanvas = "Canvas_Login";
        public static string registerCanvas = "Canvas_Register";
        public static string playerLoginCanvas = "Canvas_PlayerLogin";
        public static string lobbyCanvas = "Canvas_Lobby";
        #endregion   
    }
}
