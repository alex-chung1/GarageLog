import { type RouteConfig, route, layout } from '@react-router/dev/routes'

export default [
    // Auth
    layout('./layouts/AuthLayout.tsx', [
        // /login
        route('login', './routes/auth/login.tsx'),

        // /register
        route('register', './routes/auth/register.tsx'),

        // /logout
        route('logout', './routes/auth/logout.tsx'),
    ]),

    // Redirects to /garage
    route('/', './routes/index.tsx'),

    // Protected
    layout('./layouts/ProtectedLayout.tsx', [
        // /garage
        route('garage', './routes/protected/garage/index.tsx'),

        // /garage/vehicle/new
        route('garage/vehicle/new', './routes/protected/garage/vehicle-create.tsx'),

        // /garage/vehicle/:vehicleId
        route('garage/vehicle/:vehicleId', './routes/protected/garage/vehicle-detail.tsx'),

        // /garage/vehicle/:vehicleId/edit
        route('garage/vehicle/:vehicleId/edit', './routes/protected/garage/vehicle-edit.tsx'),

        // /garage/vehicle/:vehicleId/service-record/new
        route(
            'garage/vehicle/:vehicleId/service-record/new',
            './routes/protected/garage/service-record-create.tsx',
        ),

        // /garage/vehicle/:vehicleId/service-record/:serviceRecordId/edit
        route(
            'garage/vehicle/:vehicleId/service-record/:serviceRecordId/edit',
            './routes/protected/garage/service-record-edit.tsx',
        ),
    ]),
] satisfies RouteConfig
