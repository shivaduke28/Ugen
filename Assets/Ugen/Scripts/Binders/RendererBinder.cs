using System.Collections.Generic;
using R3;
using Ugen.Attributes;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(Renderer))]
    [AddComponentMenu("Ugen/Ugen Renderer Binder")]
    public sealed class RendererBinder : MonoBehaviour
    {
        [SerializeField] Renderer _renderer;
        [SerializeField] int _materialIndex;

        [SerializeReference, SerializeReferenceSelector]
        List<ShaderProperty> _properties = new();

        Material Material => _renderer.materials[_materialIndex];

        void Reset()
        {
            _renderer = GetComponent<Renderer>();
        }

        void Start()
        {
            foreach (var property in _properties)
            {
                Bind(property);
            }
        }

        void Bind(ShaderProperty property)
        {
            switch (property)
            {
                case FloatProperty floatProperty:
                    floatProperty.Input.AsObservable()
                        .Subscribe(x => Material.SetFloat(property.Property, x))
                        .AddTo(this);
                    break;
                case IntProperty intProperty:
                    intProperty.Input.AsObservable()
                        .Subscribe(x => Material.SetInt(property.Property, x))
                        .AddTo(this);
                    break;
                case BoolProperty boolProperty:
                    boolProperty.Input.AsObservable()
                        .Subscribe(x => Material.SetInt(property.Property, x ? 1 : 0))
                        .AddTo(this);
                    break;
                case UintProperty uintProperty:
                    uintProperty.Input.AsObservable()
                        .Subscribe(x => Material.SetInt(property.Property, (int)x))
                        .AddTo(this);
                    break;
                case Vector2Property vector2Property:
                    vector2Property.Input.AsObservable()
                        .Subscribe(x => Material.SetVector(property.Property, x))
                        .AddTo(this);
                    break;
                case Vector3Property vector3Property:
                    vector3Property.Input.AsObservable()
                        .Subscribe(x => Material.SetVector(property.Property, x))
                        .AddTo(this);
                    break;
                case Vector4Property vector4Property:
                    vector4Property.Input.AsObservable()
                        .Subscribe(x => Material.SetVector(property.Property, x))
                        .AddTo(this);
                    break;
                case ColorProperty colorProperty:
                    colorProperty.Input.AsObservable()
                        .Subscribe(x => Material.SetColor(property.Property, x))
                        .AddTo(this);
                    break;
            }
        }
    }
}
