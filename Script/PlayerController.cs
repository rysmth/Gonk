using Rewired;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Ryan Joshua Smith Gonk Keyboard Player Script

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    private Rigidbody playerRigidbody; // Automates alot, used for movement, collisions etc
    private int requiredCharacter; // Easier to seperate arrays instead of repeating code for multiple variables
    public int playerNumber; // Splits up input and stops the duplicate script from overwriting/ repeating output

    [Header("Respawn")]
    [SerializeField] Transform respawnPoint; // Position the player is moved too after falling off platform
    [SerializeField] AudioSource respawnSFX; // Audio indicator of respawn
    public int lifeCount = 3; // Best amount of lives for short bursts of gameplay

    [Header("Attack")]
    [SerializeField] Animator attackAnimation; // Visual Indicator of attack
    [SerializeField] AudioSource attackSFX; // Audio to emphasise attack colliding
    [SerializeField] bool canAttack; // Prevents spamming the attack button, used to create a timedown/ tackle attacking
    [SerializeField] float pushForce;  // Used to determine the amount to push other players away so the attack actually does something (benefit)
    [SerializeField] Transform hammerModel; // Used so the attack actually occurs in the direction the player is facing
    [SerializeField] CapsuleCollider hammerCollider; // Make Inactive to prevent random, unfair triggers

    [Header("Movement")]
    private bool canJump; // Used to prevent double jumping and or jumping forever
    [SerializeField] float moveSpeed; // Determines how fast the player can move (alterable in inspector)
    [SerializeField] float jumpImpulse; // Determines how far player can jump (alterable)
    public Transform playerModel; // Used for movement and model rotation to represent direction facing better/ realistic
    private Vector3 newPosition;
    private bool isMoving; // Simply used to determine whether the movement sound effect should stop for polish/ realism

    [Header("Effects")]
    [SerializeField] ParticleSystem movementParticleSystem; // Visual effect for movement
    [SerializeField] AudioSource movementSFX; // Audio indicator that the player is indeed moving and for immersion
    [SerializeField] AudioSource jumpSFX;

    [Header("HUD")]
    [SerializeField] TMP_Text playerLives; // Lets the player know how many lives they have
    public Image playerIcon; // Reminds and shows what character they are controlling
    [SerializeField] GameObject playerIconGameObject; // Placement of the Icon to change

    [SerializeField] Sprite[] characterAliveIcons; // Icons of the characters to change to when the player has over 0 lives
    [SerializeField] Sprite[] characterDeadIcons; // Swapped to icons of characters to visually show they are out of lives

    [Header("Characters")]
    public GameObject[] characterPrefabs; // The list of prefabs made of the game ready models to spawn

    [Header("Rewired")]
    public int playerId;
    private Rewired.Player player;


    //-----------------------------------Appropriate Icon, Model, & RB-------------------------
    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);

        requiredCharacter = CharacterSelect.activeCharacter[playerNumber]; // Gets the corresponding character type from the previous character select screen

        playerIcon.sprite = characterAliveIcons[requiredCharacter]; // Updates player's icon to the appropriate one depicting their character

        if (playerNumber == 2 && CharacterSelect.isPlayer3Active == false)
        {
            playerIconGameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        if (playerNumber == 3 && CharacterSelect.isPlayer4Active == false)
        {
            playerIconGameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        Instantiate(characterPrefabs[requiredCharacter], playerModel.transform); // Spawning the corresponding character model to the correct position

        playerRigidbody = GetComponent<Rigidbody>(); // Simply finds the rb that's on the gameobject the script is attached to

        canJump = true;
        canAttack = true;
    }

    //-----------------------------------Activating Attack & Jump Behaviour-------------------------
    private void Update()
    {
        Vector2 moveInput = GetMoveInput(playerNumber); // Control input specific to the player number typed into the inspector (stops duplicate outcome)

        Move(moveInput);

        // Controllers
        if (playerNumber == 2 && CharacterSelect.isPlayer3Active == true)
        {
            if (player.GetButtonDown("Jump"))
            {
                if (canJump == true)
                {
                    Jump();
                }
            }

            if (player.GetButtonDown("Attack"))
            {
                if (canAttack == true)
                {
                    Attack();
                }
            }
        }

        if (playerNumber == 3 && CharacterSelect.isPlayer4Active == true)
        {
            if (player.GetButtonDown("Jump"))
            {
                if (canJump == true)
                {
                    Jump();
                }
            }

            if (player.GetButtonDown("Attack"))
            {
                if (canAttack == true)
                {
                    Attack();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = GetMoveInput(playerNumber); // Control input specific to the player number typed into the inspector (stops duplicate outcome)

        Move(moveInput);

        if (playerNumber == 0)
        {
            if (Input.GetKey(KeyCode.Space)) // Change for jumping key (Player 1)
            {
                if (canJump == true)
                {
                    Jump();
                }
            }

            if (Input.GetKey(KeyCode.E)) // Player 1 attacking key
            {
                if (canAttack == true)
                {
                    Attack();
                }
            }
        }

        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.RightControl)) // Player 2 Jumping key
            {
                if (canJump == true)
                {
                    Jump();
                }
            }

            if (Input.GetKey(KeyCode.RightShift)) // Player 2 key for attacking
            {
                if (canAttack == true)
                {
                    Attack();
                }
            }
        }

        if (canJump == false)
            {
                movementSFX.Stop(); 
            }
        }

        //-----------------------------------Determining How Input Affects Movement-------------------------
        Vector2 GetMoveInput(int playerNum)
        {
            Vector2 input = Vector2.zero;

            if (playerNum == 0) // Player one movement input
            {
                if (Input.GetKey(KeyCode.W)) // Up Vertically 
                {
                    input.y += 1;
                }

                if (Input.GetKey(KeyCode.S)) // Down Vertically 
                {
                    input.y -= 1;
                }

                if (Input.GetKey(KeyCode.A)) // Left horizontally 
                {
                    input.x -= 1;
                }

                if (Input.GetKey(KeyCode.D)) // Right horizontally 
                {
                    input.x += 1;
                }
            }

            else if (playerNum == 1) // Player two movement input
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    input.y += 1;
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    input.y -= 1;
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    input.x -= 1;
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    input.x += 1;
                }
            }

        else if (playerNum == 2 || playerNum == 3)
        {
            input.x = player.GetAxisRaw("MoveHorizontal");
            input.y = player.GetAxisRaw("MoveVertical");
        }

            return input.normalized;
        }

        //-----------------------------------Player Movement-------------------------
        void Move(Vector2 moveInput)
        {
            newPosition = transform.position;

            if (moveInput.magnitude > 0.1f)
            {
                if (!isMoving && canJump)
                {
                    movementParticleSystem.Play();
                    movementSFX.Play();
                }

                isMoving = true;

                Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.deltaTime; // Determines movement for player

                // Smooth Model rotation
                Quaternion newTurnVal = Quaternion.LookRotation(new Vector3(-moveInput.x, 0f, -moveInput.y));
                transform.rotation = Quaternion.Lerp(transform.rotation, newTurnVal, 0.2f);

                hammerModel.rotation = Quaternion.Lerp(hammerModel.rotation, newTurnVal, 0.2f); // Rotates the attack to direction facing

                newPosition += movement;

                playerRigidbody.MovePosition(newPosition); // Begins moving the player using rb to the newly determined location
            }

            else // Overkill for stopping effects but makes sure it works 
            {
                if (isMoving)
                {
                    movementParticleSystem.Stop();
                    movementSFX.Stop();
                }

                isMoving = false;
            }
        }
    

    //-----------------------------------Attacking & Jumping-------------------------
    private void Attack()
    {
        canAttack = false;
        hammerCollider.enabled = true;
        attackAnimation.SetTrigger("AttackTrig"); // Visual indicator of attack to player/s
        attackSFX.Play(); // Audio que of attack to alert players 
        StartCoroutine(DelayAttack()); // Delays player from unfairly spamming attack
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(1.5f); // Delays the player from attacking again
        attackAnimation.ResetTrigger("AttackTrig"); // Resetting the animation of the attack
        canAttack = true; // Letting the player now attack
        hammerCollider.enabled = false;
    }

    private void Jump()
    {
        playerRigidbody.AddRelativeForce(0.0f, jumpImpulse, 0.0f, ForceMode.Impulse); // Making the player go up for the jump
        canJump = false; 
        jumpSFX.pitch = Random.Range(0.5f, 1.2f); // Prevents more annoying repetiveness through pseudo-random pitches
        jumpSFX.Play(); // Plays said sound effect for jumping
    }

    //-----------------------------------How To Behave With Specific Collisions-------------------------
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("OutofBounds")) // The invisible collider for when the player falls off the platform
        {
            lifeCount--; // Removes a life
            playerLives.text = lifeCount.ToString(); // Shows the player how many lives they have available

            if (lifeCount <= 0) // Hides the player's model upon death and swaps their icon to the dead one
            {
                gameObject.SetActive(false);
                playerIcon.sprite = characterDeadIcons[requiredCharacter];
            }

            gameObject.transform.position = respawnPoint.transform.position; // Moves player to the platform so they can play more
            playerRigidbody.velocity = Vector3.zero; // Reset Movement so player cannot fall off again
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true; // Player is allowed to jump once they collide with the platform
            movementParticleSystem.Play(); // Activates the particle system that emphasises the player interacting with the world
        }

        if (collision.gameObject.CompareTag("Attack")) // Behaviour for colliding with the hammer (launch off stage)
        {
            Vector3 attackPosition = collision.transform.position;

            Vector3 horizontalDir = (transform.position - attackPosition);
            horizontalDir.y = 0f;
            horizontalDir = horizontalDir.normalized;

            Vector3 launchForce = horizontalDir * pushForce + Vector3.up * (pushForce * 0.8f);

            playerRigidbody.AddForce(launchForce, ForceMode.Impulse);
        }
    }
}