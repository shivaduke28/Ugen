.PHONY: build copy release clean

# Build configuration
CONFIGURATION = Release
TARGET_FRAMEWORK = netstandard2.0
PROJECT_NAME = Ugen.SourceGenerator
DLL_NAME = $(PROJECT_NAME).dll

# Paths
BUILD_DIR = $(PROJECT_NAME)/bin/$(CONFIGURATION)/$(TARGET_FRAMEWORK)
UNITY_DIR = ../Ugen.Unity/Assets/Ugen/SourceGenerator
UNITY_DLL_PATH = $(UNITY_DIR)/$(DLL_NAME)

# Default target
all: release

# Build the source generator in release mode
build:
	@echo "Building $(PROJECT_NAME) in $(CONFIGURATION) mode..."
	@dotnet build -c $(CONFIGURATION)

# Copy the DLL to Unity project
copy: build
	@echo "Copying $(DLL_NAME) to Unity project..."
	@mkdir -p $(UNITY_DIR)
	@cp $(BUILD_DIR)/$(DLL_NAME) $(UNITY_DLL_PATH)
	@echo "Successfully copied to $(UNITY_DLL_PATH)"

# Build and copy in one command
release: copy
	@echo "Release build complete!"

# Clean build artifacts
clean:
	@echo "Cleaning build artifacts..."
	@dotnet clean
	@rm -rf bin obj
	@echo "Clean complete!"

# Show help
help:
	@echo "Available targets:"
	@echo "  make build   - Build the source generator in release mode"
	@echo "  make copy    - Build and copy DLL to Unity project"
	@echo "  make release - Same as 'copy' (build and copy)"
	@echo "  make clean   - Clean build artifacts"
	@echo "  make help    - Show this help message"
