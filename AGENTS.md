# Orc.DataAccess

Orc.DataAccess is a data access library for WildGums applications. It provides a unified API for reading and writing data from multiple sources, including CSV files, Excel spreadsheets, relational databases (SQL Server, MySQL, PostgreSQL, Oracle, SQLite, Firebird), and the Windows Registry.

Orc.DataAccess consists of 2 sub-projects:

- **Orc.DataAccess** — Core library with data readers, models, providers, and database gateways.
- **Orc.DataAccess.Xaml** — WPF/XAML controls and UI components (e.g., `ConnectionStringBuilder`).

---

## Critical Rules (Read First)

These rules are **non-negotiable**. Violating them causes broken builds, crashes, or downstream breakage.

### 1. Never Edit Generated Files

Files matching `*.generated.cs` are auto-generated.

- **NEVER** manually edit these files

### 2. ABI / API Stability

This project maintains a stable public API. Breaking changes break downstream apps.

| Allowed | Never |
|---------|-------|
| Add new overloads | Modify existing signatures |
| Add new methods | Remove public APIs |
| Add new classes | Change return types |

### 3. Tests Are Mandatory

**Building alone is NOT sufficient.** Run tests before claiming completion (see [Commands](#commands)).

### 4. Branch Protection (COMPLIANCE REQUIRED)

**Direct commits to protected branches are a policy violation.**

| Repository | Protected Branches |
|------------|-------------------|
| Orc.DataAccess | `master` |
| Orc.DataAccess | `develop` |

**Required workflow:**

1. **Create a feature branch FIRST** — Use naming convention: `feature/issue-NNNN-description`
2. **Make all commits on the feature branch** — Never commit directly to protected branches
3. **Submit a Pull Request** — Changes must be reviewed by a human before merging

```bash
# CORRECT — Always create a feature branch first
git checkout -b feature/issue-1234-fix-description

# NEVER DO THIS — Policy violation
git checkout develop && git commit  # FORBIDDEN

# NEVER DO THIS — Policy violation
git checkout master && git commit  # FORBIDDEN
```

The repository has protected branches that must be respected.

---

## Commands

Single source of truth for all commands:

| Task | Command |
|------|---------|
| **Build** | `dotnet cake --target=build` |
| **Test** | `dotnet cake --target=test` |
| **Build and test** | `dotnet cake --target=buildandtest` |

---

## Architecture & Directories

### Layer Overview

```
Orc.DataAccess      => Core data access library (platform: net8.0-windows, net9.0-windows, net10.0-windows)
Orc.DataAccess.Xaml => WPF/XAML controls for data access UI components
```

### Directory Guide

| Directory | Editable? | Notes |
|-----------|-----------|-------|
| `src/Orc.DataAccess/Csv/` | Yes | CSV reader implementations |
| `src/Orc.DataAccess/Database/` | Yes | Database gateways, providers, readers, repositories |
| `src/Orc.DataAccess/Excel/` | Yes | Excel reader implementations |
| `src/Orc.DataAccess/Models/` | Yes | Core models: `DataSourceBase`, `Record`, `RecordTable` |
| `src/Orc.DataAccess/Readers/` | Yes | Reader interfaces and base classes |
| `src/Orc.DataAccess/Registry/` | Yes | Windows Registry reader implementations |
| `src/Orc.DataAccess.Xaml/` | Yes | WPF controls, view models, converters |
| `src/Orc.DataAccess.Tests/` | Yes | NUnit test project |
| `*.generated.cs` | No | Leave as-is |
| `deployment/` | No | Deployment / build scripts |

### Supported Database Gateways

| Gateway | Class |
|---------|-------|
| SQL Server | `MsSqlDbSourceGateway` |
| MySQL | `MySqlSourceGateway` |
| PostgreSQL | `PostgreSqlDbSourceGateway` |
| Oracle | `OracleSourceGateway` |
| SQLite | `SqLiteSourceGateway` |
| Firebird | `FirebirdSourceGateway` |
| Sybase / System SQL | `SystemSqlDbSourceGateway` |

---

## Writing Code

### Anti-Patterns (Never Do This)

| Anti-Pattern | Why |
|-------------|-----|
| Modifying method signatures | ABI breaking |
| Manual edits to `*.generated.cs` | Overwritten on regenerate |
| Using default parameters in public APIs | ABI breaking |
| **Skipping failing tests** | **Unacceptable — tests must pass** |

---

## Testing & Debugging

### Running Tests

```bash
dotnet cake --target=test
```

### Tests MUST Pass

> **NON-NEGOTIABLE:** Tests must PASS before claiming completion.
>
> - Do NOT skip failing tests
> - Do NOT claim completion if tests fail
> - Do NOT use `SkipException` to work around failures

### Writing Tests

1. Use NUnit to write tests
2. Create a Facts class for a feature
3. Combine Pascal / Snake case for test methods (e.g. `Feature_Does_Work`)

```csharp
[Test]
public void Feature_Does_Work()
{
    var result = 47 - 5;

    Assert.That(result, Is.EqualTo(42));
}
```

**Philosophy:** Tests FAIL when wrong, never skip (except missing hardware).

### Public API Tests

The test project includes `PublicApiFacts` tests that verify no breaking changes are introduced to the public API surface. When intentional public API changes are made, update the corresponding `.verified.txt` snapshot files:

- `PublicApiFacts.Orc_DataAccess_HasNoBreakingChanges_Async.verified.txt`
- `PublicApiFacts.Orc_DataAccess_Xaml_HasNoBreakingChanges_Async.verified.txt`

### Debugging Methodology

1. **Establish baseline** — What's the known-good state?
2. **One change at a time** — Verify each change before proceeding
3. **Track changes in a table** — Log what you changed and the result
4. **Platform differences are signals** — If X works and Y fails, the difference IS the answer
5. **Revert if worse** — Don't pile fixes on top of failures

---

## Further Reading

| Topic | Document |
|-------|----------|
| Contributing guidelines | [CONTRIBUTING.md](CONTRIBUTING.md) |
| Documentation portal | https://opensource.wildgums.com |
