# VRShootingGame

![Image](https://github.com/user-attachments/assets/9cb24873-8933-4e80-a637-1cecc1cc5c7a)

### 몰려오는 로봇들을 다양한 방법으로 파괴하여  
### **마을을 지켜주세요!**

---

## 프로젝트 소개

**VR SHOOTING GAME**은 VR 기반 1인칭 슈팅 게임으로,  
다양한 무기를 활용해 적을 처치하고  
처치 방식에 따라 점수를 획득하여 **최고 점수 갱신**을 목표로 하는 게임입니다.

---

## 기획 및 제작 의도

**사이버펑크 세계관과 _Robo Recall_ 에서 영감을 받아 제작**하였으며,  
플레이어가 적을 처치하는 **다양한 방식**을 통해  
창의적인 플레이를 유도하도록 기획했습니다.

- 처치 방식에 따른 점수 차별화
- 반복 플레이에 재미를 더하는 점수 시스템

---

## 프로젝트 정보

- **장르**: FPS / 1인칭 VR 슈팅
- **개발 인원**: 1명 (게임 클라이언트)
- **개발 기간**: 2025.04.28 ~ 2025.05.16

---

## 참고 자료

- **Notion**
  - [https://www.notion.so/14b9dfc52f178026b95cf9ace5814eb0?pvs=74](https://root-xylocarp-b3c.notion.site/VR-SHOORING-GAME-1ff9dfc52f1780388d94e6f3745b54cf?pvs=74)

- **참고 영상**
  - https://buly.kr/DEZA0Ok

---

## 주요 기능 (Key Features)

- **적 AI**
  - 확장성 높은 FSM 기반 추격 / 공격 AI
  - 적 상태(무기 보유 여부 등)에 따른 행동 분기
  - 부위별 콜라이더 및 데미지 계수 적용

- **다양한 상호작용 아이템**
  - Pistol, Machine Gun, Grenade 등 다양한 무기 구현
  - 힐팩, 탄창, 총알, 적 등 다양한 상호작용 요소

- **보스 AI**
  - 각 패턴별 개별 쿨타임 적용
  - LINQ를 활용해 쿨타임이 끝난 패턴만 선택적으로 발동

- **특수 능력 연출**
  - Mesh 베이킹 기반 잔상 이펙트
  - Time.timeScale 조절을 활용한 슬로우 연출

- **VR IK 시스템**
  - VR 컨트롤러 위치·회전에 기반한 양손 IK
  - 자연스러운 손 움직임으로 몰입감 강화

---

## 개발 환경 (Development Environment)

| 항목 | 내용 |
|----|----|
| Engine | Unity 3D |
| Language | C# |
| Platform | PC / Mac |
| Dependencies | Oculus |

---

# 기능 구현 상세

## VR IK 기반 플레이어 조작

### 플레이어 이동 & VR IK 적용
<img width="175" src="https://github.com/user-attachments/assets/96433834-cfeb-49b0-b91a-fa385a1acdcb" />

- VR 컨트롤러의 위치·회전을 기반으로 양손 IK 적용
- 손 움직임이 자연스럽게 따라오도록 구현
- VR 몰입감 극대화

---

## 다양한 상호작용 오브젝트
<img width="359" src="https://github.com/user-attachments/assets/76918879-b2ba-407c-abe2-4439b9cbe119" />

- 인터페이스 기반 상호작용 구조 설계
- Grab Pose 적용
- 장착 / 사용 / 해제 / 투척 등 오브젝트별 개별 동작 구현

---

## 보스 및 적 AI 구현

### Ragdoll 적용 & FSM 기반 AI
<img width="494" src="https://github.com/user-attachments/assets/f5610d3a-f7d4-49fd-a8f7-f5bcc5474491" />

- 공격 / 이동 상태를 분리한 FSM 구조
- enum 기반이 아닌 **클래스 + Dictionary FSM**으로 확장성 확보
- 래그돌 적용으로 자연스러운 물리 반응
- 부위별 데미지 판정<br>
<img width="323" src="https://github.com/user-attachments/assets/546b4b32-c908-42cf-9d44-3053961d66e5" /><br>
- 보스 패턴별 개별 쿨타임 + LINQ 기반 선택 발동

---

## 라이선스 (License)

이 프로젝트는 **MIT 라이선스** 하에 배포됩니다.  
자세한 내용은 `LICENSE` 파일을 참조하세요.
