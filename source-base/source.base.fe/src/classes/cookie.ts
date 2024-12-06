import Cookies from 'js-cookie';

export class Cookie {
  public static get(key: string): string | undefined {
    return Cookies.get(key);
  }

  public static set(key: string, value: string, time = 36000) {
    Cookies.set(key, value, {
      expires: time,
    });
  }

  public static remove(key: string) {
    Cookies.remove(key);
  }
}
