# CLAUDE.md

このファイルはClaude Code (claude.ai/code)がこのリポジトリで作業する際のガイドラインです。

## プロジェクト概要

UgenはUnityでGenerativeな映像表現（特にオーディオビジュアル）を行うためのフレームワークです。Unity 6.1.4f1とHDRP (High Definition Render Pipeline)を使用し、ノードベースのビジュアルプログラミングシステムを提供します。

### 核となるコンセプト

- **UgenBehaviour**: 独立した振る舞いを表現するコンポーネント（MonoBehaviour継承）
  - 入力（UgenInput）と出力（UgenOutput）を持つ
  - 例：UgenSlider（GUI）、UgenYawRotator（Transform制御）
  
- **UgenGraph**: 複数のUgenBehaviourの接続関係を表現するグラフ構造
  - Scene内でシリアライズ（Scene内オブジェクトの参照を扱うため）
  - ノード間の接続情報を保持

- **UgenNode**: グラフエディタで使用するノード表現
  - UgenBehaviourと1:1対応（将来的にSource Generatorで自動生成予定）

### 現在の開発フェーズ

**フェーズ1: PoC実装中** - 最小限の機能でコンセプトを実証
- 詳細は`docs/todo.md`を参照
- 設計詳細は`docs/design.md`を参照

## テスト

UnityのTest Frameworkを使用:
- Unity Editor: Window → General → Test Runner
- PlayMode tests: ランタイムの動作テスト
- EditMode tests: エディタ機能のテスト

## アーキテクチャ

### コア設計

#### UgenBehaviourシステム
- MonoBehaviourを継承した抽象クラス
- 入出力ポートを持つコンポーネントベースアーキテクチャ
- R3（Reactive Extensions）による値の伝播

#### グラフシステム
- **UgenGraph**: ノード間の接続を管理するデータ構造
- **UgenManager**: グラフを実行するランタイム
- **シリアライズ**: Scene内で直接シリアライズ（オブジェクト参照のため）

### 既存システム（Audio/UI）

#### 依存性注入パターン
`UgenInstaller`を中心とした軽量DIパターン:
- `IInitializable`インターフェースによる初期化ライフサイクル
- 自動的なDisposableの追跡とクリーンアップ

#### UI（MVVM パターン）
UI Toolkitを使用したMVVM実装:
- **Views**: UI Toolkitのビジュアル要素
- **ViewModels**: R3のReactivePropertyによる状態管理
- **Binding**: R3 Observableによるデータバインディング

#### オーディオシステム
- **LASP統合**: Keijiro's LASPによる低遅延オーディオ入力
- **AudioInputDeviceManager**: デバイス選択と管理
- **AudioInputStream**: AudioLevelTrackerのラッパー

### 主要な依存関係
- **R3**: Unity用Reactive Extensions
- **UniTask**: Unity最適化されたasync/await
- **LASP**: 低遅延オーディオ処理
- **UI Toolkit**: UnityのモダンなUIシステム

## プロジェクト構造

```
Assets/
├── Ugen/                    # メインプロジェクトコード
│   ├── Scripts/
│   │   ├── Audio/          # オーディオ入力管理（既存）
│   │   ├── UI/             # UIコンポーネント（既存）
│   │   ├── Behaviours/     # UgenBehaviourの実装（新規）
│   │   ├── Graph/          # グラフシステム（新規）
│   │   └── Ugen.asmdef     # アセンブリ定義
│   ├── Scenes/             # Unityシーン
│   └── Resources/          # UI Toolkit UXMLファイル
└── Settings/               # HDRPとプロジェクト設定
```

## 開発タスク

### 新しいUgenBehaviourの追加
1. `Assets/Ugen/Scripts/Behaviours/`に新しいクラスを作成
2. `UgenBehaviour`を継承
3. 入力/出力ポートを定義
4. 対応するNodeクラスを手動作成（Source Generator導入まで）

### グラフの編集
- UgenManagerコンポーネントでグラフを管理
- Scene内でノード間の接続を設定
- 実行時に自動的に接続が確立される

### R3によるリアクティブプログラミング
- `ReactiveProperty<T>`で観測可能な状態を管理
- `.Subscribe()`で購読、適切にDispose
- `Observable.EveryUpdate()`でフレームベースの更新

## 重要事項

- **Unityバージョン**: Unity 6.1.4f1以降が必要
- **レンダーパイプライン**: HDRP使用 - アセット追加時は互換性に注意
- **アセンブリ定義**: コードはアセンブリで整理されている。依存関係追加時は`Ugen.asmdef`を更新

## コード規約

- `private`は書かない
- `sealed`がつけられる場合はつける
- 省略できるものは省略する (e.g. `Foo foo = new()`)
- 編集したファイルは `mcp__mcp-jetbrains__reformat_file` でフォーマットする
