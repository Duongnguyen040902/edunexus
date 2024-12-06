import { AuthRepository } from '@/repositories/repository-auth';
import { ClassRepository } from './repository-class';
import { TimetableRepository } from './repository-timetable';
import { TimeSlotRepository } from './repository-timeslot';
import { SubjectRepository } from './repository-subject';
import { SemesterRepository } from './repository-semester';
import { TeacherRepository } from '@/repositories/repository-teacherAccount';
import { PupilRepository } from '@/repositories/repository-pupilAccount';
import { PupilAssignRepository } from './repository-pupil';
import { SchoolSubscriptionRepository } from '@/repositories/repository-schoolSubscription';
import { AdminSchoolRepository } from './repository-admin-school';
import { NotificationRepository } from './repository-notification';
import { SchoolRepository } from './repository-school';
import { ClassEnrollmentRepository } from './repository-class_enrollment';
import { TeacherAssignRepository } from './repository-teacher';
import { ClubRepository } from './repository-club';
import { ClassApplicationRepository } from './repository-class-application';
import { AttendanceRepository } from './repository-attendance';
import { BusRepository } from './repository-bus';
import { BusRouteRepository } from './repository-bus-route';
import { ScoreRepository } from './repository-score';
import { SchoolYearRepository } from './repository-school-year';
import { FeedbackRepository } from '@/repositories/repository-feedback.ts';
import { BusStopRepository } from './repository-bus-stop';
import { InvoiceManagerRepository } from '@/repositories/repository-invoice.ts';
import { SubscriptionRepository } from '@/repositories/repository-subscription.ts';
import { PupilClubRepository } from './repository-pupil-club';
import { ServicePackageRepository } from '@/repositories/repository-service-package.ts';
import { PupilProfileRepository } from './repository-pupilProfile';
import { TeacherProfileRepository } from './repository-teacherProfile';
import { DashboardRepository } from '@/repositories/repository-dashboard.ts';
import { BusSupervisorRepository } from './repository-bus-supervisor';
import { BusEnrollmentRepository } from './repository-bus-enrollment';
import { ClubEnrollmentRepository } from './repository-club-enrollment';
export interface RepositoriesInterface {
  auth: typeof AuthRepository;
  class: typeof ClassRepository;
  timetable: typeof TimetableRepository;
  time_slot: typeof TimeSlotRepository;
  subject: typeof SubjectRepository;
  semester: typeof SemesterRepository;
  pupilAccount: typeof PupilRepository;
  schoolSubscription: typeof SchoolSubscriptionRepository;
  adminSchool: typeof AdminSchoolRepository;
  notification: typeof NotificationRepository;
  school: typeof SchoolRepository;
  teacher: typeof TeacherRepository;
  teacherAssign: typeof TeacherAssignRepository;
  pupilAssign: typeof PupilAssignRepository;
  class_enrollment: typeof ClassEnrollmentRepository;
  invoiceManager: typeof InvoiceManagerRepository;
  club: typeof ClubRepository;
  subscription: typeof SubscriptionRepository;
  classApplication: typeof ClassApplicationRepository;
  attendance: typeof AttendanceRepository;
  bus: typeof BusRepository;
  pupilClub: typeof PupilClubRepository;
  busRoute: typeof BusRouteRepository;
  score: typeof ScoreRepository;
  servicePackage: typeof ServicePackageRepository;
  schoolYear: typeof SchoolYearRepository;
  feedback: typeof FeedbackRepository;
  dashboard: typeof DashboardRepository;
  pupil: typeof PupilProfileRepository;
  teacherProfile: typeof TeacherProfileRepository;
  busSupervisor: typeof BusSupervisorRepository;
  busStop: typeof BusStopRepository;
  bus_enrollment: typeof BusEnrollmentRepository;
  clubEnrollment: typeof ClubEnrollmentRepository;
}

export const repositories: RepositoriesInterface = {
  auth: AuthRepository,
  class: ClassRepository,
  timetable: TimetableRepository,
  time_slot: TimeSlotRepository,
  subject: SubjectRepository,
  semester: SemesterRepository,
  pupilAccount: PupilRepository,
  schoolSubscription: SchoolSubscriptionRepository,
  adminSchool: AdminSchoolRepository,
  notification: NotificationRepository,
  school: SchoolRepository,
  teacher: TeacherRepository,
  pupilAssign: PupilAssignRepository,
  teacherAssign: TeacherAssignRepository,
  class_enrollment: ClassEnrollmentRepository,
  club: ClubRepository,
  invoiceManager: InvoiceManagerRepository,
  subscription: SubscriptionRepository,
  classApplication: ClassApplicationRepository,
  attendance: AttendanceRepository,
  bus: BusRepository,
  pupilClub: PupilClubRepository,
  busRoute: BusRouteRepository,
  score: ScoreRepository,
  servicePackage: ServicePackageRepository,
  schoolYear: SchoolYearRepository,
  feedback: FeedbackRepository,
  dashboard: DashboardRepository,
  pupil: PupilProfileRepository,
  teacherProfile: TeacherProfileRepository,
  busSupervisor: BusSupervisorRepository,
  busStop: BusStopRepository,
  bus_enrollment: BusEnrollmentRepository,
  clubEnrollment: ClubEnrollmentRepository,
};

export default class RepositoryFactory {
  public static create(key: keyof RepositoriesInterface) {
    return new repositories[key]();
  }
}
