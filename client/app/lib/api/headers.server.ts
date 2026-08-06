export function copySetCookieHeaders(response: Response) {
  const headers = new Headers();

  for (const cookie of response.headers.getSetCookie()) {
    headers.append('Set-Cookie', cookie);
  }

  return headers;
}
