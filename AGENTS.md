# AGENTS.md - AI Agent Guidelines for Aaru.Server

## Project Overview

**Aaru.Server** is the server-side component of the Aaru Data Preservation Suite, a fully-featured software package for preserving storage media. The server runs at https://www.aaru.app and provides:

- Statistics collection and display from Aaru client software
- Device reports storage and management
- USB vendor/product database synchronization
- CD offset database synchronization
- NES header information database
- User documentation hosting

## Technology Stack

- **Framework**: ASP.NET Core (.NET 10.0) with Blazor Server components
- **Database**: MariaDB/MySQL with Entity Framework Core (Pomelo provider)
- **ORM**: Entity Framework Core with lazy loading proxies
- **Authentication**: ASP.NET Core Identity
- **Telemetry**: OpenTelemetry with Sentry integration
- **Logging**: Serilog with console, file, and syslog sinks
- **Markdown**: Markdig with Prism syntax highlighting

## Project Structure

```
Aaru.Server/                    # Main web application
├── Components/                 # Blazor components
│   ├── Account/               # Authentication components
│   ├── Admin/                 # Admin panel components
│   ├── Layout/                # Layout components
│   └── Pages/                 # Page components (Statistics, Documentation, etc.)
├── Controllers/               # API controllers (Update, UploadReport, UploadStats)
├── Core/                      # Core business logic (ATA, SCSI decoders, etc.)
├── Services/                  # Background services
├── Aaru.Documentation/        # Markdown documentation files
└── wwwroot/                   # Static assets


Aaru.Server.Database/          # Database layer
├── Context.cs                 # EF Core DbContext
├── Models/                    # Entity models
└── Migrations/                # EF Core migrations
```

## Code Style Guidelines

Follow these rules strictly:

- **Braces**: BSD style (unindented on next line)
- **Indentation**: 4 spaces (soft tabs)
- **Line endings**: UNIX (`\n`)
- **Max line length**: 120 characters
- **No `var`**: Always use explicit types
- **Built-in keywords**: Use `uint` instead of `UInt32`, etc.
- **No unnecessary braces**: Don't use braces for single-statement blocks
- **Naming conventions**:
  - Constants: `ALL_UPPER_CASE`
  - Instance/static fields: `lowerCamelCase` (often prefixed with `_`)
  - Public fields/properties: `UpperCamelCase`
- **Expression bodies**: Only for properties, indexers, and events
- **Prefer structs**: Use struct over class when only storing data
- **Avoid abstractions**: Keep code low-level, avoid unnecessary OOP patterns
- **LINQ**: Acceptable for queries

### License Header

All C# files must include the LGPL license header:

```csharp
// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : [FileName].cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ License ] --------------------------------------------------------------
//
//     This library is free software; you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as
//     published by the Free Software Foundation; either version 2.1 of the
//     License, or (at your option) any later version.
//
//     This library is distributed in the hope that it will be useful, but
//     WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//     Lesser General Public License for more details.
//
//     You should have received a copy of the GNU Lesser General Public
//     License along with this library; if not, see <http://www.gnu.org/licenses/>.
//
// ----------------------------------------------------------------------------
// Copyright © 2011-2026 Natalia Portillo
// ****************************************************************************/
```

## Database Operations

- Use Entity Framework Core with `DbContext` from `Aaru.Server.Database`
- The database context uses lazy loading proxies
- Create migrations using EF Core tools: `dotnet ef migrations add <MigrationName>`
- Apply migrations: `dotnet ef database update`
- Database connection string is in `appsettings.json`

### Common Entity Types

- `Device` - Hardware device reports
- `UsbVendor`, `UsbProduct` - USB vendor/product database
- `CompactDiscOffset` (CdOffsets) - CD drive offset database
- `NesHeaderInfo` - NES ROM header information
- `UploadedReport` - User-submitted device reports
- Statistics: `Command`, `DeviceStat`, `Filesystem`, `Filter`, `Media`, `MediaFormat`, etc.

## API Endpoints

Key controllers in `Aaru.Server/Controllers/`:

- **UpdateController** (`/api/update`): Provides sync data for Aaru clients
- **UploadReportController**: Receives device reports
- **UploadStatsController**: Receives usage statistics

## Development Commands

```bash
# Build the solution
dotnet build

# Run the server
dotnet run --project Aaru.Server

# Add a new migration
dotnet ef migrations add <MigrationName> --project Aaru.Server.Database --startup-project Aaru.Server

# Update database
dotnet ef database update --project Aaru.Server.Database --startup-project Aaru.Server
```

## Analyzers

The project uses multiple code analyzers configured in `Directory.Build.props`:

- AsyncFixer
- ErrorProne.NET.CoreAnalyzers
- ErrorProne.NET.Structs
- Microsoft.VisualStudio.Threading.Analyzers
- Philips.CodeAnalysis.MaintainabilityAnalyzers
- Roslynator.Analyzers
- SmartAnalyzers.MultithreadingAnalyzer
- Text.Analyzers

Address all analyzer warnings before committing.

## Package Management

Packages are centrally managed via `Directory.Packages.props`. When adding packages:
1. Add version to `Directory.Packages.props`
2. Reference package without version in project `.csproj` file

## Testing

- No automated tests are present in this repository
- Manual testing against a local MariaDB/MySQL instance is required
- Ensure database migrations work correctly before deployment

## Related Repositories

This server receives data from and syncs with the main Aaru client:
- Main Aaru: https://github.com/aaru-dps/Aaru
- Aaru.CommonTypes: https://github.com/aaru-dps/Aaru.CommonTypes
- Aaru.Dto: https://github.com/aaru-dps/Aaru.Dto
- Aaru.Decoders: https://github.com/aaru-dps/Aaru.Decoders

## Common Tasks

### Adding a New Entity

1. Create model in `Aaru.Server.Database/Models/`
2. Add `DbSet<T>` to `Aaru.Server.Database/Context.cs`
3. Configure relationships in `OnModelCreating` if needed
4. Create migration
5. Create Blazor components for viewing/editing in `Aaru.Server/Components/`

### Adding a New API Endpoint

1. Create controller in `Aaru.Server/Controllers/`
2. Inject `DbContext` via constructor
3. Use `[Route("api/...")]` and `[HttpGet]`/`[HttpPost]` attributes
4. Return `ActionResult` or `ContentResult` with JSON

### Adding New Statistics Page

1. Create Razor component in `Aaru.Server/Components/Pages/Statistics/`
2. Create code-behind `.razor.cs` file
3. Inject `DbContext` and query statistics data
4. Add navigation link in sidebar if needed

