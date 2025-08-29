# Graph System TODO

## エッジのドラッグ＆ドロップ機能実装

### Phase 1: 基盤整備
- [ ] GraphViewModelクラスの作成
  - エッジのコレクション管理
  - エッジの追加・削除・検索メソッド
  - ノードのコレクション管理

- [ ] EdgeViewModelの拡張
  - IDプロパティの追加
  - OutputPort/InputPortプロパティをpublicに変更

- [ ] ポートビューの拡張
  - ConnectorのVisualElement公開
  - ホバー状態の管理

### Phase 2: ドラッグ＆ドロップ実装
- [ ] EdgeDragManipulatorクラスの作成
  - OutputPortからのドラッグ開始検出
  - ドラッグ中の仮エッジ表示
  - InputPortへのドロップ検出
  - 接続可能性の検証

- [ ] GraphViewの拡張
  - EdgeDragManipulatorの統合
  - ポートのヒットテスト実装
  - CreateEdge/RemoveEdge/UpdateEdgeメソッド

### Phase 3: エッジ編集機能
- [ ] 既存エッジの端点ドラッグ
  - エッジ端点のドラッグ開始検出
  - 新しい接続先への変更
  - 元の接続の解除

- [ ] エッジの削除機能
  - 右クリックメニュー対応
  - Deleteキー対応
  - 無効な場所へのドロップで削除

### Phase 4: UI/UX改善
- [ ] ビジュアルフィードバック
  - ドラッグ中の仮エッジ表示（点線など）
  - 接続可能なポートのハイライト
  - 接続不可能な場合の視覚的フィードバック

- [ ] インタラクション改善
  - ポートホバー時のカーソル変更
  - エッジホバー時のハイライト
  - スナップ機能

### Phase 5: 高度な機能
- [ ] 接続ルールの実装
  - ポートタイプの互換性チェック
  - 多重接続の制御
  - 循環参照の防止

- [ ] Undo/Redo対応
  - エッジ作成・削除の履歴管理
  - コマンドパターンの実装

## 実装順序
1. GraphViewModelの作成とGraphViewへの統合
2. EdgeDragManipulatorの基本実装
3. 新規エッジ作成機能
4. 既存エッジの編集機能
5. UI/UXの改善