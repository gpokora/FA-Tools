# Claude Debugging & Refactoring Instructions for IDNAC-Calculator (WPF / DevExpress / Revit API)

## Overview
This project is a **WPF (.NET) Autodesk Revit add-in** using **DevExpress WPF controls** (`DevExpress.Xpf.Grid`, `DevExpress.Xpf.Core`) for UI grids/tree lists. It also uses Revit API namespaces (`Autodesk.Revit.DB`, `Autodesk.Revit.UI`).

Claude must **not** guess APIs — if uncertain, ask before changing. All edits should be output as **unified diffs**.

---

## Tech Stack & Rules
- **Framework**: .NET (WPF / XAML, C#)
- **UI**: DevExpress WPF GridControl / TreeListControl
- **Data Models**: `IDNACCalculator.Models` namespace (e.g., `DeviceSnapshot`, `AnalysisProgress`)
- **Environment**: Autodesk Revit add-in

### Rules
1. **Do not use Native namespaces** (`DevExpress.Xpf.Core.Native`, etc.)
2. **Filtering** → must be done on the control (`GridControl` or `TreeListControl`), not the view.
3. **Visibility conflicts**:  
   ```csharp
   using WpfVisibility = System.Windows.Visibility;
   using RvtVisibility = Autodesk.Revit.DB.Visibility;
   ```
4. Wrap **UserControl dialogs** in `Window` or `DXDialog` before `ShowDialog`.
5. Import `using IDNACCalculator.Models;` for project models.
6. Avoid duplicate event handlers — keep only one definition.
7. Avoid imaginary helpers (`string.ToImageSource()`). Use `BitmapImage` or `DXImageHelper`.

---

## Fix Map

### Duplicate Files/Handlers
- If both `DXRibbonWindow1.xaml.cs` and `DXRibbonWindow1_Fixed.xaml.cs` exist → **keep** `DXRibbonWindow1.xaml.cs` and remove the other from the project.
- Remove duplicate `ScopeActiveView_Click`, `ScopeSelection_Click`, `ScopeEntireModel_Click`.

### Filtering
**Incorrect**:
```csharp
tableView.FilterString = "...";
tableView.ClearFilter();
```
**Correct**:
```csharp
myGridControl.FilterString = "...";
myGridControl.ClearFilter();
```

### Visibility
Add alias:
```csharp
using WpfVisibility = System.Windows.Visibility;
using RvtVisibility = Autodesk.Revit.DB.Visibility;
```

### Namespaces
```csharp
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IDNACCalculator.Models;
```

### Constructors
- `Thickness` → use `(uniform)` or `(left, top, right, bottom)`.

### Addressing Panel
- `AddressingPanelWindow` requires `(Document, UIDocument)`.

### Nullables
- `.HasValue` / `.Value` require `decimal?` not `decimal`.

### Cell Values
- Use `CustomUnboundColumnData` or `CustomColumnDisplayText` instead of `CellValueEventArgs.Value`.

### Image Helpers
```csharp
var bmp = new BitmapImage(new Uri(path, UriKind.Absolute));
imageControl.Source = bmp;
```

---

## DevExpress TreeList Reference

**Namespace**: `DevExpress.Xpf.Grid`

**TreeListControl**:
- Properties: `ItemsSource`, `FilterString`, `Columns`, `SelectedItem`
- Methods: `ClearFilter()`, `ExpandAllNodes()`, `CollapseAllNodes()`

**TreeListView**:
- Methods: `ExpandAllNodes()`, `CollapseAllNodes()`
- Props: `AutoWidth`, `ShowTotalSummary`

**TreeListColumn**:
- `FieldName`, `Header`, `UnboundType`, `AllowEditing`

**Events**:
- `CustomUnboundColumnData`
- `CustomColumnDisplayText`

---

## Ask Before Changing
- DevExpress version?
- Revit version?
- Are `AssignmentTreeView` / `AddressingPanelWindow` `UserControl` or `Window`?
- x:Name of the grids/tree lists?

---

## Done Criteria
- Zero compile errors.
- No `DevExpress.*.Native` usage.
- Filtering on control, not view.
- No duplicate handlers.
- Dialogs show correctly.

## Continued


## Mission
Produce a **clean build** and restore a working UI by fixing DevExpress usages, resolving merge/duplication issues, and aligning XAML with code-behind.

---

## Golden Rules
- **Don’t invent DevExpress APIs.** If a member isn’t in DevExpress WPF, don’t use it.
- Favor **small surgical edits** over broad rewrites.
- **One truth** for every handler: if duplicates exist, keep one, delete the rest.
- **Always qualify `Visibility`** (Revit vs WPF conflict) with an alias.
- If a control name used in code doesn’t exist in XAML, **either** add a stub control **or** rename the code to an existing control — **not both**.

---

## Immediate Edits (do these first in `DXRibbonWindow1.xaml.cs`)
Add these usings/alias at the top:
```csharp
using System.Windows.Controls;                // StatusBar
using System.Windows.Controls.Primitives;     // StatusBarItem
using System.Windows.Media.Imaging;           // BitmapImage
using IDNACCalculator.Models;                 // DeviceSnapshot, AnalysisProgress, etc.
using WpfVisibility = System.Windows.Visibility;
```

Safe name lookup helper:
```csharp
private T Find<T>(string name) where T : class
    => this.FindName(name) as T;
```

---

## Fix the Specific Error Buckets

### 1) Duplicate event handlers (CS0111/CS0121)
- Dups: `ScopeSelection_Click`, `ScopeActiveView_Click`, `ScopeEntireModel_Click`.
- **Action:** Keep the most complete version of each; **delete duplicates**.
- Ensure XAML `Click` points to the **kept** method names only.

### 2) Braces / Region issues (CS1513/CS1038/CS1519)
- **Action:** Fix mismatched `#region/#endregion`, stray `}`. Start at the reported lines and match scopes.

### 3) DevExpress grid filtering API (CS1061)
```csharp
gridControl.ActiveFilterString = "[ColumnA] = 'Value'";
gridControl.ClearFilter(); 
```

### 4) WPF vs Revit `Visibility` ambiguity (CS0104/CS0176)
```csharp
SettingsSubItems.Visibility = WpfVisibility.Visible;
```

### 5) Missing XAML names used in code (CS0103)
Add stubs or rename code.

### 6) StatusBar & StatusBarItem (CS0246/CS0103)
Ensure correct using or switch to DevExpress equivalent.

### 7) Image loading
```csharp
var bmp = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
IconImage.Source = bmp;
```

### 8) Dialog vs UserControl (CS1061)
Wrap UserControl in `Window` to show modal.

### 9) Progress generic mismatch (CS1503)
Match `AnalysisProgress` type in both places.

### 10) Domain types not found (CS0246)
```csharp
using IDNACCalculator.Models;
```

### 11) Thickness constructor misuse (CS7036)
```csharp
new Thickness(4); 
new Thickness(4, 6, 4, 6);
```

### 12) Missing fields
Add `_uiDocument`, `_document`, `_lastAnalysisScope`, `_electricalResults` if needed.

### 13) `decimal` vs `decimal?`
Adjust `.HasValue` usage.

### 14) Unassigned local variable
```csharp
var spinner = Find<WaitIndicator>("SpareCapacitySpinner");
if (spinner != null) spinner.IsActive = true;
```

### 15) Missing constructor arguments (CS7036)
Pass args or create overload.

### 16) Ambiguous `Visibility` (CS0176)
Always use `WpfVisibility`.

### 17) `CellValueEventArgs.Value` setter inaccessible (CS0272)
Use `CustomUnboundColumnData` pattern.

### 18) `Cannot convert` errors (CS0029)
Wrap control in a `Window`.

---

## Handy Snippets

**Wrap UserControl as modal**
```csharp
var view = new AssignmentTreeView();
new Window { Content = view, Owner = this, Width = 900, Height = 600 }.ShowDialog();
```

**DevExpress filter**
```csharp
gridControl.ActiveFilterString = "[Level] = 'Level 6'";
gridControl.ClearFilter();
```

**Image from file**
```csharp
var bmp = new BitmapImage(new Uri(iconPath, UriKind.RelativeOrAbsolute));
MyImage.Source = bmp;
```

**WPF Visibility alias**
```csharp
ScopeSubItems.Visibility = WpfVisibility.Visible;
```

**WaitIndicator lookup**
```csharp
var spinner = Find<DevExpress.Xpf.Core.WaitIndicator>("SpareCapacitySpinner");
if (spinner != null) spinner.IsActive = true;
```
