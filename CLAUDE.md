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
- **Additional**: Cinemachine, Timeline, Input System

## Architecture

### Core System (Assets/Ugen/Scripts/)
- **UgenInput**: Base abstract class for all reactive inputs using R3's Observable pattern
- **Inputs/**: Various input implementations (primitives, UI, bindings)
- **Binders/**: Components that bind reactive values to Unity objects (Transform, Renderer, Animator, etc.)
- **Attributes/**: Custom attributes for editor functionality
- **Editor/**: Custom editor UI using UI Toolkit for input selection

### Key Concepts
1. All inputs inherit from `UgenInput<T>` and provide an `Observable<T>` stream
2. Binders subscribe to inputs and apply values to Unity components
3. The system uses R3 for reactive programming patterns
4. UI Toolkit is used for custom editor interfaces

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

## Project Structure

```
Assets/
├── Ugen/                    # Main framework code
│   └── Scripts/
│       ├── Inputs/         # Input implementations
│       │   ├── Primitives/ # Basic type inputs (Vector, Color, etc.)
│       │   ├── UI/         # UI-related inputs
│       │   └── Bindings/   # Binding-specific inputs
│       ├── Binders/        # Component binders
│       ├── Attributes/     # Custom attributes
│       └── Editor/         # Editor extensions
├── UgenSample/             # Sample scenes and assets
│   ├── Main.unity         # Main sample scene
│   ├── Models/            # 3D models
│   └── Animations/        # Animation assets
└── Packages/              # NuGet packages (via NuGetForUnity)
```

## Important Notes

- Uses NuGetForUnity for package management (packages.config)
- Custom assemblies defined: Ugen.asmdef, Ugen.Editor.asmdef
- Reactive streams are the primary communication mechanism
- Editor uses UI Toolkit for custom inspectors (UXML/USS files)
- Project uses git for version control (main branch)