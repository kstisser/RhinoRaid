using System;
using System.Collections.Generic;
using Zenject;

namespace KibbleSpace
{
    /*
     * This class manages the pools. It can have more than one pool. It is
     * convenient to be able to obtain information from all of the active 
     * objects of a certain type, like how many there are.
     * 
     * 
     */
    public class PoolManager : ILateDisposable
    {
        //pool of kibbles, allowing kibble objects to be reused
        readonly KibbleFacade.Pool _kibblePool;
        //list of all active kibbles
        readonly List<KibbleFacade> _kibbles = new List<KibbleFacade>();
        readonly TextManager _textManager;
        readonly EatAndDespawnSignal _despawnSignal;
        readonly DeadDogSignal _deadDogSignal;

        public PoolManager(KibbleFacade.Pool kPool, EatAndDespawnSignal despawnSignal, DeadDogSignal deadSignal, TextManager textManager)
        {
            _kibblePool = kPool;
            _textManager = textManager;

            _despawnSignal = despawnSignal;
            _despawnSignal.Listen(RemoveKibble);

        }

        public void AddKibble()
        {
            _kibbles.Add(_kibblePool.Spawn());
        }

        public void RemoveKibble(KibbleFacade k)
        {
            _kibblePool.Despawn(k);
            _kibbles.Remove(k);
        }

        public int getActiveCount()
        {
            return _kibbles.Count;
        }

        public void clearPool()
        {
            foreach(KibbleFacade kibble in _kibbles)
            {
                RemoveKibble(kibble);
            }
        }

        public void LateDispose()
        {
            _despawnSignal.Unlisten(RemoveKibble);
        }
    }
}
