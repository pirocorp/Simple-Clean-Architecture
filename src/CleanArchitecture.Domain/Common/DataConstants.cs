namespace CleanArchitecture.Domain.Common;

public static class DataConstants
{
    public static class Department
    {
        public const int NAME_MAX_LENGTH = 200;
    }

    public static class Address
    {
        public const int STREET_MAX_LENGTH = 200;
        public const int MUNICIPALITY_MAX_LENGTH = 200;
        public const int PROVINCE_MAX_LENGTH = 200;
    }

    public static class Employee
    {
        public const int ADDRESS_MAX_LENGTH = 600;
        public const int AGE_MIN_VALUE = 0;
        public const int AGE_MAX_VALUE = 150;
        public const int EMAIL_MAX_LENGTH = 200;
        public const int NAME_MAX_LENGTH = 200;
        public const double SALARY_MIN_VALUE = 0;
        public const double SALARY_MAX_VALUE = double.MaxValue;
        public const int SALARY_PRECISION = 18;
        public const int SALARY_SCALE = 10;
    }
}
