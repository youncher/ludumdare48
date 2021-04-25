using UnityEngine;
using System.Collections;

public class RandomMovementController : MonoBehaviour {

    public float movementSpeed = 5f;
    private CardinalRenderController cardinalRenderer;

    Rigidbody2D rbody;
    private float decisionCadence = 0.8f; //sec
    private float sinceLastDecision = 0.0f;
    private float stateChangeProbability = 0.75f;
    private static int numAxes = 3; //left/right/down
    private float axisProbability = 1.0f/numAxes; // probability of choosing left/right/down
    private float maxPressDuration = 3.5f;

    private float horizontalPressDuration = 0.0f;
    private float verticalPressDuration = 0.0f;
    protected InputBroker inputBroker = new InputBroker();

    private bool isMoving = false;
    private Vector2 target;
    private Vector2 startPos;

    private void Awake()
    {
        // rbody = GetComponent<Rigidbody2D>();
        cardinalRenderer = GetComponentInChildren<CardinalRenderController>();
        target = new Vector2(transform.position.x, transform.position.y);
    }
    private void updatePosition() {
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        if (!isMoving) {
            sendInput();
            float horizontalInput = inputBroker.GetAxis("Horizontal");
            float verticalInput = inputBroker.GetAxis("Vertical");
            Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
            inputVector = Vector2.ClampMagnitude(inputVector, 1);
            var destination = currentPos + inputVector;
            if (Gameboard.Validate((int)destination.x, (int)destination.y)) {
                Gameboard.Vacate((int)currentPos.x, (int)currentPos.y);
                target = currentPos + inputVector;
                Gameboard.Occupy((int)target.x, (int)target.y);
                cardinalRenderer.SetDirection(target);
            }
        }

        float step = movementSpeed * Time.deltaTime;

        if (currentPos != target) {
            isMoving = true;
            // Debug.Log($"now moving to target {target} from {transform.position}");
            transform.position = Vector2.MoveTowards(transform.position, target, step);
        } else {
            isMoving = false;
        }
    }

    protected virtual void sendInput()
    {
        randomInput();
    }

    protected void randomInput()
    {
        if (sinceLastDecision > decisionCadence) {
            bool positionChange = Random.Range(0.0f, 1.0f) < stateChangeProbability;
            if (positionChange) {
                inputBroker.clearInputs();
                float randomAxis = Random.Range(0.0f, 1.0f);
                if (randomAxis < axisProbability) { // left
                    verticalPressDuration = 0.0f;
                    if (!inputBroker.HorizontalPressed()) {
                        inputBroker.setHorizontal(-1.0f);
                    } else {
                        inputBroker.setHorizontal(0.0f);
                    }
                } else if (randomAxis < (2 * axisProbability)) { // right
                    verticalPressDuration = 0.0f;
                    if (!inputBroker.HorizontalPressed()) {
                        inputBroker.setHorizontal(1.0f);
                    } else {
                        inputBroker.setHorizontal(0.0f);
                    }
                } else { // down
                    horizontalPressDuration = 0.0f;
                    if (!inputBroker.VerticalPressed()) {
                        inputBroker.setVertical(-1.0f);
                    } else {
                        inputBroker.setVertical(0.0f);
                    }
                }
            }
            sinceLastDecision = 0.0f;
        }

        if (horizontalPressDuration > maxPressDuration) {
            inputBroker.setHorizontal(0.0f);
            sinceLastDecision = 0.0f;
        }
        if (verticalPressDuration > maxPressDuration) {
            inputBroker.setVertical(0.0f);
            sinceLastDecision = 0.0f;
        }
    }
    void Update () {
        sinceLastDecision += Time.deltaTime;
        if (inputBroker.HorizontalPressed()) {
            horizontalPressDuration += Time.deltaTime;
        } else {
            horizontalPressDuration = 0;
        }
        if (inputBroker.VerticalPressed()) {
            verticalPressDuration += Time.deltaTime;
        } else {
            verticalPressDuration = 0;
        }
        updatePosition();
    }

    public bool IsMoving() {
        return isMoving;
    }

    public Vector2 Target() {
        return target;
    }
}