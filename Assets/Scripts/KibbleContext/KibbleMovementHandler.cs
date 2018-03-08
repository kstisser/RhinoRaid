using UnityEngine;

namespace KibbleSpace
{
    public class KibbleMovementHandler
    {
        public Vector3 generateRandomLocation()
        {
            return new Vector3(Random.Range(-0.5f,1f),Random.Range(-0.5f,0.5f),Random.Range(3.5f,8f));
        }
    }
}
