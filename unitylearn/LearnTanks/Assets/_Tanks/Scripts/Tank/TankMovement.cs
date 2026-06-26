using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Tanks.Complete
{
    [DefaultExecutionOrder(-10)]
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;
        public float m_Speed = 12f;
        public float m_TurnSpeed = 180f;
        public bool m_IsDirectControl;

        public AudioSource m_MovementAudio;
        public AudioClip m_EngineIdling;
        public AudioClip m_EngineDriving;
        public float m_PitchRange = 0.2f;

        public bool m_IsComputerControlled = false;

        [HideInInspector]
        public TankInputUser m_InputUser;

        public Rigidbody Rigidbody => m_Rigidbody;

        public int ControlIndex { get; set; } = -1;

        private Rigidbody m_Rigidbody;
        private float m_MovementInputValue;
        private float m_TurnInputValue;
        private Vector3 m_ExplosionForceValue;
        private float m_OriginalPitch;
        private ParticleSystem[] m_particleSystems;

        private InputAction m_MoveAction;
        private InputAction m_TurnAction;

        private bool m_UseVector2MoveAction;
        private Vector3 m_RequestedDirection;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();

            m_InputUser = GetComponent<TankInputUser>();
            if (m_InputUser == null)
                m_InputUser = gameObject.AddComponent<TankInputUser>();
        }

        private void OnEnable()
        {
            if (m_Rigidbody != null)
                m_Rigidbody.isKinematic = false;

            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;
            m_ExplosionForceValue = Vector3.zero;

            m_particleSystems = GetComponentsInChildren<ParticleSystem>();

            for (int i = 0; i < m_particleSystems.Length; ++i)
                m_particleSystems[i].Play();
        }

        private void OnDisable()
        {
            if (m_Rigidbody != null)
                m_Rigidbody.isKinematic = true;

            if (m_particleSystems == null)
                return;

            for (int i = 0; i < m_particleSystems.Length; ++i)
                m_particleSystems[i].Stop();
        }

        private void Start()
        {
            if (m_IsComputerControlled)
            {
                var ai = GetComponent<TankAI>();
                if (ai == null)
                    gameObject.AddComponent<TankAI>();
            }

            if (ControlIndex == -1 && !m_IsComputerControlled)
                ControlIndex = m_PlayerNumber;

            var mobileControl = FindAnyObjectByType<MobileUIControl>();

            //if (mobileControl != null && ControlIndex == 1)
            //{
            //    m_InputUser.SetNewInputUser(InputUser.PerformPairingWithDevice(mobileControl.Device));
            //    m_InputUser.ActivateScheme("Gamepad");
            //}
            //else
            //{
            //    // Unity 6 기본 Control Scheme
            //    m_InputUser.ActivateScheme("Keyboard&Mouse");
            //}
            if (mobileControl != null && ControlIndex == 1)
            {
                m_InputUser.SetNewInputUser(InputUser.PerformPairingWithDevice(mobileControl.Device));
                m_InputUser.ActivateScheme("Gamepad");
            }
            else
            {
                // Player1 = KeyboardLeft
                // Player2 = KeyboardRight
                m_InputUser.ActivateScheme(ControlIndex == 1 ? "KeyboardLeft" : "KeyboardRight");
            }

            SetupInputActions();

            if (m_MovementAudio != null)
                m_OriginalPitch = m_MovementAudio.pitch;
        }

        private void SetupInputActions()
        {
            if (m_InputUser == null || m_InputUser.ActionAsset == null)
            {
                Debug.LogError("TankInputUser 또는 ActionAsset이 없습니다.");
                return;
            }

            // Unity 6 기본 Input Actions에서는 Move가 Vector2인 경우가 많음
            m_MoveAction = m_InputUser.ActionAsset.FindAction("Move", false);

            if (m_MoveAction != null)
            {
                m_UseVector2MoveAction = true;
                m_MoveAction.Enable();
                return;
            }

            // 기존 Tanks 방식 fallback
            m_MoveAction = m_InputUser.ActionAsset.FindAction("Vertical", false);
            m_TurnAction = m_InputUser.ActionAsset.FindAction("Horizontal", false);

            if (m_MoveAction == null)
            {
                Debug.LogError("Move 또는 Vertical 액션을 찾지 못했습니다.");
                return;
            }

            if (m_TurnAction == null)
            {
                Debug.LogError("Horizontal 액션을 찾지 못했습니다.");
                return;
            }

            m_UseVector2MoveAction = false;
            m_MoveAction.Enable();
            m_TurnAction.Enable();
        }

        private void Update()
        {
            if (!m_IsComputerControlled)
            {
                ReadMovementInput();
            }

            if (m_MovementAudio != null)
            {
                EngineAudio();
            }
        }

        private void ReadMovementInput()
        {
            if (m_MoveAction == null)
                return;

            if (m_UseVector2MoveAction)
            {
                Vector2 move = m_MoveAction.ReadValue<Vector2>();

                m_MovementInputValue = move.y;
                m_TurnInputValue = move.x;
            }
            else
            {
                m_MovementInputValue = m_MoveAction.ReadValue<float>();

                if (m_TurnAction != null)
                    m_TurnInputValue = m_TurnAction.ReadValue<float>();
            }
        }

        private void EngineAudio()
        {
            if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
            {
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(
                        m_OriginalPitch - m_PitchRange,
                        m_OriginalPitch + m_PitchRange
                    );
                    m_MovementAudio.Play();
                }
            }
            else
            {
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(
                        m_OriginalPitch - m_PitchRange,
                        m_OriginalPitch + m_PitchRange
                    );
                    m_MovementAudio.Play();
                }
            }
        }

        private void FixedUpdate()
        {
            bool isGamepad = IsCurrentScheme("Gamepad");

            if (isGamepad || m_IsDirectControl)
            {
                if (Camera.main != null)
                {
                    var camForward = Camera.main.transform.forward;
                    camForward.y = 0;

                    if (camForward.sqrMagnitude < 0.0001f)
                    {
                        camForward = Camera.main.transform.up;
                        camForward.y = 0;
                    }

                    camForward.Normalize();
                    var camRight = Vector3.Cross(Vector3.up, camForward);

                    m_RequestedDirection =
                        camForward * m_MovementInputValue +
                        camRight * m_TurnInputValue;

                    if (m_RequestedDirection.sqrMagnitude > 0.0001f)
                        m_RequestedDirection.Normalize();
                }
            }

            Move();
            Turn();
        }

        private bool IsCurrentScheme(string schemeName)
        {
            if (m_InputUser == null)
                return false;

            if (!m_InputUser.InputUser.controlScheme.HasValue)
                return false;

            return m_InputUser.InputUser.controlScheme.Value.name == schemeName;
        }

        private void Move()
        {
            float speedInput;

            if (IsCurrentScheme("Gamepad") || m_IsDirectControl)
            {
                speedInput = m_RequestedDirection.magnitude;

                if (m_RequestedDirection.sqrMagnitude > 0.0001f)
                {
                    speedInput *= 1.0f - Mathf.Clamp01(
                        (Vector3.Angle(m_RequestedDirection, transform.forward) - 90) / 90.0f
                    );
                }
            }
            else
            {
                speedInput = m_MovementInputValue;
            }

            Vector3 movement = transform.forward * speedInput * m_Speed;

            m_Rigidbody.linearVelocity = movement + m_ExplosionForceValue;
            m_ExplosionForceValue = Vector3.Lerp(
                m_ExplosionForceValue,
                Vector3.zero,
                Time.deltaTime * 3f
            );
        }

        private void Turn()
        {
            Quaternion turnRotation;

            if (IsCurrentScheme("Gamepad") || m_IsDirectControl)
            {
                if (m_RequestedDirection.sqrMagnitude < 0.0001f)
                    return;

                float angleTowardTarget =
                    Vector3.SignedAngle(m_RequestedDirection, transform.forward, transform.up);

                float rotatingAngle =
                    Mathf.Sign(angleTowardTarget) *
                    Mathf.Min(Mathf.Abs(angleTowardTarget), m_TurnSpeed * Time.deltaTime);

                turnRotation = Quaternion.AngleAxis(-rotatingAngle, Vector3.up);
            }
            else
            {
                float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
                turnRotation = Quaternion.Euler(0f, turn, 0f);
            }

            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }

        public void AddExplosionForce(
            float explosionForce,
            Vector3 explosionPosition,
            float explosionRadius,
            float upwardsModifier = 0f)
        {
            Vector3 explosionDir = transform.position - explosionPosition;
            float explosionDistance = explosionDir.magnitude;

            if (upwardsModifier != 0)
            {
                explosionDir.y += upwardsModifier;
                explosionDir.Normalize();
            }
            else
            {
                explosionDir = explosionDir.normalized;
            }

            float attenuation = 1f - Mathf.Clamp01(explosionDistance / explosionRadius);

            Vector3 velocityChange = explosionDir * (explosionForce * attenuation);

            m_ExplosionForceValue = velocityChange;
        }
    }
}