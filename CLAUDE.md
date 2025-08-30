# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Ugen is a Unity project built with Unity 6 (6000.0.29f1) that provides a reactive visual programming system for audio-visual applications using R3 (Reactive Extensions for Unity) and various audio/MIDI libraries.

## Key Technologies

- **Unity Version**: 6000.0.29f1
- **Render Pipeline**: HDRP (High Definition Render Pipeline) 17.2.0
- **Reactive Programming**: R3 (Cysharp) and UniTask
- **Audio/MIDI**: LASP, Minis (MIDI), RtMidi, SoundIO
- **Visual Effects**: Visual Effect Graph, Shader Graph
- **UI Framework**: Unity UI Toolkit (UXML/USS)
- **Additional**: Cinemachine, Timeline, Input System

## Architecture

### Core System (Assets/Ugen/Scripts/)

#### UgenInput System
- **UgenInput<T>**: Base abstract class for all reactive inputs using R3's Observable pattern
- **Inputs/**: Various input implementations
  - **Primitives/**: Basic type inputs (FloatInput, Vector2/3/4Input, ColorInput)
  - **UI/**: UI-related inputs (ButtonInput, SliderInput, ToggleInput)
  - **Bindings/**: Type-specific bindings (IntBinding, FloatBinding, Vector2/3/4Binding, ColorBinding, BoolBinding, UintBinding)
  - **Audio/**: Audio processing inputs (AudioLevelInput, AudioLevelLowInput, AudioLevelMidInput, AudioLevelHighInput)
  - **Utility Inputs**: IntervalInput, UpdateInput, RandomInput, MergeInput, PositionInput

#### Binders System
- **UgenBinder**: Base class for all binders
- **Component Binders**: 
  - TransformBinder (position, rotation, scale)
  - RendererBinder (material properties)
  - AnimatorBinder (animation parameters)
  - GameObjectBinder (active state)
  - LightBinder (intensity, color)
- **Camera System**: 
  - CinemachineCameraBinder
  - CinemachineSplineDollyBinder
  - CinemachineSwitcher
- **Property Systems**:
  - ShaderProperty (for material shader properties)
  - AnimationProperty (for animation parameters)

#### Graph System (Assets/Ugen/Scripts/Graphs/)
Visual node-based programming system using UI Toolkit and R3.

##### View Layer
- **GraphView**: Main graph canvas with nodeLayer and edgeLayer
- **NodeView**: Node visualization with DragManipulator
- **EdgeView**: Bezier curve connections between ports
- **Port System**:
  - InputPortView/OutputPortView: Port visualization
  - PortConnectorView: Visual connector element
  - PortPickerView: Port selection UI
  - EdgePreviewDragger: Drag & drop edge creation

##### ViewModel Layer (MVVM Pattern)
- **GraphViewModel**: Graph state management
- **NodeViewModel**: Node data and position (ReactiveProperty<Vector2>)
- **EdgeViewModel**: Edge connections between ports
- **PortViewModel**: Port state and world position tracking

##### Data Flow
1. User drags node → DragManipulator → NodeViewModel.Move()
2. NodeViewModel.Position updates → NodeView.SetPosition()
3. Port positions update → EdgeViewModel notified
4. EdgeView re-renders with new positions

#### Editor Extensions (Assets/Ugen/Scripts/Editor/)
- **UgenInputSelectionWindow**: UI Toolkit-based input selector
- **UgenInputSelectorPropertyDrawer**: Custom property drawer
- **SerializeReferenceSelectorAttributeDrawer**: Polymorphic serialization support

### Key Concepts
1. **Reactive Streams**: All inputs provide `Observable<T>` streams via R3
2. **Subscription Pattern**: Binders subscribe to inputs and apply values reactively
3. **MVVM Architecture**: Graph system uses Model-View-ViewModel pattern
4. **UI Toolkit**: All custom editors use UXML/USS for UI
5. **Separation of Concerns**: Clear separation between data (ViewModel) and presentation (View)

## Development Commands

### Unity Editor Commands
- **Unity エディターにアタッチ**: Attach debugger to Unity Editor
- **Unity エディターにアタッチして再生**: Attach debugger and play
- **Unity の起動**: Launch Unity
- **ユニットテスト (バッチモード)**: Run unit tests in batch mode

### Building and Testing
- Build through Unity Editor: File > Build Settings
- Run tests through Unity Test Runner: Window > General > Test Runner
- Use JetBrains Rider's run configurations for debugging

### Code Formatting with dotnet format
```bash
# Format the entire Ugen project
dotnet format Ugen.csproj

# Format only files in Ugen/Scripts directory
dotnet format Ugen.csproj --include Assets/Ugen/Scripts/**/*.cs

# Run with verbose output to see what's being changed
dotnet format Ugen.csproj --verbosity diagnostic
```

## Project Structure

```
Assets/
├── Ugen/                    # Main framework code
│   ├── Scripts/
│   │   ├── Inputs/         # Input implementations
│   │   │   ├── Primitives/ # Basic type inputs
│   │   │   ├── UI/         # UI-related inputs
│   │   │   ├── Bindings/   # Type-specific bindings
│   │   │   └── Audio/      # Audio processing inputs
│   │   ├── Binders/        # Component binders
│   │   │   └── Cameras/    # Camera-specific binders
│   │   ├── Graphs/         # Node-based graph system
│   │   │   ├── Ports/      # Port system components
│   │   │   └── ARCHITECTURE.md # Graph system details
│   │   ├── Attributes/     # Custom attributes
│   │   └── Editor/         # Editor extensions
│   ├── Resources/          # Font and other resources
│   └── Prefabs/            # Reusable prefabs
├── UgenSample/             # Sample scenes and assets
│   ├── Main.unity         # Main sample scene
│   ├── Models/            # 3D models
│   └── Animations/        # Animation assets
└── Packages/              # NuGet packages (via NuGetForUnity)
```

## Important Files and Documentation

### Core Classes
- `Assets/Ugen/Scripts/Inputs/UgenInput.cs`: Base class for all inputs
- `Assets/Ugen/Scripts/Binders/UgenBinder.cs`: Base class for all binders
- `Assets/Ugen/Scripts/Graphs/GraphView.cs`: Main graph view implementation
- `Assets/Ugen/Scripts/Graphs/GraphViewModel.cs`: Graph data model

### Documentation
- `Assets/Ugen/Scripts/Graphs/ARCHITECTURE.md`: Detailed graph system architecture
- `.editorconfig`: Code style configuration

## Current Development Focus

### Graph System Enhancement (現在実装中)
The team is currently working on enhancing the graph system with:
1. **Edge Creation**: Drag & drop edge creation between ports
2. **GraphViewModel**: Implementing proper graph state management
3. **Port Validation**: Connection compatibility checking
4. **Visual Feedback**: Improved user interaction feedback

## Important Notes

- **NuGet Package Management**: Uses NuGetForUnity (packages.config)
- **Assembly Definitions**: Ugen.asmdef, Ugen.Editor.asmdef for proper code organization
- **Reactive Patterns**: Heavy use of R3's Observable patterns - understand reactive programming
- **UI Toolkit Priority**: All new UI should use UI Toolkit, not IMGUI
- **Git Workflow**: Main branch for stable code, feature branches for development
- **Performance**: Use ReactiveProperty sparingly in hot paths, consider performance implications

## Common Patterns

### Creating a New Input
```csharp
public class MyInput : UgenInput<float>
{
    [SerializeField] private float value;
    
    protected override Observable<float> CreateObservable()
    {
        return Observable.Return(value);
    }
}
```

### Creating a New Binder
```csharp
public class MyBinder : UgenBinder
{
    [SerializeField, UgenInputSelector] 
    private UgenInput<float> input;
    
    private void OnEnable()
    {
        input?.GetObservable()
            .Subscribe(value => ApplyValue(value))
            .AddTo(this);
    }
}
```

## Debugging Tips

1. **Graph System**: Enable debug logging in GraphView for edge/node operations
2. **Reactive Streams**: Use `.Do(x => Debug.Log(x))` to inspect stream values
3. **UI Toolkit**: Use UI Toolkit Debugger (Window > UI Toolkit > Debugger)
4. **Performance**: Use Unity Profiler to identify reactive subscription bottlenecks

## Testing Strategy

- Unit tests for input/binder logic
- Integration tests for graph system
- Performance tests for reactive stream operations
- UI tests for custom editors using UI Toolkit Test Framework