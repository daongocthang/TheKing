using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private GameObject enemyGroupPrefab;

        private Camera mainCamera;
        private BoxCollider2D cameraBoxCollider;

        private void Awake()
        {
            this.mainCamera = Camera.main;
            this.cameraBoxCollider = GameObject.Find("Bounds").GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            SpawnEnemyGroup(12, 0);
        }

        private Vector3 RandomSpawnPosition()
        {
            var cameraBounds = this.cameraBoxCollider.bounds;
            var center = cameraBounds.center;
            var size = cameraBounds.size;
            var bounds = new Bounds(center, size);
            var num = 0;
            Vector3 vector;
            bool inViewport;
            do
            {
                vector = new Vector3(Random.Range(-65.5f, 65.5f), Random.Range(-36.5f, 36.5f));
                var viewportPoint = this.mainCamera.WorldToViewportPoint(vector);
                inViewport = viewportPoint.x >= -0.1 && viewportPoint.x <= 1.1 && viewportPoint.y >= -0.1 &&
                             viewportPoint.y >= 1.1;
                num++;
            } while (num < 10000 && (!bounds.Contains(vector) || inViewport));

            return vector;
        }

        private void Spawn(GameObject parent, Vector3 position, int id)
        {
            Object.Instantiate<GameObject>(this.enemyPrefabs[id], position, Quaternion.identity,
                parent.transform);
        }

        private void SpawnEnemies(GameObject enemyGroup, int numOfPlayers, int playerLevel)
        {
            var num = 0.5f;
            var numOfEnemies = 6;
            var parentPosition = enemyGroup.transform.position;
            Spawn(enemyGroup, parentPosition, playerLevel);
            while (numOfPlayers > 0)
            {
                var distance = Random.Range(0, numOfEnemies);
                for (var i = 0; i < numOfEnemies; i++)
                {
                    if (Random.Range(1, 11) >= 7) continue;
                    numOfPlayers--;
                    if (numOfPlayers < 0) break;
                    var pos = ((i + distance) * 2) * 3.1415927f / numOfEnemies;
                    var dx = Mathf.Sin(pos) * num;
                    var dy = Mathf.Cos(pos) * num;
                    var newPosition = new Vector3(parentPosition.x + dx, parentPosition.y + dy, parentPosition.z);
                    Spawn(enemyGroup, newPosition, playerLevel);
                }

                num += 0.5f;
                numOfEnemies += 6;
            }
        }

        private void SpawnEnemyGroup(int numOfGroups, int playerLevel)
        {
            for (var i = 0; i < numOfGroups; i++)
            {
                var enemyGroup = Object.Instantiate<GameObject>(this.enemyGroupPrefab, this.RandomSpawnPosition(),
                    Quaternion.identity, base.transform);
                SpawnEnemies(enemyGroup, Random.Range(1, 10), playerLevel);
            }
        }
    }
}