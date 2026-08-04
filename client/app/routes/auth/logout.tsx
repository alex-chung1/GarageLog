import type { Route } from "./+types/logout";
import { redirect } from "react-router";

export async function action({ request }: Route.ActionArgs) {
    const isDevelopment = import.meta.env.DEV;

    const cookie = [
        "auth_token=",
        "Path=/",
        "Expires=Thu, 01 Jan 1970 00:00:00 GMT",
        "HttpOnly",
        !isDevelopment ? "Secure" : null,
        `SameSite=${!isDevelopment ? "Strict" : "Lax"}`,
    ]
        .filter(Boolean)
        .join("; ");

    return redirect("/login", {
        headers: {
            "Set-Cookie": cookie,
        },
    });
}
