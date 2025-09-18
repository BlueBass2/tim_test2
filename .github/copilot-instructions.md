# AutoTest - C# Console Application

AutoTest is a .NET Framework 4.8 C# console application that provides skeleton methods for BMI calculation, data persistence, and SQL data retrieval. This is a simple single-project solution that can be built and run on Linux using Mono.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Prerequisites and Environment Setup
- Install Mono complete toolkit: `sudo apt update && sudo apt install -y mono-complete`
- Verify installation: `mono --version` (should show version 6.8.0.105 or later)
- Verify build tools: `which xbuild` (should return `/usr/bin/xbuild`)
- Note: xbuild is deprecated but works for .NET Framework projects

### Building the Application
- Navigate to project directory: `cd AutoTest/`
- Clean build: `xbuild AutoTest.sln /t:Clean` -- takes <1 second. NEVER CANCEL.
- Debug build: `xbuild AutoTest.sln` -- takes ~1.5 seconds. NEVER CANCEL. Set timeout to 30+ seconds.
- Release build: `xbuild AutoTest.sln /p:Configuration=Release` -- takes ~1.5 seconds. NEVER CANCEL. Set timeout to 30+ seconds.
- Expected warnings: "TargetFrameworkVersion 'v4.8' not supported by this toolset" (safe to ignore)
- Expected warnings: "Don't know how to handle GlobalSection ExtensibilityGlobals" (safe to ignore)

### Running the Application
- Run Debug version: `mono bin/Debug/AutoTest.exe`
- Run Release version: `mono bin/Release/AutoTest.exe`
- Expected behavior: Application starts and exits immediately with exit code 0 (Main method is empty)
- No output is produced - this is normal and expected

### Testing and Validation
- No unit test framework is configured in this project
- No automated tests exist
- Manual validation: Verify build succeeds without errors and executable runs without crashes
- ALWAYS test both Debug and Release builds after making changes
- ALWAYS run the executable after building to ensure it starts correctly: `mono bin/Debug/AutoTest.exe`

### Validation Scenario
To test that the application methods are accessible and functional, you can temporarily modify the Main method in Program.cs:
```csharp
static void Main(string[] args)
{
    // Quick validation test
    var program = new Program();
    float bmi = program.CalculateBMI();
    System.Console.WriteLine($"BMI test result: {bmi}");
    program.Save("test.txt");
    var data = program.GetUserData("test", null);
    System.Console.WriteLine($"Data test result: {data.Columns.Count} columns");
    System.Console.WriteLine("AutoTest validation complete.");
}
```
Expected output when run:
```
BMI test result: 0
Data test result: 0 columns
AutoTest validation complete.
```
Remember to revert this change before committing code unless implementing actual functionality.

### Development Workflow
- Build artifacts are automatically excluded via .gitignore (bin/ and obj/ directories)
- No linting tools or code quality checkers are configured
- No external dependencies or NuGet packages are used
- No CI/CD validation beyond the simple GitHub workflow that displays PR merge messages

## Project Structure and Navigation

### Key Files and Directories
- `AutoTest/AutoTest.sln` - Solution file
- `AutoTest/AutoTest.csproj` - Project file (targets .NET Framework 4.8)
- `AutoTest/Program.cs` - Main source file containing entry point and skeleton methods
- `AutoTest/Properties/AssemblyInfo.cs` - Assembly metadata
- `AutoTest/App.config` - Application configuration (targets .NET Framework 4.8 runtime)
- `.github/workflows/main-pr-merge.yml` - Simple workflow that shows PR merge messages

### Source Code Overview
The `Program.cs` file contains:
- `Main(string[] args)` - Empty entry point method
- `CalculateBMI()` - Returns 0 (skeleton implementation)
- `Save(string userInputFileName)` - Empty method for file operations
- `GetUserData(string userInput, SqlConnection connection)` - Returns empty DataTable

### Build Output Locations
- Debug build: `bin/Debug/AutoTest.exe` and `bin/Debug/AutoTest.exe.config`
- Release build: `bin/Release/AutoTest.exe` and `bin/Release/AutoTest.exe.config`
- Debug symbols: `*.mdb` files alongside executables

## Troubleshooting

### Common Issues
- **Error: "reference assemblies for .NETFramework,Version=v4.8 were not found"**
  - This occurs with `dotnet build` command - use `xbuild` instead
  - Mono provides the .NET Framework 4.8 API compatibility layer

- **Error: "msbuild command not found"**
  - Use `xbuild` instead of `msbuild` for .NET Framework projects on Linux with Mono
  - `xbuild` is Mono's implementation of MSBuild for older project formats

- **Warning: "xbuild tool is deprecated"**
  - This warning is expected and safe to ignore for .NET Framework projects
  - Modern .NET Core/5+ projects should use `dotnet build`

### Validation Commands
Run these commands to verify the development environment is working:
```bash
# Verify mono installation
mono --version

# Verify build tools
which xbuild

# Clean, build, and run cycle
cd AutoTest/
xbuild AutoTest.sln /t:Clean
xbuild AutoTest.sln
mono bin/Debug/AutoTest.exe
echo "Exit code: $?"
```

Expected output: Clean build messages, successful build messages, no application output, exit code 0.

## Common Development Tasks

### Making Code Changes
1. Edit source files in `AutoTest/Program.cs` or add new .cs files
2. If adding new files, manually add them to the `AutoTest.csproj` file in the `<Compile Include="..."/>` section
3. Build: `xbuild AutoTest.sln`
4. Test: `mono bin/Debug/AutoTest.exe`
5. Always test both Debug and Release configurations before submitting changes

### Project Modifications
- To add NuGet packages: Manually edit `AutoTest.csproj` to include `<PackageReference>` items
- To change target framework: Edit `<TargetFrameworkVersion>` in `AutoTest.csproj`
- To add project references: Use `<ProjectReference>` items in the project file

### Quick Reference Commands
```bash
# Project root directory structure
ls -la
# Output: .github/, AutoTest/, .gitignore

# AutoTest directory structure  
ls -la AutoTest/
# Output: App.config, AutoTest.csproj, AutoTest.sln, Program.cs, Properties/

# Check current build status
ls -la AutoTest/bin/Debug/ 2>/dev/null || echo "No Debug build found"
ls -la AutoTest/bin/Release/ 2>/dev/null || echo "No Release build found"
```