// `source-base/source.base.fe/src/helpers/textHelpers.ts`
export const truncatedDescription = (description: string, maxLength: number = 40): string => {
  if (description.length > maxLength) {
    return description.substring(0, maxLength) + '...';
  }
  return description;
};