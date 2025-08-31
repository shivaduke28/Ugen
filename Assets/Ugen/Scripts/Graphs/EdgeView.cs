using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class EdgeView : VisualElement, IDisposable
    {
        readonly CompositeDisposable _disposable = new();
        readonly Subject<Vector2> _onClickPanelPosition = new();
        public Observable<Vector2> OnClickPanelPosition() => _onClickPanelPosition;

        Vector2 _startLocalPosition;
        Vector2 _endLocalPosition;

        // EdgeViewModel用のコンストラクタ
        public EdgeView()
        {
            style.position = Position.Absolute;
            style.left = 0;
            style.top = 0;
            style.right = 0;
            style.bottom = 0;

            generateVisualContent += OnGenerateVisualContent;


            // 右クリックでコンテキストメニューを表示
            RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button == 1) // 右クリック
                {
                    evt.StopPropagation();
                    _onClickPanelPosition.OnNext(evt.position);
                }
            });
        }

        public void Bind(IEdgeEndPoints edgeViewModel)
        {
            // StartPositionとEndPositionの変更を購読して再描画
            // ワールド座標をローカル座標に変換
            edgeViewModel.OutputPanelPosition.Subscribe(panelPosition =>
            {
                if (parent == null)
                {
                    Debug.LogError("parent is null");
                }

                _startLocalPosition = parent?.WorldToLocal(panelPosition) ?? panelPosition;
                MarkDirtyRepaint();
            }).AddTo(_disposable);

            edgeViewModel.InputPanelPosition.Subscribe(panelPosition =>
            {
                _endLocalPosition = parent?.WorldToLocal(panelPosition) ?? panelPosition;
                MarkDirtyRepaint();
            }).AddTo(_disposable);
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            const float threshold = 10f;
            const float offset = 50f;

            // Z字型の4つの点
            var p1 = _startLocalPosition;
            var p2 = new Vector2(_startLocalPosition.x + offset, _startLocalPosition.y);
            var p3 = new Vector2(_endLocalPosition.x - offset, _endLocalPosition.y);
            var p4 = _endLocalPosition;

            // 3つの線分との距離を計算
            var dist1 = DistanceToLineSegment(localPoint, p1, p2);
            var dist2 = DistanceToLineSegment(localPoint, p2, p3);
            var dist3 = DistanceToLineSegment(localPoint, p3, p4);

            var minDistance = Mathf.Min(dist1, Mathf.Min(dist2, dist3));
            return minDistance <= threshold;
        }

        static float DistanceToLineSegment(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            var lineVec = lineEnd - lineStart;
            var pointVec = point - lineStart;
            var lineLength = lineVec.sqrMagnitude;

            if (lineLength == 0)
                return Vector2.Distance(point, lineStart);

            var t = Mathf.Clamp01(Vector2.Dot(pointVec, lineVec) / lineLength);
            var projection = lineStart + t * lineVec;
            return Vector2.Distance(point, projection);
        }

        void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            if (_startLocalPosition == _endLocalPosition)
                return;

            var painter = mgc.painter2D;
            painter.strokeColor = Color.white;
            painter.lineWidth = 1.0f;

            painter.BeginPath();
            painter.MoveTo(_startLocalPosition);

            // Z字型のパス
            const float offset = 10f;
            var p2 = new Vector2(_startLocalPosition.x + offset, _startLocalPosition.y);
            var p3 = new Vector2(_endLocalPosition.x - offset, _endLocalPosition.y);

            painter.LineTo(p2);
            painter.LineTo(p3);
            painter.LineTo(_endLocalPosition);
            painter.Stroke();
        }

        public void Dispose()
        {
            _onClickPanelPosition.Dispose();
            _disposable.Dispose();
            generateVisualContent -= OnGenerateVisualContent;
        }
    }
}
