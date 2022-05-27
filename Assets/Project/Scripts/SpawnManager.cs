using UnityEngine;

public enum WhichColor
{
    red = 0,
    green = 1,
    blue = 2
}
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance = null;

    [SerializeField] GameObject redCollactablePrefab;
    [SerializeField] GameObject greenCollactablePrefab;
    [SerializeField] GameObject blueCollactablePrefab;
    [SerializeField] Transform redCollactablesParent;
    [SerializeField] Transform greenCollactablesParent;
    [SerializeField] Transform blueCollactablesParent;
    [SerializeField] LayerMask layer;
    [SerializeField] float firstStageX;
    [SerializeField] float firstStageY;
    [SerializeField] float firstStageZ;
    [SerializeField] float secondStageX;
    [SerializeField] float secondStageY;
    [SerializeField] float secondStageMinZ;
    [SerializeField] float secondStageMaxZ;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void FirstStageCollactableSpawn(int colorNumber, Vector3 positionToRemove, CharacterControl character = null)
    {
        switch (colorNumber)
        {
            case 0:
                GenerateCollactable(redCollactablePrefab, redCollactablesParent, character, firstStageX, firstStageY, -firstStageZ, firstStageZ);
                break;
            case 1:
                GenerateCollactable(greenCollactablePrefab, greenCollactablesParent, character, firstStageX, firstStageY, -firstStageZ, firstStageZ);
                break;
            case 2:
                GenerateCollactable(blueCollactablePrefab, blueCollactablesParent, character, firstStageX, firstStageY, -firstStageZ, firstStageZ);
                break;
        }
    }
    public void SecondStageCollactableSpawn(int colorNumber, int spawnCount, CharacterControl character = null)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            switch (colorNumber)
            {
                case 0:
                    GenerateCollactable(redCollactablePrefab, redCollactablesParent, character, secondStageX, secondStageY, secondStageMinZ, secondStageMaxZ);
                    break;
                case 1:
                    GenerateCollactable(greenCollactablePrefab, greenCollactablesParent, character, secondStageX, secondStageY, secondStageMinZ, secondStageMaxZ);
                    break;
                case 2:
                    GenerateCollactable(blueCollactablePrefab, blueCollactablesParent, character, secondStageX, secondStageY, secondStageMinZ, secondStageMaxZ);
                    break;
            }
        }
    }
    private void GenerateCollactable(GameObject prefab, Transform parent, CharacterControl character, float posX, float posy, float posMinZ, float posMaxZ)
    {
        GameObject brick = Instantiate(prefab, parent);
        Vector3 desirePosition = RandomBrickPos(posX, posy, posMinZ, posMaxZ);
        brick.SetActive(false);
        Collider[] hitColliders = Physics.OverlapSphere(desirePosition, 1, layer);
        while (hitColliders.Length != 0)
        {
            desirePosition = RandomBrickPos(posX, posy, posMinZ, posMaxZ);
            hitColliders = Physics.OverlapSphere(desirePosition, 1, layer);
        }
        brick.transform.position = desirePosition;
        brick.SetActive(true);
        if (character != null)
            character.AddTarget(brick.transform);
    }
    private Vector3 RandomBrickPos(float x, float y, float minZ, float maxZ)
    {
        return new Vector3(Random.Range(-x, x), y, Random.Range(minZ, maxZ));
    }
}