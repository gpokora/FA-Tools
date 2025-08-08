# IDNAC Calculator – UI & UX Implementation Instructions for Claude

## Objective
Implement a **clean, intuitive, fully transparent** WPF designer UI for the IDNAC Calculator tool using **DevExpress WPF** components.  
No data is to be hidden from designers — all columns visible by default.  
Primary goals: clarity, easy scanning, and direct editing capabilities.

---

## 1. Top-Level UI Structure
Use **DockLayoutManager** for overall layout, **TabControl** for main navigation.

### Tabs
1. **Overview**
2. **Devices**
    - IDNET Devices
    - IDNAC Devices
3. **Analysis**
    - IDNAC Analysis
    - IDNET Analysis
4. **Panels & Equipment** ✅ *(new)*
5. **Optimization**
6. **Reports**

---

## 2. General UI Rules
- **All columns visible by default** (use ColumnChooser for optional hiding, but default is full visibility).
- **Persistent layouts** per user: `GridControl.View.SaveLayoutToXml` and `RestoreLayoutFromXml`.
- **Bands for grouping** related columns (BandedTableView and TreeList bands).
- **Summary row pinned** at top/bottom to show totals, limits, and warnings.
- **Master-detail drilldown**: double-click rows to open a right-hand detail pane.
- **Always-on filtering** (AutoFilterRow enabled on all grids).
- **Inline validation**: show icons + tooltips, not modal dialogs.
- **Column bands match data model** (Location, Electrical, Network, Compliance, Maintenance).

---

## 3. Overview Tab
### KPI Strip (Top, non-interactive)
Display as **3 horizontal cards**:
- **IDNAC**: Device Qty, Circuits Qty, Total Strobe Current (A), Total Speaker Wattage (W)
- **IDNET**: Device Qty, Channels Qty, Unit Loads, Segments
- **Panels**: Nodes, Transponders, Amplifiers, Repeaters, Annunciators

### By-Level Snapshot (Main Grid)
**Bands**:
- Level (Level, Elevation, Zone)
- IDNAC (Devices, Current A, Wattage W, Circuits)
- IDNET (Devices, Points, Unit Loads, Channels)
- Utilization (Utilization %, Limiting Factor)
- Notes

**Behavior**:
- Summary row totals for numeric columns.
- Double-click opens detail pane with circuits and channels for that level.

---

## 4. Devices Tab
Two subtabs: **IDNET Devices** and **IDNAC Devices**.

### IDNET Devices Columns:
- **Identity**: Device ID, ElementId
- **Location**: Level, Elevation, Zone
- **Type**: Family Name, Device Type, Mounting
- **Network**: Segment, Channel, Address, Unit Load
- **Electrical**: Current (A), Power Source
- **Compliance**: Validation Status, Issues

### IDNAC Devices Columns:
- **Identity**: Device ID, ElementId
- **Location**: Level, Elevation, Zone
- **Notification**: Device Type, Candela, Mounting
- **Electrical**: Current (A), Wattage (W), Unit Loads
- **Addressing**: Panel, Branch, NAC Address
- **Compliance**: Validation Status, Issues

---

## 5. Analysis Tab
### IDNAC Analysis
**Columns**:
- Panel, Branch, Name
- Devices, Total Current A, Total Wattage W, Total Unit Loads
- Utilization %, Voltage Drop %, Limiting Factor
- Length, Wire Gauge, EOL, Has Isolator?, Has Repeater?
- Validation (errors/warnings list)

### IDNET Analysis
**Columns**:
- Channel, Segment, Panel/Transponder
- Total Devices, Detectors, Modules, Points, Unit Loads
- Utilization %, Limiting Factor, Channels Required
- Cable Length, Loop Redundancy, Isolators
- Validation (errors/warnings list)

---

## 6. Panels & Equipment Tab (New)
Two views: **Tree** and **Flat Grids**.

### Equipment Tree
**Hierarchy**:
Panel → Transponder → Channel/Segment → NAC Circuit → Device

**Bands**:
1. **Identity**: Type, Name, Model, Manufacturer, Firmware
2. **Location**: Level, Room, Elevation, Zone
3. **Electrical**: Capacity A/W/UL, Assigned Load A/W/UL, Spare %, Battery Ah
4. **Network**: Address, Segment, Channel, Topology, Redundancy
5. **Compliance**: Validation Status, Issues
6. **Maintenance**: Enclosure, Rack Units, Notes

### Flat Grids (subtabs)
- Panels
- Transponders
- Amplifiers / NAC PSUs
- Repeaters / Isolators
- Annunciators
- Batteries & Power
- Channels / Segments
- NAC Circuits

---

## 7. Optimization Tab
### IDNET Optimization
- Combined Levels, Individual Levels
- Totals: Detectors, Modules, Points, Unit Loads
- Channels Required (Current vs Proposed)
- Side-by-side diff with delta highlight

### IDNAC Optimization
- Combined Levels, Individual Levels
- Totals: Strobes, Speakers, Speaker-Strobes, Current, Wattage, Unit Loads
- Circuits Required (Current vs Proposed)
- Utilization and Limiting Factor columns

---

## 8. Reports Tab
- Export each grid to XLSX, CSV, or PDF
- Preserve filters and column order in export

---

## 9. DevExpress Implementation Tips
- **Use fully qualified namespaces** to avoid Revit/WPF conflicts (`System.Windows.Visibility`).
- **Predefine columns** to avoid virtualization errors.
- **TreeList**: set `KeyFieldName` and `ParentFieldName` for hierarchy.
- **BandedTableView**: keep bands logical and small enough to scan without horizontal overload.
- Enable **AutoWidth** for TreeList, fixed width for KPI strips.
- **Save/restore layouts** to `%AppData%` or project-specific path.

---

## 10. Data Model Hints
- `LevelOverviewItem`: add IdnetChannels, Zone, Comments
- `IdnacCircuitItem`: include WireGauge, Length, EOL, VoltageDropPercent, HasIsolator, HasRepeater
- `IdnetChannelItem`: include Segment, Redundancy, Isolators, CableLength
- `EquipmentNode`: Id, ParentId, Type, Name, Model, Manufacturer, Firmware, Location, Electrical, Network, Compliance, Maintenance

---

## Deliverable
Claude should implement:
- Updated **XAML layouts** with DevExpress grids/trees using bands.
- ViewModels updated with properties to match the columns above.
- Layout persistence logic.
- All columns visible by default with grouping/bands.
- Inline validation icons and tooltips.
- Master-detail drilldown behavior.
