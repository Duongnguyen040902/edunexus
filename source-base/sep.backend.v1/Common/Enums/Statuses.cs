namespace sep.backend.v1.Common.Enums
{
    public enum Statuses
    {
        Active = 1, // ACTIVE STATUS OF THE ACCOUNT, STUDENT LIST IN CLASS, BUS, CLUB IN ENROLLMENT AND USER TABLE
        Inactive = 2, // INACTIVE STATUS OF THE ACCOUNT, OR STUDENT ON LEAVE
        Delete = 3, // DELETED STATUS OF THE ACCOUNT
        Registration = 4, // REGISTRATION STATUS OF THE STUDENT IN THE REGISTRATION TABLE
        Approve = 5, // APPROVED STATUS OF THE STUDENT IN THE REGISTRATION TABLE
        Reject = 6, // REJECTED STATUS OF THE STUDENT IN THE REGISTRATION TABLE
        Finnish = 7, // COMPLETED STATUS OF THE STUDENT IN CLASS, CLUB, OR COMPLETED A BUS ROUTE IN THE ENROLLMENT TABLE
        NotPromoted = 8, // STATUS OF THE STUDENT WHO DID NOT MEET PROMOTION CRITERIA TO MOVE TO THE NEXT CLASS
    }

    public enum PaymentStatuses
    {
        Success = 1,
        Error = 2,
    }

    public enum StatusAccount
    {
        Active = 1,
        Inactive = 2,
        Deleted = 3
    }

    public enum ClubEnrollmentStatus
    {
        Register = 1,
        Approved = 2,
        Rejected = 3,
        Cancel = 4,
        Teaching = 5
    }

    public enum ClassApplicationStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }

    public enum ResponseApplicationStatus
    {
        Approved = 2,
        Rejected = 3
    }

    public enum AttendanceType
    {
        CLASSATTENDANCE = 1,
        CLUBATTENDANCE = 2,
        BUSATTENDANCE = 3,
        Moring= 1,
        Afternoon = 2,
        EXTRACURRICULAR = 3,
        BUS = 4,
    }

    public enum InvoiceStatuses
    {
        Pending = 1,
        Paid = 2,
        Cancel = 3,
        Sent = 4,
        Change = 5
    }

    public enum ScoreStatus
    {
        IsPrivate = 1,
        IsPublic = 2
    }

    public enum FeedbackStatus
    {
        Active = 1,
        Inactive = 2
    }

    public enum ClassStatus
    {
        Active = 1,
        Inactive = 0
    }
}