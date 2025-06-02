# VRShootingGame
![Image](https://github.com/user-attachments/assets/9cb24873-8933-4e80-a637-1cecc1cc5c7a)
</br>
**Tank-Trun-Game**은 Unity 3D에서 제작된 X-COM 장르 게임입니다. **CSV/JSON**을 활용해 맵 구조와 적을 배치하였으며, **확장성 높은 FSM**을 통해 적 AI를 구현하였습니다. PC와 Mac 환경에서 플레이 가능하며, 메뉴 화면에서 **Start** 옵션으로 게임을 시작할 수 있습니다.
VR 슈팅 게임 입니다.
---

## 주요 기능 (Key Features)

- **적 AI**: 확장성 높은 FSM을 사용하여 플레이어를 추격/공격하는 적 AI 구현.
- **다양한 무기 시스템**: CSV/JSON을 사용한 다양한 맵, 스테이지 형식으로 클리어 여부 제공.
- **보스전 및 웨이브**: PC, Mac 환경에서 플레이 가능.
- **특수 능력**: Start 옵션으로 게임 시작 가능.
- **상호작용 가능한 오브젝트**:
- **VR IK 기능**:
---

## 플레이 방법 (How to Play)

1. **게임 설치 및 실행**:
   - PC에 게임을 설치하고 실행합니다.

2. **메뉴 화면**:
   - 게임 시작 시 메뉴 화면에서 **Start**(게임 시작하기)를 클릭하고 **Stage**(스테이지)를 선택합니다.

4. **설정(Setting)**:
   - 로비로 돌아갈 수 있으며 음악 볼륨을 조절할 수 있습니다.

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
- 플레이어의 공격, 이동 상태를 분리하기 위해 FSM을 사용했습니다.
- 기존에 사용하던 enum 방식의 FSM이 아닌 클래스를 만들어 딕셔너리 형태로 저장하는 FSM을 사용하여 더욱 확장성을 높일 수 있었습니다.
  
### 다양한 상호작용 오브젝트
<img width="1010" alt="Image" src="https://github.com/user-attachments/assets/65f794c3-8f48-4527-9538-e4d5802aadfd" />
- 엑셀을 활용해 맵의 크기, 적의 숫자 등을 설정하고 이를 JSON으로 파싱하여 간편하게 여러 맵을 구현할 수 있고
- 스테이지 형식으로 JSON을 불러와 맵을 체험할 수 있으며 Json으로 클리어 여부를 판단합니다.

### Ragdoll 적용 및 적 AI 구현
<img width="1010" alt="Image" src="https://github.com/user-attachments/assets/65f794c3-8f48-4527-9538-e4d5802aadfd" />
- 엑셀을 활용해 맵의 크기, 적의 숫자 등을 설정하고 이를 JSON으로 파싱하여 간편하게 여러 맵을 구현할 수 있고
- 스테이지 형식으로 JSON을 불러와 맵을 체험할 수 있으며 Json으로 클리어 여부를 판단합니다.

---
### Notion 
- [https://www.notion.so/14b9dfc52f178026b95cf9ace5814eb0?pvs=74](https://buly.kr/BeKLB2M)
### 참고 영상
- https://buly.kr/DEZA0Ok
---

## 라이선스 (License)

이 프로젝트는 MIT 라이선스 하에 배포됩니다. 자세한 내용은 `LICENSE` 파일을 참조하세요.
