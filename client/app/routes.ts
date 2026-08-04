import {
    type RouteConfig,
    route,
    index,
    layout,
} from "@react-router/dev/routes";

export default [
    // Auth
    layout("./layouts/AuthLayout.tsx", [
        route("login", "./routes/auth/login.tsx"),
        route("register", "./routes/auth/register.tsx"),
        route("logout", "./routes/auth/logout.tsx"),
    ]),
    // Root re-direct -> /garage
    route("/", "./routes/index.tsx"),

    // Protected App
    layout("./layouts/ProtectedLayout.tsx", [
        route("garage", "./routes/protected/garage/index.tsx"),
        route(
            "garage/vehicle/:vehicleId",
            "./routes/protected/garage/vehicle-detail.tsx",
        ),
    ]),
] satisfies RouteConfig;
