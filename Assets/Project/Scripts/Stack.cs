using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField] WhichColor whichColor;
    [SerializeField] CharacterControl character;
    [SerializeField] PlayerMovement player;
    [SerializeField] Renderer rend;
    Color characterColor;
    bool goBack = false;
    int droppedBrickCount = 0;
    List<Collactable> bricks = new List<Collactable>();
    public int ColorIndex { get { return (int)whichColor; } private set { } }
    public int DroppedBrickCount { get { return droppedBrickCount; } private set { } }
    public bool GoBack { get { return goBack; } private set { } }
    public List<Collactable> CollectedBricks { get { return bricks; } private set { } }

    private void Awake()
    {
        characterColor = rend.material.color;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.GameEnded)
        {
            if (other.gameObject.CompareTag("Collactable"))
                CollectBrick(other);
            if (other.CompareTag("Slot"))
                DropToSlot(other);
        }
    }
    private void DropToSlot(Collider other)
    {
        if (bricks.Count > 0)
        {
            Renderer mesh = other.GetComponent<Renderer>();
            if (characterColor != mesh.material.color)
            {
                mesh.enabled = true;
                mesh.material = rend.material;
                Collactable lastBrick = bricks[bricks.Count - 1];
                lastBrick.Drop();
                bricks.Remove(lastBrick);
                droppedBrickCount++;
            }
        }
        else
            goBack = true;
    }
    private void CollectBrick(Collider other)
    {
        Collactable col = other.gameObject.GetComponent<Collactable>();
        if (characterColor == col.GetColor() && !col.CollectSituation())
        {
            bricks.Add(col);
            col.Collect(bricks.Count, transform, "Untagged");
            if (character != null)
            {
                character.RemoveTarget(other.transform);
                if (character.Stage == 1)
                    SpawnManager.Instance.FirstStageCollactableSpawn((int)whichColor, other.transform.position, character);
                else
                    SpawnManager.Instance.SecondStageCollactableSpawn((int)whichColor, 1, character);
            }
            if (player != null)
            {
                if (player.Stage == 1)
                    SpawnManager.Instance.FirstStageCollactableSpawn((int)whichColor, other.transform.position);
                else
                    SpawnManager.Instance.SecondStageCollactableSpawn((int)whichColor, 1);
            }
        }
    }
    public void ClearStack()
    {
        foreach (Collactable brick in bricks)
        {
            brick.Drop();
        }
        bricks.Clear();
    }
    public void ResetDroppedBrickCount()
    {
        droppedBrickCount = 0;
    }
    public void Back()
    {
        goBack = false;
    }
}