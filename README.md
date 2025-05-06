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



## 유니티를 이용한 하이브리드 캐주얼게임 클로닝

