using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace KibbleSpace
{
    public class TextManager : MonoBehaviour
    {
        public Text display;

        //display text on screen
        public void displayText(string text)
        {
            display.text = text;
            StartCoroutine(sequenceTextOffscreen());
        }

        //after 3 seconds, remove the message from the screen
        IEnumerator sequenceTextOffscreen()
        {
            yield return new WaitForSeconds(4f);
            display.text = " ";
        }

    }
}