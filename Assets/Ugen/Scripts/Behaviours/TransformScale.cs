using System;
using R3;
using UnityEngine;

namespace Ugen.Behaviours
{
    public sealed class TransformScale : UgenComponent
    {
        [SerializeField] FloatInput scale = new("Scale", 1);

        IInput[] inputs;

        public override string Name => nameof(TransformScale);
        public override IInput[] Inputs => inputs ??= new IInput[] { scale };
        public override IOutput[] Outputs => Array.Empty<IOutput>();

        void Start()
        {
            scale.Observable()
                .Subscribe(x => transform.localScale = new Vector3(x, x, x))
                .AddTo(this);
        }
    }
}
