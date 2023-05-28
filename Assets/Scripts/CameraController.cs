using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float targetDistance = 3f;

    [SerializeField]
    private float horizontalInputStrength;

    private Transform target = null;
    public Transform Target
    {
        get => target;
        set
        {
            if (!value)
                return;

            if(target)
                target.eulerAngles = Vector3.zero;

            target = value;
            transform.SetParent(target, true);
        }
    }

    public static CameraController Instance { get; set; }

    private Vector3 targetCameraPos => new Vector3(0f, 3f, -targetDistance);

    private Vector3 previousInput = Vector2.zero;

    private void Awake()
    {
        // Terrible singleton management hahaha
        Instance = this;
    }

    // I don't like using Input class anymore either, or Update for that matter, but this was faster ;)
    private void Update()
    {
        if (!Target)
            return;

        if (Input.GetMouseButtonDown(0))
            previousInput = Input.mousePosition;


        if (Input.GetMouseButton(0))
        {
            var input = Input.mousePosition;

            var deltaInput = previousInput - input;

            float horizontalInput = deltaInput.x * horizontalInputStrength;
            Target.eulerAngles = Vector3.Lerp(Target.eulerAngles, Target.eulerAngles + new Vector3(0f, horizontalInput, 0f), 0.01f);
            previousInput = input;
        }

        // Kinda ew, but works
        Vector3 rot = transform.eulerAngles;
        transform.LookAt(Target);
        Vector3 newRot = transform.eulerAngles;

        Vector3 targetRot = new Vector3
            (
            Mathf.LerpAngle(rot.x, newRot.x, 0.015f),
            Mathf.LerpAngle(rot.y, newRot.y, 0.015f),
            Mathf.LerpAngle(rot.z, newRot.z, 0.015f)
        );

        transform.eulerAngles = targetRot;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetCameraPos, 0.015f);
    }


}
