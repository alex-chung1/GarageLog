import type { Route } from '../+types/root';

import { Link, Outlet, redirect } from 'react-router';
import { AuthApi } from '~/lib/api/auth.server';

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
        <div className="mx-auto flex h-18 max-w-9xl items-center justify-between px-4 md:px-6">
          {/* Brand */}
          <Link
            to="/"
            className="text-xl font-bold tracking-tight text-primary transition-opacity hover:opacity-80"
          >
            GarageLog
          </Link>
        </div>
      </nav>

      <main>
        <Outlet />
      </main>
    </div>
  );
}
