using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOBSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> gravityObjectPrefab;
    [SerializeField] int numberOfObjectsToSpawn;
    public Vector3 spawnAreaSize = new Vector3(10f, 10f, 10f);
    public Vector3 spawnAreaPosition = Vector3.zero;
    public Collider2D exclusionArea;    
    public float minDistanceBetweenObjects;

    private Transform objectsParent;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        objectsParent = new GameObject("GravityOBSpawner").transform;
        StartCoroutine(UpdateObjectPositions());
    }

    IEnumerator UpdateObjectPositions()
    {
        while (true)
        {
            UpdateGravityObjectPositions();
            yield return new WaitForSeconds(10f);
        }
    }

    void UpdateGravityObjectPositions()
    {
        // 기존에 생성된 블랙홀을 제거
        foreach (var obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear();

        Vector3 spawnAreaCenter = transform.position + spawnAreaPosition;

        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            int prefabIndex = Random.Range(0, gravityObjectPrefab.Count);
            GameObject selectedPrefab = gravityObjectPrefab[prefabIndex];

            Vector3 spawnPosition;
            do
            {
                spawnPosition = new Vector3(
                    Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
                    Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2),
                    Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2)
                );
            } while (IsInExclusionArea(spawnPosition) || IsTooCloseToOtherObjects(spawnPosition));

            GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
            newObject.transform.parent = objectsParent;

            spawnedObjects.Add(newObject);
        }
    }

    bool IsInExclusionArea(Vector3 position)
    {
        if (exclusionArea != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 5f);
            foreach (var collider in colliders)
            {
                if (collider == exclusionArea)
                {
                    return true;
                }
            }
        }

        return false;
    }

    bool IsTooCloseToOtherObjects(Vector3 position)
    {
        foreach (var obj in spawnedObjects)
        {
            if (Vector3.Distance(position, obj.transform.position) < minDistanceBetweenObjects)
            {
                return true;
            }
        }

        return false;
    }


}
