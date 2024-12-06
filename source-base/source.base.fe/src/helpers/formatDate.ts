
export const formatDate = (
  date: Date | string,
  format: string = 'dd/mm/yyyy',
): string => {
  const dateObj = typeof date === 'string' ? new Date(date) : date;

  const formatOptions: Record<string, Intl.DateTimeFormatOptions> = {
    'dd/mm/yyyy': { day: '2-digit', month: '2-digit', year: 'numeric' },
    'yyyy/mm/dd': { year: 'numeric', month: '2-digit', day: '2-digit' },
    'mm/dd/yyyy': { month: '2-digit', day: '2-digit', year: 'numeric' },
  };

  const options = formatOptions[format] || formatOptions['dd/mm/yyyy'];

  return dateObj.toLocaleDateString('en-GB', options);
};

export const getCurrentDateFormatted = (): string => {
  return new Date().toISOString().substr(0, 10);
};