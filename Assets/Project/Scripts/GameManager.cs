using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    [SerializeField] PlayerMovement player;
    [SerializeField] CharacterControl redCharacter;
    [SerializeField] CharacterControl blueCharacter;
    [SerializeField] CameraFollow cam;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject restartButton;
    bool gameEnded = false;
    public bool GameEnded { get { return gameEnded; } private set { } }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void EndGame(int index)
    {
        if (gameEnded)
            return;
        gameEnded = true;
        switch (index)
        {
            case 0:
                redCharacter.End(0);
                player.End(2);
                blueCharacter.End(-2);
                cam.End();
                break;
            case 1:
                redCharacter.End(2);
                player.End(0);
                blueCharacter.End(-2);
                cam.End();
                break;
            case 2:
                redCharacter.End(-2);
                player.End(2);
                blueCharacter.End(0);
                cam.End();
                break;
        }
        restartButton.SetActive(true);
    }
    public void Restart()
    {
        gameEnded = true;
        gameOverPanel.SetActive(true);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}