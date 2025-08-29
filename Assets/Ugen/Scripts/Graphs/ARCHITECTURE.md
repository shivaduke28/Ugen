# Graph System Architecture

## 概要
UgenのGraph Systemは、ビジュアルプログラミングのためのノードベースのグラフエディタシステムです。
Unity UI ToolkitとR3 (Reactive Extensions)を使用して実装されています。

## 現在の実装

### コアコンポーネント

#### View層
- **GraphView**: グラフ全体のビュー管理
  - nodeLayerとedgeLayerの2層構造
  - ノードとエッジの作成・管理
  
- **NodeView**: ノードの表示
  - DragManipulatorによるドラッグ移動機能
  - InputPortViewとOutputPortViewのコンテナ
  
- **EdgeView**: エッジの描画
  - ベジェ曲線によるポート間の接続表示
  - StartPositionとEndPositionの変更を購読して自動再描画

- **InputPortView/OutputPortView**: ポートの表示
  - connectorのワールド座標を追跡
  - ViewModelへの座標更新通知

#### ViewModel層
- **NodeViewModel**: ノードのデータと状態管理
  - Position (ReactiveProperty<Vector2>)
  - InputPorts/OutputPorts配列
  - Move()メソッドによる位置更新

- **EdgeViewModel**: エッジのデータ管理
  - OutputPortとInputPortの参照を保持
  - StartPosition/EndPositionをポートから取得

- **InputPortViewModel/OutputPortViewModel**: ポートのデータ管理
  - ConnectorWorldPosition (ReactiveProperty<Vector2>)
  - 所属するNodeへの参照

### ユーティリティ
- **DragManipulator**: MouseManipulatorを継承したドラッグ操作ハンドラ
- **VisualElementFactory**: UXML/USSからVisualElementを生成

## データフロー

1. **座標更新の流れ**:
   - ユーザーがNodeをドラッグ
   - DragManipulator → NodeViewModel.Move()
   - NodeViewModel.Position更新 → NodeView.SetPosition()
   - PortView.GetConnectorWorldPosition() → PortViewModel.ConnectorWorldPosition更新
   - EdgeViewModel.StartPosition/EndPosition変更通知
   - EdgeView.OnGenerateVisualContent()で再描画

2. **エッジの管理**:
   - 現在は静的に生成（GraphViewのコンストラクタ内）
   - EdgeViewModelがOutputPortとInputPortの参照を保持
   - ポート座標の変更を自動的に反映

## 技術スタック
- **UI Framework**: Unity UI Toolkit (UXML/USS)
- **Reactive Programming**: R3 (Cysharp)
- **Rendering**: MeshGenerationContext (Painter2D API)

## 現在の制限事項
1. エッジは静的に生成され、実行時の作成・削除は未実装
2. GraphViewModelが存在せず、グラフ全体の状態管理が不完全
3. ポートのドラッグ＆ドロップによるエッジ作成機能が未実装
4. エッジの選択・削除機能が未実装
5. ポート接続の検証ロジックが未実装