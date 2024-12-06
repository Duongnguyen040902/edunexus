export const mapErrorKeys = <T>(errorKeys: (keyof T)[], targetErrors: T, sourceErrors: T) => {
  errorKeys.forEach(key => {
    targetErrors[key] = (sourceErrors[key] || []) as T[keyof T];
  });
};

export const clearErrorKeys = <T>(errorKeys: (keyof T)[], targetErrors: T) => {
  errorKeys.forEach(key => {
    targetErrors[key] = [] as unknown as T[keyof T];
  });
};