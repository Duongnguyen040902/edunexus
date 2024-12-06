namespace sep.backend.v1.Common.Const
{
    public static class Responses
    {
        public const string ConflictErrorMessage = "A conflict occurred.";
        public const string ConflictErrorCode = "CONFLICT_ERROR";
        public const string CheckoutErrorMessage = "A checkout error occurred.";
        public const string CheckoutErrorCode = "CHECKOUT_ERROR";
        public const string CommitTransactionErrorMessage = "An error occurred while committing the transaction.";
        public const string RollbackTransactionErrorMessage = "An error occurred while rolling back the transaction.";
        public const string NotFoundMessageTemplate = "Không tìm thấy {attribute}";
        public const string FirstNameRequired = "Họ không được bỏ trống";
        public const string LastNameRequired = "Tên không được bỏ trống";
        public const string DonorNameRequired = "Tên người giám hộ không được bỏ trống";
        public const string ConflictAddNewClass = "Lớp học đã tồn tại!";
        public const string ConflictMaxClass = "Nhà trường chỉ có thể thêm mới tối đa 50 lớp học!";
        public const string NotFoundClass = "Không tìm thấy lớp học!";
        public const string ConflictTimeTable = "Lớp học không thể xóa vì đã có thời khóa biểu.";
        public const string ConflictAttendanceRecord = "Lớp học không thể xóa vì đã có hồ sơ điểm danh.";
        public const string ConflictNotification = "Lớp học không thể xóa vì đã có thông báo liên quan.";
        public const string NotFoundTeacher = "Không tìm thấy giáo viên";
        public const string ConflictAssignTeacher = "Lớp học đã có giáo viên trong kì học này!";
        public const string EmptyRequest = "Chưa chọn học sinh!";
        public const string MultipleClassesError = "Không thể thêm 1 học sinh vào nhiều lớp học!";
        public const string NotFoundPupil = "Không tìm thấy học sinh trong danh sách lớp!";
        public const string ConflictNameRoute = "Tên tuyến xe đã tồn tại!";
        public const string ConflictTime = "Thời gian ước tính không thể nhỏ hơn hoặc bằng thời gian ước tính của điểm dừng xe buýt cuối cùng hiện tại.";
        public const string DuplicateBusStopName = "Tên điểm dừng đã tồn tại trong tuyến xe!";
        public const string InvalidTime = "Thời gian trả về phải muộn hơn thời gian đón cho cùng một điểm dừng.";
        public const string InvalidPickUpTime = "Thời gian đón của điểm dừng mới phải muộn hơn thời gian đón của tuyến trước.";
        public const string InvalidReturnTime = "Thời gian quay về của điểm dừng mới phải muộn hơn thời gian quay về của tuyến cuối cùng!.";
        public const string InvalidUpdatePickUpTime1 = "Thời gian đón của điểm dừng phải muộn hơn thời gian đón của điểm dừng trước!";
        public const string InvalidUpdateReturnTime1 = "Thời gian quay về của điểm dừng phải muộn hơn thời gian của điểm dừng trước!.";
        public const string InvalidUpdatePickUpTime2 = "Thời gian đón của điểm dừng phải sớm hơn thời gian đón của điểm dừng sau!";
        public const string InvalidUpdateReturnTime2 = "Thời gian quay về của điểm dừng phải sớm hơn thời gian của điểm dừng sau!.";
        public const string ConflictBusName = "Tên xe đã tồn tại!.";
        public const string ConflictBus = "Đã gặp lỗi về việc chọn xe!.";
        public const string ConflictAddPupilsToBus = "Không thể thêm học sinh. Hiện tại chỉ còn trống {actualSeat} ghế!";
        public const string ConflictNameTemplate = "{attribute} đã tồn tại";
        public const string ConflictNameClub = "Tên câu lạc bộ đã tồn tại!";
        public const string ConflictAssignToClub = "Thêm thành viên vào Câu lạc bộ đã xảy ra lỗi, thành viên lựa chọn đã tồn tại trong kì lự chọn!!";
        public const string ConflictAddMemberBus = "Học sinh hoặc giám sát viên đã đăng ký vào xe buýt này trong kỳ học lựa chọn!";
        public const string ConflictAddMemberClub = "Thành viên đã đăng ký vào xe buýt này trong kỳ học lựa chọn!";
        public const string ConflictAddMemberClass = "Học sinh tham gia vào lớp học trong kỳ học lựa chọn!";
        public const string FileRequired = "Tệp tin không được bỏ trống";
        public const string ConflictAssignToClass = "Thêm thành viên vào lớp học đã xảy ra lỗi, thành viên lựa chọn đã tồn tại trong kì lự chọn!";
    }
}

 
