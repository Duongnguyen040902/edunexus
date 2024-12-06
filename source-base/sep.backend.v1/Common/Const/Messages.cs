namespace sep.backend.v1.Common.Const
{
    internal static class Messages
    {
        public const string USERNAME_REQUIRED = "Tên đăng nhập không được bỏ trống";
        public const string USERNAME_LENGTH = "Tên đăng nhập phải có từ 2 đến 50 ký tự";
        public const string USERNAME_LETTER = "Tên đăng nhập chỉ chứa chữ cái và số";
        public const string USERNAME_EXISTS = "Tên đăng nhập đã tồn tại";
        public const string NAME_REQUIRED = "Tên không được bỏ trống";
        public const string EMAIL_REQUIRED = "Email không được bỏ trống";
        public const string EMAIL_INVALID = "Email không hợp lệ";
        public const string EMAIL_EXISTS = "Email đã tồn tại";
        public const string PASSWORD_REQUIRED = "Mật khẩu không được bỏ trống";
        public const string PASSWORD_LENGTH = "Mật khẩu phải có ít nhất 6 ký tự";
        public const string PASSWORD_MATCH = "Mật khẩu không khớp";
        public const string NAME_LENGTH = "Tên phải có từ 2 đến 50 ký tự";
        public const string NAME_LETTER = "Tên chỉ chứa chữ cái và khoảng trắng";
        public const string PHONE_NUMBER_LENGTH = "Số điện thoại phải có độ dài từ {min} đến {max} ký tự";
        public const string FILE_SIZE_EXCEEDED = "Kích thước tập tin không được vượt quá {maxSize} MB";

        // Custom messages for TimeTableValidator
        public const string CLASS_ID_REQUIRED = "ClassId không được bỏ trống";
        public const string SEMESTER_ID_REQUIRED = "Kỳ học không được bỏ trống";
        public const string TIME_SLOT_ID_REQUIRED = "TimeSlotId không được bỏ trống";
        public const string SUBJECT_ID_REQUIRED = "SubjectId không được bỏ trống";
        public const string DAY_OF_WEEK_REQUIRED = "DayOfWeek không được bỏ trống";
        public const string CONFIRM_CODE_REQUIRED = "Mã xác nhận không được bỏ trống";
        public const string CONFIRM_CODE_INVALID = "Mã xác nhận không hợp lệ";
        public const string DATE_OF_BIRTH_REQUIRED = "Ngày sinh không được bỏ trống";
        public const string DATE_OF_BIRTH_IN_THE_PAST = "Ngày sinh phải ở trong quá khứ";
        public const string PHONE_NUMBER_REQUIRED = "Số điện thoại không được bỏ trống";
        public const string PHONE_NUMBER_INVALID = "Số điện thoại phải là số điện thoại hợp lệ";
        public const string ADDRESS_REQUIRED = "Địa chỉ không được bỏ trống";
        public const string SUBSCRIPTION_PLAN_NOT_EXISTS = "Gói dịch vụ không tồn tại";
        public const string REQUIRED = "{attribute} không được bỏ trống";
        public const string INVALID = "{attribute} không hợp lệ";
        public const string MAX = "{attribute} không được vượt quá {maxLength} ký tự.";
        public const string MIN = "{attribute} không được ít hơn {minLength} ký tự.";
        public const string REGEX = "{attribute} không hợp lệ";
        public const string NOT_EXIST = "{attribute} không tồn tại";
        public const string IS_EXIST = "{attribute} đã tồn tại";
        public const string NOT_ASSIGN = "Chưa phân công";
        public const string NOT_FOUND = "{attribute} không tìm thấy";
        public const string CONFLICT = "{attribute}";
        public const string DATE_REQUIRED = "Ngày không được bỏ trống";
        public const string DATE_INVALID = "Ngày không hợp lệ";
        public const string DATE_IN_THE_PAST = "Ngày phải ở trong quá khứ";
        public const string DATE_IN_THE_FUTURE = "Ngày phải ở trong tương lai";
        public const string DATE_NOT_IN_CURRENT_YEAR = "Ngày phải trong năm nay";
        public const string DATE_BEFORE = "{attribute} phải trước {comparisonDate}";
        public const string DATE_AFTER = "{attribute} phải sau {comparisonDate}";
        public const string GREATER_THAN = "{attribute} phải lớn hơn {comparisonValue}";
        public const string UNIQUE = "{attribute} đã tồn tại";
        public const string MAX_LENGTH = "Không được dài hơn {maxLength} ký tự.";
        public const string LICENSE_PLATE_INVALID = "Biển số xe không hợp lệ. Vui lòng nhập đúng định dạng.";
        public const string SEAT_NUMBER_INVALID = "Số ghế phải lớn hơn 4.";
        public const string INVALID_RETURNTIME = "Thời gian đưa về phải lớn hơn thời gian đón!";
        public const string INVALID_PICKUPTIME = "Thời gian đón phải lớn hơn thời gian đón của điểm trước!";
        public const string ACCOUNT_LIMIT = "Số lượng tài khoản đã vượt quá giới hạn gói cung cấp";
        public const string SUBJECT_ASSIGN = "Phải đăng ký ít nhất một bộ môn";
        public const string DATE_OF_BIRTH_YEAR_ENOUGH = "Giáo viên phải ít nhất 18 tuổi";
        public const string FILE_TYPE_EXCEEDED = "Các file hợp lệ {type}.";
        public const string VERIFY_ACCOUNT = "Tài khoản chưa được người dùng xác thực. Không thể kích hoạt tài khoản!";
        public const string START_TIME_REQUIRED = "Thời gian bắt đầu tiết học không được bỏ trống!";
        public const string END_TIME_REQUIRED = "Thời gian kết thúc tiết học không được bỏ trống!";
        public const string END_TIME_AFTER_START_TIME = "Thời gian kết thúc tiết học phải lớn hơn thời gian bắt đầu tiết học!";
        public const string IS_ACTIVE_REQUIRED = "Trạng thái không được để trống!";
        public const string AGE_INVALID = "{attribute}";
        public const string FIRST_NAME_INVALID = "Vui lòng chỉ nhập các ký tự chữ cái và không có dấu cách hoặc ký tự đặc biệt"; 
        public const string LAST_NAME_INVALID = "Họ chỉ chứa chữ cái và khoảng trắng";
        public const string FIRST_NAME_LENGTH = "Tên phải có từ {min} đến {max} ký tự";
        public const string LAST_NAME_LENGTH = "Họ phải có từ {min} đến {max} ký tự";

    }
}