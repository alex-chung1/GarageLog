import type { Route } from '../+types/root';

import { Link, Outlet, redirect } from 'react-router';
import { AuthApi } from '~/lib/api/auth.server';

import ThemeToggle from '~/components/ThemeToggle';

export async function loader({ request }: Route.LoaderArgs) {
  try {
    const response = await AuthApi.getCurrentUser(request);

    if (response.ok) return redirect('/');

    return null;
  } catch {
    return null;
  }
}

export default function AuthLayout() {
  return (
    <div className="min-h-screen bg-background text-text">
      <nav className="border-b border-border bg-card">
        <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-4 md:px-8">
          {/* Brand */}
          <Link to="/" className="text-xl font-bold text-primary">
            GarageLog
          </Link>

          {/* Navigation */}
          <div className="flex items-center gap-4">
            <Link to="/" className="text-sm font-medium text-muted transition hover:text-primary">
              Home
            </Link>

            <ThemeToggle />
          </div>
        </div>
      </nav>

      <main>
        <Outlet />
      </main>
    </div>
  );
}
