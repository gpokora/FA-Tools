using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace IDNACCalculator
{

    public class ElectricalData
    {
        public double Wattage { get; set; }
        public double Current { get; set; }
        public double Voltage { get; set; } = 24.0; // Fire alarm system nominal voltage
        public double ApparentPower { get; set; }
        public double PowerFactor { get; set; } = 1.0;
        public List<string> FoundParams { get; set; } = new List<string>();
        public List<string> CalculatedParams { get; set; } = new List<string>();
    }

    public class ElementData
    {
        public long Id { get; set; }
        public FamilyInstance Element { get; set; }
        public string FamilyName { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public double Wattage { get; set; }
        public double Current { get; set; }
        public double Voltage { get; set; }
        public List<string> FoundParams { get; set; } = new List<string>();
        public List<string> CalculatedParams { get; set; } = new List<string>();
        public string LevelName { get; set; } = "Unknown Level";
        
        // Additional property for grid display
        public long ElementId => Id;
    }

    public class FamilyData
    {
        public int Count { get; set; }
        public double Wattage { get; set; }
        public double Current { get; set; }
    }

    public class ElectricalResults
    {
        public List<ElementData> Elements { get; set; } = new List<ElementData>();
        public Dictionary<string, double> Totals { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, FamilyData> ByFamily { get; set; } = new Dictionary<string, FamilyData>();
        public Dictionary<string, LevelData> ByLevel { get; set; } = new Dictionary<string, LevelData>();
        
        /// <summary>
        /// Alias for ByLevel for backward compatibility
        /// </summary>
        public Dictionary<string, LevelData> LevelData => ByLevel;
        
        /// <summary>
        /// Total current across all devices
        /// </summary>
        public double TotalCurrent => Elements?.Sum(e => e.Current) ?? 0.0;
        
        /// <summary>
        /// Total wattage across all devices
        /// </summary>
        public double TotalWattage => Elements?.Sum(e => e.Wattage) ?? 0.0;
        
        /// <summary>
        /// Total number of devices
        /// </summary>
        public int TotalDevices => Elements?.Count ?? 0;
        
        /// <summary>
        /// Total unit loads across all devices
        /// </summary>
        public int TotalUnitLoads => Elements?.Sum(e => (int)(e.Current * 1.25)) ?? 0; // Estimate UL from current
    }

    public class LevelData
    {
        public int Devices { get; set; }
        public double Current { get; set; }
        public double Wattage { get; set; }
        public Dictionary<string, int> Families { get; set; } = new Dictionary<string, int>();
        public bool Combined { get; set; }
        public bool RequiresIsolators { get; set; }
        public List<string> OriginalFloors { get; set; } = new List<string>();
        public double UtilizationPercent { get; set; }
    }

    // IDNAC Analysis Models
    public class IDNACAnalysisResult
    {
        public int IdnacsRequired { get; set; }
        public string Status { get; set; }
        public string LimitingFactor { get; set; }
        public double Current { get; set; }
        public double Wattage { get; set; }
        public int Devices { get; set; }
        public int UnitLoads { get; set; }
        public SpareCapacityInfo SpareInfo { get; set; } = new SpareCapacityInfo();
        public bool IsCombined { get; set; }
        public List<string> OriginalFloors { get; set; } = new List<string>();
        public bool RequiresIsolators { get; set; }
    }

    public class SpareCapacityInfo
    {
        public double SpareCurrent { get; set; }
        public int SpareDevices { get; set; }
        public double CurrentUtilization { get; set; }
        public double DeviceUtilization { get; set; }
    }

    public class IDNACWarning
    {
        public string Type { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public string Recommendation { get; set; }
        public Dictionary<string, object> Details { get; set; } = new Dictionary<string, object>();
    }

    public class IDNACSystemResults
    {
        public Dictionary<string, IDNACAnalysisResult> LevelAnalysis { get; set; } = new Dictionary<string, IDNACAnalysisResult>();
        public int TotalIdnacsNeeded { get; set; }
        public List<IDNACWarning> Warnings { get; set; } = new List<IDNACWarning>();
        public Dictionary<string, FamilyData> FireAlarmFamilies { get; set; } = new Dictionary<string, FamilyData>();
        public Dictionary<string, FamilyData> OtherFamilies { get; set; } = new Dictionary<string, FamilyData>();
        public OptimizationSummary OptimizationSummary { get; set; }
        
        /// <summary>
        /// Alias for LevelAnalysis for backward compatibility
        /// </summary>
        public Dictionary<string, IDNACAnalysisResult> LevelResults => LevelAnalysis;
        
        /// <summary>
        /// Total current across all IDNAC circuits
        /// </summary>
        public double TotalCurrent => LevelAnalysis?.Values.Sum(l => l.Current) ?? 0.0;
        
        /// <summary>
        /// Total number of devices in the IDNAC system
        /// </summary>
        public int TotalDevices => LevelAnalysis?.Values.Sum(l => l.Devices) ?? 0;
        
        /// <summary>
        /// Total unit loads across all IDNAC circuits
        /// </summary>
        public int TotalUnitLoads => LevelAnalysis?.Values.Sum(l => l.UnitLoads) ?? 0;
    }

    public class OptimizationSummary
    {
        public int OriginalFloors { get; set; }
        public int OptimizedFloors { get; set; }
        public int Reduction { get; set; }
        public double ReductionPercent { get; set; }
        public List<Tuple<string, LevelData>> CombinedFloors { get; set; } = new List<Tuple<string, LevelData>>();
    }

    // Amplifier Models
    public class DeviceTypeAnalysis
    {
        public Dictionary<string, DeviceTypeData> DeviceTypes { get; set; } = new Dictionary<string, DeviceTypeData>();
    }

    public class DeviceTypeData
    {
        public int Count { get; set; }
        public double Current { get; set; }
        public double Wattage { get; set; }
        public Dictionary<string, int> Families { get; set; } = new Dictionary<string, int>();
    }

    public class AmplifierRequirements
    {
        public int AmplifiersNeeded { get; set; }
        public string AmplifierType { get; set; }
        public int AmplifierBlocks { get; set; }
        public double AmplifierPowerUsable { get; set; }
        public double AmplifierPowerMax { get; set; }
        public double AmplifierCurrent { get; set; }
        public int SpeakerCount { get; set; }
        public int SpareCapacityPercent { get; set; } = 20;
    }

    // Panel Placement Models
    public class PanelPlacementRecommendation
    {
        public string Strategy { get; set; }
        public int PanelCount { get; set; }
        public Tuple<string, string> Location { get; set; } // Location, Reasoning
        public string Reasoning { get; set; }
        public CabinetConfiguration Equipment { get; set; }
        public AmplifierRequirements AmplifierInfo { get; set; }
        public List<string> Advantages { get; set; } = new List<string>();
        public List<string> Considerations { get; set; } = new List<string>();
        public List<PanelInfo> Panels { get; set; } = new List<PanelInfo>();
        public string AmplifierStrategy { get; set; }
        public Dictionary<string, object> SystemTotals { get; set; } = new Dictionary<string, object>();
        public int MinPanelsNeeded { get; set; }
    }

    public class PanelInfo
    {
        public string PanelId { get; set; }
        public string Location { get; set; }
        public List<string> ServesLevels { get; set; } = new List<string>();
        public int IdnacsRequired { get; set; }
        public int SpeakersEstimated { get; set; }
        public int AmplifiersNeeded { get; set; }
        public string AmplifierType { get; set; }
        public int AmplifierBlocks { get; set; }
        public double AmplifierCurrent { get; set; }
        public string AmplifierInfo { get; set; }
        public string Reasoning { get; set; }
    }

    public class CabinetConfiguration
    {
        public string CabinetType { get; set; }
        public int PowerSupplies { get; set; }
        public int TotalIdnacs { get; set; }
        public int AvailableBlocks { get; set; }
        public int AmplifierBlocksUsed { get; set; }
        public int RemainingBlocks { get; set; }
        public double AmplifierCurrent { get; set; }
        public double EsPsCapacity { get; set; }
        public double PowerMargin { get; set; }
        public bool BatteryChargerAvailable { get; set; }
        public List<string> ModelConfig { get; set; } = new List<string>();
    }

    // UI Data Models for Results Display
    public class LevelDetailItem
    {
        public string Level { get; set; }
        public int Devices { get; set; }
        public string Current { get; set; }
        public string Wattage { get; set; }
        public int IdnacsRequired { get; set; }
        public string Status { get; set; }
        public string LimitingFactor { get; set; }
    }

    public class FamilyDetailItem
    {
        public string FamilyName { get; set; }
        public int Count { get; set; }
        public string TotalCurrent { get; set; }
        public string TotalWattage { get; set; }
        public string AvgCurrent { get; set; }
        public string Category { get; set; }
    }

    // Data model for IDNAC Analysis Grid

    public class IDNACAnalysisGridItem
    {
        public string Level { get; set; }
        public int Devices { get; set; }
        public string Current { get; set; }
        public string Wattage { get; set; }
        public int IdnacsRequired { get; set; }
        public string UtilizationPercent { get; set; }
        public string UtilizationCategory { get; set; }
        public string Status { get; set; }
        public string LimitingFactor { get; set; }
        public string SpareCurrent { get; set; }
        public string SpareDevices { get; set; }
        public string IsolatorsRequired { get; set; }
        public string OriginalFloors { get; set; }
        public bool IsCombined { get; set; }

        // Enhanced properties for visual styling
        public System.Windows.Media.Brush UtilizationBrush
        {
            get
            {
                if (UtilizationCategory == "Optimized")
                    return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80));
                if (UtilizationCategory == "Excellent")
                    return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 150, 243));
                if (UtilizationCategory == "Underutilized")
                    return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(244, 67, 54));
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(158, 158, 158));
            }
        }

    }

    // Enhanced Visual Metric Cards
    public class SystemMetricCard
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string Icon { get; set; }
        public System.Windows.Media.Brush IconColor { get; set; }
        public System.Windows.Media.Brush ValueColor { get; set; }
        public string Subtitle { get; set; }
        public double ProgressValue { get; set; }
        public System.Windows.Media.Brush ProgressColor { get; set; }
    }

    // Enhanced Device Category Display
    public class DeviceCategoryCard
    {
        public string CategoryName { get; set; }
        public string DisplayName => CategoryName?.ToUpper();
        public int Count { get; set; }
        public string CountDisplay => Count.ToString("N0");
        public double Current { get; set; }
        public string CurrentDisplay => $"{Current:F2}A";
        public double Wattage { get; set; }
        public string WattageDisplay => $"{Wattage:F1}W";
        public string AvgCurrent => Count > 0 ? $"{(Current / Count):F3}A" : "0A";
        public string Icon { get; set; }
        public System.Windows.Media.Brush CategoryColor { get; set; }
        public string PowerRequirement { get; set; }
        public bool RequiresAmplifier { get; set; }
        public Dictionary<string, int> Families { get; set; } = new Dictionary<string, int>();

        // Visual properties
        public string StatusIcon => RequiresAmplifier ? "🔊⚡" : "⚡";
        public System.Windows.Media.Brush StatusColor => RequiresAmplifier ?
            new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 39, 176)) :
            new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80));
    }


    // Enhanced Warning Display
    public class SystemWarning
    {
        public string Type { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public string Recommendation { get; set; }
        public string Icon { get; set; }
        public System.Windows.Media.Brush SeverityColor { get; set; }
        public System.Windows.Media.Brush BackgroundColor { get; set; }
        public Dictionary<string, object> Details { get; set; } = new Dictionary<string, object>();

        // Visual styling based on severity
        public static SystemWarning FromIDNACWarning(IDNACWarning warning)
        {
            var systemWarning = new SystemWarning
            {
                Type = warning.Type,
                Severity = warning.Severity,
                Message = warning.Message,
                Recommendation = warning.Recommendation,
                Details = warning.Details
            };

            switch (warning.Severity?.ToUpper())
            {
                case "HIGH":
                    systemWarning.Icon = "🚨";
                    systemWarning.SeverityColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(244, 67, 54));
                    systemWarning.BackgroundColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 235, 238));
                    break;
                case "MEDIUM":
                    systemWarning.Icon = "⚠️";
                    systemWarning.SeverityColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 152, 0));
                    systemWarning.BackgroundColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 243, 224));
                    break;
                default:
                    systemWarning.Icon = "ℹ️";
                    systemWarning.SeverityColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 150, 243));
                    systemWarning.BackgroundColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));
                    break;
            }

            return systemWarning;
        }
    }

    // Enhanced Amplifier Display
    public class AmplifierAnalysisCard
    {
        public string RequirementType { get; set; }
        public string Value { get; set; }
        public string Details { get; set; }
        public string Icon { get; set; }
        public System.Windows.Media.Brush IconColor { get; set; }
        public bool IsHighlight { get; set; }
        public string Unit { get; set; }
        public double NumericValue { get; set; }
        public string Status { get; set; }

        // Progress indicator for requirements
        public double ProgressPercent { get; set; }
        public System.Windows.Media.Brush ProgressColor { get; set; }
    }

    // Action Items and Recommendations
    public class SystemRecommendation
    {
        public string Action { get; set; }
        public string TabReference { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }
        public string Recommendation { get; set; }
        public string Impact { get; set; }
        public string Justification { get; set; }
        public Dictionary<string, object> TechnicalDetails { get; set; } = new Dictionary<string, object>();

        // Visual properties
        public System.Windows.Media.Brush PriorityColor { get; set; }
        public string PriorityIcon { get; set; }
        public System.Windows.Media.Brush CategoryColor { get; set; }
        public string ActionRequired { get; set; }
        public bool IsUrgent { get; set; }
    }

    // Panel Placement Analysis
    public class PanelPlacementCard
    {
        public string LocationName { get; set; }
        public string FloorLocation { get; set; }
        public int RecommendedPanels { get; set; }
        public List<string> FloorsCovered { get; set; } = new List<string>();
        public double CoverageRadius { get; set; }
        public string AccessibilityRating { get; set; }
        public string MaintenanceRating { get; set; }
        public List<string> Advantages { get; set; } = new List<string>();
        public List<string> Considerations { get; set; } = new List<string>();

        // Technical requirements
        public int IdnacsSupported { get; set; }
        public int AmplifiersRequired { get; set; }
        public double PowerRequirement { get; set; }
        public string CabinetConfiguration { get; set; }
        public bool BatteryBackupRequired { get; set; }

        // Visual properties
        public System.Windows.Media.Brush LocationColor { get; set; }
        public string LocationIcon { get; set; }
        public System.Windows.Media.Brush RatingColor { get; set; }
        public double OverallScore { get; set; }
        public string RecommendationLevel { get; set; }
    }

    // Optimization Result Display
    public class OptimizationResultCard
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int OriginalFloors { get; set; }
        public int OptimizedFloors { get; set; }
        public int Reduction { get; set; }
        public double ReductionPercent { get; set; }
        public double EfficiencyGain { get; set; }
        public string SummaryText { get; set; }
        public List<string> CombinedFloors { get; set; } = new List<string>();
        public string ImpactLevel { get; set; }

        // Visual properties
        public System.Windows.Media.Brush ImpactColor { get; set; }
        public string ImpactIcon { get; set; }
        public double ProgressValue => ReductionPercent;
        public System.Windows.Media.Brush ProgressColor { get; set; }
        public string SuccessLevel { get; set; }
    }

    // Enhanced Level Analysis Display
    public class LevelAnalysisCard
    {
        public string LevelName { get; set; }
        public string DisplayName { get; set; }
        public int DeviceCount { get; set; }
        public double Current { get; set; }
        public double Wattage { get; set; }
        public int IdnacsRequired { get; set; }
        public double UtilizationPercent { get; set; }
        public string Status { get; set; }
        public string LimitingFactor { get; set; }
        public bool IsCombined { get; set; }
        public bool RequiresIsolators { get; set; }
        public List<string> OriginalFloors { get; set; } = new List<string>();
        public Dictionary<string, int> DeviceFamilies { get; set; } = new Dictionary<string, int>();

        // Spare capacity details
        public double SpareCurrent { get; set; }
        public int SpareDevices { get; set; }
        public double CurrentUtilization { get; set; }
        public double DeviceUtilization { get; set; }

        // Visual styling
        public System.Windows.Media.Brush UtilizationColor { get; set; }
        public System.Windows.Media.Brush StatusColor { get; set; }
        public string StatusIcon { get; set; }
        public bool IsOptimized { get; set; }
        public string OptimizationBadge { get; set; }
    }

    // System Summary Dashboard
    public class SystemSummaryDashboard
    {
        public SystemMetricCard TotalDevices { get; set; }
        public SystemMetricCard TotalCurrent { get; set; }
        public SystemMetricCard IdnacsRequired { get; set; }
        public SystemMetricCard SpeakerSystem { get; set; }
        public SystemMetricCard SystemEfficiency { get; set; }
        public SystemMetricCard PowerSupplies { get; set; }
        public SystemMetricCard EstimatedPanels { get; set; }
        public SystemMetricCard ComplianceStatus { get; set; }

        public OptimizationResultCard OptimizationResults { get; set; }
        public List<SystemWarning> SystemWarnings { get; set; } = new List<SystemWarning>();
        public List<string> KeyRecommendations { get; set; } = new List<string>();
        public List<string> NextSteps { get; set; } = new List<string>();

        // Overall system health
        public string SystemHealth { get; set; }
        public System.Windows.Media.Brush HealthColor { get; set; }
        public string HealthIcon { get; set; }
        public double OverallScore { get; set; }
    }

    // Action Items and Recommendations
    public class ActionItem
    {
        public int Priority { get; set; }
        public string PriorityDisplay => $"{Priority}️⃣";
        public string Action { get; set; }
        public string Category { get; set; }
        public string TabReference { get; set; }
        public string Urgency { get; set; }
        public System.Windows.Media.Brush UrgencyColor { get; set; }
        public string Icon { get; set; }
        public bool IsCompleted { get; set; }
        public string EstimatedEffort { get; set; }
        public string ExpectedBenefit { get; set; }
    }

    // Export and Reporting Models
    public class ReportSection
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> KeyPoints { get; set; } = new List<string>();
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public List<ReportTable> Tables { get; set; } = new List<ReportTable>();
        public List<ReportChart> Charts { get; set; } = new List<ReportChart>();
    }

    public class ReportTable
    {
        public string Title { get; set; }
        public List<string> Headers { get; set; } = new List<string>();
        public List<List<string>> Rows { get; set; } = new List<List<string>>();
    }

    public class ReportChart
    {
        public string Title { get; set; }
        public string ChartType { get; set; }
        public Dictionary<string, double> Data { get; set; } = new Dictionary<string, double>();
    }

    #region IDNET Data Models

    public class IDNETDevice
    {
        public string DeviceId { get; set; }
        public string FamilyName { get; set; }
        public string DeviceType { get; set; }
        public string Location { get; set; }
        public string Level { get; set; }
        public double PowerConsumption { get; set; }
        public int UnitLoads { get; set; }
        public string Zone { get; set; }
        public XYZ Position { get; set; }
        public int SuggestedAddress { get; set; }
        public string NetworkSegment { get; set; }
        
        // ENHANCED: Additional IDNET parameter extraction
        public string Address { get; set; }              // Any parameter containing "ADDRESS"
        public string Function { get; set; }             // Any parameter containing "FUNCTION"  
        public string Area { get; set; }                 // Any parameter containing "AREA"
        
        // Additional extracted parameters for comprehensive analysis
        public Dictionary<string, string> AddressParameters { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> FunctionParameters { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> AreaParameters { get; set; } = new Dictionary<string, string>();
        
        // Summary of all extracted parameters for debugging/analysis
        public List<string> ExtractedParameters { get; set; } = new List<string>();
    }

    public class IDNETLevelAnalysis
    {
        public string LevelName { get; set; }
        public int TotalDevices { get; set; }
        public Dictionary<string, int> DeviceTypeCount { get; set; }
        public double TotalPowerConsumption { get; set; }
        public int TotalUnitLoads { get; set; }
        public int SuggestedNetworkSegments { get; set; }
        public List<IDNETDevice> Devices { get; set; }
    }

    public class IDNETNetworkSegment
    {
        public string SegmentId { get; set; }
        public List<IDNETDevice> Devices { get; set; }
        public double EstimatedWireLength { get; set; }
        public int DeviceCount { get; set; }
        public bool RequiresRepeater { get; set; }
        public string StartingAddress { get; set; }
        public string EndingAddress { get; set; }
        public List<string> CoveredLevels { get; set; }
    }

    public class IDNETSystemResults
    {
        public Dictionary<string, IDNETLevelAnalysis> LevelAnalysis { get; set; }
        public List<IDNETDevice> AllDevices { get; set; }
        public int TotalDevices { get; set; }
        public double TotalPowerConsumption { get; set; }
        public int TotalUnitLoads { get; set; }
        public List<IDNETNetworkSegment> NetworkSegments { get; set; }
        public IDNETSystemSummary SystemSummary { get; set; }
        public DateTime AnalysisTimestamp { get; set; }
        
        /// <summary>
        /// Number of channels required for the IDNET system
        /// </summary>
        public int ChannelsRequired => SystemSummary?.RecommendedNetworkChannels ?? 0;
        
        /// <summary>
        /// Maximum number of devices per channel
        /// </summary>
        public int MaxDevicesPerChannel => 159; // NFPA standard for IDNET
    }

    public class IDNETSystemSummary
    {
        public int RecommendedNetworkChannels { get; set; }
        public int RepeatersRequired { get; set; }
        public double TotalWireLength { get; set; }
        public List<string> SystemRecommendations { get; set; }
        public bool IntegrationWithIDNAC { get; set; }
        public string PowerSupplyRequirements { get; set; }
    }

    public class IDNETAnalysisGridItem
    {
        public string Level { get; set; }
        public string SmokeDetectors { get; set; }
        public string HeatDetectors { get; set; }
        public string ManualStations { get; set; }
        public string Modules { get; set; }
        public string TotalDevices { get; set; }
        public string PowerConsumption { get; set; }
        public string NetworkSegments { get; set; }
        public string AddressRange { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Data model for the System Overview grid showing combined IDNAC and IDNET analysis by level
    /// </summary>
    public class SystemOverviewData
    {
        public string Level { get; set; }
        public int IDNACDevices { get; set; }
        public int IDNETDevices { get; set; }
        public int TotalDevices { get; set; }
        public string IDNACCurrent { get; set; }
        public string IDNETCurrent { get; set; }
        public int IDNACCircuits { get; set; }
        public int IDNETSegments { get; set; }
        public string Status { get; set; }
    }

    #endregion

}