using System.Reflection;
using UnityEngine;

namespace UnityFactorySceneHDRP
{
    public class ArmIK : MonoBehaviour
    {
        // 로봇팔 받침대
        // 좌우(Y축) 회전 담당
        [SerializeField] private Transform _stand;

        // 첫 번째 관절(어깨)
        [SerializeField] private Transform _arm1;

        // 두 번째 관절(팔꿈치)
        [SerializeField] private Transform _arm2;

        // 끝단(Effector)
        [SerializeField] private Transform _arm3;

        [Space(10)]

        // IK 계산 기준 좌표계
        [SerializeField] private Transform _arm1Base;

        // 로봇팔이 따라갈 목표 위치
        [SerializeField] private Transform _target;

        // 상완 길이
        private float _upperArmLength;

        // 하완 길이
        private float _foreArmLength;

        private void Awake()
        {
            // Arm1 → Arm2 거리
            _upperArmLength =
                Vector3.Distance(
                    _arm1.position,
                    _arm2.position);

            // Arm2 → Arm3 거리
            _foreArmLength =
                Vector3.Distance(
                    _arm2.position,
                    _arm3.position);
        }

        private void Update()
        {
            //---------------------------------------------------
            // 1. 받침대 회전
            //---------------------------------------------------

            // Target을 향하는 방향 계산

            float arm1Angle =
                Mathf.Atan2(
                    (_target.position - _stand.position).x,
                    (_target.position - _stand.position).z
                ) * Mathf.Rad2Deg;

            // 받침대를 Target 방향으로 회전

            _stand.rotation =
                Quaternion.Euler(
                    0,
                    arm1Angle - 90,
                    0
                );

            //---------------------------------------------------
            // 에디터에서 길이 변경 시 즉시 반영
            //---------------------------------------------------

            #if UNITY_EDITOR
            _upperArmLength =
                Vector3.Distance(
                    _arm1.position,
                    _arm2.position);

            _foreArmLength =
                Vector3.Distance(
                    _arm2.position,
                    _arm3.position);
            #endif

            //---------------------------------------------------
            // 2. Target을 로컬 좌표계로 변환
            //---------------------------------------------------

            Vector2 targetLocalPos =
                _arm1Base.InverseTransformPoint(
                    _target.position);

            // Target까지 거리
            float targetDistance =
                targetLocalPos.magnitude;

            //---------------------------------------------------
            // 3. 도달 가능한 경우
            //---------------------------------------------------

            if (targetDistance <
                _upperArmLength + _foreArmLength)
            {
                //------------------------------------------------
                // 역기구학(IK)
                //------------------------------------------------

                // 목표점 방향 각도

                float angleA =
                    Mathf.Asin(
                        targetLocalPos.y /
                        targetDistance
                    ) * Mathf.Rad2Deg;

                // 어깨 각도 계산

                float angleB =
                    Mathf.Acos(
                        (
                            _upperArmLength *
                            _upperArmLength +

                            targetDistance *
                            targetDistance -

                            _foreArmLength *
                            _foreArmLength
                        )
                        /
                        (
                            2 *
                            _upperArmLength *
                            targetDistance
                        )
                    ) * Mathf.Rad2Deg;

                // 팔꿈치 각도 계산

                float angleC =
                    Mathf.Acos(
                        (
                            _upperArmLength *
                            _upperArmLength +

                            _foreArmLength *
                            _foreArmLength -

                            targetDistance *
                            targetDistance
                        )
                        /
                        (
                            2 *
                            _upperArmLength *
                            _foreArmLength
                        )
                    ) * Mathf.Rad2Deg;

                //------------------------------------------------
                // 관절 회전 적용
                //------------------------------------------------

                // 어깨

                _arm1.localRotation =
                    Quaternion.Euler(
                        0,
                        0,
                        -(90 - (angleA + angleB))
                    );

                // 팔꿈치

                _arm2.localRotation =
                    Quaternion.Euler(
                        0,
                        0,
                        -(180 - angleC)
                    );
            }
            else
            {
                //------------------------------------------------
                // 4. 도달 불가능한 경우
                //------------------------------------------------

                float angleA =
                    Mathf.Asin(
                        targetLocalPos.y /
                        targetDistance
                    ) * Mathf.Rad2Deg;

                // 최대한 Target 방향으로 뻗음

                _arm1.localRotation =
                    Quaternion.Euler(
                        0,
                        0,
                        -(90 - angleA)
                    );

                // 팔꿈치 펴기

                _arm2.localRotation =
                    Quaternion.identity;
            }
        }
    }
}