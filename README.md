# 흑우용사 (Pushover Hero) - 단기투자 미니게임

## 프로젝트 소개

'흑우용사'는 주식 투자를 테마로 한 로그라이크 게임으로, 본 프로젝트는 게임 내 단기투자(Short Term Trading) 미니게임 기능을 구현한 것입니다. 리소스와 외부 플러그인이 제거된 포트폴리오용 버전입니다.

## 게임 플레이

[👉 Unity Play에서 플레이하기](https://play.unity.com/en/games/6b0acb14-6de9-4183-b9f5-fbb6c9ae2fad/webgl)

## 주요 기능

- **실시간 주식 차트 시뮬레이션**: 랜덤 가격 변동으로 실제 주식 거래와 유사한 경험 제공
- **매수/매도 시스템**: 1개 또는 10개 단위로 주식 매수/매도 가능
- **수익/손실 계산**: 실시간으로 사용자의 투자 성과 표시
- **시간 제한 시스템**: 제한된 시간 내에서 최대한의 수익을 내도록 설계

## 기술 스택

- **엔진**: Unity
- **언어**: C#
- **아키텍처**: 싱글톤 패턴, MVC 패턴
- **UI**: Unity UI 시스템, TextMeshPro

## 코드 구조

### 핵심 클래스

#### Controllers

- `ShortTremTrade.cs`: 단기 투자 시스템의 핵심 컨트롤러
  - 주식 가격 변동, 매수/매도 기능, 수익 계산 등을 관리
  - 싱글톤 패턴으로 구현되어 다른 클래스에서 쉽게 접근 가능

#### UI

- `ShortWindow.cs`: 단기 투자 화면의 UI 관리
  - 버튼 상호작용, 텍스트 업데이트, 차트 표시 등 담당
  - 사용자 입력을 받아 ShortTremTrade 컨트롤러에 전달

#### 데이터

- `ChartData.cs`: 주식 차트 데이터 구조
  - Vector2 리스트로 시간-가격 데이터 저장

### 주요 기능 설명

#### 주식 거래 시스템

```csharp
// 매수 기능
public void Purchase(int amount)
{
    var price = _chartData.Stock[^1].y;
    var totalPrice = price * amount;
    UserDataController.Instance.Currency -= totalPrice;
    HoldingAmount += amount;
}

// 매도 기능
public void Sell(int amount)
{
    var price = _chartData.Stock[^1].y;
    var totalPrice = price * amount;
    UserDataController.Instance.Currency += totalPrice;
    HoldingAmount -= amount;
}
```

#### 주식 가격 변동 시스템

```csharp
private void UpdateChart()
{
    var weight = Random.Range(-0.2f, 0.2f); // -20% ~ 20% 랜덤 변동
    var lastData = _chartData.Stock[^1];
    var newPrice = lastData.y * (1 + weight);
    _chartData.Stock.Add(new Vector2(lastData.x + 1, newPrice));
}
```

## 코딩 스타일

코딩 스타일에 대한 자세한 예시는 다음 스크립트를 참고해 주세요:

- [UIManager.cs](https://github.com/akpil/PushoverHero_Portfolio/blob/master/PushoverHero_PF/Assets/Scripts/UI/UIManager.cs) - UI 관리 시스템 코드
- [UnitSetter.cs](https://github.com/akpil/PushoverHero_Portfolio/blob/master/PushoverHero_PF/Assets/Scripts/Utility/UnitSetter.cs) - 유틸리티 함수 코드

## 개발 및 실행 환경

- Unity 버전: 2021.3 이상
- 지원 플랫폼: PC, WebGL

## 라이센스

이 프로젝트는 포트폴리오 목적으로 제작되었습니다.
