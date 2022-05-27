using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] int nextStage;
    Transform slotsParent;
    int bricks;

    private void Awake()
    {
        slotsParent = transform.parent;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!GameManager.Instance.GameEnded)
        {
            if (other.CompareTag("Stack"))
                CheckSlots(other);
        }
    }
    private void CheckSlots(Collider other)
    {
        Stack stack = other.GetComponent<Stack>();
        PlayerMovement player = other.gameObject.GetComponentInParent<PlayerMovement>();
        CharacterControl character = other.gameObject.GetComponentInParent<CharacterControl>();
        if (stack.DroppedBrickCount >= slotsParent.childCount && player != null)
        {
            player.ChangeStage(nextStage);
            SpawnManager.Instance.SecondStageCollactableSpawn(stack.ColorIndex, 10);
            stack.ResetDroppedBrickCount();
        }
        else if (stack.DroppedBrickCount >= slotsParent.childCount && character != null)
        {
            character.StopAgent();
            character.RemoveAllTargets();
            character.ChangeStage(nextStage);
            SpawnManager.Instance.SecondStageCollactableSpawn(stack.ColorIndex, 10, character);
            character.AgentDestination();
            stack.ResetDroppedBrickCount();
        }
    }
}