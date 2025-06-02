# VRShootingGame
![Image](https://github.com/user-attachments/assets/9cb24873-8933-4e80-a637-1cecc1cc5c7a)
</br>
**Tank-Trun-Game**은 Unity 3D에서 제작된 X-COM 장르 게임입니다. **CSV/JSON**을 활용해 맵 구조와 적을 배치하였으며, **확장성 높은 FSM**을 통해 적 AI를 구현하였습니다. PC와 Mac 환경에서 플레이 가능하며, 메뉴 화면에서 **Start** 옵션으로 게임을 시작할 수 있습니다.
VR 슈팅 게임 입니다.
---

## 주요 기능 (Key Features)

- **적 AI**: 확장성 높은 FSM을 사용하여 플레이어를 추격/공격하는 적 AI 구현 적의 상태(예: 무기 보유 여부)에 따라 다른 행동을 수행, 각 부위에 콜라이더 및 데미지 계수를 설정하여 명중 위치에 따라 피해량 차등 적용.
- **다양한 상호작용 아이템**: Pistol, Machine Gun, Grenade 등 다양한 적 공격 수단 구현, 힐팩, 탄창, 총알, 적 등 다양한 상호작용 요소
- **보스 AI**: 각 패턴은 개별 쿨타임을 가지며, LINQ를 활용해 쿨타임이 끝난 패턴만 선택적으로 발동하도록 구현.
- **특수 능력**: 사용 시 잔상 이펙트 (Mesh 베이킹), 화면 연출, Time.timeScale 조절로 슬로우 효과 연출
- **VR IK 기능**: VR 컨트롤러의 위치와 회전을 기반으로 캐릭터 양손이 자연스럽게 따라가도록 구현, 손에 **IK**를 적용하여 플레이어의 몰입감을 극대화함.
---

## 플레이 방법 (How to Play)

1. **게임 설치 및 실행**:
   - PC에 게임을 설치하고 실행합니다.

2. **메뉴 화면**:
   - 게임 시작 시 메뉴 화면에서 **Start**(게임 시작하기)를 클릭하고 **Stage**(스테이지)를 선택합니다.
3. **인 게임**:
   - 몰려오는 적을 막으며 최대한 높은 점수를 얻으세요!   
---

## 개발 환경 (Development Environment)

| 항목              | 내용                          |
|-------------------|------------------------------|
| **Engine**          | Unity 3D                     |
| **Language**          | C#                           |
| **Platform**        | PC, Mac                 |
| **Dependencies**     | Oculus          |

---

## IK 기능을 사용한 자유로운 VR 캐릭터 조종 및 다양한 상호작용 구현  

### 플레이어 이동, VR IK 적용
<img width="919" alt="Image" src="https://github.com/user-attachments/assets/8d89f448-a108-4be2-8484-810bf3a6b6d4" />
<img width="943" alt="Image" src="https://github.com/user-attachments/assets/ad6ba180-417f-492b-9492-678636cc14f7" />
- VR 컨트롤러의 위치와 회전을 기반으로 캐릭터 양손이 자연스럽게 따라가도록 구현.
- 손에 **IK**를 적용하여 플레이어의 몰입감을 극대화함.
  
### 다양한 상호작용 오브젝트
<img width="1010" alt="Image" src="https://github.com/user-attachments/assets/65f794c3-8f48-4527-9538-e4d5802aadfd" />
- 인터페이스를 사용하여 효율적인 상호작용 기능을 구현했으며 장착 시 Grab Pose를 사용, 장착 중 사용 기능, 장착 해제 시 던지는 기능 등을 구현하였고 각 오브젝트 별로 다르도록 구현하였습니다.

### Ragdoll 적용 및 적 AI 구현
<img width="1010" alt="Image" src="https://github.com/user-attachments/assets/65f794c3-8f48-4527-9538-e4d5802aadfd" />
- 적의 공격, 이동 상태를 분리하기 위해 FSM을 사용했습니다 적의 상태(예: 무기 보유 여부)에 따라 다른 행동을 수행하도록 구현하였습니다. 
- 기존에 사용하던 enum 방식의 FSM이 아닌 클래스를 만들어 딕셔너리 형태로 저장하는 FSM을 사용하여 더욱 확장성을 높일 수 있었습니다.
- 적에게 래그돌을 적용하여 던지거나 휘두를 때 자연스럽게 관절이 날아감.
- 각 부위에 콜라이더 및 데미지 계수를 설정하여 명중 위치에 따라 피해량 차등 적용.
- 각 패턴은 개별 쿨타임을 가지며, LINQ를 활용해 쿨타임이 끝난 패턴만 선택적으로 발동하도록 구현.

---
### Notion 
- [https://www.notion.so/14b9dfc52f178026b95cf9ace5814eb0?pvs=74](https://buly.kr/BeKLB2M)
### 참고 영상
- https://buly.kr/DEZA0Ok
---

## 라이선스 (License)

이 프로젝트는 MIT 라이선스 하에 배포됩니다. 자세한 내용은 `LICENSE` 파일을 참조하세요.
