# IoT 샘플 패키지

이 패키지는 Unity 내에서 IoT 디바이스 통합을 간소화하기 위해 설계되었으며, Unity GameObject를 IoT 텔레메트리 데이터와 같은 외부 데이터 소스에 연결할 수 있는 모듈형 프레임워크를 제공합니다. 여러 외부 시스템으로부터 텔레메트리 데이터를 수신, 처리 및 시각화하는 과정의 복잡성을 추상화하여, 개발자가 연결성 문제보다는 애플리케이션의 핵심 로직과 사용자 경험에 집중할 수 있도록 합니다.

이 패키지는 오픈소스 M2MQTT 라이브러리와의 통합을 통해 MQTT 통신을 지원하며, MQTT 브로커와의 양방향 통신을 가능하게 합니다. 또한 Microsoft Azure SDK를 활용한 Azure IoT Hub 네이티브 지원을 제공합니다. 아키텍처는 확장 가능하여, 개발자가 추가 IoT 프로토콜이나 맞춤형 REST API를 간단한 플러그인 인터페이스를 통해 쉽게 추가할 수 있습니다.

통신 로직을 추상화하고 서드파티 확장을 위한 플러그인 시스템을 제공함으로써, 개발자가 저수준 IoT 통신 및 메시지 분배를 직접 구현할 필요가 없어집니다. 이를 통해 개발자는 개발 속도를 가속화하고 Unity 내에서 몰입감 있고 데이터 기반의 IoT 경험을 만드는 데 집중할 수 있습니다.


## IoT 샘플 패키지 설치 가이드

이 가이드는 MQTT 브로커 또는 Azure EventHub로부터 메시지를 수신할 수 있는 IoT 디바이스를 설정하는 방법을 안내합니다.

### 시작하기

MQTT 연결을 빠르게 설정하고 Unity 씬에 IoT 디바이스를 통합합니다.

#### MQTT 커넥터 추가

1. **Project** 탭에서 패키지 폴더를 찾습니다.  
2. **MQTT Connector** 프리팹을 씬에 드래그합니다.  
3. **Inspector**에서 MQTT Connector 프리팹에 MQTT 브로커 정보를 입력합니다.  

**예시 브로커:**
- **Host:** `localhost`  
- **Port:** `1883`  
- **Topic:** `devices/updates`  

#### IoT 디바이스 생성

씬 내의 모든 GameObject는 메시지를 수신하는 IoT 디바이스로 설정할 수 있습니다.

1. 씬에 새 GameObject를 생성합니다.  
2. GameObject에 **GenericDevice** 컴포넌트를 추가합니다.  
   이 컴포넌트는 기본 핫스팟을 제공하여 수신된 텔레메트리 데이터를 일반 UI 창에 표시합니다.  
3. **GenericDevice** 컴포넌트의 **Device Id**를 Unity로 메시지를 보낼 IoT 디바이스 ID와 일치하도록 설정합니다.  

#### 핫스팟 추가

IoT 디바이스에 핫스팟 요소를 할당하여 사용자 입력을 처리하고 텔레메트리 데이터를 노출할 수 있습니다.

1. 새로 생성한 IoT 디바이스의 **GenericDevice** 컴포넌트를 찾습니다.  
2. `IoTSamplePackage/Runtime/Prefabs` 폴더에서 **DeviceHotspot** 프리팹을 가져와 **GenericDevice** 컴포넌트의 `Hotspot Prefab` 항목에 추가합니다.  

#### 컨텍스트 UI 추가

1. `IoTSamplePackage/Runtime/Prefabs` 폴더에서 **TelemetryCanvas** 프리팹을 씬에 추가합니다.  
2. **GenericContextWindow**의 위치를 원하는 대로 조정합니다.  
3. 초기에는 **GenericContextWindow** 프리팹을 비활성화하여 숨깁니다. 핫스팟 클릭 시 표시됩니다.  
4. IoT GameObject의 **GenericDevice** 컴포넌트에서 창 이름을 방금 추가한 컨텍스트 윈도우 프리팹 이름으로 설정합니다.  
5. **Create > Assets > UI > EventSystem**을 통해 **EventSystem** 객체를 씬에 추가합니다.  


### IoT 디바이스 테스트

이제 Unity에서 IoT 디바이스를 성공적으로 생성하고 설정했습니다.  

Unity Editor에서 **Play** 버튼을 클릭하면 IoT 시스템으로부터 텔레메트리 메시지를 수신하기 시작합니다.  


## 플러그인 추가

IoT 샘플 패키지는 오픈소스 M2MQTT 라이브러리를 통한 MQTT 통신과 Azure IoT Hub 통합을 기본적으로 지원합니다. 이러한 라이브러리는 NuGet Unity Package Manager를 통해 Unity 프로젝트에 쉽게 추가할 수 있습니다.  

편의를 위해 M2MQTT와 Azure Messaging EventHubs 라이브러리는 패키지의 Plugins 폴더에도 포함되어 있습니다.  

기본 제공 라이브러리 외에도 IoT 샘플 패키지는 BestMQTT, RocWorks와 같은 서드파티 MQTT 통합을 지원합니다. 이는 특히 WebGL 애플리케이션처럼 기본 M2MQTT 라이브러리가 요구사항을 충족하지 못하는 경우에 유용합니다.  

기본적으로 샘플 패키지는 MQTT 통신에 M2MQTT 라이브러리를 사용합니다. Azure IoT Hub 또는 다른 플러그인을 활성화하려면 프로젝트 설정에서 조건부 컴파일 플래그를 사용할 수 있습니다.  

Microsoft Azure 통합을 활성화하려면 **Project Settings > Player > Scripting Define Symbols**로 이동하여 **USE_AZURE**를 추가합니다. 그러면 빌드에 Azure 라이브러리가 포함됩니다.  

Azure 플래그가 설정되면 IoT 샘플 패키지의 **Runtime > Plugins > Azure** 폴더에서 Azure Connector 프리팹을 선택할 수 있습니다.  

Azure 커넥터 구현은 Azure IoT Event Hub 엔드포인트에 연결하여 IoT 디바이스 상태 업데이트와 텔레메트리 정보를 실시간으로 수신합니다.  
