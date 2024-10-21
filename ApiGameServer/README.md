# 서버 설명

- 게임 API 서버
- 게임과 관련된 기능 처리(로그인, 프로필 확인, 캐릭터 도감, 랭킹, 출석체크, 우편함 등)
  
|종류|라이브러리|
|------|------|
|Framework|`ASP.NET Core 8.0`|
|Database|`MySqlConnector`, `SqlKata`|
|Redis|`CloudStructures`|

---
# Database 구조

```mermaid
erDiagram
    UserData {
        bigint uid PK "auto_increment, not null"
        varchar id "유저 이메일, not null, unique"
        varchar nickname "닉네임, not null"
        timestamp create_date "가입 날짜, default current_timestamp, not null"
        int total_games "총 플레이 횟수, default 0, not null"
        int win_count "승리 횟수, default 0, not null"
        int draw_count "비긴 횟수, default 0, not null"
        int lose_count "패배 횟수, default 0, not null"
        int tier_score "현재 티어 점수, not null"
    }

    User_Character {
        bigint user_id FK "유저 ID, not null"
        int character_id FK "캐릭터 ID, not null"
        timestamp collected_date "캐릭터 획득 날짜, default current_timestamp, not null"
    }

    Attendance {
        bigint user_id FK "유저 ID, not null"
        timestamp last_attendance_date "마지막 출석일, not null"
        int consecutive_days "연속 출석 일수, default 0, not null"
    }

    Mail {
        bigint mail_id PK "auto_increment, not null"
        bigint user_id FK "유저 ID, not null"
        varchar title "메일 제목, not null"
        text content "메일 내용, not null"
        timestamp send_date "발송일, default current_timestamp"
    }

    User_Character }o--|| UserData : "contains"
    Attendance ||--|| UserData : "belongs to"
    Mail ||--|| UserData : "sent by"

```

---
# Database Schema


---
# 로그인 Sequence Diagram

```mermaid
sequenceDiagram
  autonumber
  actor A as client
  participant B as GameServer
  participant C as GameRedis
  participant D as GameMySQL
  participant E as AccountServer
  participant F as AccountRedis
  participant G as AccountMySQL
  A->>E:로그인 요청
  E->>G: 데이터 조회 요청
  G-->>E: 데이터 반환
  E->>F: 이메일, 인증토큰 저장
  E-->>A: 이메일, 인증토큰 전달
  A->>B: 이메일, 인증토큰을 통한 로그인 요청
  B-->>E: 이메일, 인증토큰 유효성 검증 요청
  E->>F: 이메일, 인증토큰으로 데이터 조회
  F-->>E: 이메일, 인증토큰 존재 여부 반환
  E->>B: 유효성 체크 결과 전달
  alt 검증 실패
      B-->>A: 로그인 실패 응답
  else
      B->>D: 이메일을 통해 유저 데이터 요청
      alt 존재하지 않는 유저
          D->>D: 유저 데이터 생성
      else
          D-->>B: 유저 데이터 로드
      end
      B-->>C: 이메일, 인증토큰 저장
      B-->>A: 로그인 성공 응답
  end
```
---
# 플레이어 프로필 Sequence Diagram
```mermaid
sequenceDiagram
    actor C as Client
    participant GS as GameServer
    participant GR as GameRedis
    participant GM as GameMySql

    C->>GS: 플레이어 프로필 정보 요청
    GS->>GR: Middleware 통한 사용자 인증
    GR->>GR: id, 인증토큰 조회
    GR->>GS: 조회 결과 전달
    alt 검증 실패
        GS->>C: 실패 응답
    else
        GS->>GM: 플레이어 정보 조회 요청
        GM->>GM: 조회
        GM->>GS: 조회 결과 반환
        GS->>C: 에러코드+조회 결과 반환
        C->>C: 결과에 따라 처리
    end

```
---
# 캐릭터 도감 Sequence Diagram
```mermaid
sequenceDiagram
    actor C as Client
    participant GS as GameServer
    participant GR as GameRedis
    participant GM as GameMySql

    C->>GS: 보유 캐릭터 정보 요청
    GS->>GR: Middleware 통한 사용자 인증
    GR->>GR: id, 인증토큰 조회
    GR->>GS: 조회 결과 전달
    alt 검증 실패
        GS->>C: 실패 응답
    else
        GS->>GM: 보유 캐릭터 정보 조회 요청
        GM->>GM: 조회
        GM->>GS: 조회 결과 반환
        GS->>C: 에러코드+조회 결과 반환
        C->>C: 결과에 따라 처리
    end

```
