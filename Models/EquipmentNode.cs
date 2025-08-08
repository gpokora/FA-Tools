using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IDNACCalculator.Models
{
    /// <summary>
    /// Data model for equipment hierarchy in Panels & Equipment tab
    /// </summary>
    public class EquipmentNode : INotifyPropertyChanged
    {
        private string _id;
        private string _parentId;
        private string _type;
        private string _name;
        private string _model;
        private string _manufacturer;
        private string _firmware;
        
        // Location properties
        private string _level;
        private string _room;
        private double _elevation;
        private string _zone;
        
        // Electrical properties
        private double _capacityA;
        private double _capacityW;
        private int _capacityUL;
        private double _assignedLoadA;
        private double _assignedLoadW;
        private int _assignedLoadUL;
        private double _sparePercent;
        private double _batteryAh;
        
        // Network properties
        private string _address;
        private string _segment;
        private string _channel;
        private string _topology;
        private string _redundancy;
        
        // Compliance properties
        private string _validationStatus;
        private string _issues;
        
        // Maintenance properties
        private string _enclosure;
        private int _rackUnits;
        private string _notes;
        
        // Hierarchy
        private ObservableCollection<EquipmentNode> _children;

        public string Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string ParentId
        {
            get => _parentId;
            set { _parentId = value; OnPropertyChanged(); }
        }

        public string Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Model
        {
            get => _model;
            set { _model = value; OnPropertyChanged(); }
        }

        public string Manufacturer
        {
            get => _manufacturer;
            set { _manufacturer = value; OnPropertyChanged(); }
        }

        public string Firmware
        {
            get => _firmware;
            set { _firmware = value; OnPropertyChanged(); }
        }

        // Location properties
        public string Level
        {
            get => _level;
            set { _level = value; OnPropertyChanged(); }
        }

        public string Room
        {
            get => _room;
            set { _room = value; OnPropertyChanged(); }
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

        // Electrical properties
        public double CapacityA
        {
            get => _capacityA;
            set { _capacityA = value; OnPropertyChanged(); UpdateSparePercent(); }
        }

        public double CapacityW
        {
            get => _capacityW;
            set { _capacityW = value; OnPropertyChanged(); }
        }

        public int CapacityUL
        {
            get => _capacityUL;
            set { _capacityUL = value; OnPropertyChanged(); }
        }

        public double AssignedLoadA
        {
            get => _assignedLoadA;
            set { _assignedLoadA = value; OnPropertyChanged(); UpdateSparePercent(); }
        }

        public double AssignedLoadW
        {
            get => _assignedLoadW;
            set { _assignedLoadW = value; OnPropertyChanged(); }
        }

        public int AssignedLoadUL
        {
            get => _assignedLoadUL;
            set { _assignedLoadUL = value; OnPropertyChanged(); }
        }

        public double SparePercent
        {
            get => _sparePercent;
            set { _sparePercent = value; OnPropertyChanged(); }
        }

        public double BatteryAh
        {
            get => _batteryAh;
            set { _batteryAh = value; OnPropertyChanged(); }
        }

        // Network properties
        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        public string Segment
        {
            get => _segment;
            set { _segment = value; OnPropertyChanged(); }
        }

        public string Channel
        {
            get => _channel;
            set { _channel = value; OnPropertyChanged(); }
        }

        public string Topology
        {
            get => _topology;
            set { _topology = value; OnPropertyChanged(); }
        }

        public string Redundancy
        {
            get => _redundancy;
            set { _redundancy = value; OnPropertyChanged(); }
        }

        // Compliance properties
        public string ValidationStatus
        {
            get => _validationStatus;
            set { _validationStatus = value; OnPropertyChanged(); }
        }

        public string Issues
        {
            get => _issues;
            set { _issues = value; OnPropertyChanged(); }
        }

        // Maintenance properties
        public string Enclosure
        {
            get => _enclosure;
            set { _enclosure = value; OnPropertyChanged(); }
        }

        public int RackUnits
        {
            get => _rackUnits;
            set { _rackUnits = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public ObservableCollection<EquipmentNode> Children
        {
            get => _children ?? (_children = new ObservableCollection<EquipmentNode>());
            set { _children = value; OnPropertyChanged(); }
        }

        private void UpdateSparePercent()
        {
            if (CapacityA > 0 && AssignedLoadA >= 0)
            {
                SparePercent = ((CapacityA - AssignedLoadA) / CapacityA) * 100;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}