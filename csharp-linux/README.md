# Clash Verge Linux (C# Native Skeleton)

This is a Linux-only C# GUI starter that removes the Rust/Tauri backend and talks to Mihomo directly over HTTP.

## Stack
- .NET 8
- Avalonia UI
- YamlDotNet

## What is included
- Native window with Home / Proxies / Settings tabs
- `verge.yaml` load/save in user app data (`clash-verge-csharp`)
- Basic Mihomo API integration (`/version`, `/traffic`, `/proxies`)

## Run
```bash
dotnet restore csharp-linux/ClashVergeLinux.sln
dotnet run --project csharp-linux/ClashVergeLinux/ClashVergeLinux.csproj
```

## Notes
- This is intentionally Rust-free and Linux-focused.
- You can expand this into a full replacement by adding:
  - profile management YAML editor
  - rule editor
  - log stream panel
  - system proxy/tun integrations via Linux tools
