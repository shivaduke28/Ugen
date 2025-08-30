using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class EdgeView : VisualElement, IDisposable
    {
        readonly CompositeDisposable _disposable = new();

        Vector2 _startPos;
        Vector2 _endPos;

        public EdgeView(IEdgeEndPoints edgeEndPoints)
        {
            style.position = Position.Absolute;
            style.left = 0;
            style.top = 0;
            style.right = 0;
            style.bottom = 0;

            generateVisualContent += OnGenerateVisualContent;

            // StartPositionとEndPositionの変更を購読して再描画
            edgeEndPoints.StartPosition.Subscribe(pos =>
            {
                _startPos = pos;
                MarkDirtyRepaint();
            }).AddTo(_disposable);

            edgeEndPoints.EndPosition.Subscribe(pos =>
            {
                _endPos = pos;
                MarkDirtyRepaint();
            }).AddTo(_disposable);
        }

        void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            if (_startPos == _endPos)
                return;

            var painter = mgc.painter2D;
            painter.strokeColor = Color.white;
            painter.lineWidth = 1.0f;

            painter.BeginPath();
            painter.MoveTo(_startPos);

            // ベジェ曲線のコントロールポイントを計算
            var distance = Vector2.Distance(_startPos, _endPos);
            var controlPointOffset = Mathf.Min(distance * 0.5f, 100f);

            var controlPoint1 = new Vector2(_startPos.x + controlPointOffset, _startPos.y);
            var controlPoint2 = new Vector2(_endPos.x - controlPointOffset, _endPos.y);

            painter.BezierCurveTo(controlPoint1, controlPoint2, _endPos);
            painter.Stroke();
        }

        public void Dispose()
        {
            _disposable.Dispose();
            generateVisualContent -= OnGenerateVisualContent;
        }
    }
}
