using UnityEngine;
using Zenject;
using HoloToolkit.Unity.InputModule;
using System;

namespace KibbleSpace
{
    /*
     * This class provides a facade or way of delegating all tasks between
     * handlers and information scripts
     */
    public class KibbleFacade : MonoBehaviour, IInputClickHandler
    {
        KibbleMovementHandler _movementHandler;
        EatAndDespawnSignal _eatAndDespawnSignal;

        [Inject]
        public void Begin(KibbleMovementHandler movementHandler, EatAndDespawnSignal eatAndDespawnSignal)
        {
            _movementHandler = movementHandler;
            _eatAndDespawnSignal = eatAndDespawnSignal;
        }

        void Start()
        {
            generateNewLocation();
        }

        public void generateNewLocation()
        {
            this.transform.position = _movementHandler.generateRandomLocation();
        }

        //method called when the gesture recognizer notices a select
        public void OnInputClicked(InputClickedEventData eventData)
        {
            _eatAndDespawnSignal.Fire(this);
        }

        //triggered by voice command "Zap"
        public void clickedOnKibble()
        {
            _eatAndDespawnSignal.Fire(this);
        }

        //this is a Pool, which allows this type of object to be reused
        public class Pool : MonoMemoryPool<KibbleFacade>
        {
            protected override void Reinitialize(KibbleFacade kibble)
            {
                base.Reinitialize(kibble);
                kibble.generateNewLocation();
            }
        }
    }
    
    /*this needs to be declared for zenject, but the signal will be used
    * to show when a kibble is sent to be fed to the dog
    */ 
    public class EatAndDespawnSignal : Signal<KibbleFacade, EatAndDespawnSignal>
    {

    }
}
