using System;
using System.Windows;

namespace WeighBridgeApplication
{
    public class MainWindowViewModel : PropertyChangedNotify
    {
        private int _weighBridgeScale;
        private string _scaleDate;
        private int _firstMass;
        private int _secondMass;
        public DelegateCommand ProcessWeightCommand { get; set; }
        public static int Counter = 0;
        public MainWindowViewModel(int weighBridgeScale)
        {
            _weighBridgeScale = weighBridgeScale;
            FirstMass = 0;
            SecondMass = 0;
            ScaleDate = string.Format("Nett Mass is {0} KG", FirstMass - SecondMass);
            ProcessWeightCommand = new DelegateCommand(ProcessWeight);
        }

        private void ProcessWeight()
        {
            if (Counter == 0)
            {
                FirstMass = WeighBridgeScale;
            }
            if (Counter == 1)
            {
                SecondMass = WeighBridgeScale;
                GetNettMass();
            }

            Counter++;
        }

        private void GetNettMass()
        {
            ScaleDate = string.Format("{0} KG", FirstMass - SecondMass);
        }


        public int WeighBridgeScale
        {
            get { return _weighBridgeScale; }
            set
            {
                if (_weighBridgeScale != value)
                {
                    _weighBridgeScale = value;
                    OnPropertyChanged();
                }
            }
        }
        public int FirstMass
        {
            get { return _firstMass; }
            set
            {
                if (_firstMass != value)
                {
                    _firstMass = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SecondMass
        {
            get { return _secondMass; }
            set
            {
                if (_secondMass != value)
                {
                    _secondMass = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ScaleDate
        {
            get { return _scaleDate; }
            set
            {
                if (_scaleDate != value)
                {
                    _scaleDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SaveToDatabase(int num1)
        {
            WeighBridgeScale = num1;
        }
    }
}