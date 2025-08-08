using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IDNACCalculator.Models
{
    /// <summary>
    /// Data model for the System Overview by-level snapshot grid
    /// </summary>
    public class SystemOverviewData : INotifyPropertyChanged
    {
        private string _level;
        private double _elevation;
        private string _zone;
        private int _idnacDevices;
        private double _idnacCurrent;
        private double _idnacWattage;
        private int _idnacCircuits;
        private int _idnetDevices;
        private int _idnetPoints;
        private int _idnetUnitLoads;
        private int _idnetChannels;
        private double _utilizationPercent;
        private string _limitingFactor;
        private string _comments;

        public string Level
        {
            get => _level;
            set { _level = value; OnPropertyChanged(); }
        }

        public double Elevation
        {
            get => _elevation;
            set { _elevation = value; OnPropertyChanged(); }
        }

        public string Zone
        {
            get => _zone;
            set { _zone = value; OnPropertyChanged(); }
        }

        // IDNAC Properties
        public int IDNACDevices
        {
            get => _idnacDevices;
            set { _idnacDevices = value; OnPropertyChanged(); }
        }

        public double IDNACCurrent
        {
            get => _idnacCurrent;
            set { _idnacCurrent = value; OnPropertyChanged(); }
        }

        public double IDNACWattage
        {
            get => _idnacWattage;
            set { _idnacWattage = value; OnPropertyChanged(); }
        }

        public int IDNACCircuits
        {
            get => _idnacCircuits;
            set { _idnacCircuits = value; OnPropertyChanged(); }
        }

        // IDNET Properties
        public int IDNETDevices
        {
            get => _idnetDevices;
            set { _idnetDevices = value; OnPropertyChanged(); }
        }

        public int IDNETPoints
        {
            get => _idnetPoints;
            set { _idnetPoints = value; OnPropertyChanged(); }
        }

        public int IDNETUnitLoads
        {
            get => _idnetUnitLoads;
            set { _idnetUnitLoads = value; OnPropertyChanged(); }
        }

        public int IDNETChannels
        {
            get => _idnetChannels;
            set { _idnetChannels = value; OnPropertyChanged(); }
        }

        // Utilization Properties
        public double UtilizationPercent
        {
            get => _utilizationPercent;
            set { _utilizationPercent = value; OnPropertyChanged(); }
        }

        public string LimitingFactor
        {
            get => _limitingFactor;
            set { _limitingFactor = value; OnPropertyChanged(); }
        }

        public string Comments
        {
            get => _comments;
            set { _comments = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}