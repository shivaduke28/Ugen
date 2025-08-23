using System.Collections.Generic;
using R3;
using Ugen.Attributes;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(Renderer))]
    public sealed class UgenRendererBinder : MonoBehaviour
    {
        [SerializeField] Renderer _renderer;
        [SerializeField] int _materialIndex;

        [SerializeReference, SerializeReferenceSelector]
        List<UgenExposedProperty> _properties = new();

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

        void Bind(UgenExposedProperty property)
        {
            switch (property)
            {
                case FloatProperty floatProperty:
                    foreach (var prop in floatProperty.Inputs)
                    {
                        prop.Observable()
                            .Subscribe(x => Material.SetFloat(property.Property, x))
                            .AddTo(this);
                    }

                    break;
                case IntProperty intProperty:
                    foreach (var prop in intProperty.Inputs)
                    {
                        prop.Observable()
                            .Subscribe(x => Material.SetInt(property.Property, x))
                            .AddTo(this);
                    }

                    break;
                case BoolProperty boolProperty:
                    foreach (var prop in boolProperty.Inputs)
                    {
                        prop.Observable()
                            .Subscribe(x => Material.SetInt(property.Property, x ? 1 : 0))
                            .AddTo(this);
                    }

                    break;
                case UintProperty uintProperty:
                    foreach (var prop in uintProperty.Inputs)
                    {
                        prop.Observable()
                            .Subscribe(x => Material.SetInt(property.Property, (int)x))
                            .AddTo(this);
                    }

                    break;
                case Vector2Property vector2Property:
                    foreach (var prop in vector2Property.Inputs)
                    {
                        prop.Observable()
                            .Subscribe(x => Material.SetVector(property.Property, x))
                            .AddTo(this);
                    }

                    break;
                case Vector3Property vector3Property:
                    foreach (var prop in vector3Property.Inputs)
                    {
                        prop.Observable()
                            .Subscribe(x => Material.SetVector(property.Property, x))
                            .AddTo(this);
                    }

                    break;
                case Vector4Property vector4Property:
                    foreach (var prop in vector4Property.Inputs)
                    {
                        prop.Observable()
                            .Subscribe(x => Material.SetVector(property.Property, x))
                            .AddTo(this);
                    }

                    break;
                case ColorProperty colorProperty:
                    foreach (var prop in colorProperty.Inputs)
                    {
                        prop.Observable()
                            .Subscribe(x => Material.SetColor(property.Property, x))
                            .AddTo(this);
                    }

                    break;
            }
        }
    }
}
