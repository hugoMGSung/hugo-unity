## Unity Learn

### Unity 핵심 기초

- [Unity Tutorial](https://learn.unity.com/tutorial/unity-editeo-ihae?uv=2020.3&courseId=614c0b2fedbc2a47200dce44&projectId=614bdef2edbc2a3d83d75f48#)
    - 내 Tutorial1&2_GettingStarted.zip 사용

    ![alt text](image-1.png)

    - 이동 : 마우스 오른쪽 버튼 + WSAD
    - 회전 : 마우스 오른쪽 버튼

- UnityStudy01 : 오브젝트 선택 

    ![alt text](image.png)

- 카메라 프로젝션
    1. Perspective
    2. Orthographic

#### 물리엔진

- Collider : 콜라이더 

    ![alt text](image-2.png)

- Trigger : 트리거
    - 오브젝트가 트리거 영역에 들어가거나 나올 때 이벤트 호출

- RigidBody : 리지드바디
    - 중력 프로퍼티

- Collision Detection : 출동 탐지
    - 스크립트

    ```cs
    // 충돌이 인식되면
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            // Hit the enemy
        }
    }

    // 충돌 중에 호출
    void OnCollisionStay(Collision collision) {
        // ...
    }

    // 충돌이 정지하면 호출
    void OnCollisionExit(Collision collision) {
        // ...
    }

    // 중력 활성화/비활성화
    bool rigidBody.useGravity;

    // 특정방향으로 힘 추가
    *rigidBody.AddForce(Vector3);

    // 축 주위 회전력 추가
    rigidbody.AddTorque(Vector3, Force Mode)
    ```

- Force Mode : 힘 모드
    - Acceleration : 일정한 비율로 증가하는 힘 적용
    - Force : 기본 설정, 질량을 고려하여 힘이 점진적으로 적용
    - Impulse : 순간적인 힘 적용
    - VelocityChange : 다양한 방향으로 순간적인 힘 적용. 질량을 무시됨

- Update 함수
    - Update : 프레임당 한 번 호출
    - FixedUpdate : 프레임당 여러번 호출. 물리 계산 등 

- 물리 머티리얼 : 표면 마찰 등 다른 표면과 상호작용 방식 제어

- 물리 조인트 : 리지드 바디를 또 다른 리지드 바디나 공간 내 정해진 지점에 연결. 관절등의 기능 구현에 사용

- 래이 캐스팅 : 물리 오브젝트와 다른 오브젝트 간의 광선 또는 보이지 않는 연결을 캐스트

#### 머티리얼

- 질감 표현

    ![alt text](image-3.png)
    
- 머티리얼 중요 파라미터
    - Rendering Mode : Opaque | Transparent
    - Albedo : 색상, 텍스처
    - Metalic : 금속 질감
        - Smootheness
    - ...


    ![alt text](image-4.png)


#### 프리팹

- 완전히 완성된 게임 오브젝트. 재사용 가능



### 유니티 실습

#### 존 레몬의 공포체험 

![alt text](image-5.png)

![alt text](image-6.png)

- 캐릭터 조정 : 플레이 상태에서 확인

    ![alt text](image-7.png)

    ![alt text](image-8.png)

- 프리팹 변환 : 하이라키에서 프로젝트 프리팹으로 드래그

    ![alt text](image-9.png)