using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform cameraParent;
    [SerializeField] float speed;
    [SerializeField] Vector3 eulerAngles;
    void Update()
    {
        cameraParent.Rotate(eulerAngles * speed * Time.deltaTime);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}