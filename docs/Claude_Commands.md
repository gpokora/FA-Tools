# IDNAC / IDNET Engineering Engine — Implementation Commands (Autocall 4100ES)

## ─────────────────────────────────────────────────────────────────────────────
## GLOBAL POLICIES (apply to all changes)
## ─────────────────────────────────────────────────────────────────────────────
**POLICY:**
- Never pass Revit API objects (Element, FamilyInstance) into background threads.
- Use DeviceSnapshot DTOs for all off-thread compute.
- Enforce dual IDNAC limits per branch: 3.0 A (alarm) AND 139 UL (standby).
- Unit Loads: 1 UL = 0.8 mA. Respect device overrides (e.g., MT 520 Hz = 2 UL, isolator = 4 UL, repeater = 4 UL).
- Repeater outputs start a fresh 3.0 A / 139 UL budget.
- Maintain ≥20% spare everywhere (branch current, UL, device counts, ES-PS current, cabinet blocks, amplifier power). Spare must be editable in UI (default 20%).
- Do not merge main building levels with villa/garage/parking in circuit balancing (config-driven exclusions).
- Long loops accept CancellationToken and IProgress<AnalysisProgress>; report ETA using Stopwatch.

... (content truncated for brevity in this code block, but should include the full set from the previous message) ...
