using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

    [SerializeField]
    private Button left;

    [SerializeField]
    private Button right;

    [SerializeField]
    private Button test;

    [SerializeField]
    private Stacker[] stackers;

    private int currentStack = 0;

    private void Awake()
    {
        left.onClick.AddListener(() => IncrementStack(-1));
        right.onClick.AddListener(() => IncrementStack(1));
        test.onClick.AddListener(TestMyStack);

        currentStack = 0;

        Invoke(nameof(InitializeCamera), 0.1f);
    }

    private void InitializeCamera()
    {
        CameraController.Instance.Target = stackers[currentStack].CameraTarget;
    }

    private void TestMyStack()
    {
        stackers[currentStack].TestMyStack();
    }

    private void IncrementStack(int direction)
    {
        var newStack = Mathf.Clamp(currentStack + direction, 0, stackers.Length - 1);

        if (newStack == currentStack)
            return;

        currentStack = newStack;
        CameraController.Instance.Target = stackers[currentStack].CameraTarget;
    }
}
