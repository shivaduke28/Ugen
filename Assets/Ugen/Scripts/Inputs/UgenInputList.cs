using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    [Serializable]
    public sealed class UgenInputList<T>
    {
        [SerializeField] List<UgenInput<T>> _inputs = new();

        public Observable<T> Observable() => _inputs.Select(x => x.Observable()).Merge();
    }
}
