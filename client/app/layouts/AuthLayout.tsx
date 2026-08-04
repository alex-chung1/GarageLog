import type { Route } from '../+types/root'

import { Outlet, redirect } from 'react-router'
import { AuthApi } from '~/lib/api/auth.server'

import ThemeToggle from '~/components/ThemeToggle'

export async function loader({ request }: Route.LoaderArgs) {
    const user = await AuthApi.getCurrentUser(request)

    if (user) return redirect('/')

    return null
}

export default function AuthLayout() {
    return (
        <div className="relative min-h-screen bg-background text-text">
            <div className="absolute right-4 top-4">
                <ThemeToggle />
            </div>

            <Outlet />
        </div>
    )
}
