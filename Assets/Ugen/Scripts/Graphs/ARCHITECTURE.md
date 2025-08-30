# Graph System Architecture

## 概要
UgenのGraph Systemは、ビジュアルプログラミングのためのノードベースのグラフエディタシステムです。
Unity UI ToolkitとR3 (Reactive Extensions)を使用して実装されています。

## アーキテクチャパターン
- **MVVM (Model-View-ViewModel)**: View層とデータ層の分離
- **Reactive Programming**: R3を使用した状態変更の自動伝播
- **Command Pattern**: 操作の抽象化と実行管理

## 現在の実装

### コアコンポーネント

#### View層
- **GraphView**: グラフ全体のビュー管理
  - nodeLayerとedgeLayerの2層構造
  - ノードとエッジの作成・管理
  - コンテキストメニューサポート
  
- **NodeView**: ノードの表示
  - DragManipulatorによるドラッグ移動機能
  - InputPortViewとOutputPortViewのコンテナ
  - 選択状態の視覚的フィードバック
  
- **EdgeView**: エッジの描画
  - ベジェ曲線によるポート間の接続表示
  - StartPositionとEndPositionの変更を購読して自動再描画
  - 選択可能（クリックイベント対応）

- **InputPortView/OutputPortView**: ポートの表示
  - connectorのワールド座標を追跡
  - ViewModelへの座標更新通知
  - PortConnectorView（視覚的なコネクタ要素）
  - PortPickerView（ポート選択UI）

#### ViewModel層
- **GraphViewModel**: グラフ全体の状態管理
  - Nodes（ノードのコレクション）
  - Edges（エッジのコレクション）
  - CreateNode/RemoveNode/CreateEdge/RemoveEdgeメソッド
  - イベント通知（NodeCreated, NodeRemoved, EdgeCreated, EdgeRemoved）

- **NodeViewModel**: ノードのデータと状態管理
  - Id (string): ユニークID
  - Position (ReactiveProperty<Vector2>)
  - InputPorts/OutputPorts配列
  - Move()メソッドによる位置更新

- **EdgeViewModel**: エッジのデータ管理
  - Id (string): ユニークID
  - OutputPortとInputPortの参照を保持
  - StartPosition/EndPositionをポートから取得

- **InputPortViewModel/OutputPortViewModel**: ポートのデータ管理
  - Id (string): ユニークID
  - ConnectorWorldPosition (ReactiveProperty<Vector2>)
  - 所属するNodeへの参照

### コンテキストメニューシステム
- **GraphContextMenu**: グラフ背景の右クリックメニュー
  - 新規ノード作成
  - グラフ操作コマンド

- **NodeContextMenu**: ノードの右クリックメニュー
  - ノード削除
  - ノード複製（予定）
  - プロパティ編集（予定）

- **EdgeContextMenu**: エッジの右クリックメニュー
  - エッジ削除
  - エッジの再接続（予定）

### エッジ作成システム
- **EdgePreviewDragger**: ドラッグ＆ドロップによるエッジ作成
  - ポートからのドラッグ開始検出
  - プレビューエッジの描画
  - 接続先ポートの検証
  - 接続完了時のエッジ生成

### ユーティリティ
- **DragManipulator**: MouseManipulatorを継承したドラッグ操作ハンドラ
- **VisualElementFactory**: UXML/USSからVisualElementを生成
- **CreateEdgeCommand**: エッジ作成コマンド
- **RemoveEdgeCommand**: エッジ削除コマンド
- **CreateNodeCommand**: ノード作成コマンド
- **RemoveNodeCommand**: ノード削除コマンド

## データフロー

### 1. 座標更新の流れ
```
ユーザードラッグ
    ↓
DragManipulator.OnMouseMove()
    ↓
NodeViewModel.Move(delta)
    ↓
Position.Value更新
    ↓
NodeView購読 → SetPosition()
    ↓
PortView.GetConnectorWorldPosition()
    ↓
PortViewModel.ConnectorWorldPosition更新
    ↓
EdgeViewModel購読 → StartPosition/EndPosition更新
    ↓
EdgeView購読 → OnGenerateVisualContent()再描画
```

### 2. エッジ作成フロー
```
ポートクリック
    ↓
EdgePreviewDragger.OnMouseDown()
    ↓
プレビューエッジ生成・表示
    ↓
マウスドラッグ中プレビュー更新
    ↓
対象ポートでマウスリリース
    ↓
接続検証
    ↓
GraphViewModel.CreateEdge()
    ↓
EdgeCreatedイベント
    ↓
GraphView.OnEdgeCreated()
    ↓
EdgeView生成・追加
```

### 3. コンテキストメニューフロー
```
右クリック
    ↓
ContextualMenuPopulateEvent
    ↓
メニュー項目追加
    ↓
項目選択
    ↓
対応するCommandクラス実行
    ↓
GraphViewModel更新
    ↓
View層への自動反映
```

## 技術スタック
- **UI Framework**: Unity UI Toolkit (UXML/USS)
- **Reactive Programming**: R3 (Cysharp)
- **Rendering**: MeshGenerationContext (Painter2D API)
- **Event System**: Unity UI Toolkit Event System

## 実装済み機能
- ✅ ノードのドラッグ移動
- ✅ エッジのベジェ曲線描画
- ✅ リアクティブな座標更新
- ✅ GraphViewModel実装
- ✅ コンテキストメニューシステム
- ✅ ノードの作成・削除
- ✅ エッジの作成・削除
- ✅ ドラッグ＆ドロップによるエッジ作成

## 今後の実装予定
- ⬜ ノードタイプシステム
- ⬜ ポート接続の型検証
- ⬜ Undo/Redoシステム
- ⬜ ノードのグループ化
- ⬜ ミニマップ
- ⬜ ズーム・パン機能
- ⬜ ノードの検索機能
- ⬜ ショートカットキー
- ⬜ データの永続化（Serialization）
- ⬜ ノードのコピー＆ペースト

## パフォーマンス考慮事項
1. **ReactivePropertyの使用**: 頻繁に更新される値にのみ使用
2. **イベントの購読管理**: Disposableによるメモリリークの防止
3. **描画の最適化**: MarkDirtyRepaint()の適切な使用
4. **大規模グラフ対応**: 仮想化やカリングの実装（将来）

## コーディング規約
- ViewModelは純粋なデータとロジックのみ（UI依存なし）
- Viewは表示とユーザー入力のみ（ビジネスロジックなし）
- Commandパターンで操作を抽象化
- R3のObservableパターンで状態変更を伝播
- UI ToolkitのUXML/USSでUIを定義