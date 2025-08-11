# Design

これはUgenプロジェクト全体の要件および設計について説明するdocです。

## Ugen

UgenはUnityでGenerativeな映像表現（特にオーディオビジュアル）を行うためのフレームワークです。

Ugenは以下の２つで構成されます。
- UgenBehaviour: 独立した１つのオブジェクトの振る舞いを表現する
  - `UgenYawRotator`: アタッチしたGameObjectのTransformをY軸に沿って回転させる。回転する速度をfloatの入力として受け付ける。
  - `UgenSlider`: GUIのスライダー。float値を出力する。
- UgenGraph: 複数のUgenBehaviourの間のつながりを表現する
  - `UgenSlider`のoutputを`UgenYawRotator`のinputに接続する

機能要件
- システムへの入力
  - GUI
  - keyboard
  - MIDI/OSC
  - 音声入力
  - テクスチャ
  - ビデオ
  - etc
- 入力を使用するコンポーネント
  - Transform
  - Animator
  - Cinemachine
  - Timeline
  - ShaderGraph
  - VFXGraph
  - etc


## アーキテクチャ

UgenBehaviour
- MonoBehaviourを継承する抽象クラス
- コンポーネントパターンを採用する
  - 振る舞いごとにUgenBehaviourを継承したクラスを作る
  - 一つのGameObjectに複数のUgenBehaviourをつけてGameObjectの振る舞いを表現する
- UgenBehaviourは入力と出力をそれぞれ0個以上持つことができる
  - UgenInput/UgenOutputと呼ぶ
  - UgenInput/UgenOutputはそれぞれ値の型（ValueType）を持つ
  - ValueTypeはfloat, int, R3.Unit, boolなどが使用できる

UgenNode
- UgenGraphで使用する抽象クラス
- Serializable
- 一意なidで識別できる
- InputPortとOutputPortをもつ

UgenBehaviourNode
- UgenNodeを継承する抽象クラス
- UgenBehaviourNodeを継承したクラスがUgenBehaviourを継承したクラスと1:1で存在する
  - e.g. `UgenYawRotator`と`YawRotatorNode`
  - UgenNodeの入力と出力はUgenBehaviourの型情報から決定できる
  - SourceGeneratorなどを使ってUgenBehaviourにattributeをつけることで自動生成できるとベター
- 対応するUgenBehaviourへの参照を直接シリアライズしてよい

Behaviour以外からつくられるNode
- e.g. `AddNode`: floatのxとyを入力にし、x+yを出力する

UgenEdge
- UgenGraphで使用する
- 一意なidで識別できる

UgenGraph
- pure C#クラスでSerializable
- UgenNodeとUgenEdgeの配列を持つ
- UgenNodeはSerializeReferenceを使う
- 専用のエディタを使って編集する

UgenManager
- UgenGraphをフィールドにもつMonoBehaviour
- UgenManagerのインスペクタで登録したUgenBehaviourがUgenGraphのエディタでNodeとして使用できるようになる
- シーン開始時にUgenGraphを実行する
  - つまりUgenGraphによって表現されたUgenBehaviour間の参照をRxで実際に動かす

UgenGraphView
- Editorで使用するUgenGraphを編集するGUI
- GraphViewを使用する

UgenNodeView
- UgenNodeをUgenGraphViewで表示するためのView
- GraphViewのNodeを継承する
- UgenNodeの具象に対してViewを個別に作ってもいいが最初は共通のクラスでOK
