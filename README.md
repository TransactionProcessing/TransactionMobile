# TransactionMobile

![Alt](https://repobeats.axiom.co/api/embed/8c4c40e2db665c43af4c4e1014a3dca92fe330aa.svg "Repobeats analytics image")

## Release versioning

Releases are tagged in calendar format: **`vYYYY.M.D`** (e.g. `v2026.4.1`).

The release workflows derive three version values from this tag automatically.

| Value | Formula | Example (`v2026.4.1`, DOY = 91) |
|---|---|---|
| **ApplicationDisplayVersion** (SemVer 3-part) | `YY.M.DOY` | `26.4.91` |
| **Windows MSIX/Appx package version** | `YYYY.M.D.0` | `2026.4.1.0` |
| **Android versionCode** | `YYYY * 1000 + DOY` | `2026091` |

Where:
- `YY = YYYY - 2000`
- `M = month number` (1–12)
- `D = day of month` (1–31)
- `DOY = day-of-year` (1–366)

### Key properties

- **ApplicationDisplayVersion** is a valid 3-part SemVer-like version, satisfying `Microsoft.Maui.Resizetizer` validation requirements.
- **Windows package version** uses the full 4-part `YYYY.M.D.0` format required by MSIX/Appx.
- **Android versionCode** is a monotonically increasing integer across all releases.

### Creating a release

Tag format: `vYYYY.M.D` (single or double digit month/day both accepted).

Example tags: `v2026.4.1`, `v2026.12.31`, `v2027.1.5`
