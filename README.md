# Pancht
# 프로젝트 소개
- 일대일 멀티 Yacht 게임의 온라인 서버를 제작한 프로젝트입니다.
- 3개의 API 서버(APi Account 서버, Api Game 서버, Matching 서버)와 1개의 소켓 서버(Yacht 로직 서버)로 구성되어 있습니다.

---

# 프로젝트 개요
- 개발 기간: 2024.08 -
- 참여 인원: 5인(기획 1명, 아트 2명, 서버 1명, 클라이언트 1명)
- 참여 역할: 서버
- 사용 언어: C#
- 사용 도구: ASP.NET Core 8.0, MySQL, Redis

---
# 개발자

PM/서버 프로그래머: 한희선

기획/사운드: 조하경

캐릭터 원화: 최영서

UI/보드판&주사위 모델링: 서수민

클라이언트 프로그래머: 양서현

---
# 서버 구조
![서버 구조 다이어그램_판추](https://github.com/user-attachments/assets/a39f4a5c-d648-4bc3-a90b-b87872801e42)


```mermaid
graph LR
    A[계정 관리 서버] -->|인증 완료| B[인게임 서버]
    B --> |인증 요청| A
    B -->|게임 매칭 요청| C[게임 매칭 서버]
    C -->|매칭 완료| D[Yacht 게임 소켓 서버]
    D --> |게임 종료| B
    subgraph "계정 관리 서버 사용 DB"
    A1[계정 DB]
    A2[인증토큰 캐시 메모리]
    end
    subgraph "인게임 서버 사용 DB"
    B1[인게임 정보 DB]
    B2[인증토큰 캐시 메모리]
    end
    subgraph "게임 매칭 서버 사용 DB"
    C1[매칭 Redis]
    end
    
    A --> A1
    A --> A2
    B --> B1
    B --> B2
    C --> C1
    D --> B1
    D --> B2
```

---
# 폴더 설명
## ApiAccountServer
- ASP.NET Core 8.0 으로 제작된 API 서버입니다.
- 유저 정보를 관리하는 서버입니다.
- 회원가입, 로그인 등 유저 정보를 저장하는 기능을 하고 있습니다.
- [Pancht 계정 서버 폴더](https://github.com/hhes0225/Pancht/tree/main/ApiAccountServer)
  
## ApiGameServer
- ASP.NET Core 8.0 으로 제작된 API 서버입니다.
- 게임 서버로서, Pancht의 API 기능을 담당하는 서버입니다.
- [Pancht API 서버 폴더](https://github.com/hhes0225/Pancht/tree/main/ApiGameServer)

## ApiMatchingServer
- ASP.NET Core 8.0 으로 제작된 API 서버입니다.
- 대전 게임의 매칭을 담당하는 서버입니다.
- 매칭 로직(티어 점수 기준)에 따라 유저를 매칭하고, 소켓 서버와 방 주소를 전달합니다.
- [Pancht Matching 서버 폴더](https://github.com/hhes0225/Pancht/tree/main/ApiMatchingServer)
- 매칭 로직
  
| 구간 명           | 최하 티어 | 최상 티어 |
| -------------- | -------- | -------- |
| 1구간           | Bronze1  | Bronze1  |
| 2구간        | Bronze3  | Silver1  |
| 3구간      | Silver1  | Gold1  |
| 4구간    | Silver3 | Gold3  |
| 5구간    | Gold1  | Gold3  |

---

# TODO-LIST

## 계정 기능
| 기능           | 완료 여부 |
| -------------- | -------- |
| 유저 등록      | ✅       |
| 로그인         | ✅       |
| 유저 인증      | ✅       |
| 유저 데이터 로드 | ✅       |
| 게임 데이터 로드 | ✅       |

## 게임 기능
| 기능           | 완료 여부 |
| -------------- | -------- |
| 매칭           | 🔄       |
| 방 입장        | ⬜       |
| 게임 로직      | ⬜       |
| heart-beat    | ⬜       |
| 방 상태체크    | ⬜       |
| 점수 집계      | ⬜       |
| 승패 기록 저장 | ⬜       |

## 부가 기능
| 기능           | 완료 여부 |
| -------------- | -------- |
| 프로필         | ✅       |
| 출석 체크      | ✅       |
| 캐릭터 도감    | ✅       |
| 우편함         | ⬜       |
| 친구           | ⬜       |
| 랭킹           | 🔄       |

## 배포
| 기능           | 완료 여부 |
| -------------- | -------- |
| 개발용 서버         | 🔄       |
| 배포용 서버           | ⬜       |
