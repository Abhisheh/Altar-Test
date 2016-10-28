using Assets.GlobalInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UnityElementsAccessClasses
{
    /// <summary>
    /// Thought for generalising finding elements
    /// that are in a specified game object
    /// And return them for assignment purposes
    /// </summary>
    public class ElementLocator
    {


        public static IEnumerable<InputField> getCollectionFromCanvasINPUT(GameObject obj)
        {
            return (obj.GetComponentsInChildren<InputField>()).OfType<InputField>();
        }

        public static IEnumerable<Button> getCollectionFromCanvasBUTTON(GameObject obj)
        {
            return (obj.GetComponentsInChildren<Button>()).OfType<Button>();
        }

        public static IEnumerable<Text> getCollectionFromCanvasTEXT(GameObject obj)
        {
            return (obj.GetComponentsInChildren<Text>()).OfType<Text>();
        }

        /// <summary>
        /// Using elvis operator because I am fancy
        /// </summary>
        /// <param name="elements">Collection of all fields</param>
        /// <param name="elementname">The name of the element we want to take from the fields</param>
        public static InputField getElementForFieldINPUT(IEnumerable<InputField> elements, string elementname)
        {
            var element = elements.Any(f => f.name == elementname)
                ? elements.Where(f => f.name == elementname).First()
                : null;

            if (element == null) throw new ArgumentException(elementNotFoundInUnity(elementname));
            else return element;
        }


        public static Button getElementForBUTTON(IEnumerable<Button> elements, string elementname)
        {
            var element = elements.Any(f => f.name == elementname)
                ? elements.Where(f => f.name == elementname).First()
                : null;

            if (element == null) throw new ArgumentException(elementNotFoundInUnity(elementname));
            else return element;
        }

        public static Text getElementForTEXT(IEnumerable<Text> elements, string elementname)
        {
            var element = elements.Any(f => f.name == elementname)
                ? elements.Where(f => f.name == elementname).First()
                : null;

            if (element == null) throw new ArgumentException(elementNotFoundInUnity(elementname));
            else return element;
        }


        public static string elementNotFoundInUnity(string elementname)
        {
            return "The element specified in code wasn't found in unity. " + elementname + "(C#)";
        }
        public static string elementIsNull(string elementname)
        {
            return "The element you are trying to access is NULL (check if you are accessing it before assigning it) " + elementname + "(C#)";
        }

    }
}
