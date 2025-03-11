# 3D Platfomer Project

## 1. 게임 개요

- 3D 환경에서 플랫포머 게임에 필요한 요소를 구현
- 이동 조작 및 점프대, 함정 등의 요소를 포함함
    
![스크린샷 2025-03-10 210028](https://github.com/user-attachments/assets/dfd68dcf-f94a-4c00-82b9-65e05fd9973c)

## 2. 구현 사항

### 필수 과제

- [x]  기본 이동 및 점프
- [x]  체력바
- [x]  Raycast를 통한 환경 조사
- [x]  점프대
- [x]  ScriptableObject를 이용한 아이템 데이터 관리
- [x]  아이템 사용 + 일정 시간동안 지속 효과

### 도전 과제

- [x]  추가 UI
- [x]  상호작용 가능한 오브젝트 표시
- [ ]  3인칭 시점 (미완)
- [ ]  움직이는 플랫폼 (미완)

## 3. 조작법

| 이동 | W,A,S,D 키 |
| --- | --- |
| 점프 | Space 키 |
| 카메라 | 마우스 이동 |
| 아이템 사용 | E 키 |
| 카메라 모드 변경 | F 키 |

## 4. 게임 특징

- 키보드와 마우스를 이용한 캐릭터 조작
- health, stamina, speed, jump power로 4개의 스탯을 가짐
- 해당 스탯에 가감 효과를 주는 아이템, 플랫폼 구현
- 아이템 정보를 조준점으로 바라보는 것으로 확인할 수 있음
