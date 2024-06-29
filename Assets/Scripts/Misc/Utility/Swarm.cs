// using UnityEngine;
// using System.Collections.Generic;

// public class Swarm : MonoBehaviour
// {
//     public List<Firefly> Flock;
//     [SerializeField] private float _centeringFactor;
//     [SerializeField] private float _matchingFactor;
//     [SerializeField] private float _avoidFactor;
//     [SerializeField] private float _turnFactor;
//     public float CenteringFactor { get { return _centeringFactor; } }
//     public float TurnFactor { get { return _turnFactor; } }
//     public float AvoidFactor { get { return _avoidFactor; } }
//     public float MatchingFactor { get { return _matchingFactor; } }

//     void Start()
//     {
//         Flock = new List<Firefly>();
//     }

//     public void AddFirefly(Firefly firefly)
//     {
//         if (Flock.Contains(firefly)) return;
//         Flock.Add(firefly);
//     }

//     public Vector3 GetMidpoint()
//     {
//         Vector3 midpoint = Vector3.zero;
//         int count = 0;
//         foreach (Firefly ff in Flock)
//         {
//             midpoint += ff.transform.position;
//             count++;
//         }
//         midpoint /= count;
//         Debug.Log(midpoint);
//         return midpoint;
//     }
// }