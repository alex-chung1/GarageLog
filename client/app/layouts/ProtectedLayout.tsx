import type { Route } from '../+types/root';
import type { UserResponse } from '~/types/auth';

import { useState } from 'react';
import { Link, Form, Outlet, redirect, useLoaderData, useNavigation } from 'react-router';
import { AuthApi } from '~/lib/api/auth.server';

import ThemeToggle from '~/components/ThemeToggle';

export async function loader({ request }: Route.LoaderArgs) {
  try {
    const response = await AuthApi.getCurrentUser(request);

    if (!response.ok) return redirect('/login');

    const user: UserResponse = await response.json();

    return { user };
  } catch {
    return redirect('/login');
  }
}

export default function ProtectedLayout() {
  const { user } = useLoaderData<typeof loader>();

  const navigation = useNavigation();
  const isSubmitting = navigation.state === 'submitting';

  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <div className="min-h-screen bg-background text-text">
      {/* Global Submit Loading Overlay */}
      {isSubmitting && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm">
          <div className="rounded-xl border border-border bg-card px-8 py-6 shadow-lg">
            <div className="flex flex-col items-center gap-4">
              <div className="h-8 w-8 animate-spin rounded-full border-4 border-primary border-t-transparent" />

              <p className="font-medium text-text">Saving...</p>

              <p className="text-sm text-muted">Please wait</p>
            </div>
          </div>
        </div>
      )}

      <nav className="border-b border-border bg-card">
        <div className="flex h-16 items-center justify-between px-4">
          {/* Brand */}
          <Link to="/">
            <h1 className="text-xl font-bold text-primary">GarageLog</h1>
          </Link>

          {/* Desktop Menu */}
          <div className="hidden items-center gap-4 md:flex">
            <span className="text-sm text-muted">Welcome, {user.firstName}</span>

            <ThemeToggle />

            <Form method="post" action="/logout">
              <button className="rounded-lg bg-primary px-4 py-2 text-sm font-medium text-white transition hover:opacity-90">
                Log out
              </button>
            </Form>
          </div>

          {/* Mobile Actions */}
          <div className="flex items-center gap-3 md:hidden">
            <button className="text-2xl text-muted" onClick={() => setMenuOpen(!menuOpen)}>
              {menuOpen ? '✕' : '☰'}
            </button>
          </div>
        </div>

        {/* Mobile Menu */}
        {menuOpen && (
          <div className="border-t border-border bg-card md:hidden">
            <div className="flex flex-col items-start gap-4 p-4">
              <span className="text-sm text-muted">Welcome, {user.firstName}</span>

              <ThemeToggle />

              <Form method="post" action="/logout">
                <button
                  type="submit"
                  className="rounded-lg bg-primary px-4 py-2 text-sm font-medium text-white transition hover:opacity-90"
                >
                  Log out
                </button>
              </Form>
            </div>
          </div>
        )}
      </nav>

      {/* Page Content */}
      <main className="p-4 md:p-8">
        <Outlet />
      </main>
    </div>
  );
}
