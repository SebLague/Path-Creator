using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

namespace PathCreation.Examples {

    public class PathSpawner : MonoBehaviour {

        public PathCreator pathPrefab;
        public PathFollower followerPrefab;
        public Transform[] spawnPoints;

        void Start () {
            foreach (Transform t in spawnPoints) {
                var path = Instantiate (pathPrefab, t.position, t.rotation);
                var follower = Instantiate (followerPrefab);
                follower.pathCreator = path;
            }
        }
    }

}