using UnityEngine;
using Zenject;

namespace KibbleSpace {
    public class DogfoodSizeManager : MonoBehaviour {

        EatAndDespawnSignal _feedSignal;
        DeadDogSignal _deadSignal;
        RestartSignal _restartSignal;
        BeagleFacade _beagle;
        PoolManager _poolManager;

        Vector3 stepVector = new Vector3(0.05f, 0.05f, 0.05f);
        bool startedEating;
        Vector3 scale;

        //inject all necessary signals
        [Inject]
        public void Begin(EatAndDespawnSignal feedSignal, DeadDogSignal deadSignal, RestartSignal restartSignal, BeagleFacade beagle, PoolManager poolManager)
        {
            _feedSignal = feedSignal;
            _feedSignal.Listen(increaseSize);

            _deadSignal = deadSignal;

            _restartSignal = restartSignal;
            _restartSignal.Listen(setup);

            _beagle = beagle;
            _poolManager = poolManager;
        }

        void Start() {
            setup();
        }

        //start the food size as 0 until the player starts feeding, and initiate decreasing the size of the food every 2 seconds
        public void setup()
        {
            startedEating = false;
            this.transform.localScale = new Vector3(0, 0, 0);
            InvokeRepeating("decreaseSize", 1f, 2f);
        }

        //check if the pup still has food
        public bool hasFood()
        {
            scale = this.transform.localScale;
            bool returnval = (scale.x <= 0 || scale.y <= 0 || scale.z <= 0) ? false : true;
            if (!returnval) this.transform.localScale = Vector3.zero;
            return returnval;
        }

        //decrease the size of the food as the dog eats it
        private void decreaseSize()
        {
            //don't start until the user has started feeding the dog
            if (startedEating)
            {
                if (!hasFood())
                {
                    startedEating = false;
                    //fire the signal to prompt the dog to die
                    _deadSignal.Fire();
                    CancelInvoke();
                }
                else
                {
                    //decrease the size of the food
                    this.transform.localScale -= stepVector;
                }
            }
        }

        //increase the size of the food by adding the step vector
        //KibbleFacade is only a variable sent in because the PoolManager needs it to despawn based on the same signal
        private void increaseSize(KibbleFacade k)
        {
            //_poolManager.RemoveKibble(k);
            if (!startedEating)
            {
                startedEating = true;
                //trigger beagle's eat animation
                _beagle.eat();
            }
            this.transform.localScale += stepVector;
        }

        //stop listening to feed the dog
        private void OnDestroy()
        {
            _feedSignal.Unlisten(increaseSize);
            _restartSignal.Unlisten(setup);
        }
    }

    //declare signal for Zenject to prompt the dog to die
    public class DeadDogSignal : Signal<DeadDogSignal>
    {

    }
}
