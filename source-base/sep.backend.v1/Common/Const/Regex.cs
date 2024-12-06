namespace sep.backend.v1.Common.Const
{
    internal static class Regex
    {
        public const string EMAIL = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public const string PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";
        public const string PHONE = @"^0[0-9]{9}$";
        public const string NAME = @"^[a-zA-Z0-9 ]{1,50}$";
        public const string ADDRESS = @"^[a-zA-Z0-9 ]{1,100}$";
        public const string DATE = @"^(\d{4})-(\d{2})-(\d{2})$";
        public const string TIME = @"^(\d{2}):(\d{2}):(\d{2})$";
        public const string DATETIME = @"^(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2})$";
    }
}
