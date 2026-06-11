# Unity Factory 실습

## Unity Factory 에셋 가져오기

### Part 1. Unity Factory 에셋 소개

![alt text](image-23.png)

Unity Technologies Japan에서 공개한 무료 공장 환경 에셋입니다.

특징은 다음과 같습니다.

* 무료
* HDRP 지원
* 실제 제조공장 분위기 제공
* 생산라인 예제 포함
* 산업용 로봇 포함
* 디지털트윈 실습에 적합

### 핵심정보


| 항목            | 내용                            |
| ----------------- | --------------------------------- |
| 이름            | **Unity Factory**               |
| 퍼블리셔        | **UJ Unity Technologies Japan** |
| 가격            | **무료**                        |
| 파일 크기       | **882 MB**                      |
| 최신 버전       | **1.0**                         |
| 출시일          | **2024년 3월 25일**             |
| 원본 Unity 버전 | **2022.3.16**                   |
| 렌더 파이프라인 | **HDRP만 호환**                 |
| Built-in / URP  | 호환 안 됨                      |

### Part 2. HDRP 프로젝트 생성

#### 새 프로젝트 만들기

Unity Hub 실행

![alt text](image-24.png)

새 프로젝트 클릭

![alt text](image-25.png)

High Definition 3D 선택 후 진행

---

### Part 3. Unity Factory Import

#### Asset Store 추가

가장 간단한 방법,

https://assetstore.unity.com/packages/3d/environments/industrial/unity-factory-276400 웹사이트에서 **내 애셋에 추가** 버튼 클릭 후 **Unity에서 열기** 로 진행

#### HDRP Wizard 확인

![alt text](img_20260611_001.jpg)

- Convert All Built-In Materials to HDRP 확인 필요

#### Import

![alt text](image-26.png)

#### Project 창

1. UnityFactorySceneHDRP > Scene_Factory 클릭
2. FactorySceneSample 클릭
3. Unity Editor 재시작
4. Spline 오류 해결
   1. Package Manager > Unity Register 에서 `Splines` 검색 후 설치
5. Project Settings
   1. Player > Other Settings > Active Input Handling 을 Both 로 선택, 재시작
6. Hierarchy 창
   1. Global Volume 선택 Inspector > Global Volume 이름 앞 체크박스 해제
   2. Player와 RobotArm White Glow 해결

![](assets\20260611_194859_image.png)

### Part 4. 공장 씬 둘러보기

#### Play

- WSAD 키와 마우스 오른쪽 버튼으로 이동

![](assets\20260611_194928_image.png)

#### 주요 구성 요소

![alt text](image-27.png)

Hierarchy에서 확인

```
Factory
```

생산라인

```
Conveyor
```

컨베이어

```
Arm
```

산업용 로봇

```
Machine
```

제조 장비

```
Worker
```

작업자

#### 공장 구조 분석

생산라인 흐름

```
입고
 ↓
컨베이어
 ↓
가공장비
 ↓
검사장비
 ↓
출고
```

---

## Unity Factory 기반 스마트팩토리 디지털트윈 튜토리얼

### 프로젝트 목표

최종적으로 아래와 같은 시스템을 만듭니다.

```

┌─────────────────────┐
│ Raspberry Pi Sensor │
└──────────┬──────────┘
           │ MQTT
           ▼
┌─────────────────────┐
│ MQTT Broker         │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Unity Factory       │
├─────────────────────┤
│ Conveyor Belt       │
│ Product Box         │
│ Robot Arm           │
│ Dashboard           │
└─────────────────────┘
```

### Part 1. 새 디지털트윈 씬 만들기

#### 기존 씬 복사

현재 샘플 씬은 건드리지 않습니다.

FactorySceneSample 씬 복사


#### 불필요한 오브젝트 제거

남길 것

```
Building
Light
Skybox
Reflection Probe
Global Volume
```

제거

```
Animation Sample
Worker
Product
Controller
```


### Part 2. 생산라인 설계

#### 공정 구조

우리가 만들 공정

```
투입
 ↓
컨베이어
 ↓
센서
 ↓
로봇암
 ↓
출고
```

계속