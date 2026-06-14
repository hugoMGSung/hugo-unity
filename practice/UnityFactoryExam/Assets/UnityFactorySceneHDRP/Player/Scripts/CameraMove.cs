using UnityEngine;

namespace UnityFactorySceneHDRP
{
    public class CameraMove : MonoBehaviour
    {
        // 플레이어 충돌과 이동을 담당하는 CharacterController
        [SerializeField] private CharacterController _characterController;

        // 플레이어 전체의 기준 Transform
        // 좌우 회전(Yaw)은 이 오브젝트가 담당
        [SerializeField] private Transform _playerRoot;

        // 실제 카메라 Transform
        // 위아래 회전(Tilt)은 카메라가 담당
        [SerializeField] private Transform _camera;

        [Space(10)]

        // 기본 이동 속도
        [SerializeField] private float _moveSpeed = 2;

        // 마우스 회전 속도
        [SerializeField] private float _rotateSpeed = 2;

        [Space(10)]

        // 플레이어가 내려갈 수 있는 최소 Y 위치
        // 바닥 아래로 떨어지는 것을 방지
        [SerializeField] private float _minWorldY;

        // 좌우 회전값
        private float _yaw = 0;

        // 위아래 카메라 회전값
        private float _tilt = 0;

        // 빠른 이동 모드 여부
        private bool _isRunning = false;

        // 걷기 모드 여부
        // true  = 걷기 모드
        // false = 비행 모드
        private bool _isWalkMode = true;

        private void Awake()
        {
            // 현재 플레이어의 Y 회전값을 저장
            _yaw = _playerRoot.eulerAngles.y;

            // 현재 카메라의 X 회전값을 저장
            _tilt = _camera.localEulerAngles.x;
        }

        private void Update()
        {
            // =========================
            // 1. 마우스 우클릭 회전
            // =========================

            // 마우스 오른쪽 버튼을 누르고 있을 때만 시점 회전
            if (Input.GetMouseButton(1))
            {
                // 마우스 좌우 이동값으로 플레이어를 좌우 회전
                _yaw += Input.GetAxis("Mouse X") * _rotateSpeed;

                // 마우스 위아래 이동값으로 카메라를 상하 회전
                // 마우스를 위로 올리면 화면이 위를 보도록 - 처리
                _tilt -= Input.GetAxis("Mouse Y") * _rotateSpeed;

                // 너무 위/아래로 돌아가서 뒤집히지 않도록 제한
                _tilt = Mathf.Clamp(_tilt, -89, 89);

                // 플레이어 전체는 Y축만 회전
                _playerRoot.eulerAngles = new Vector3(0, _yaw, 0);

                // 카메라는 X축만 회전
                _camera.localEulerAngles = new Vector3(_tilt, 0, 0);
            }

            // =========================
            // 2. 이동 입력 받기
            // =========================

            // Horizontal = A/D 또는 ←/→
            // Vertical   = W/S 또는 ↑/↓
            Vector3 dir = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            );

            // Q/E 키로 카메라 높이 조절
            // Q = 아래
            // E = 위
            // Mathf.Max(0, ...)는 카메라 높이가 0 아래로 내려가지 않게 함
            float height = Mathf.Max(
                0,
                _camera.localPosition.y +
                (
                    (Input.GetKey(KeyCode.Q) ? -_moveSpeed : 0) +
                    (Input.GetKey(KeyCode.E) ? _moveSpeed : 0)
                ) * Time.deltaTime
            );

            // =========================
            // 3. 빠른 이동 모드 전환
            // =========================

            // Shift를 한 번 누를 때마다 빠른 이동 ON/OFF
            // 누르고 있는 동안만 빠른 이동이 아니라 토글 방식
            if (Input.GetKeyDown(KeyCode.LeftShift) ||
                Input.GetKeyDown(KeyCode.RightShift))
            {
                _isRunning = !_isRunning;
            }

            // =========================
            // 4. 걷기 모드 이동
            // =========================

            if (_isWalkMode)
            {
                // 입력 방향을 플레이어가 바라보는 방향 기준으로 변환
                // 예: 플레이어가 오른쪽을 보고 있으면 W는 오른쪽 방향 이동
                dir = Quaternion.Euler(
                    0,
                    _playerRoot.localEulerAngles.y,
                    0
                ) * dir;

                // CharacterController의 SimpleMove 사용
                // SimpleMove는 내부적으로 Time.deltaTime과 중력을 처리함
                _characterController.SimpleMove(
                    dir * _moveSpeed * (_isRunning ? 3 : 1)
                );

                // 걷기 모드에서는 Q/E로 카메라 눈높이만 변경
                _camera.localPosition = new Vector3(0, height, 0);
            }
            else
            {
                // =========================
                // 5. 비행 모드 이동
                // =========================

                // 카메라가 바라보는 방향까지 포함해서 이동 방향 계산
                // 위를 보고 W를 누르면 위쪽으로 날아감
                dir = Quaternion.Euler(
                    _camera.localEulerAngles.x,
                    _playerRoot.localEulerAngles.y,
                    _camera.localEulerAngles.z
                ) * dir;

                // CharacterController.Move 사용
                // Move는 Time.deltaTime을 직접 곱해줘야 함
                _characterController.Move(
                    dir * _moveSpeed * (_isRunning ? 3 : 1) * Time.deltaTime
                );
            }

            // =========================
            // 6. 바닥 아래로 떨어지는 것 방지
            // =========================

            // 플레이어 위치가 최소 Y값보다 낮아지면
            // 강제로 최소 Y값으로 올림
            if (_playerRoot.position.y < _minWorldY)
            {
                Vector3 position = _playerRoot.position;
                position.y = _minWorldY;
                _playerRoot.position = position;
            }

            // =========================
            // 7. 걷기 / 비행 모드 전환
            // =========================

            // F 키를 누르면 걷기 모드와 비행 모드 전환
            if (Input.GetKeyDown(KeyCode.F))
            {
                _isWalkMode = !_isWalkMode;

                if (_isWalkMode)
                {
                    // 비행 모드 → 걷기 모드

                    // 플레이어 위치를 바닥 높이로 내림
                    _playerRoot.position = new Vector3(
                        _playerRoot.position.x,
                        _minWorldY,
                        _playerRoot.position.z
                    );

                    // 카메라는 사람 눈높이 정도로 올림
                    _camera.localPosition = new Vector3(0, 1.5f, 0);
                }
                else
                {
                    // 걷기 모드 → 비행 모드

                    // 현재 카메라의 월드 Y 위치를 플레이어 위치로 사용
                    _playerRoot.position = new Vector3(
                        _playerRoot.position.x,
                        _camera.position.y,
                        _playerRoot.position.z
                    );

                    // 카메라는 Player Root 위치와 일치시킴
                    _camera.localPosition = Vector3.zero;
                }
            }
        }
    }
}