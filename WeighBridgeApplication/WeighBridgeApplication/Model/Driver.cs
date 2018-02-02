using WeighBridgeApplication.Util;

namespace WeighBridgeApplication.Model
{
    public class Driver : PropertyChangedNotify
    {
        private string _firstname;
        private string _fullName;
        private string _idNumber;
        private string _surname;
        public int Id { get; set; }
        
        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (_firstname != value)
                {
                    _firstname = value;
                    OnPropertyChanged();
                }
            }
        }

        public string IdNumber
        {
            get { return _idNumber; }
            set
            {
                if (_idNumber != value)
                {
                    _idNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Gender { get; set; }

        public string Surname
        {
            get { return _surname; }
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged();
                }
            }
        }

        public string IdLocation { get; set; }

        public string VatFormLocation { get; set; }

        public string ImageName { get; set; }

        public Supplier SupplierInfo { get; set; }

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}