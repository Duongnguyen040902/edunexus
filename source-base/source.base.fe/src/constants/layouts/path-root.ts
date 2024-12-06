export const LAYOUT_MAPPING: Record<'admin' | 'teacher' | 'pupil' | 'school' | 'supervisor', string> = {
  admin: 'Admin',
  teacher: 'Teacher',
  pupil: 'Pupil',
  school: 'School',
  supervisor: 'Supervisor',
};

export type LayoutKeys = keyof typeof LAYOUT_MAPPING;