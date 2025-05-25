# hugo-unity
유니티 학습 리포지토리

## 유니티 기본 학습

### 인터페이스

- 인터페이스 설명

    <img src="./image/unity0001.png" width="200">

    - Center/Pivot 
        - Center : 여러 개의 오브젝트를 선택했을 때, `전체의 중심`을 기준으로 조작
        - Pivot : 각 오브젝트의 피벗, 즉 원래 설정된 중심점을 기준으로 이동/회전/스케일 조작

    - Local/Global
        - Local : 오브젝트 자체의 로컬 좌표계를 기준으로
        - Global : 월드 좌표게 기준으로

    - 손모양 부터 아래로
        - View Tool : (손모양)화면이동 (Q)
        - Move Tool : 오브젝트 이동 (W)
        - Rotate Tool : 오브젝트 회전 (E)
        - Scale Tool : 오브젝트 크기 (R)
        - Rect Tool : 2D 오브젝트나 UI요소에 사용 (T) 
        - Transform Tool : 이동+회전+스케일을 하나로 통합한 툴 (Y)

- 다음 인터페이스

    <img src="./image/unity0002.png" width="150">

    - Play : 게임 실행(Ctrl + P)
    - Pause : 게임 일시정지 (Ctrl + Shift + P)
    - Step : 1스텝 실행(Pause일 경우) (Ctrl + Alt + P)

### 서바이벌 FPS 클로닝

1. 프로젝트 생성 후
2. Assets에 필요 에셋 붙여넣기

    <img src="./image/unity0003.png" width="600">

3. 3D Object > Plane 추가

4. 3D Object > Capsule 추가 후 Main Camera를 Capsule을 이름 바꾼 Player에 추가
    - Player의 Position Y도 1로 변경
    - Main Camera의 Transform을 Reset 후 Position Y를 1로 변경
    - Player의 Add Component로 RigidBody 추가

5. Assets 아래에 Scripts 폴더 생성
    - PlayerController.cs 생성
    - VS에서 작성

6. Unity Editor에서 Player에 PlayerController.cs 드래그



### 유니티를 이용한 하이브리드 캐주얼게임 클로닝


### Unity ML-Agents
- Unity Machine Learning Agents Toolkit(ML-Agents)은 게임과 시뮬레이션을 지능형 에이전트 훈련 환경으로 활용할 수 있도록 하는 오픈소스 프로젝트
- PyTorch 기반의 최첨단 알고리즘 구현을 제공하여 게임 개발자와 취미 개발자들이 2D, 3D 및 VR/AR 게임용 지능형 에이전트를 쉽게 훈련할 수 있도록 지원

#### ML-Agents 다운로드
- https://github.com/Unity-Technologies/ml-agents
- ML-Agents Release 22 다운로드

#### Python 가상환경 활성화
- Python 3.10 버전에 맞추기
- 파이썬 가상환경

    ```shell
    > python -m venv mlagent-env
    > pip install mlagents
    ```

- 확인

    ```shell
    > pip show mlagents
    ...
    > pip show mlagents-envs
    ...
    ```

#### Unity Hub
- Add > Add project from disk 선택
- {압축경로}\ml-agents-release_22\Project 선택
- 자신의 Unity버전에 맞게 변경 오픈
- ML-Agents 플러그인 적용
    - Menu Window > Package Manager > 왼쪽 상단 + > Install package from disk...
    - com.unity.ml-agents / com.unity.ml-agents.extensions 의 package.json 파일 선택
    

## 유니티 강화학습

### 강화학습 이론
- 기계학습의 종류
    - 지도학습, 비지도학습, `강화학습(Reinforcement Learning)`

- 강화학습의 예시
    - 에이전트와 환경 : 자전거를 처음 배우는 사람과 자전거를 타는 환경
    - 패널티 : 넘어지거나 충돌이 발생하면 패널티
    - 보상 : 자전거를 잘타면 보상
    - 다수의 시도를 통ㅎ애 점점 넘어지지 않고 자전거 타는 것이 성공하도록 반복 학습

- 강화학습 적용분야
    - 자율주행, 금융, 게임, 로봇, 드론 등에 적용 가능

### 기초 용어
- MDP(Markov Decision Process)
    - 순차적으로 행동을 결정하는 문제를 풀기 위한 기법
    - 상태(s), 행동(a), 보상함수(R/a/s), 상태변환확률(P/a/ss), 감가율(γ)
- Agent
    - 강화학습에서 의사결정을 하는 역할. 제어의 대상이 되는 주인공
- Environment
    - 에이전트의 의사결정을 반영하고, 에이전트에게 정보를 주는 시스템
    
- Observateion
    - 관측. 환경에서 제공하는 정보
- State
    - 상태. 에이전트는 상태를 기반으로 의사경정을 수행
- Action
    - 행동. 에이전트가 의사결정을 통해 취할 수 있는 행동
- Step & Episode
    - 스텝. 에이전트가 한번 행동을 취함
    - 에피소드. 게임 한판이 끝남
- Policy
    - 정책. 특정상태에서 취할 수 있는 행동을 선택할 확률분포
- State Transiiton Function
    - 상태 변환 확률. 상태에서 행동을 했을 때 다음상태가 될 확률
- Reward Function
    - 보상함수. 에이전트는 특정상태에서 특정행동을 했을 때 이에 해당하는 보상을 받음

- ... 생략
- Bellman Equation
    - 벨만 방정식. 현재 상태의 가치함수와 다음 상태의 가치함수 사이의 관계를 나타낸 식


### ML-Agents 설치
- 생략

### 3D Ball
- 머리위의 공이 떨어지지 않도록 박스헤드를 제어하는 환경

