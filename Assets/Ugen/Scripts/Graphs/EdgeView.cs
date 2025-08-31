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
            const int sampleCount = 20;

            var minDistance = float.MaxValue;

            for (var i = 0; i <= sampleCount; i++)
            {
                var t = i / (float)sampleCount;
                var pointOnCurve = CalculateBezierPoint(t);
                var distance = Vector2.Distance(localPoint, pointOnCurve);
                minDistance = Mathf.Min(minDistance, distance);

                if (minDistance <= threshold)
                    return true;
            }

            return minDistance <= threshold;
        }

        Vector2 CalculateBezierPoint(float t)
        {
            // ベジェ曲線のコントロールポイントを計算（OnGenerateVisualContentと同じロジック）
            var distance = Vector2.Distance(_startLocalPosition, _endLocalPosition);
            var controlPointOffset = Mathf.Min(distance * 0.5f, 100f);
            var controlPoint1 = new Vector2(_startLocalPosition.x + controlPointOffset, _startLocalPosition.y);
            var controlPoint2 = new Vector2(_endLocalPosition.x - controlPointOffset, _endLocalPosition.y);

            // 3次ベジェ曲線の計算
            var oneMinusT = 1f - t;
            var oneMinusTSquared = oneMinusT * oneMinusT;
            var oneMinusTCubed = oneMinusTSquared * oneMinusT;
            var tSquared = t * t;
            var tCubed = tSquared * t;

            return oneMinusTCubed * _startLocalPosition +
                   3f * oneMinusTSquared * t * controlPoint1 +
                   3f * oneMinusT * tSquared * controlPoint2 +
                   tCubed * _endLocalPosition;
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

            // ベジェ曲線のコントロールポイントを計算
            var distance = Vector2.Distance(_startLocalPosition, _endLocalPosition);
            var controlPointOffset = Mathf.Min(distance * 0.5f, 100f);

            var controlPoint1 = new Vector2(_startLocalPosition.x + controlPointOffset, _startLocalPosition.y);
            var controlPoint2 = new Vector2(_endLocalPosition.x - controlPointOffset, _endLocalPosition.y);

            painter.BezierCurveTo(controlPoint1, controlPoint2, _endLocalPosition);
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
