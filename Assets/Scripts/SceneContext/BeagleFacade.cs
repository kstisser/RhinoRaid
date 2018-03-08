using UnityEngine;
using Zenject;

namespace KibbleSpace
{
    public class BeagleFacade : MonoBehaviour
    {

        public Animator anim;
        DeadDogSignal _deadSignal;
        RestartSignal _restartSignal;
        TextManager _displayText;

        [Inject]
        public void Begin(DeadDogSignal deadSignal, TextManager displayText, RestartSignal restartSignal)
        {
            _deadSignal = deadSignal;
            _deadSignal.Listen(die);

            _restartSignal = restartSignal;
            _displayText = displayText;
        }

        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void eat()
        {
            anim.SetBool("hasFood", true);
        }

        private void die()
        {
            anim.SetBool("isDead", true);
            anim.SetBool("hasFood", false);
            _displayText.displayText("Fail! Rhino died, you took too long to feed your dog :(");
            InvokeRepeating("promptToRevitalize", 7f, 7f);
        }

        private void promptToRevitalize()
        {
            _displayText.displayText("Say \"Get Up\" to try again.");
        }

        public void backToLife()
        {
            CancelInvoke("promptToRevitalize");
            _displayText.displayText("Just kidding! \nTry again.");
            anim.SetBool("isDead", false);
            _restartSignal.Fire();
        }

        private void OnDestroy()
        {
            _deadSignal.Unlisten(die);   
        }
    }

    public class RestartSignal : Signal<RestartSignal>
    {

    }
}
